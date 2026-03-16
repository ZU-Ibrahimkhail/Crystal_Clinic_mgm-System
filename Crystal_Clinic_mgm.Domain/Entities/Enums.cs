namespace Crystal_Clinic_Mgm.Domain
{
    #region Enums
    public enum OrderType
    {
        Sale,
        Rental,
        Mixed
    }

    public enum AttachmentType
    {
        ShareHolder,
        JournalEntry,
        AccountReceivable,
        AccountPayable,
        PurchaseOrder,
        PatientDocument,
        SalesInvoice,
        VendorBill
    }

    public enum ReturnStatus
    {
        Pending,
        Completed,
        Partial,
        Damaged
    }
    public enum StockCheckType
    {
        Exact,       // Must have exactly the required quantity
        AtLeast,     // Must have at least the required quantity
        Percentage   // Check if stock is above a certain percentage threshold
    }

    public enum PaymentMethod
    {
        Cash,
        CreditCard,
        BankTransfer,
        Check
    }

    public enum CallResponseType
    {
        Answered,
        Timeout,
        Bussy,
        Off,
        OutOfCoverageArea
    }
    public enum CallingReason
    {
        Marketting,
        Meeting,
        Procedure_Implementation_Follow_Up,
        Next_Session_Implementation_Reminder,
        Other
    }
    public enum AdjustmentType
    {
        PriceOverride,
        Discount,
        DamageFee,
        AdditionalCharge,
        QuantityChange
    }
    public enum MovementReason
    {
        // Inventory adjustments
        PurchaseReceipt,    // New stock received
        ManualAdjustment,   // Physical count correction
        Adjustment,         // General stock adjustment

        // Order-related
        SaleDeduction,      // Item sold
        RentalReservation,  // Temporarily blocked for rental
        RentalReturn,       // Rental items returned
        SaleReturn,       // Rental items returned

        // Transfers
        BranchTransferOut,  // Sent to another branch
        BranchTransferIn,   // Received from another branch

        // Quality control
        DamageWriteOff,     // Unrepairable damage
        ExpiredStock,        // Discarded perishables
        OrderUpdate
    }

    public enum ValuationMethod
    {
        FIFO,               // First In, First Out
        WeightedAverage,    // Moving Average Cost
        SpecificIdentification, // Specific lot identification
        LIFO                // Last In, First Out (not IFRS compliant)
    }

    public enum ServiceStatus
    {
        Pending,        // Created but not scheduled
        Scheduled,      // Date/time assigned
        Dispatched,     // Technician en route
        InProgress,     // Service being performed
        OnHold,         // Waiting for parts/approval
        Completed,      // Successfully finished
        Cancelled,      // Service terminated
        RequiresFollowUp // Needs secondary visit
    }

    public enum OrderStatus
    {
        Draft,
        Pending,
        Confirmed,
        InProgress,
        PartiallyReturned,
        Completed,
        Cancelled
    }
    public enum CustomerType
    {
        Individual,     // Regular retail customers
        Business,       // Corporate clients (restaurants, hotels)
        Wholesaler,     // Bulk purchasers
        Contractor,     // Event/wedding planners
        Government,     // Municipal/state entities
        NonProfit,       // Charitable organizations
        Regular
    }

    public enum ReservationStatus
    {
        Pending,
        Active,
        Completed,
        Cancelled
    }
    public enum MovementType
    {
        In,
        Out,
        Adjustment
    }

    public enum DurationType
    {
        Hours,
        Days
    }

    #region Accounting Enums
    public enum AccountType
    {
        Asset = 1,
        Liability = 2,
        Equity = 3,
        Revenue = 4,
        Expense = 5,
        ContraAsset = 6,
        OtherIncome = 7
    }

    public enum AccountCategory
    {
        CurrentAsset = 100,
        FixedAsset = 101,
        OtherAsset = 102,
        CurrentLiability = 200,
        LongTermLiability = 201,
        RetainedEarnings = 300,
        Capital = 301,
        ServiceRevenue = 400,
        OtherRevenue = 401,
        OperatingExpense = 500,
        AdministrativeExpense = 501,
        FinancialExpense = 502
    }

    public enum NormalBalanceType
    {
        Debit,
        Credit
    }

    public enum JournalEntryStatus
    {
        Draft,
        Unposted,
        Posted,
        Voided
    }

    public enum ARStatus
    {
        Draft,
        Open,
        PartiallyPaid,
        Paid,
        Overdue,
        WrittenOff
    }

    public enum APStatus
    {
        Draft,
        Approve,
        PartiallyPaid,
        Paid,
        Overdue
    }

    public enum SalesStatus
    {
        Draft,
        Issued,
        Paid,
        Partial,
        Void,
        Refunded
    }

    public enum POStatus
    {
        Draft,
        Open,
        Received,
        Cancelled,
        Closed
    }

    public enum BillStatus
    {
        Unpaid,
        Partial,
        Paid,
        Overdue
    }

    public enum EquityTransactionType
    {
        Investment,
        Drawing,
        Dividend,
        ProfitShare
    }

    public enum BudgetStatus
    {
        Draft,
        Submitted,
        Approved,
        Active,
        Closed
    }

    public enum RecurringFrequency
    {
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    public enum TransactionType
    {
        Receipt = 1,    // Money received from customer
        Refund = 2,     // Change given back to customer
    }
    #endregion

    #region HR Enums
    public enum EmploymentStatus
    {
        Active = 1,
        Probation = 2,
        OnLeave = 3,
        Suspended = 4,
        Terminated = 5,
        Retired = 6
    }

    public enum LeaveStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4
    }

    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Late = 3,
        OnLeave = 4,
        HalfDay = 5
    }

    public enum PayrollComponentType
    {
        Benefit = 1,        // Added to salary
        Deduction = 2       // Subtracted from salary
    }

    public enum ComponentCalculationType
    {
        FixedAmount = 1,
        PercentageOfBaseSalary = 2,
        PercentageOfGross = 3
    }

    public enum PayCycle
    {
        Monthly = 1,
        BiWeekly = 2,
        Weekly = 3,
        Hourly = 4
    }

    public enum PayrollAdjustmentType
    {
        Reward = 1,
        Charge = 2,
        Overtime = 3,
        Bonus = 4,
        Penalty = 5
    }

    public enum HRTaskCategory
    {
        Onboarding = 1,
        Offboarding = 2
    }

    public enum HRTaskStatus
    {
        Pending = 1,
        Completed = 2,
        Cancelled = 3
    }

    public enum TaxCalculationType
    {
        StepByStep = 1,  // Progressive - calculate on overdraft amount in bracket
        Single = 2       // Flat - single bracket applies
    }
    #endregion

    #endregion
}
