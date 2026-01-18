using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Configuration.Accounting;
using Crystal_Clinic_Mgm.Persistence.Configuration.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Configuration.BranchStock.Look;
using Crystal_Clinic_Mgm.Persistence.Configuration.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic;
using Crystal_Clinic_Mgm.Persistence.Configuration.General;
using Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject;
using Crystal_Clinic_Mgm.Persistence.Configuration.Look;
using Crystal_Clinic_Mgm.Persistence.Configuration.Stocks;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Persistence.Contexts
{
    public class ERP_DbContext(DbContextOptions<ERP_DbContext> options) : DbContext(options)
    {
        #region LookUps
        public DbSet<Branch> Branchs { get; set; }
        public DbSet<BranchDetails> BranchDetails { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<CurrencyType> CurrencyType { get; set; }
        public DbSet<AssetType> AssetType { get; set; }
        public DbSet<PayType> Paytype { get; set; }
        public DbSet<LoanType> LoanType { get; set; }
        public DbSet<ExpenseType> ExpenseType { get; set; }
        #endregion

        #region Human Resource
        public DbSet<Partners> Partners { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<PositionTitle> PositionTitles { get; set; }
        public DbSet<ContractDetails> ContractDetails { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<PayrollTracking> PayrollTracking { get; set; }
        public DbSet<AdvancePayment> AdvancePayment { get; set; }

        #endregion

        #region General
        public DbSet<News> News { get; set; }
        public DbSet<NewsNotification> NewsNotification { get; set; }
        public DbSet<TrainingVideo> TrainingVideos { get; set; }
        #endregion

        #region Asset Managment
        public DbSet<MainAccount> MainAccount { get; set; }
        public DbSet<AccountTracking> AccountTracking { get; set; }
        public DbSet<ExpenseTracking> ExpenseTracking { get; set; }
        public DbSet<TradeTracking> TradeTracking { get; set; }
        public DbSet<WithdrawalTracking> WithdrawalTracking { get; set; }
        #endregion


        public DbSet<StockMovement> StockMovements { get; set; }
        //public DbSet<ItemCleaningJob> ItemCleaningJob { get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        // New inventory enhancement entities
        public DbSet<InventorySite> InventorySites { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<AdjustmentCategory> AdjustmentCategories { get; set; }
        public DbSet<InventoryKit> InventoryKits { get; set; }
        public DbSet<InventoryKitLine> InventoryKitLines { get; set; }
        public DbSet<InventoryReservation> InventoryReservations { get; set; }
        public DbSet<ReservedItem> ReservedItems { get; set; } 
        public DbSet<Doctor> Doctor { get; set; } 
        public DbSet<Patient> Patient { get; set; } 
        public DbSet<Visit> Visit { get; set; } 
        public DbSet<VisitMedication> VisitMedication { get; set; } 
        public DbSet<VisitServices> VisitServices { get; set; } 
        public DbSet<VisitPayment> VisitPayment { get; set; }
        public DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }
        public DbSet<ServiceSessions> ServiceSessions { get; set; }

        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<SupplierDue> SupplierDue { get; set; }
        public DbSet<DuePayment> DuePayment { get; set; }
        public DbSet<CallList> CallList { get; set; }

        #region Accounting
        public DbSet<CompanyProfile> CompanyProfile { get; set; }
        public DbSet<ChartOfAccounts> ChartOfAccounts { get; set; }
        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<JournalEntryLine> JournalEntryLines { get; set; }
        public DbSet<GeneralLedger> GeneralLedgers { get; set; }
        public DbSet<RecurringJournalTemplate> RecurringJournalTemplates { get; set; }
        public DbSet<RecurringJournalLine> RecurringJournalLines { get; set; }
        public DbSet<AccountsReceivable> AccountsReceivables { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<AccountsPayable> AccountsPayables { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<VendorBill> VendorBills { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInvoiceLine> SalesInvoiceLines { get; set; }
        public DbSet<SalesReceipt> SalesReceipts { get; set; }
        public DbSet<SalesEstimate> SalesEstimates { get; set; }
        public DbSet<SalesEstimateLine> SalesEstimateLines { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<POLine> POLines { get; set; }
        public DbSet<FixedAsset> FixedAssets { get; set; }
        public DbSet<Shareholder> Shareholders { get; set; }
        public DbSet<EquityTransaction> EquityTransactions { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetLine> BudgetLines { get; set; }
        public DbSet<BankStatementImport> BankStatementImports { get; set; }
        public DbSet<BankStatementLine> BankStatementLines { get; set; }
        public DbSet<BankMatch> BankMatches { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }
        public DbSet<ForecastSnapshot> ForecastSnapshots { get; set; }
        public DbSet<ForecastLine> ForecastLines { get; set; }
        public DbSet<ServiceInventoryLink> ServiceInventoryLinks { get; set; }
        public DbSet<ProcedureLog> ProcedureLogs { get; set; }
        public DbSet<LabTestTemplate> LabTestTemplates { get; set; }
        public DbSet<LabOrderLine> LabOrderLines { get; set; }
        #endregion

        public async Task<decimal> GetExchangeRate(int fromCurrencyId, int toCurrencyId, CancellationToken cancellationToken)
        {
            if (fromCurrencyId == toCurrencyId)
                return 1m;

            var rate = await CurrencyExchangeRates
                .Where(r => !r.IsDeleted && r.FromCurrencyId == fromCurrencyId && r.ToCurrencyId == toCurrencyId)
                .OrderByDescending(r => r.ModifiedOn ?? r.CreatedOn)
                .Select(r => r.ExchangeRate)
                .FirstOrDefaultAsync(cancellationToken);

            if (rate == 0)
                throw new InvalidOperationException($"No exchange rate found from CurrencyId {fromCurrencyId} to CurrencyId {toCurrencyId}.");

            return rate;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region LookUps 
            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new BranchDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentsConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AssetTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaytypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoanTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseTypeConfiguration());
            #endregion

            #region HR Configuration

            // LookUp
            modelBuilder.ApplyConfiguration(new ContractTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PositionTitleConfiguration());

            // main table
            modelBuilder.ApplyConfiguration(new ContractDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeProfileConfiguration());


            modelBuilder.ApplyConfiguration(new PayrollTrackingConfiguration());
            modelBuilder.ApplyConfiguration(new AdvancePaymentConfiguration());

            #endregion

            #region General
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new NewsNotificationConfiguration());
            modelBuilder.ApplyConfiguration(new TrainingVideoConfiguration());
            #endregion
                        
            #region Asset Managment
            modelBuilder.ApplyConfiguration(new MainAccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountTrackingConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseTrackingConfiguration());
            modelBuilder.ApplyConfiguration(new TradeTrackingConfiguration());
            modelBuilder.ApplyConfiguration(new WithdrawalTrackingConfiguration());
            #endregion

            #region Inventory Configuration
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new ItemCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ItemUnitConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new StockConfiguration());
            modelBuilder.ApplyConfiguration(new StockMovementConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierDueConfiguration());
            modelBuilder.ApplyConfiguration(new DuePaymentConfiguration());
            modelBuilder.ApplyConfiguration(new ReservedItemConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryKitLineConfiguration());
            #endregion

            modelBuilder.ApplyConfiguration(new DoctorConfiguration());
            modelBuilder.ApplyConfiguration(new PatientConfiguration());
            modelBuilder.ApplyConfiguration(new VisitConfiguration());
            modelBuilder.ApplyConfiguration(new VisitMedicationConfiguration());
            modelBuilder.ApplyConfiguration(new VisitServicesConfiguration());
            modelBuilder.ApplyConfiguration(new VisitPaymentConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyExchangeRateConfiguration());
            modelBuilder.ApplyConfiguration(new CallListConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceSessionsConfiguration());

            #region Accounting Configuration
            modelBuilder.ApplyConfiguration(new CompanyProfileConfiguration());
            modelBuilder.ApplyConfiguration(new ChartOfAccountsConfiguration());
            modelBuilder.ApplyConfiguration(new JournalEntryConfiguration());
            modelBuilder.ApplyConfiguration(new JournalEntryLineConfiguration());
            modelBuilder.ApplyConfiguration(new GeneralLedgerConfiguration());
            modelBuilder.ApplyConfiguration(new RecurringJournalTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new RecurringJournalLineConfiguration());
            modelBuilder.ApplyConfiguration(new AccountsReceivableConfiguration());
            modelBuilder.ApplyConfiguration(new ReceiptConfiguration());
            modelBuilder.ApplyConfiguration(new AccountsPayableConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new VendorBillConfiguration());
            modelBuilder.ApplyConfiguration(new SalesInvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new SalesInvoiceLineConfiguration());
            modelBuilder.ApplyConfiguration(new SalesReceiptConfiguration());
            modelBuilder.ApplyConfiguration(new SalesEstimateConfiguration());
            modelBuilder.ApplyConfiguration(new SalesEstimateLineConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
            modelBuilder.ApplyConfiguration(new POLineConfiguration());
            modelBuilder.ApplyConfiguration(new FixedAssetConfiguration());
            modelBuilder.ApplyConfiguration(new ShareholderConfiguration());
            modelBuilder.ApplyConfiguration(new EquityTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetLineConfiguration());
            modelBuilder.ApplyConfiguration(new BankStatementImportConfiguration());
            modelBuilder.ApplyConfiguration(new BankStatementLineConfiguration());
            modelBuilder.ApplyConfiguration(new BankMatchConfiguration());
            modelBuilder.ApplyConfiguration(new AuditTrailConfiguration());
            modelBuilder.ApplyConfiguration(new ForecastSnapshotConfiguration());
            modelBuilder.ApplyConfiguration(new ForecastLineConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceInventoryLinkConfiguration());
            modelBuilder.ApplyConfiguration(new ProcedureLogConfiguration());
            modelBuilder.ApplyConfiguration(new LabTestTemplateConfiguration());
            modelBuilder.ApplyConfiguration(new LabOrderLineConfiguration());
            #endregion

        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.EnableSensitiveDataLogging(true);
                builder.UseSqlServer(AppConfig.ERP_DbContext, (opts) =>
                {
                });
            }
            base.OnConfiguring(builder);
        }
    }
}
