---
description: Master Modernization Roadmap
alwaysApply: true
---

# Master Roadmap

## Phase 0: Foundations (Week 0)
- Finalize shared architectural decisions (event names, REST DTOs, coding standards).
- Spin up automated pipelines for build, contract testing, and UI smoke tests.
- Seed baseline master data (currencies, branches, tax tables) since rollout is greenfield.

## Phase 1: Financial Revamp (Weeks 1-6)
- Deliver chart of accounts, journal entry engine, AR/AP, budgeting, and reporting per `financial_system_plan.md`.
- Publish versioned APIs/events (`JournalEntry`, `AccountsReceivable`, `AccountsPayable`, `FinancialBridge` hooks) that downstream modules will consume.
- Rebuild finance UI components and certify contract tests for Inventory, Sales, Clinic, and HR integrations.

## Phase 2: Inventory + Sales/Procurement (Weeks 7-12)
- Implement inventory schema hardening, IInventoryService, valuation hooks, and event emissions (`Inventory.StockAdjusted`, `Inventory.KitConsumed`, `Inventory.StockExpired`).
- Launch Sales/Procurement flows (estimate→invoice, PO→bill, expense capture) that reuse financial schemas and raise `Sales.EstimateConverted`, `Sales.InvoicePaid`, `Procurement.POReceived`, and `Expense.Recorded`.
- Run end-to-end contract suites between FinancialBridge, IValuationService, and Clinic reservers; update UI components for stock, PO, quote, invoice, and expense management.

## Phase 3: Clinic Enhancements (Weeks 13-18)
- Apply clinical data hardening, ProcedureLog, lab templates, and hybrid workflow APIs per `clinic_system_enhancement_plan.md`.
- Integrate tightly with Inventory/Financial modules through `Clinic.ServiceInventoryRequest`, `Clinic.VisitCompleted`, and `Clinic.ProcedureLogged` events.
- Refresh visit intake, clinical note, and discharge UIs; execute full workflow tests (intake → treatment → discharge → billing) plus accessibility sweeps.

## Phase 4: HR Enhancements (Weeks 19-24)
- Implement payroll contracts, attendance, adjustments, and HR task automation as described in `hr_system_enhancement_plan.md`.
- Consume financial fixed-asset data while emitting `HR.PayrollGenerated`, `HR.PayrollPaid`, `HR.LeaveStatusChanged`, and `HR.AssetAssignmentChanged` events.
- Rebuild HR portals (employee profile, leave, payroll, asset assignment) and run contract tests with Clinic scheduling and Financial payroll posting flows.

## Cross-Phase Testing & Governance
- Maintain a shared contract test suite verifying every published event/REST DTO before promotion.
- Run weekly UI/UX regression packs (Cypress/Playwright) that stitch together Finance → Inventory/Sales → Clinic → HR journeys.
- Hold integration readiness reviews before each phase moves to the next, ensuring upstream APIs are stable and downstream test environments are unblocked.
