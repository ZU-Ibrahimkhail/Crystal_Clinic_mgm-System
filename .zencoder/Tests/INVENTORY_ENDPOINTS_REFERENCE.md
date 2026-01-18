# Crystal Clinic Inventory System - Complete API Reference

**Last Updated**: 2026-01-18  
**API Version**: 1.0  
**Base URL**: `https://api.crystalclinic.local/api/inventory`  
**Authentication**: Bearer Token (Required for all endpoints)

---

## Quick Summary

**Total Endpoints**: 13  
**Categories**: Stock Queries (1) | Movements (2) | Reservations (4) | Kits (2) | Visibility & Compliance (4)

| Category | Count | Endpoints |
|----------|-------|-----------|
| **Stock Queries** | 1 | Get Stock Level |
| **Movement Management** | 2 | Register Movement, Perform Stock Take |
| **Reservation Management** | 4 | Reserve Items, Commit, Release, Get Details |
| **Reservation Visibility** | 2 | Get Details, List Active |
| **Kit Operations** | 2 | Get Kits, Consume Kit |
| **Compliance & Reporting** | 4 | Movement History, Expiring Stock, Valuation, Active Reservations |

---

## Detailed Endpoint Documentation

### 1. GET Stock Level
**Category**: Stock Queries | **Priority**: Essential  
**Endpoint**: `GET /api/inventory/stock/{itemId}`  
**Authentication**: ✅ Required

**Purpose**: Get current stock levels and batch details for a specific item

**Parameters**:
- `itemId` (path, required): The item ID to query

**Query Parameters**: None

**Request Example**:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/stock/4521" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
{
  "itemId": 4521,
  "itemName": "Titanium Implant Grade-5",
  "totalQuantity": 15,
  "usableQuantity": 12,
  "reservedQuantity": 2,
  "availableQuantity": 10,
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
      "quantityRemaining": 4,
      "expiryDate": "2027-06-15T00:00:00Z",
      "isExpired": false,
      "unitCost": 448
    }
  ]
}
```

**Use Cases**:
- Check availability before reservation
- Display stock levels to clinicians
- Verify FIFO batch order
- Monitor reserved vs available quantities

**Branch Scoping**: ✅ Auto-filtered to user's branch  
**Business Rules**:
- Excludes expired batches from available quantity
- Shows reservations for items already allocated
- Ordered by purchase date (FIFO)

---

### 2. POST Register Movement
**Category**: Movement Management | **Priority**: Essential  
**Endpoint**: `POST /api/inventory/movement`  
**Authentication**: ✅ Required

**Purpose**: Record a manual stock movement (adjustment in/out)

**Request Body**:
```json
{
  "itemId": 4521,
  "stockId": null,
  "quantity": 5,
  "type": "In",
  "reason": "PurchaseReceipt",
  "referenceId": "PO-2026-00234",
  "notes": "Received from Supplier A"
}
```

**Parameters**:
- `itemId` (required): Item ID
- `stockId` (optional): Specific batch; if null, uses FIFO
- `quantity` (required): Quantity to move (must be positive)
- `type` (required): `In` or `Out`
- `reason` (required): `PurchaseReceipt`, `SaleDeduction`, `Adjustment`, `DamageWriteOff`, `Expired`
- `referenceId` (optional): PO number, invoice, etc.
- `notes` (optional): Additional details

**Request Example**:
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/movement" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "itemId": 4521,
    "quantity": 5,
    "type": "In",
    "reason": "PurchaseReceipt",
    "referenceId": "PO-2026-00234"
  }'
```

**Response (200 OK)**:
```json
{
  "success": true,
  "newBalance": 12,
  "movementId": 98421,
  "unitCost": 449.50,
  "totalCost": 2247.50,
  "quantity": 5
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "errorMessage": "Insufficient stock available"
}
```

**Use Cases**:
- Purchase receipt of new stock
- Manual stock adjustments
- Record damage/wastage
- Stock transfers between locations

**Branch Scoping**: ✅ Auto-set to user's branch  
**Business Rules**:
- Creates multi-lot records for `Out` movements (FIFO)
- Generates separate StockMovement per batch used
- Events published to Accounting system
- Validates item exists and is active
- Validates sufficient stock for `Out` movements

---

### 3. POST Perform Stock Take
**Category**: Movement Management | **Priority**: Essential  
**Endpoint**: `POST /api/inventory/stocktake`  
**Authentication**: ✅ Required

**Purpose**: Record physical inventory count adjustment for a specific batch

**Request Body**:
```json
{
  "stockId": 1001,
  "actualQuantity": 7,
  "reason": "Physical Count - Monthly Audit",
  "notes": "Discrepancy of 1 unit found"
}
```

**Parameters**:
- `stockId` (required): Specific batch ID to adjust
- `actualQuantity` (required): Actual quantity counted
- `reason` (required): Reason for adjustment
- `notes` (optional): Additional details

**Request Example**:
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/stocktake" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "stockId": 1001,
    "actualQuantity": 7,
    "reason": "Physical Count - Monthly Audit"
  }'
```

**Response (200 OK)**:
```json
{
  "succeeded": true
}
```

**Response (400 Bad Request)**:
```json
{
  "succeeded": false,
  "error": "Stock batch not found"
}
```

**Use Cases**:
- Monthly physical inventory counts
- Record variance adjustments
- Correct counting errors
- Audit adjustments

**Branch Scoping**: ✅ User's branch (via batch lookup)  
**Business Rules**:
- Calculates difference: actualQuantity - currentQuantity
- Creates In or Out movement based on difference
- Uses `MovementType.Adjustment`
- Creates audit trail via StockMovement

---

### 4. POST Reserve Items
**Category**: Reservation Management | **Priority**: Critical  
**Endpoint**: `POST /api/inventory/reserve`  
**Authentication**: ✅ Required

**Purpose**: Reserve items for a patient visit/service (holds inventory for future use)

**Request Body**:
```json
{
  "visitId": 8724,
  "serviceId": 501,
  "items": [
    { "itemId": 4521, "quantity": 1 },
    { "itemId": 2015, "quantity": 5 }
  ],
  "idempotencyToken": "VIS-8724-20260118-A7F2",
  "ttlSeconds": 3600
}
```

**Parameters**:
- `visitId` (required): Visit ID
- `serviceId` (required): Service ID
- `items` (required): Array of items to reserve
  - `itemId`: Item ID
  - `quantity`: Quantity to reserve
- `idempotencyToken` (required): Unique token for this reservation (prevents duplicates on retry)
- `ttlSeconds` (optional, default 300): Reservation expiry in seconds

**Request Example**:
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reserve" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "visitId": 8724,
    "serviceId": 501,
    "items": [
      { "itemId": 4521, "quantity": 1 }
    ],
    "idempotencyToken": "VIS-8724-20260118-A7F2",
    "ttlSeconds": 3600
  }'
```

**Response (200 OK)**:
```json
{
  "success": true,
  "reservationId": 5421,
  "expiresAt": "2026-01-18T09:00:00Z",
  "reservedLots": [
    {
      "stockId": 1001,
      "itemId": 4521,
      "reservedQuantity": 1,
      "unitCost": 450
    }
  ]
}
```

**Response (400 Bad Request - Insufficient Stock)**:
```json
{
  "success": false,
  "errorMessage": "Insufficient stock for one or more items",
  "shortages": [
    {
      "itemId": 4521,
      "requestedQuantity": 1,
      "availableQuantity": 0,
      "itemName": "Titanium Implant Grade-5"
    }
  ]
}
```

**Use Cases**:
- Reserve items for upcoming patient treatment
- Lock inventory for specific visit
- Prevent double-allocation
- FIFO allocation from oldest batch first

**Branch Scoping**: ✅ Auto-set to user's branch  
**Business Rules**:
- Idempotent: same token = same result (handles network retries)
- FIFO allocation: uses oldest batch first
- Skips expired batches
- Decrements QuantityRemaining on reserved items
- Expires after TTL (default 5 minutes)
- Atomic: all items reserved or none

---

### 5. GET Reservation Details
**Category**: Reservation Visibility | **Priority**: Critical  
**Endpoint**: `GET /api/inventory/reservation/{reservationId}`  
**Authentication**: ✅ Required

**Purpose**: Get complete details of a reservation including all reserved items

**Parameters**:
- `reservationId` (path, required): Reservation ID

**Query Parameters**: None

**Request Example**:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/reservation/5421" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
{
  "id": 5421,
  "visitId": 8724,
  "serviceId": 501,
  "status": "Active",
  "expiresAt": "2026-01-18T09:00:00Z",
  "requestedBy": "550e8400-e29b-41d4-a716-446655440000",
  "createdOn": "2026-01-18T08:00:00Z",
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

**Response (404 Not Found)**:
```json
{}
```

**Use Cases**:
- Verify reservation before committing
- Display to user which items are allocated
- Verify expiry time
- Audit trail

**Branch Scoping**: ✅ No cross-branch filtering (but user can only see their branch's items)  
**Business Rules**:
- Returns empty object (Id: 0) if not found
- Shows item names from Items table
- Shows lot numbers from Stocks table
- Includes unit and total costs

---

### 6. GET List Active Reservations
**Category**: Reservation Visibility | **Priority**: Critical  
**Endpoint**: `GET /api/inventory/reservations/active`  
**Authentication**: ✅ Required

**Purpose**: List all active (non-expired, non-committed) reservations for user's branch

**Query Parameters**: None

**Request Example**:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/reservations/active" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
[
  {
    "id": 5421,
    "visitId": 8724,
    "status": "Active",
    "expiresAt": "2026-01-18T09:00:00Z",
    "minutesUntilExpiry": 45,
    "totalReservedItems": 2,
    "totalReservedValue": 460,
    "requestedBy": "550e8400-e29b-41d4-a716-446655440000"
  },
  {
    "id": 5422,
    "visitId": 8725,
    "status": "Active",
    "expiresAt": "2026-01-18T10:30:00Z",
    "minutesUntilExpiry": 105,
    "totalReservedItems": 4,
    "totalReservedValue": 1250,
    "requestedBy": "550e8400-e29b-41d4-a716-446655440001"
  }
]
```

**Use Cases**:
- Dashboard showing current holds
- Alerts for reservations about to expire
- Prevent over-allocation of stock
- Track total reserved value per visit

**Branch Scoping**: ✅ Auto-filtered to user's branch  
**Business Rules**:
- Filters to `Status == Active` only
- Calculates `MinutesUntilExpiry` dynamically
- Sums reserved items and their total value
- Ordered by expiry time (soonest first)

---

### 7. POST Commit Reservation
**Category**: Reservation Management | **Priority**: Critical  
**Endpoint**: `POST /api/inventory/reservation/{reservationId}/commit`  
**Authentication**: ✅ Required

**Purpose**: Finalize a reservation by deducting reserved items from stock

**Parameters**:
- `reservationId` (path, required): Reservation ID to commit

**Request Body**: Empty (no body required)

**Request Example**:
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reservation/5421/commit" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
{
  "succeeded": true
}
```

**Response (400 Bad Request)**:
```json
{
  "succeeded": false,
  "error": "Reservation not found or not active"
}
```

**Use Cases**:
- Finalize items after treatment
- Deduct items from inventory
- Trigger accounting events
- Create audit trail

**Branch Scoping**: ✅ Based on reservation's branch  
**Business Rules**:
- Checks reservation is Active (not already Committed/Released)
- Checks not expired
- Creates StockMovement records per batch
- Uses optimistic locking (RowVersion check)
- Publishes InventoryMovementEvent
- Atomic: all batches moved or none
- Prevents concurrent commits

---

### 8. POST Release Reservation
**Category**: Reservation Management | **Priority**: Critical  
**Endpoint**: `POST /api/inventory/reservation/{reservationId}/release`  
**Authentication**: ✅ Required

**Purpose**: Cancel a reservation without deducting items (returns items to available)

**Parameters**:
- `reservationId` (path, required): Reservation ID to release

**Request Body**: Empty (no body required)

**Request Example**:
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/reservation/5421/release" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
{
  "succeeded": true
}
```

**Response (400 Bad Request)**:
```json
{
  "succeeded": false,
  "error": "Cannot release a committed reservation. Reserved items have already been deducted."
}
```

**Use Cases**:
- Cancel reservation if patient cancels
- Release items for other patients
- Recover reserved items

**Branch Scoping**: ✅ Based on reservation's branch  
**Business Rules**:
- Checks reservation exists
- Prevents release if already Committed
- Returns reserved quantities to Stock.QuantityRemaining
- Updates status to Released
- Removes ReservedItems records

---

### 9. GET Available Kits
**Category**: Kit Operations | **Priority**: Essential  
**Endpoint**: `GET /api/inventory/kits`  
**Authentication**: ✅ Required

**Purpose**: List all available inventory kits for user's branch

**Query Parameters**: None

**Request Example**:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/kits" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
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
      }
    ]
  },
  {
    "id": 8,
    "kitName": "Emergency Dental Kit",
    "description": "Quick response kit for emergencies",
    "isActive": true,
    "branchId": 1,
    "kitLines": [...]
  }
]
```

**Use Cases**:
- Display available kits to clinicians
- Select kit for consumption
- View kit composition

**Branch Scoping**: ✅ Auto-filtered to user's branch  
**Business Rules**:
- Only returns active (IsActive = true) kits
- Includes kit lines with item details
- Excludes deleted kits

---

### 10. POST Consume Kit
**Category**: Kit Operations | **Priority**: Essential  
**Endpoint**: `POST /api/inventory/kits/{kitId}/consume`  
**Authentication**: ✅ Required

**Purpose**: Consume a kit (deduct all kit items atomically)

**Parameters**:
- `kitId` (path, required): Kit ID to consume

**Request Body**:
```json
{
  "quantity": 1,
  "referenceId": "VIS-8724"
}
```

**Parameters**:
- `quantity` (required): Number of kits to consume
- `referenceId` (required): Visit ID or reference

**Request Example**:
```bash
curl -X POST "https://api.crystalclinic.local/api/inventory/kits/7/consume" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "quantity": 1,
    "referenceId": "VIS-8724"
  }'
```

**Response (200 OK)**:
```json
{
  "success": true,
  "itemMovements": [
    {
      "success": true,
      "itemId": 102,
      "quantity": 2,
      "unitCost": 3,
      "totalCost": 6,
      "newBalance": 87
    },
    {
      "success": true,
      "itemId": 103,
      "quantity": 5,
      "unitCost": 0.5,
      "totalCost": 2.5,
      "newBalance": 145
    }
  ],
  "totalCost": 8.5
}
```

**Response (400 Bad Request)**:
```json
{
  "success": false,
  "errorMessage": "Failed to consume Sterile Gloves: Insufficient stock available"
}
```

**Use Cases**:
- Consume surgical kits during procedures
- Atomic multi-item deduction
- FIFO batch allocation per kit item
- Financial cost tracking

**Branch Scoping**: ✅ Kit limited to user's branch  
**Business Rules**:
- Processes atomically (all items or none)
- Each kit item uses FIFO (oldest batch first)
- Creates StockMovement per item per batch
- Publishes InventoryKitConsumedEvent
- Calculates total cost
- Rolls back if any item fails

---

### 11. GET Movement History
**Category**: Compliance & Reporting | **Priority**: Critical  
**Endpoint**: `GET /api/inventory/movements`  
**Authentication**: ✅ Required

**Purpose**: Get complete audit trail of all stock movements

**Query Parameters**:
- `itemId` (optional): Filter by item ID
- `startDate` (optional): Start date (ISO 8601)
- `endDate` (optional): End date (ISO 8601)

**Request Example**:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/movements?itemId=4521&startDate=2026-01-01&endDate=2026-01-31" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
[
  {
    "movementId": 98421,
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
    "processedDate": "2026-01-18T08:30:00Z",
    "notes": "Dental implant surgery [Lot: TI-2025-001]"
  },
  {
    "movementId": 97854,
    "itemId": 4521,
    "itemName": "Titanium Implant Grade-5",
    "stockId": 1000,
    "lotNumber": "TI-2025-000",
    "type": "In",
    "reason": "PurchaseReceipt",
    "quantity": 8,
    "unitCost": 445,
    "totalCost": 3560,
    "referenceId": "PO-2026-00234",
    "processedBy": "550e8400-e29b-41d4-a716-446655440001",
    "processedDate": "2026-01-10T14:20:00Z",
    "notes": "Received from Supplier A [Lot: TI-2025-000]"
  }
]
```

**Use Cases**:
- IFRS compliance: complete audit trail
- Product recall: trace which patients got which batches
- Medication safety: verify batch dispensed to patient
- Financial reconciliation: match COGS entries
- Regulatory compliance: healthcare audits
- Performance analytics: usage patterns

**Branch Scoping**: ✅ Auto-filtered to user's branch  
**Business Rules**:
- Optional item filter
- Optional date range filter (inclusive)
- Ordered by date descending (newest first)
- Includes item names and lot numbers
- Shows all movement types and reasons

---

### 12. GET Expiring Stock
**Category**: Compliance & Reporting | **Priority**: Critical  
**Endpoint**: `GET /api/inventory/expiring`  
**Authentication**: ✅ Required

**Purpose**: Monitor stock expiring within specified days

**Query Parameters**:
- `daysUntilExpiry` (optional, default 30): Days until expiry to check

**Request Example**:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/expiring?daysUntilExpiry=30" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
[
  {
    "stockId": 1987,
    "itemId": 2015,
    "itemName": "Antibiotic Capsule",
    "lotNumber": "AB-2025-045",
    "quantity": 15,
    "expiryDate": "2026-01-25T00:00:00Z",
    "daysUntilExpiry": 7,
    "urgency": "Critical",
    "totalValue": 30,
    "unitCost": 2
  },
  {
    "stockId": 1234,
    "itemId": 4521,
    "itemName": "Titanium Implant",
    "lotNumber": "TI-2025-002",
    "quantity": 4,
    "expiryDate": "2026-02-15T00:00:00Z",
    "daysUntilExpiry": 28,
    "urgency": "Warning",
    "totalValue": 1792,
    "unitCost": 448
  }
]
```

**Use Cases**:
- Patient safety: alert before using expired medications
- Proactive disposal: plan disposal before items expire
- FIFO prioritization: use soon-to-expire items first
- Financial write-offs: calculate losses from expiration
- Regulatory compliance: healthcare inspections
- Wastage prevention: reduce losses from expiration

**Branch Scoping**: ✅ Auto-filtered to user's branch  
**Business Rules**:
- Calculates expiry window: today to today + daysUntilExpiry
- Filters to active, non-deleted, remaining > 0
- Urgency levels:
  - Critical: < 7 days
  - High: 8-14 days
  - Warning: 15+ days
- Ordered by expiry date (soonest first)
- Includes total value calculation

---

### 13. GET Stock Valuation Report
**Category**: Compliance & Reporting | **Priority**: Critical  
**Endpoint**: `GET /api/inventory/valuation`  
**Authentication**: ✅ Required

**Purpose**: Generate FIFO-based stock valuation for financial statements

**Query Parameters**:
- `asOfDate` (optional): Valuation as-of date (ISO 8601, defaults to now)

**Request Example**:
```bash
curl -X GET "https://api.crystalclinic.local/api/inventory/valuation?asOfDate=2026-01-31" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

**Response (200 OK)**:
```json
{
  "asOfDate": "2026-01-31T00:00:00Z",
  "valuationMethod": "FIFO",
  "branchId": 1,
  "items": [
    {
      "itemId": 4521,
      "itemName": "Titanium Implant Grade-5",
      "totalQuantity": 12,
      "totalValue": 5394,
      "weightedAverageCost": 449.5,
      "batches": [
        {
          "lotNumber": "TI-2025-001",
          "quantity": 8,
          "unitCost": 450,
          "batchValue": 3600
        },
        {
          "lotNumber": "TI-2025-002",
          "quantity": 4,
          "unitCost": 448,
          "batchValue": 1792
        }
      ]
    },
    {
      "itemId": 2015,
      "itemName": "Antibiotic Capsule",
      "totalQuantity": 120,
      "totalValue": 240,
      "weightedAverageCost": 2,
      "batches": [
        {
          "lotNumber": "AB-2025-045",
          "quantity": 120,
          "unitCost": 2,
          "batchValue": 240
        }
      ]
    }
  ],
  "totalInventoryValue": 5634,
  "currencyCode": "PKR"
}
```

**Use Cases**:
- Balance sheet: report inventory as current asset
- COGS calculation: cost of goods sold for periods
- IFRS 2 compliance: FIFO valuation method
- Financial reporting: monthly, quarterly, year-end statements
- Audit preparation: detailed batch-level breakdown
- Variance analysis: compare actual vs expected inventory values

**Branch Scoping**: ✅ Auto-filtered to user's branch  
**Business Rules**:
- Values as-of date (defaults to now if not provided)
- Filters to active, non-expired, remaining > 0
- Groups by item ID
- Orders batches by purchase date (FIFO)
- Calculates weighted average cost per item
- Returns batch-level breakdown for detailed reporting
- Totals inventory value across all items

---

## Common Response Patterns

### Success Response
```json
{
  "success": true,
  "data": {}
}
```

### Error Response
```json
{
  "success": false,
  "errorMessage": "Specific error message"
}
```

### Result Pattern
```json
{
  "succeeded": true,
  "error": null
}
```

---

## Error Codes & Meanings

| HTTP Code | Meaning | Example |
|-----------|---------|---------|
| 200 | Success | Item found and returned |
| 400 | Bad Request | Invalid parameters or validation failure |
| 401 | Unauthorized | Missing or invalid token |
| 403 | Forbidden | User not authorized |
| 404 | Not Found | Reservation/item not found |
| 500 | Server Error | Internal error |

---

## Authentication

All endpoints require a Bearer token in the `Authorization` header:

```
Authorization: Bearer YOUR_JWT_TOKEN
```

Token should be obtained from the authentication service.

---

## Rate Limiting

No explicit rate limiting is enforced, but clients should implement reasonable request throttling.

---

## Pagination

Currently, endpoints return all results. Pagination will be added in future versions.

---

## Data Types Reference

### StockLevel
```json
{
  "itemId": 0,
  "itemName": "string",
  "totalQuantity": 0,
  "usableQuantity": 0,
  "reservedQuantity": 0,
  "availableQuantity": 0,
  "batches": [{"StockBatch": "object"}]
}
```

### ReservationResult
```json
{
  "success": true,
  "errorMessage": null,
  "reservationId": 0,
  "expiresAt": "2026-01-18T09:00:00Z",
  "reservedLots": [{"ReservedLot": "object"}],
  "shortages": [{"ShortageItem": "object"}]
}
```

### MovementResult
```json
{
  "success": true,
  "errorMessage": null,
  "newBalance": 0,
  "movementId": 0,
  "unitCost": 0,
  "totalCost": 0,
  "quantity": 0
}
```

---

## Integration Examples

### Patient Treatment Workflow
```bash
# 1. Check stock availability
GET /api/inventory/stock/4521

# 2. Reserve items for visit
POST /api/inventory/reserve
{
  "visitId": 8724,
  "serviceId": 501,
  "items": [{"itemId": 4521, "quantity": 1}],
  "idempotencyToken": "VIS-8724-20260118"
}

# 3. Get reservation details (verify)
GET /api/inventory/reservation/5421

# 4. Consume surgical kit
POST /api/inventory/kits/7/consume
{
  "quantity": 1,
  "referenceId": "VIS-8724"
}

# 5. Commit reservation
POST /api/inventory/reservation/5421/commit

# 6. Check movement history
GET /api/inventory/movements?referenceId=VIS-8724
```

### Monthly Audit & Reporting
```bash
# 1. Get expiring items
GET /api/inventory/expiring?daysUntilExpiry=30

# 2. Get valuation for financial statements
GET /api/inventory/valuation?asOfDate=2026-01-31

# 3. Get movement history for the month
GET /api/inventory/movements?startDate=2026-01-01&endDate=2026-01-31

# 4. Check active reservations
GET /api/inventory/reservations/active
```

---

**Last Updated**: 2026-01-18  
**Status**: Complete ✅  
**Total Endpoints Documented**: 13
