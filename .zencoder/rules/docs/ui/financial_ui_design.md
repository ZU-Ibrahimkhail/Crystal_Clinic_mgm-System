# Financial UI Design Reference

This guide visualizes the core financial pages, forms, and dialog boxes that surface the GL/AR/AP capabilities described in `financial_system_plan.md`. Each artifact includes its purpose, the actions triggered by primary buttons, and the exact APIs/events invoked so downstream teams can wire their clients consistently. Text wireframes illustrate approximate layouts for alignment discussions.

---

## 1. Journal Entry Workbench (Page)

**Purpose**: Create, edit, validate, and post multi-line journal entries with real-time debits/credits balancing.

**Primary Actions & APIs**

- `Save Draft` → `POST /api/Finance/JournalEntry` (new) or `PUT /api/Finance/JournalEntry/{id}` (update); keeps `status=Draft`.
- `Validate` → `POST /api/Finance/JournalEntry/{id}/Post?dryRun=true`; same endpoint as posting, but the backend short-circuits before persisting and simply returns validation errors plus balancing hints.
- `Post Entry` → `POST /api/Finance/JournalEntry/{id}/Post`; if validation fails, it returns the structured error payload instead of posting, so callers never need a separate validation API. Successful calls emit `Finance.JournalEntryPosted`.
- `Attach Document` → `POST /api/Finance/JournalEntry/{id}/Attachment`.

**Supporting Calls**

- `GET /api/Finance/ChartOfAccounts?branchId=...` for account picker.
- `GET /api/Core/Branches`, `GET /api/Core/Currencies` for dropdowns.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Header: Journal Entry #JE-2026-014      Status: Draft          [Post Entry]      |
+----------------------------------------------------------------------------------+
| Entry Metadata                                                                |
| Date [____]  Branch [Clinic HQ▼]  Currency [AFN▼]  Reference [___________]     |
| Description [______________________________________________________________]  |
+----------------------------------------------------------------------------------+
| Line Items (Grid)                                                              |
| ------------------------------------------------------------------------------ |
| | # | Account        | Debit      | Credit     | Description        | Actions | |
| | 1 | 1101 Cash      | 12,500.00  |            | Deposit            |  [x]    | |
| | 2 | 3101 Capital   |            | 12,500.00  | Owner funding      |  [x]    | |
| ------------------------------------------------------------------------------ |
| [Add Line]  Total Debit: 12,500.00   Total Credit: 12,500.00  Difference: 0.00  |
+----------------------------------------------------------------------------------+
| Footer Buttons:  [Attach Document]  [Save Draft]  [Validate]  [Post Entry]      |
+----------------------------------------------------------------------------------+
```

---

## 2. Accounts Receivable Invoice Screen (Page + Form)

**Purpose**: Issue AR invoices, preview patient balances, send to FinancialBridge.

**Primary Actions & APIs**

- `Fetch Patient` → `GET /api/Clinic/Patient/{id}` for demographics and price levels.
- `Load Visit Charges` → `GET /api/Clinic/Visit/{visitId}/Charges` to prefill performed services and medications.
- `Calculate Tax` → `POST /api/Sales/Invoice/Calculate` with draft totals.
- `Save Invoice` (Draft) → `POST /api/Finance/AccountsReceivable`.
- `Issue & Post` → `POST /api/Finance/AccountsReceivable/{id}/Issue`; triggers `Sales.EstimateConverted` (if estimate source) and `Finance.ARPosted` events.
- `Send to Patient` → `POST /api/Notifications/Invoice/{id}/Send`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| AR Invoice INV-2026-088                          Status: Draft    [Issue & Post] |
+----------------------------------------------------------------------------------+
| Patient Lookup: [Search patient or scan QR]      Visit Ref [_____][Load Charges] |
| Patient Summary Card: Allergies, Risk Step, Open Balance                           |
+----------------------------------------------------------------------------------+
| Line Items (Services & Medications)                                                  |
| ------------------------------------------------------------------------------     |
| | Item/Service       | Qty | Unit Price | Tax | Line Total | Source | Actions |    |
| | Hair Transplant    | 1   | 8,000.00   |15% | 9,200.00   | Clinic |  [x]    |    |
| | Laser Gel (Item44) | 2   |   250.00   |15% |   575.00   | InvRes |  [x]    |    |
| ------------------------------------------------------------------------------     |
| [Add Service] [Add Item]                                                            |
+----------------------------------------------------------------------------------+
| Totals Panel: Subtotal, Tax, Discounts, Net Amount, AR Balance, Payment Terms       |
+----------------------------------------------------------------------------------+
| Footer Buttons: [Save Draft]  [Send to Patient]  [Issue & Post]                     |
+----------------------------------------------------------------------------------+
```

---

## 3. Accounts Payable Bill Dialog (Modal)

**Purpose**: Quick-capture vendor bills from Purchase Orders or manual entries inside any workflow (Inventory receipt, Finance workbench, etc.).

**Primary Actions & APIs**

- `Open Dialog` → invoked from PO view (`GET /api/Procurement/PurchaseOrder/{id}` for context).
- `Prefill from PO` → `GET /api/Procurement/PurchaseOrder/{id}/Receipts`.
- `Save Draft Bill` → `POST /api/Finance/AccountsPayable`.
- `Approve & Post` → `POST /api/Finance/AccountsPayable/{id}/Approve` (writes Journal Entry, emits `Procurement.POReceived`).
- `Upload Document` → `POST /api/Finance/AccountsPayable/{id}/Attachment`.

**Text Wireframe (Modal)**

```
+-------------------------------- Accounts Payable Bill -----------------------------+
| Vendor [MedicSuppliers▼]           Bill No [__________]  Date [__/__/2026]        |
| Source PO [PO-2026-144▼] (optional)                                                |
|-----------------------------------------------------------------------------       |
| | Item/Charge         | Qty | Unit Cost | Line Total | GL Account  | Actions |    |
| | Paracetamol Batch   | 500 |    12.00  | 6,000.00   | 1103 Inv    |  [x]    |    |
|-----------------------------------------------------------------------------       |
| Tax Code [VAT 15%▼]   Shipping [____]   Other Fees [____]                          |
| Notes [______________________________________________________________]            |
| Attachments: [Upload PO PDF]                                                      |
|-----------------------------------------------------------------------------------|
| [Cancel] [Save Draft] [Approve & Post]                                            |
+-----------------------------------------------------------------------------------+
```

---

## 4. Financial Reporting Dashboard (Page)

**Purpose**: Provide at-a-glance KPIs (Trial Balance deltas, AR aging, AP aging, cash flow) with drill-through to underlying data.

**Primary Actions & APIs**

- `Load KPIs` → `GET /api/Finance/Reporting/Dashboard?asOf=...` (aggregates GL + AR + AP).
- `Drill to Trial Balance` → `GET /api/Finance/Reporting/TrialBalance?asOf=...`.
- `AR Aging Widget` → `GET /api/Finance/AccountsReceivable/Aging`.
- `AP Aging Widget` → `GET /api/Finance/AccountsPayable/Aging`.
- `Export` buttons → `POST /api/Finance/Reporting/Export` with widget context.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Finance Dashboard (As of 2026-02-10)      [Date Picker▼]   [Export All]         |
+----------------------------------------------------------------------------------+
| Row 1: [Total Cash Card]  [AR Aging Card]  [AP Aging Card]  [Net Income Card]    |
+----------------------------------------------------------------------------------+
| Row 2: Trial Balance Waterfall (chart)                                         |
| -----------------------------------------------------------------------------  |
| |           | Assets | Liabilities | Equity |                                  |
| -----------------------------------------------------------------------------  |
+----------------------------------------------------------------------------------+
| Row 3 Split:                                                                    |
| Left Pane: Cash Flow Trend (line chart)                                         |
| Right Pane: Alerts Feed (failed postings, overdue approvals)                    |
+----------------------------------------------------------------------------------+
| Footer: [View Trial Balance] [View AR Aging] [View AP Aging]                    |
+----------------------------------------------------------------------------------+
```

---

## 5. Shared Dialog: Posting Confirmation

**Purpose**: Standard confirmation modal before any irreversible posting (JE post, AR issue, AP approve) to keep UX consistent.

**Primary Actions & APIs**

- `Confirm Post` → Calls the respective endpoint (`/JournalEntry/{id}/Post`, `/AccountsReceivable/{id}/Issue`, `/AccountsPayable/{id}/Approve`). Each endpoint performs validation internally and returns structured errors when blocking rules trip, so the UI never issues separate validation calls.
- `View Validation Log` → `GET /api/Finance/Validation/{entity}/{id}`; retrieves the most recent error set generated by those post attempts (including dry-run validations).

**Text Wireframe (Compact)**

```
+------------------------- Confirm Posting -------------------------+
| You are about to post JE-2026-014. This action will:              |
| • Lock all journal lines                                          |
| • Publish Finance.JournalEntryPosted                              |
| • Update Trial Balance immediately                                |
| Are you sure?                                                     |
| [View Validation Log]                                             |
|                                  [Cancel]   [Confirm & Post]      |
+------------------------------------------------------------------+
```

---

## 6. Chart of Accounts Management (Page)

**Purpose**: Maintain the hierarchical chart, toggle availability, and capture metadata like normal balance and consolidation flags.

Note: we can only create and edit accounts that are USER_ACOUNTS not SYSTEM_ACCOUNTS.

**Primary Actions & APIs**

- `Create Account` → `POST /api/Finance/ChartOfAccounts`.
- `Edit Account` → `PUT /api/Finance/ChartOfAccounts/{id}`.
- `Deactivate` → `POST /api/Finance/ChartOfAccounts/{id}/Deactivate`.
- `Reorder Hierarchy` → `POST /api/Finance/ChartOfAccounts/{id}/Move` with new parent/order info.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Chart of Accounts                                         [Add Account] [Export] |
+----------------------------------------------------------------------------------+
| Filter: Type [Asset▼]  Status [Active▼]  Search [__________]                     |
+----------------------------------------------------------------------------------+
| Tree/Grid                                                                               |
| Assets (1xxx)
|   Current Assets (11xx)
|     1101 Cash & Equivalents      Normal Balance: Debit   [Edit] [Deactivate]
|     1102 Accounts Receivable     Normal Balance: Debit   [Edit] [Deactivate]
|   Fixed Assets (12xx) ...
+----------------------------------------------------------------------------------+
| Drawer (when editing): Account Code, Name, Type, Category, Normal Balance, Branch |
| Restrictions, Toggle `IsSystemAccount`. Buttons: [Cancel] [Save Account]         |
+----------------------------------------------------------------------------------+
```

## 7. Trial Balance Report (Page)

**Purpose**: Provide period-over-period comparisons, drilldowns to GL, export to Excel.

**Primary Actions & APIs**

- `Load Report` → `GET /api/Finance/Reporting/TrialBalance?asOf=...&compareTo=...`.
- `Export` → `POST /api/Finance/Reporting/TrialBalance/Export`.
- `Drill to GL` → `GET /api/Finance/GeneralLedger?accountId=...&dateFrom=...&dateTo=...`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Trial Balance (As of 2026-03-31 vs 2026-02-29)    [Refresh] [Export XLSX]        |
+----------------------------------------------------------------------------------+
| Filters: Branch [All▼]  Consolidate [Yes/No]  Currency [AFN▼]                    |
+----------------------------------------------------------------------------------+
| Table                                                                               |
| Account Code | Account Name           | Debit (Current) | Credit (Current) | Delta |
| 1101         | Cash                   | 1,200,000       |                  | +5%   |
| 1102         | Accounts Receivable    |   450,000       |                  | -2%   |
| ...                                                                               |
+----------------------------------------------------------------------------------+
| Footer: Totals, Balance Check, [View GL]                                          |
+----------------------------------------------------------------------------------+
```

## 8. AR Aging Report (Page)

**Purpose**: Show outstanding invoices by customer across aging buckets.

**Primary Actions & APIs**

- `Load Aging` → `GET /api/Finance/AccountsReceivable/Aging?asOf=...`.
- `Export` → `POST /api/Finance/AccountsReceivable/Aging/Export`.
- `Send Reminder` → `POST /api/Finance/AccountsReceivable/{invoiceId}/Reminder`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| AR Aging (As of 2026-03-31)                         [Export PDF] [Send Reminders] |
+----------------------------------------------------------------------------------+
| Filters: Customer [All▼]  Branch [All▼]  Currency [AFN▼]                          |
+----------------------------------------------------------------------------------+
| Bucket Cards: 0-30: 320k | 30-60: 180k | 60-90: 75k | 90+: 22k                    |
+----------------------------------------------------------------------------------+
| Detail Grid: Customer | Invoice | Due Date | 0-30 | 30-60 | 60-90 | 90+ | Actions |
| KabulCare  | INV-1001 | 2026-02-15 | 0 | 45k | 0 | 0 | [View Invoice]            |
+----------------------------------------------------------------------------------+
```

## 9. AP Aging Report (Page)

**Purpose**: Track vendor liabilities and plan payments.

**Primary Actions & APIs**

- `Load Aging` → `GET /api/Finance/AccountsPayable/Aging?asOf=...`.
- `Schedule Payment` → `POST /api/Finance/AccountsPayable/{billId}/MarkForPayment`.
- `Export` → `POST /api/Finance/AccountsPayable/Aging/Export`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| AP Aging (As of 2026-03-31)                               [Export] [Batch Pay]    |
+----------------------------------------------------------------------------------+
| Bucket Summary Row similar to AR                                                    |
+----------------------------------------------------------------------------------+
| Grid: Vendor | Bill No | Due | 0-30 | 30-60 | 60-90 | 90+ | Payment Terms | Action |
| MediSupplies | BILL-22 | 03/10 | 18k | 0 | 0 | 0 | Net 30 | [Mark for Payment]     |
+----------------------------------------------------------------------------------+
```

## 10. Invoice Reconciliation (Page)

**Purpose**: Match receipts to invoices, mark as paid/partial, and handle write-offs.

**Primary Actions & APIs**

- `Load Invoice` → `GET /api/Finance/AccountsReceivable/{id}`.
- `Record Payment` → `POST /api/Finance/AccountsReceivable/{id}/RecordPayment` with amount, method, reference.
- `Apply Credit Note` → `POST /api/Finance/AccountsReceivable/{id}/ApplyCredit`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Invoice Reconciliation: INV-2026-088                Status: Partially Paid        |
+----------------------------------------------------------------------------------+
| Summary Card: Net 12,500 | Paid 5,000 | Balance 7,500 | Buckets (0-30 etc)       |
+----------------------------------------------------------------------------------+
| Payment History Table: Date | Method | Amount | Reference | Actions               |
| New Payment Form: Amount [____] Method [Cash▼] Ref [_____] Date [__/__/__]        |
| Buttons: [Record Payment]  [Apply Credit]  [Write Off Balance]                    |
+----------------------------------------------------------------------------------+
```

## 11. Bill Payment Workflow (Page)

**Purpose**: Approve bills, batch them for payouts, and trigger payment instructions.

**Primary Actions & APIs**

- `Load Payable Queue` → `GET /api/Finance/AccountsPayable?status=Approved`.
- `Mark for Payment` → `POST /api/Finance/AccountsPayable/{id}/MarkForPayment`.
- `Batch Pay` → `POST /api/Finance/Payments/Batch` with selected bill IDs.
- `Confirm Disbursement` → `POST /api/Finance/Payments/{batchId}/Confirm`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Bill Payment Center                                         [Create Payment Run] |
+----------------------------------------------------------------------------------+
| Filters: Vendor, Due Date Range, Currency, Branch                                  |
+----------------------------------------------------------------------------------+
| Table with checkboxes: Bill # | Vendor | Due | Amount | Status | Select           |
| Footer: Selected Total, Buttons [Mark for Payment] [Batch Pay]                    |
+----------------------------------------------------------------------------------+
| Side Panel shows batch details, bank account selection, approval status           |
+----------------------------------------------------------------------------------+
```

## 12. General Ledger Viewer (Page)

**Purpose**: Inspect posted transactions per account/date and drill to source documents.

**Primary Actions & APIs**

- `Search` → `GET /api/Finance/GeneralLedger?accountId=...&dateFrom=...&dateTo=...`.
- `Export` → `POST /api/Finance/GeneralLedger/Export`.
- `View Source` → hyperlinked to JE/Invoice/Bill endpoints.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| General Ledger Viewer                                         [Search] [Export]   |
+----------------------------------------------------------------------------------+
| Filters: Account [Tree picker], Date Range, Branch, Currency                       |
+----------------------------------------------------------------------------------+
| Grid: Date | Entry # | Description | Debit | Credit | Balance | Source Link       |
| Running balance row after each entry                                              |
+----------------------------------------------------------------------------------+
```

## 13. Bank Reconciliation (Page)

**Purpose**: Match bank statement lines with GL cash transactions, highlight gaps.

**Primary Actions & APIs**

- `Import Statement` → `POST /api/Finance/BankReconciliation/Upload`.
- `Auto-Match` → `POST /api/Finance/BankReconciliation/Match` with algorithm parameters.
- `Manual Match` → drag/drop UI calling `POST /api/Finance/BankReconciliation/ManualMatch`.
- `Finalize` → `POST /api/Finance/BankReconciliation/{periodId}/Close`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Bank Reconciliation - March 2026                         [Import CSV] [Auto-Match]|
+----------------------------------------------------------------------------------+
| Left Column: Bank Statement Lines (Date, Description, Amount, Status)             |
| Right Column: GL Cash Transactions (Date, JE #, Amount, Balance)                  |
| Drag line from left to right to match; mismatches flagged in Alerts panel         |
| Footer: Adjustments form, Difference summary, [Finalize Period] button            |
+----------------------------------------------------------------------------------+
```

## 14. Expense Report Submission (Form)

**Purpose**: Allow employees to submit reimbursable expenses with receipts and routing metadata.

**Primary Actions & APIs**

- `Submit Report` → `POST /api/Finance/Expense` (payload contains employeeId, items[], attachments).
- `Upload Receipt` → `POST /api/Finance/Expense/Attachment` (pre-signed URL flow if needed).
- `Save Draft` → `POST /api/Finance/Expense?status=Draft`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Expense Report Submission                               [Save Draft] [Submit]     |
+----------------------------------------------------------------------------------+
| Employee auto-filled, Cost Center selector, Trip Details                          |
| Line Items repeater: Category [Travel▼], Amount, Currency, Date, Notes, Receipt   |
| Attachment uploader per line plus summary attachments                            |
| Approval routing dropdown (Manager, Finance)                                      |
+----------------------------------------------------------------------------------+
```

## 16. Financial Audit Trail (Page)

**Purpose**: Provide immutable change logs for every financial entity, supporting compliance audits and troubleshooting.

**Primary Actions & APIs**

- `Search Logs` → `GET /api/Finance/AuditTrail?entityType=...&entityId=...&dateFrom=...`.
- `Export` → `POST /api/Finance/AuditTrail/Export`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Financial Audit Trail                                         [Search] [Export]   |
+----------------------------------------------------------------------------------+
| Filters: Entity Type [JournalEntry▼]  Entity Id [________]  User [All▼]           |
+----------------------------------------------------------------------------------+
| Timeline/Grid: Timestamp | User | Action | Before | After | Correlation ID        |
| Expand row to show field-level diffs in side-by-side columns.                     |
+----------------------------------------------------------------------------------+
```

## 17. Budget vs. Actual Report (Page)

**Purpose**: Compare planned budgets vs. actual spending by cost center or GL segment.

**Primary Actions & APIs**

- `Load Variance` → `GET /api/Finance/Reporting/BudgetVariance?period=FY26&costCenter=...`.
- `Export` → `POST /api/Finance/Reporting/BudgetVariance/Export`.
- `Drill` → `GET /api/Finance/GeneralLedger?accountId=...` filtered to the selected bucket.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Budget vs Actual (FY26 Q1)                         [Refresh] [Export XLSX]        |
+----------------------------------------------------------------------------------+
| Filters: Cost Center [All▼]  Program [All▼]  Currency [AFN▼]                      |
+----------------------------------------------------------------------------------+
| Table: Cost Center | Budget | Actual | Variance | % | Status (On Track / At Risk) |
| Progress bars visualize burn rate; clicking row opens GL detail drawer.           |
+----------------------------------------------------------------------------------+
```

## 18. Multi-Currency Exchange Rate Management (Page)

**Purpose**: Maintain exchange rates used across Finance, ensuring history and approval flows.

**Primary Actions & APIs**

- `Add Rate` → `POST /api/Finance/Currency/Rates` with base currency, foreign currency, effective date, rate.
- `Edit Rate` → `PUT /api/Finance/Currency/Rates/{id}`.
- `Publish` → `POST /api/Finance/Currency/Rates/{id}/Publish` to lock the rate for downstream modules.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Exchange Rate Management                                [Add Rate] [Import CSV]   |
+----------------------------------------------------------------------------------+
| Grid: Effective Date | Base | Foreign | Rate | Status | Approved By | Actions     |
| Inline chart shows recent trend for selected currency pair.                       |
+----------------------------------------------------------------------------------+
| Drawer form: Base Currency, Foreign Currency, Rate, Effective Window, Comments    |
+----------------------------------------------------------------------------------+
```

## 19. Financial Forecasting Dashboard (Page)

**Purpose**: Project future liquidity, AR/AP balances, and net income scenarios.

**Primary Actions & APIs**

- `Load Forecast` → `GET /api/Finance/Forecasting?horizonMonths=6&scenario=Base`.
- `Adjust Scenario` → `POST /api/Finance/Forecasting/Scenario` (optional) to tweak assumptions.
- `Export` → `POST /api/Finance/Forecasting/Export`.

**Text Wireframe**

```
+----------------------------------------------------------------------------------+
| Financial Forecasting Dashboard                         [Scenario Builder]        |
+----------------------------------------------------------------------------------+
| Cards: Projected Cash (Next 90 days), Projected AR, Projected AP, Projected NI    |
+----------------------------------------------------------------------------------+
| Charts: Cash Flow Projection (area chart), AR/AP Trend (stacked), Net Income Line |
| Sidebar: Assumption sliders (Revenue growth %, Expense inflation %, FX rate)      |
| Footer: [Export Forecast] [Create Scenario]                                      |
+----------------------------------------------------------------------------------+
```

### Visualization Notes

- Wireframes are intentionally text-based for quick iteration; high-fidelity designs will follow in Figma referencing these layouts.
- All submit buttons follow the same pattern: optimistic UI state change → spinner → API call → success toast with navigation hint. Error states show inline field errors plus toast with correlation ID.
- Each form/dialog logs idempotency tokens on submit to ensure retries align with the financial eventing guidelines.
