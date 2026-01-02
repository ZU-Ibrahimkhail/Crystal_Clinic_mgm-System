---
description: Inventory System Enhancement Plan
alwaysApply: true
---

# Inventory System Enhancement Plan

## Executive Summary

This plan transforms the current financial asset management system into a comprehensive inventory management platform suitable for clinic operations. It adds physical inventory tracking for medications, medical supplies, and equipment while maintaining financial integration for cost accounting.

Legacy AssetMS functionality (asset depreciation, valuation) now resides inside the financial module; this inventory plan focuses on consumables, kits, and operational deductions, emitting events back to finance for valuation.

## Current State Assessment (Dec 2025)

The current inventory system is a **basic functional foundation** (readiness: ~23%) that allows for simple quantity tracking but lacks the auditability and financial depth required for a professional medical facility.

---

## Technical Implementation Details

### 1. Database Schema Changes (Entity Level)

#### Table: `Item` (Existing)
| Field Name | Change | Purpose |
| :--- | :--- | :--- |
| `ItemCode` | Add (string) | Unique identifier for SKU/Stock management. |
| `Barcode` | Add (string) | Supports scanner integration for faster operations. |
| `RequiresExpiration` | Add (bool) | Toggles mandatory expiry date logic for medications. |
| `UnitCost` | Add (decimal) | Stores the **Moving Average Cost (MAC)** for financial valuation. |
| `IsActive` | Add (bool) | Soft-disable items no longer used. |

#### Table: `Stock` (Existing)
| Field Name | Change | Purpose |
| :--- | :--- | :--- |
| `LotNumber` | Add (string) | Mandatory for medical batch tracking and recalls. |
| `ManufactureDate` | Add (DateTime) | Essential for shelf-life analysis. |
| `IsExpired` | Add (bool) | Indexed flag for fast filtering of unusable stock. |
| `QuantityRemaining` | Add (decimal) | Tracks usable stock separately from total quantity. |

#### Table: `StockMovement` (Modified)
| Field Name | Change | Purpose |
| :--- | :--- | :--- |
| `StockId` | Add (int FK) | Links the movement to a specific batch/lot. |
| `ProcessedBy` | Add (Guid FK) | Links movement to the user who performed it. |
| `UnitCost` | Add (decimal) | Records the value of the item at the exact moment of movement. |
| `TotalCost` | Add (decimal) | UnitCost * Quantity (Audit record). |

#### Table: `InventoryKit`
*Purpose: A template for a group of items used together (e.g., "Surgery Kit").*
- `Id` (int PK)
- `KitName` (string)
- `IsFreeForPatient` (bool) -> If true, the cost is internal expense.

#### Table: `InventoryKitLine`
- `Id` (int PK)
- `KitId` (int FK)
- `ItemId` (int FK)
- `Quantity` (decimal)

---

### 2. Service Layer Specifications

#### `IInventoryService`
The central brain for all stock changes.
- **`RegisterMovementAsync(MovementRequest request)`**
  - **Input**: `itemId`, `quantity`, `type` (In/Out/Adjust), `reason`, `sourceBatchId` (optional).
  - **Output**: `MovementResult` (Success/Fail, NewBalance).
  - **Purpose**: Replaces all direct quantity updates. Atomically updates `Stock.quantity` and inserts into `StockMovement`.

#### Integration Contracts & Events
- **Event Names**: `Inventory.StockAdjusted`, `Inventory.KitConsumed`, `Inventory.StockExpired`, each carrying `itemId`, `stockId`, `quantity`, `unitCost`, and `valuationMethod` to feed the financial module’s `FinancialBridge`.
- **Financial Hooks**: Every movement publishes to `IValuationService` for moving-average recalculation and to the Sales/Procurement module for PO receipt reconciliation.
- **Clinic Hooks**: `ServiceInventoryLink` consumers raise `Inventory.KitConsumed` so Clinic discharges can reconcile with actual stock deductions.

#### Reservation & Idempotency Contract
- **`Clinic.ServiceInventoryRequest` (Sync API)**
  - **Payload**: `{ "visitId": 501, "serviceId": 12, "items": [{ "itemId": 88, "quantity": 2 }], "idempotencyToken": "VIS-501-SVC-12", "ttlSeconds": 120 }`
  - **Semantics**: Inventory reserves batches for `ttlSeconds` and returns `{ status: "Reserved", reservationId, expiresAt, reservedLots[] }`. If the same `idempotencyToken` replays, the existing reservation is returned so retries never double-deduct.
  - **Completion**: Clinic calls `POST /api/Inventory/Reservation/{reservationId}/Commit` (or `.../Release`) once the procedure succeeds/fails. Background sweeper auto-releases expired reservations to avoid leakage.
  - **Failure Handling**: Network or downstream failures force Clinic to retry with the same token; Inventory responds deterministically. When Inventory cannot honor a request, it returns `409` with shortage details so Clinic can prompt substitution before proceeding.

---

### 3. API Payload & Response Definitions

#### `POST /api/Inventory/StockTake` (Manual Adjustment)
- **Payload**:
```json
{
  "stockId": 105,
  "actualQuantity": 45,
  "reason": "Damage",
  "notes": "Found 5 broken vials during audit"
}
```

---

### 4. Step-by-Step Implementation Roadmap

1.  **Phase 1: Database Hardening**: Execute SQL migrations to add fields to `Item`, `Stock`, and `StockMovement`.
2.  **Phase 2: Service Migration**: Implement `IInventoryService`. Refactor `VisitCRUD.cs` to call `IInventoryService` instead of modifying `context.Stocks` directly.
3.  **Phase 3: Valuation Logic**: Implement `IValuationService` and hook it into the `Purchase` API.
4.  **Phase 4: Financial Events**: Create `MedicationDispensed` domain event. Implement `FinancialBridge` listener to generate Journal Entries (Debit COGS, Credit Inventory).
5.  **Phase 5: Background Jobs**: Implement `ExpiryMonitoringJob` using Hangfire or BackgroundService to flag expired stock daily.

---

## 5. Inventory System ERD (Enhanced)

```mermaid
erDiagram
    BRANCH ||--o{ ITEM : "manages"
    BRANCH ||--o{ STOCK : "stores"
    ITEM_CATEGORY ||--o{ ITEM : "categorizes"
    ITEM ||--o{ STOCK : "has batches"
    SUPPLIER ||--o{ STOCK : "supplies"
    ITEM ||--o{ STOCK_MOVEMENT : "tracks"
    STOCK ||--o{ STOCK_MOVEMENT : "logs"
    INVENTORY_TRANSACTION ||--o{ STOCK_MOVEMENT : "groups"
    
    ITEM {
        int ItemId PK
        string Name
        string Description
        string BaseUnit
        decimal CurrentStock
        decimal UseableStock
        decimal ReorderLevel
        string ImagePath
        int BranchId FK
        int CategoryId FK
        string ItemCode
        decimal MaximumStock
        bool IsActive
        bool RequiresExpiration
        string Barcode
        decimal UnitCost
    }

    ITEM_CATEGORY {
        int categoryId PK
        string Name
        string Description
        string CategoryCode
        int ParentCategoryId FK
        bool IsActive
    }

    STOCK {
        int stockId PK
        int itemId FK
        int quantity
        int BranchId FK
        int SupplierId FK
        decimal purchasePrice
        decimal sellPrice
        DateTime purchaseDate
        string batchNumber
        string barCode
        DateTime expiryDate
        string LotNumber
        DateTime ManufactureDate
        decimal QuantityRemaining
        bool IsExpired
    }

    STOCK_MOVEMENT {
        int StockMovementId PK
        DateTime Date
        enum MovementType
        enum MovementReason
        int ItemId FK
        int StockId FK
        int SourceBranchId FK
        decimal Quantity
        decimal UnitCost
        decimal TotalCost
        int ProcessedBy FK
        string ReferenceId
        string Notes
    }

    SUPPLIER {
        int Id PK
        string Name
        string ContactPerson
        string ContactInfo
        string Email
        string Address
        string SupplierCode
        int PaymentTerms
        bool IsActive
    }

    INVENTORY_TRANSACTION {
        int Id PK
        string TransactionNumber
        DateTime TransactionDate
        enum TransactionType
        string Reference
        int BranchId FK
        int ProcessedBy FK
    }
```

## UI/UX & Deployment Strategy
- Rebuild stock, kit, and stock-take screens where necessary; new UI components can replace legacy grids outright.
- Coordinate form changes with Clinic and Sales teams so shared `ServiceInventoryLink` selectors remain consistent.
- No legacy data migration is required—only seed master data (items, categories, suppliers) and rely on automated smoke data for demos.

## Testing Strategy
### Unit Tests
- Movement calculation (quantity, cost) and kit explosion logic
- Expiry flag transitions and background job schedulers
- Validation of `MovementRequest` inputs and branch isolation rules

### Integration & Contract Tests
- Event payload validation for `Inventory.StockAdjusted`, `Inventory.KitConsumed`, `Inventory.StockExpired`
- FinancialBridge/IValuationService contract tests ensuring unit cost updates reconcile with the financial ledger
- Clinic flow tests confirming `ServiceInventoryLink` consumption deducts matching batches

### UI/UX Validation
- Cypress/Playwright flows for stock intake, kit issuance, and stock-take adjustments
- Accessibility and localization checks for new forms
- Usability dry-runs with pharmacists and storekeepers to validate redesigned layouts
