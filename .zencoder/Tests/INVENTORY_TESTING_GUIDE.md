# Crystal Clinic Inventory System - Testing Guide

**Date**: 2026-01-18  
**Version**: 1.0  
**Target Environment**: Development/QA  
**API Base URL**: `https://api.crystalclinic.local/api/inventory`

---

## Prerequisites

### Required Setup
1. Valid JWT Bearer token from authentication service
2. Test environment with populated Items, Stocks, and Branches tables
3. Postman or curl installed for API testing
4. Real test data loaded into database

### Test User Details
- **Username**: dr-sarah@crystalclinic.local
- **Branch ID**: 1 (Lahore)
- **Branch Name**: Crystal Clinic Lahore
- **User GUID**: `550e8400-e29b-41d4-a716-446655440000`
- **JWT Token**: Replace `YOUR_TOKEN` in all requests

### Test Data - Items
| Item ID | Name | Unit Cost | Status |
|---------|------|-----------|--------|
| 4521 | Titanium Implant Grade-5 | 450 | Active |
| 2015 | Antibiotic Capsule | 2 | Active |
| 102 | Sterile Gloves | 3 | Active |
| 103 | Surgical Mask | 0.5 | Active |
| 104 | Sterile Field | 5 | Active |

### Test Data - Stocks (Batches)
| Stock ID | Item ID | Lot Number | Quantity | Expiry Date | Status |
|----------|---------|-----------|----------|-------------|--------|
| 1001 | 4521 | TI-2025-001 | 8 | 2027-12-31 | Active |
| 1002 | 4521 | TI-2025-002 | 7 | 2027-06-15 | Active |
| 3401 | 2015 | AB-2025-045 | 120 | 2026-06-30 | Active |
| 5001 | 102 | GL-2025-100 | 50 | 2026-12-31 | Active |
| 5002 | 103 | MS-2025-050 | 100 | 2027-03-15 | Active |

### Test Data - Kits
| Kit ID | Name | Items | Branch |
|--------|------|-------|--------|
| 7 | Dental Surgical Kit | 102 (qty:2), 103 (qty:5), 104 (qty:1) | 1 |

---

## Test Execution Plan

**Total Endpoints**: 13  
**Estimated Test Duration**: 30-45 minutes  
**Test Sequence**: Sequential (follows realistic workflow)

```
Workflow Order:
1. Query Stock → Get baseline
2. Register Movement (In) → Add new stock
3. Perform Stock Take → Adjust inventory
4. Reserve Items → Hold items for visit
5. Get Reservation Details → Verify reservation
6. List Active Reservations → Dashboard check
7. Consume Kit → Multi-item deduction
8. Get Movement History → Audit trail
9. Commit Reservation → Finalize deduction
10. Release Reservation → (Alternative) Cancel
11. Get Expiring Stock → Compliance check
12. Get Valuation Report → Financial reporting
13. Get Available Kits → Kit management
```

---

# ENDPOINT TEST CASES

## Test 1: GET /api/inventory/stock/{itemId}

**Purpose**: Retrieve stock level and batch information  
**Priority**: P0 - Essential  
**Prerequisites**: Item 4521 must exist in database

### Request
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/stock/4521" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
{
  "itemId": 4521,
  "itemName": "Titanium Implant Grade-5",
  "totalQuantity": 15,
  "usableQuantity": 15,
  "reservedQuantity": 0,
  "availableQuantity": 15,
  "batches": [
    {
      "stockId": 1001,
      "lotNumber": "TI-2025-001",
      "quantity": 8,
      "quantityRemaining": 8,
      "expiryDate": "2027-12-31T00:00:00Z",
      "isExpired": false,
      "unitCost": 450
    },
    {
      "stockId": 1002,
      "lotNumber": "TI-2025-002",
      "quantity": 7,
      "quantityRemaining": 7,
      "expiryDate": "2027-06-15T00:00:00Z",
      "isExpired": false,
      "unitCost": 448
    }
  ]
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] itemId matches request: 4521
- [ ] itemName is correct: "Titanium Implant Grade-5"
- [ ] totalQuantity = sum of all batches: 15
- [ ] Batches ordered by purchase date (FIFO)
- [ ] No expired batches included

### Notes
- Baseline check before any modifications
- Verifies FIFO ordering

---

## Test 2: POST /api/inventory/movement (Register In-Movement)

**Purpose**: Add new stock to inventory  
**Priority**: P0 - Essential  
**Prerequisites**: Item 4521 exists, initial stock known

### Request
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/movement" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "itemId": 4521,
    "stockId": null,
    "quantity": 5,
    "type": "In",
    "reason": "PurchaseReceipt",
    "referenceId": "PO-2026-00789",
    "notes": "Emergency purchase from Supplier B"
  }'
```

### Expected Response (200 OK)
```json
{
  "success": true,
  "newBalance": 20,
  "movementId": 99001,
  "unitCost": 449.50,
  "totalCost": 2247.50,
  "quantity": 5
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] success = true
- [ ] movementId is assigned (> 0)
- [ ] newBalance = previous balance + 5
- [ ] totalCost = quantity × unitCost
- [ ] StockMovement record created in database

### Notes
- Simulates purchase receipt
- stockId is null (auto-creates new batch or uses FIFO)
- Reference number for procurement tracking

---

## Test 3: POST /api/inventory/movement (Register Out-Movement)

**Purpose**: Deduct stock manually  
**Priority**: P0 - Essential  
**Prerequisites**: Stock 1001 has at least 2 units

### Request
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/movement" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "itemId": 4521,
    "stockId": 1001,
    "quantity": 2,
    "type": "Out",
    "reason": "DamageWriteOff",
    "referenceId": "DAMAGE-2026-001",
    "notes": "Damaged during storage - unit 1001"
  }'
```

### Expected Response (200 OK)
```json
{
  "success": true,
  "newBalance": 6,
  "movementId": 99002,
  "unitCost": 450,
  "totalCost": 900,
  "quantity": 2
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] success = true
- [ ] newBalance reduced (8 → 6)
- [ ] Exact stockId used (1001)
- [ ] StockMovement created with Reason: DamageWriteOff
- [ ] Stock 1001 quantity reduced in database

### Notes
- Tests damage write-off scenario
- Specific stockId specified (not null)
- Important for inventory reconciliation

---

## Test 4: POST /api/inventory/stocktake

**Purpose**: Record physical inventory count adjustment  
**Priority**: P1 - High  
**Prerequisites**: Stock 1002 exists

### Pre-Condition
Assume Stock 1002 currently has 7 units but physical count shows 6

### Request
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/stocktake" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "stockId": 1002,
    "actualQuantity": 6,
    "reason": "Monthly Physical Count - January 2026",
    "notes": "Discrepancy of 1 unit found during audit"
  }'
```

### Expected Response (200 OK)
```json
{
  "succeeded": true
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] succeeded = true
- [ ] Stock 1002 quantity adjusted from 7 to 6
- [ ] StockMovement created with Type: Out, Reason: Adjustment
- [ ] Movement quantity = 1 (difference)

### Error Case - Stock Not Found
If StockId 9999 doesn't exist:

**Expected Response (400 Bad Request)**:
```json
{
  "succeeded": false,
  "error": "Stock batch not found"
}
```

### Validations
- [ ] HTTP Status: 400
- [ ] succeeded = false
- [ ] Meaningful error message

### Notes
- Tests variance adjustment
- Important for inventory audit

---

## Test 5: POST /api/inventory/reserve (Success Case)

**Purpose**: Reserve items for a patient visit  
**Priority**: P0 - Critical  
**Prerequisites**: Items 4521 and 2015 have available stock

### Request
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reserve" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "visitId": 8724,
    "serviceId": 501,
    "items": [
      {
        "itemId": 4521,
        "quantity": 1
      },
      {
        "itemId": 2015,
        "quantity": 5
      }
    ],
    "idempotencyToken": "VIS-8724-20260118-A7F2",
    "ttlSeconds": 3600
  }'
```

### Expected Response (200 OK)
```json
{
  "success": true,
  "reservationId": 5421,
  "expiresAt": "2026-01-18T22:50:00Z",
  "reservedLots": [
    {
      "stockId": 1001,
      "itemId": 4521,
      "reservedQuantity": 1,
      "unitCost": 450
    },
    {
      "stockId": 3401,
      "itemId": 2015,
      "reservedQuantity": 5,
      "unitCost": 2
    }
  ]
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] success = true
- [ ] reservationId assigned (> 0)
- [ ] expiresAt = now + 3600 seconds
- [ ] All items reserved with correct quantities
- [ ] InventoryReservation record created
- [ ] ReservedItems records created for each item
- [ ] Stock quantities decremented (but not removed)

### Database Checks
```sql
-- Verify reservation created
SELECT * FROM InventoryReservations WHERE Id = 5421;

-- Verify reserved items
SELECT * FROM ReservedItems WHERE ReservationId = 5421;

-- Verify stock quantities reduced
SELECT stockId, itemId, QuantityRemaining FROM Stocks 
WHERE stockId IN (1001, 3401);
```

### Notes
- Idempotency token prevents duplicates
- Reserve for specific visit
- Items not yet deducted (just held)

---

## Test 6: POST /api/inventory/reserve (Idempotency Test)

**Purpose**: Test idempotent behavior - same token returns same result  
**Priority**: P1 - High  
**Prerequisites**: Previous reservation (Test 5) still active

### Request (Same as Test 5)
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reserve" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "visitId": 8724,
    "serviceId": 501,
    "items": [
      {
        "itemId": 4521,
        "quantity": 1
      },
      {
        "itemId": 2015,
        "quantity": 5
      }
    ],
    "idempotencyToken": "VIS-8724-20260118-A7F2",
    "ttlSeconds": 3600
  }'
```

### Expected Response (200 OK)
```json
{
  "success": true,
  "reservationId": 5421,
  "expiresAt": "2026-01-18T22:50:00Z",
  "reservedLots": [
    {
      "stockId": 1001,
      "itemId": 4521,
      "reservedQuantity": 1,
      "unitCost": 450
    },
    {
      "stockId": 3401,
      "itemId": 2015,
      "reservedQuantity": 5,
      "unitCost": 2
    }
  ]
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] success = true
- [ ] **SAME reservationId**: 5421 (not new)
- [ ] **SAME expiry time** (not extended)
- [ ] **No duplicate records** in database
- [ ] No additional StockMovement created

### Notes
- Critical for network reliability
- Prevents double-reservations on retry
- Simulates network timeout recovery

---

## Test 7: POST /api/inventory/reserve (Insufficient Stock)

**Purpose**: Test reservation failure when stock unavailable  
**Priority**: P1 - High

### Request
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reserve" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "visitId": 8725,
    "serviceId": 502,
    "items": [
      {
        "itemId": 4521,
        "quantity": 50
      }
    ],
    "idempotencyToken": "VIS-8725-20260118-SHORTAGE",
    "ttlSeconds": 3600
  }'
```

### Expected Response (400 Bad Request)
```json
{
  "success": false,
  "errorMessage": "Insufficient stock for one or more items",
  "reservationId": 0,
  "expiresAt": "0001-01-01T00:00:00Z",
  "reservedLots": [],
  "shortages": [
    {
      "itemId": 4521,
      "requestedQuantity": 50,
      "availableQuantity": 15,
      "itemName": "Titanium Implant Grade-5"
    }
  ]
}
```

### Validations
- [ ] HTTP Status: 400
- [ ] success = false
- [ ] Shortage details provided
- [ ] **No reservation created**
- [ ] **No stock quantities reduced**
- [ ] Meaningful shortage message

### Notes
- Tests business rule validation
- Returns available vs requested quantities
- Prevents invalid reservations

---

## Test 8: GET /api/inventory/reservation/{reservationId}

**Purpose**: Retrieve full reservation details  
**Priority**: P0 - Critical  
**Prerequisites**: Reservation 5421 created (Test 5)

### Request
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/reservation/5421" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
{
  "id": 5421,
  "visitId": 8724,
  "serviceId": 501,
  "status": "Active",
  "expiresAt": "2026-01-18T22:50:00Z",
  "requestedBy": "550e8400-e29b-41d4-a716-446655440000",
  "createdOn": "2026-01-18T18:50:00Z",
  "reservedItems": [
    {
      "itemId": 4521,
      "itemName": "Titanium Implant Grade-5",
      "stockId": 1001,
      "lotNumber": "TI-2025-001",
      "reservedQuantity": 1,
      "unitCost": 450,
      "totalCost": 450
    },
    {
      "itemId": 2015,
      "itemName": "Antibiotic Capsule",
      "stockId": 3401,
      "lotNumber": "AB-2025-045",
      "reservedQuantity": 5,
      "unitCost": 2,
      "totalCost": 10
    }
  ]
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] id = 5421 (correct reservation)
- [ ] status = "Active"
- [ ] visitId = 8724
- [ ] All reserved items included with names and lot numbers
- [ ] Costs calculated correctly
- [ ] requestedBy matches user GUID

### Error Case - Not Found
Request non-existent reservation 99999:

**Expected Response (404)**:
```json
{}
```

### Validations
- [ ] HTTP Status: 404
- [ ] Empty object returned

### Notes
- Detailed item information (names, lots)
- Used for verification before commit

---

## Test 9: GET /api/inventory/reservations/active

**Purpose**: List all active reservations for branch  
**Priority**: P1 - High  
**Prerequisites**: At least one active reservation (5421)

### Request
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/reservations/active" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
[
  {
    "id": 5421,
    "visitId": 8724,
    "status": "Active",
    "expiresAt": "2026-01-18T22:50:00Z",
    "minutesUntilExpiry": 59,
    "totalReservedItems": 2,
    "totalReservedValue": 460,
    "requestedBy": "550e8400-e29b-41d4-a716-446655440000"
  }
]
```

### Validations
- [ ] HTTP Status: 200
- [ ] Array of reservations returned
- [ ] Only Active status included
- [ ] minutesUntilExpiry calculated correctly
- [ ] totalReservedItems = count of reserved items
- [ ] totalReservedValue = sum of (quantity × unitCost)
- [ ] Ordered by expiry time (soonest first)
- [ ] Only user's branch shown

### Notes
- Dashboard view of current holds
- Real-time expiry monitoring

---

## Test 10: GET /api/inventory/kits

**Purpose**: List available inventory kits  
**Priority**: P1 - High  
**Prerequisites**: Kit 7 exists and is active

### Request
```bash
curl -X GET "https://api.crystalclinic.local/
" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
[
  {
    "id": 7,
    "kitName": "Dental Surgical Kit",
    "description": "Complete set for dental implant procedures",
    "isActive": true,
    "branchId": 1,
    "kitLines": [
      {
        "id": 71,
        "itemId": 102,
        "quantity": 2,
        "item": {
          "itemId": 102,
          "name": "Sterile Gloves",
          "isActive": true
        }
      },
      {
        "id": 72,
        "itemId": 103,
        "quantity": 5,
        "item": {
          "itemId": 103,
          "name": "Surgical Mask",
          "isActive": true
        }
      },
      {
        "id": 73,
        "itemId": 104,
        "quantity": 1,
        "item": {
          "itemId": 104,
          "name": "Sterile Field",
          "isActive": true
        }
      }
    ]
  }
]
```

### Validations
- [ ] HTTP Status: 200
- [ ] Array of kits returned
- [ ] Only active kits (isActive = true)
- [ ] Only user's branch (branchId = 1)
- [ ] Kit lines included with quantities
- [ ] Item details populated (names, IDs)

### Notes
- Lists available kits for procedures
- Used for kit consumption selection

---

## Test 11: POST /api/inventory/kits/{kitId}/consume

**Purpose**: Consume a kit (deduct all items atomically)  
**Priority**: P0 - Critical  
**Prerequisites**: Kit 7 active, all kit items have sufficient stock

### Pre-Condition Check
Verify stock levels for kit items:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/stock/102" \
  -H "Authorization: Bearer YOUR_TOKEN"

curl -X GET "https://api.crystalclinic.local/api/inventory/stock/103" \
  -H "Authorization: Bearer YOUR_TOKEN"

curl -X GET "https://api.crystalclinic.local/api/inventory/stock/104" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### Request
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/kits/7/consume" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "quantity": 1,
    "referenceId": "VIS-8724"
  }'
```

### Expected Response (200 OK)
```json
{
  "success": true,
  "itemMovements": [
    {
      "success": true,
      "newBalance": 48,
      "movementId": 99010,
      "unitCost": 3,
      "totalCost": 6,
      "quantity": 2
    },
    {
      "success": true,
      "newBalance": 95,
      "movementId": 99011,
      "unitCost": 0.5,
      "totalCost": 2.5,
      "quantity": 5
    },
    {
      "success": true,
      "newBalance": 49,
      "movementId": 99012,
      "unitCost": 5,
      "totalCost": 5,
      "quantity": 1
    }
  ],
  "totalCost": 13.5
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] success = true
- [ ] All kit items consumed (3 movements)
- [ ] Quantities match kit lines (2, 5, 1)
- [ ] totalCost = sum of all item costs
- [ ] Each item has movementId assigned
- [ ] New balances reduced correctly
- [ ] All StockMovement records created
- [ ] InventoryKitConsumedEvent published

### Database Checks
```sql
-- Verify movements created
SELECT * FROM StockMovements 
WHERE ReferenceId = 'VIS-8724' AND Date >= CAST(GETDATE() AS DATE);

-- Verify stock quantities reduced
SELECT stockId, itemId, QuantityRemaining FROM Stocks 
WHERE itemId IN (102, 103, 104);
```

### Error Case - Insufficient Stock
If any kit item has insufficient stock:

**Expected Response (400 Bad Request)**:
```json
{
  "success": false,
  "errorMessage": "Failed to consume Surgical Mask: Insufficient stock available"
}
```

### Validations
- [ ] HTTP Status: 400
- [ ] success = false
- [ ] Specific item identified in error
- [ ] **NO items consumed** (atomic transaction)
- [ ] Stock quantities unchanged

### Notes
- Atomic operation: all items or none
- Uses FIFO for each kit item
- Tracks to visit/reference

---

## Test 12: POST /api/inventory/reservation/{reservationId}/commit

**Purpose**: Finalize reservation by deducting reserved items  
**Priority**: P0 - Critical  
**Prerequisites**: Reservation 5421 is Active and not expired

### Request
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reservation/5421/commit" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
{
  "succeeded": true
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] succeeded = true
- [ ] Reservation status changed from Active to Committed
- [ ] StockMovement records created for each reserved batch
- [ ] Stock quantities permanently reduced:
  - Stock 1001: 8 → 7 (Titanium Implant)
  - Stock 3401: 120 → 115 (Antibiotic Capsule)
- [ ] ReservedItems records remain (for history)
- [ ] InventoryMovementEvent published

### Database Checks
```sql
-- Verify reservation committed
SELECT Id, Status FROM InventoryReservations WHERE Id = 5421;
-- Should show: Status = Committed

-- Verify stock movements created
SELECT * FROM StockMovements 
WHERE StockId IN (1001, 3401) 
AND ReferenceId LIKE 'VIS-%' 
AND Date >= CAST(GETDATE() AS DATE);

-- Verify stock quantities reduced
SELECT stockId, QuantityRemaining FROM Stocks WHERE stockId IN (1001, 3401);
```

### Error Cases

**Case 1: Reservation Not Found**
Request non-existent reservation 99999:

**Expected Response (400)**:
```json
{
  "succeeded": false,
  "error": "Reservation not found or not active"
}
```

**Case 2: Reservation Expired**
If reservation created with ttlSeconds=1 and waited >1 second:

**Expected Response (400)**:
```json
{
  "succeeded": false,
  "error": "Reservation has expired"
}
```

**Case 3: Reservation Already Committed**
Commit reservation 5421 twice:

**Expected Response (400)**:
```json
{
  "succeeded": false,
  "error": "Reservation not found or not active"
}
```

### Notes
- Irreversible operation
- Creates full audit trail
- Prevents double-commit via RowVersion

---

## Test 13: POST /api/inventory/reservation/{reservationId}/release

**Purpose**: Cancel reservation without deducting items  
**Priority**: P1 - High  
**Prerequisites**: Create new active reservation (different from 5421)

### Setup: Create New Reservation to Release
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reserve" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "visitId": 8726,
    "serviceId": 502,
    "items": [
      {
        "itemId": 4521,
        "quantity": 2
      }
    ],
    "idempotencyToken": "VIS-8726-RELEASE-TEST",
    "ttlSeconds": 3600
  }'
```

**Expected Response**:
```json
{
  "success": true,
  "reservationId": 5422
}
```

### Main Test: Release Reservation
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reservation/5422/release" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
{
  "succeeded": true
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] succeeded = true
- [ ] Reservation status changed from Active to Released
- [ ] Stock quantities restored (not permanently reduced):
  - Stock 1001: returned 2 units back to available pool
- [ ] ReservedItems records removed
- [ ] **NO StockMovement records created**

### Database Checks
```sql
-- Verify reservation released
SELECT Id, Status FROM InventoryReservations WHERE Id = 5422;
-- Should show: Status = Released

-- Verify NO movements created
SELECT COUNT(*) AS MovementCount FROM StockMovements 
WHERE Reason = 'SaleDeduction' AND Date >= CAST(GETDATE() AS DATE);

-- Verify stock quantities restored
SELECT stockId, QuantityRemaining FROM Stocks WHERE stockId = 1001;
```

### Error Cases

**Case 1: Cannot Release Committed Reservation**
Try to release committed reservation 5421:

**Expected Response (400)**:
```json
{
  "succeeded": false,
  "error": "Cannot release a committed reservation. Reserved items have already been deducted."
}
```

**Case 2: Reservation Not Found**
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reservation/99999/release" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Expected Response (400)**:
```json
{
  "succeeded": false,
  "error": "Reservation not found"
}
```

### Notes
- Reversible operation (unlike commit)
- Items returned to available pool
- Used for cancellations

---

## Test 14: GET /api/inventory/movements

**Purpose**: Retrieve movement history (audit trail)  
**Priority**: P0 - Critical  
**Prerequisites**: At least 5 movements created from previous tests

### Request 1: All Movements for Item 4521
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/movements?itemId=4521" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
[
  {
    "movementId": 99002,
    "itemId": 4521,
    "itemName": "Titanium Implant Grade-5",
    "stockId": 1001,
    "lotNumber": "TI-2025-001",
    "type": "Out",
    "reason": "SaleDeduction",
    "quantity": 1,
    "unitCost": 450,
    "totalCost": 450,
    "referenceId": "VIS-8724",
    "processedBy": "550e8400-e29b-41d4-a716-446655440000",
    "processedDate": "2026-01-18T19:30:00Z",
    "notes": "Dental implant surgery [Lot: TI-2025-001]"
  },
  {
    "movementId": 99001,
    "itemId": 4521,
    "itemName": "Titanium Implant Grade-5",
    "stockId": 1002,
    "lotNumber": "TI-2025-002",
    "type": "In",
    "reason": "PurchaseReceipt",
    "quantity": 5,
    "unitCost": 449.50,
    "totalCost": 2247.50,
    "referenceId": "PO-2026-00789",
    "processedBy": "550e8400-e29b-41d4-a716-446655440000",
    "processedDate": "2026-01-18T18:15:00Z",
    "notes": "Emergency purchase from Supplier B"
  }
]
```

### Validations
- [ ] HTTP Status: 200
- [ ] Array of movements ordered by date descending
- [ ] Item names included (from Items table)
- [ ] Lot numbers included (from Stocks table)
- [ ] All movement types shown (In, Out)
- [ ] All reasons shown (PurchaseReceipt, SaleDeduction, DamageWriteOff, Adjustment)
- [ ] Only user's branch shown
- [ ] Costs calculated correctly

### Request 2: Movements with Date Range
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/movements?itemId=4521&startDate=2026-01-18&endDate=2026-01-18" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Validations
- [ ] HTTP Status: 200
- [ ] Only movements between dates included
- [ ] Inclusive of start and end dates

### Request 3: All Movements (No Filter)
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/movements" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Validations
- [ ] HTTP Status: 200
- [ ] All movements for branch returned
- [ ] Multiple items shown
- [ ] Ordered by date descending

### Use Case Validation
**Product Recall Scenario**: Find all patients who got batch TI-2025-001
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/movements?itemId=4521&startDate=2026-01-01&endDate=2026-12-31" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

Response should show which batches were dispensed and when (via referenceId linking to visits).

### Notes
- Complete IFRS audit trail
- Used for product recalls
- Financial reconciliation source

---

## Test 15: GET /api/inventory/expiring

**Purpose**: Monitor expiring stock  
**Priority**: P0 - Critical  
**Prerequisites**: Stock with near-future expiry dates

### Request 1: All Expiring in 30 Days (Default)
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/expiring" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
[
  {
    "stockId": 1987,
    "itemId": 2015,
    "itemName": "Antibiotic Capsule",
    "lotNumber": "AB-2025-045",
    "quantity": 115,
    "expiryDate": "2026-06-30T00:00:00Z",
    "daysUntilExpiry": 163,
    "urgency": "Warning",
    "totalValue": 230,
    "unitCost": 2
  }
]
```

### Validations
- [ ] HTTP Status: 200
- [ ] Array ordered by expiry date (soonest first)
- [ ] Only active, non-deleted stock
- [ ] Only remaining quantity > 0
- [ ] Only user's branch
- [ ] Urgency levels correct:
  - Critical: < 7 days
  - High: 8-14 days
  - Warning: 15+ days
- [ ] daysUntilExpiry calculated correctly
- [ ] totalValue = quantity × unitCost

### Request 2: Expiring in 7 Days (Critical Only)
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/expiring?daysUntilExpiry=7" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Validations
- [ ] HTTP Status: 200
- [ ] Only items expiring within 7 days shown
- [ ] All have urgency = "Critical"

### Request 3: Expiring in 90 Days
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/expiring?daysUntilExpiry=90" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Validations
- [ ] More items returned than 7-day query
- [ ] Includes Warning urgency items

### Notes
- Patient safety critical
- Staff alert mechanism
- Regulatory compliance

---

## Test 16: GET /api/inventory/valuation

**Purpose**: Generate stock valuation for financial statements  
**Priority**: P0 - Critical  
**Prerequisites**: Multiple items with multiple batches

### Request 1: Current Valuation
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/valuation" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Expected Response (200 OK)
```json
{
  "asOfDate": "2026-01-18T22:50:00Z",
  "valuationMethod": "FIFO",
  "branchId": 1,
  "items": [
    {
      "itemId": 4521,
      "itemName": "Titanium Implant Grade-5",
      "totalQuantity": 12,
      "totalValue": 5386,
      "weightedAverageCost": 448.83,
      "batches": [
        {
          "lotNumber": "TI-2025-001",
          "quantity": 6,
          "unitCost": 450,
          "batchValue": 2700
        },
        {
          "lotNumber": "TI-2025-002",
          "quantity": 6,
          "unitCost": 448,
          "batchValue": 2688
        }
      ]
    },
    {
      "itemId": 2015,
      "itemName": "Antibiotic Capsule",
      "totalQuantity": 115,
      "totalValue": 230,
      "weightedAverageCost": 2,
      "batches": [
        {
          "lotNumber": "AB-2025-045",
          "quantity": 115,
          "unitCost": 2,
          "batchValue": 230
        }
      ]
    },
    {
      "itemId": 102,
      "itemName": "Sterile Gloves",
      "totalQuantity": 48,
      "totalValue": 144,
      "weightedAverageCost": 3,
      "batches": [
        {
          "lotNumber": "GL-2025-100",
          "quantity": 48,
          "unitCost": 3,
          "batchValue": 144
        }
      ]
    }
  ],
  "totalInventoryValue": 5760,
  "currencyCode": "PKR"
}
```

### Validations
- [ ] HTTP Status: 200
- [ ] asOfDate = current date/time (or specified date)
- [ ] valuationMethod = "FIFO"
- [ ] branchId = 1
- [ ] All active items included
- [ ] Batches ordered by purchase date (FIFO)
- [ ] Weighted average cost calculated correctly
- [ ] Batch values: quantity × unitCost
- [ ] Item total value: sum of batch values
- [ ] Grand total: sum of all item values
- [ ] Only items with remaining quantity > 0
- [ ] Only non-deleted, non-expired items

### Request 2: Historical Valuation
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/valuation?asOfDate=2026-01-01" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json"
```

### Validations
- [ ] HTTP Status: 200
- [ ] asOfDate = 2026-01-01 (specified date)
- [ ] Values based on stock at that date

### Database Checks
```sql
-- Verify FIFO ordering in batches
SELECT * FROM Stocks 
WHERE itemId = 4521 AND QuantityRemaining > 0 AND NOT IsExpired AND NOT IsDeleted
ORDER BY purchaseDate ASC;
-- Batches should be in this order in response

-- Verify total inventory value
SELECT SUM(QuantityRemaining * purchasePrice) AS TotalValue FROM Stocks
WHERE QuantityRemaining > 0 AND NOT IsExpired AND NOT IsDeleted;
```

### Use Cases
- Balance sheet: inventory asset valuation
- COGS calculation
- IFRS 2 compliance
- Period-end financial statements

### Notes
- FIFO method required for healthcare
- Batch-level breakdown for auditors
- Historical data support

---

# Test Execution Checklist

## Pre-Execution
- [ ] JWT token obtained and valid
- [ ] Test database populated with test data
- [ ] All prerequisite items and stocks exist
- [ ] Postman or curl available
- [ ] Database access for verification queries

## Test Sequence
- [ ] Test 1: GET Stock Level
- [ ] Test 2: POST Register In-Movement
- [ ] Test 3: POST Register Out-Movement (Damage)
- [ ] Test 4: POST Stock Take
- [ ] Test 5: POST Reserve Items (Success)
- [ ] Test 6: POST Reserve Items (Idempotency)
- [ ] Test 7: POST Reserve Items (Insufficient Stock)
- [ ] Test 8: GET Reservation Details
- [ ] Test 9: GET List Active Reservations
- [ ] Test 10: GET Available Kits
- [ ] Test 11: POST Consume Kit
- [ ] Test 12: POST Commit Reservation
- [ ] Test 13: POST Release Reservation
- [ ] Test 14: GET Movement History
- [ ] Test 15: GET Expiring Stock
- [ ] Test 16: GET Valuation Report

## Post-Execution
- [ ] All tests passed
- [ ] Database integrity verified
- [ ] No orphaned records
- [ ] Audit trail complete
- [ ] Performance acceptable (< 2 seconds per request)
- [ ] Test report generated

---

# Known Limitations & Workarounds

| Issue | Workaround | Status |
|-------|-----------|--------|
| JWT token expiry during long test runs | Refresh token before each test | Known |
| Reservation TTL too short | Increase ttlSeconds parameter | By Design |
| Cannot delete test data | Use new test IDs for each run | Acceptable |
| Database state persistence | Clean up after tests or use new IDs | Recommended |

---

# Support & Troubleshooting

**401 Unauthorized**: Token invalid or expired. Obtain new token from auth service.

**400 Bad Request**: Check request payload matches DTOs exactly. Verify parameter types (int, decimal, string).

**404 Not Found**: Entity doesn't exist. Verify IDs match test data.

**500 Internal Error**: Check logs. May indicate database connectivity or business logic issue.

---

**Last Updated**: 2026-01-18  
**Testing Duration**: ~45 minutes  
**Total Test Cases**: 16 (with error cases)  
**Status**: Ready for QA
