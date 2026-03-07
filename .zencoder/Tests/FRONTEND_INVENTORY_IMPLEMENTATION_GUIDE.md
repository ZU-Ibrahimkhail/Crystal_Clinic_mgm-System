# Frontend Inventory Implementation Guide

This guide outlines the unique logic and architectural patterns required for the Crystal Clinic Inventory Management System frontend development.

## 1. Idempotency Logic
The `POST /api/inventory/reserve` endpoint requires an `idempotencyToken`. 
- **Pattern**: Generate a unique string for each reservation attempt.
- **Example**: `VIS-{id}-{timestamp}` or a UUID.
- **Purpose**: Prevents double-booking items during network retries or accidental double-clicks.

## 2. Two-Phase Transaction Workflow
The inventory system follows a "Reserve-then-Commit" or "Reserve-then-Release" pattern:
- **Phase 1: Reserve**: Holds stock temporarily (TTL based). Items are marked as reserved but not yet deducted from the physical count.
- **Phase 2a: Commit**: Permanently deducts items once the procedure/visit is finalized.
- **Phase 2b: Release**: Explicitly cancels the hold if the procedure is cancelled or the items are no longer needed.

## 3. Reservation TTL (Time-To-Live)
All reservations have an `expiresAt` timestamp.
- **UI Requirement**: The frontend should track this timestamp.
- **Best Practice**: Show a countdown timer to the user and disable the "Commit" button once the reservation expires.

## 4. Atomic Kit Consumption
The `/api/inventory/kits/{kitId}/consume` endpoint is atomic.
- **Behavior**: It either deducts ALL items in the kit or fails entirely if a single item is short.
- **Error Handling**: On failure, the API returns a `400 Bad Request` with specific shortage details. The UI should highlight exactly which item caused the shortage.

## 5. Expiry Urgency Levels
The `/api/inventory/expiring` endpoint returns an `urgency` enum. Use the following styling conventions:
- **Critical** (< 7 days): Style with **Red / Blinking** alerts.
- **High** (8-14 days): Style with **Orange** warnings.
- **Warning** (15+ days): Style with **Yellow** indicators.

## 6. FIFO Batching (Lot Management)
Stock is retrieved at the batch level (`stockId`).
- **UI Requirement**: Display multiple "Batches" (Lots) for a single Item ID when necessary.
- **Sorting**: The backend handles FIFO, but the UI should clearly show lot numbers and expiry dates per batch.

## 7. Status Mapping
| Status | UI Action/Representation |
|--------|-------------------------|
| Active | Normal operations allowed |
| Reserved | Locked for a specific visit; show as "Pending" |
| Committed | Deducted; move to history |
| Released | Hold removed; return to available pool |
| Expired | Move to "Waste/Disposal" workflow |
