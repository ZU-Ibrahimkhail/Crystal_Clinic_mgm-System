using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Domain.Entities.AssetMS;
using Crystal_Clinic_Mgm.Domain.Entities.General;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Domain.Entities.Order.Look;
using Crystal_Clinic_Mgm.Persistence.Configuration.AssetMS;
using Crystal_Clinic_Mgm.Persistence.Configuration.General;
using Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks;
using Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject;
using Crystal_Clinic_Mgm.Persistence.Configuration.Look;
using Crystal_Clinic_Mgm.Persistence.Configuration.Order;
using Crystal_Clinic_Mgm.Persistence.Configuration.Order.Look;
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


        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemUnit> ItemUnits { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<OrderService> OrderServices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<Return> Returns { get; set; }
        public DbSet<DamageReport> DamageReport { get; set; }
        public DbSet<OrderAdjustment> OrderAdjustments { get; set; }
        public DbSet<OrderPayment> OrderPayments { get; set; }
        public DbSet<RentalReservation> RentalReservations { get; set; }
        public DbSet<ItemCleaningJob> ItemCleaningJob { get; set; } 
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

            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new StockMovementConfiguration());
            modelBuilder.ApplyConfiguration(new ReturnConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ItemCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ItemUnitConfiguration());
            modelBuilder.ApplyConfiguration(new ReturnPaymentConfiguration()); 
            modelBuilder.ApplyConfiguration(new DamageReportConfiguration());
            modelBuilder.ApplyConfiguration(new OrderAdjustmentConfiguration());


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
