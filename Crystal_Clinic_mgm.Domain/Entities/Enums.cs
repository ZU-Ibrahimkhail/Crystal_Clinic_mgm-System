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
    #endregion
}
