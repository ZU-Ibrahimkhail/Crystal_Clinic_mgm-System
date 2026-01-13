namespace Crystal_Clinic_Mgm.Domain
{
    #region Enums
    public enum OrderType
    {
        Sale,
        Rental,
        Mixed
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
        Out
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
        Open,
        PartiallyPaid,
        Paid,
        Overdue,
        WrittenOff
    }

    public enum APStatus
    {
        Open,
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

    #endregion
}
