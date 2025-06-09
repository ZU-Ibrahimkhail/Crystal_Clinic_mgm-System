using Crystal_Clinic_Mgm.Common.AppConfig;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;

using Crystal_Clinic_Mgm.Persistence.Configuration.UMS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Persistence.Contexts
{
    public class UMS_DbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public UMS_DbContext(DbContextOptions<UMS_DbContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

        }
#pragma warning disable CS0114 //  To make the current member override that implementation, add the override keyword.Otherwise add the new keyword
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Applications> Application { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<UserAudit> UserAudits { get; set; }
        public DbSet<TrackingTable> TrakingTable { get; set; }
        public DbSet<UserAllowedDocTypesSecurityLevels> UserAllowedDocTypesSecurityLevels { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<RefreshToken> RefreshTokenes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationMessage> NotificationMessages { get; set; }
        public DbSet<EmailHistory> EmailHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            #region UMSTables
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserAuditConfiguration());
            modelBuilder.ApplyConfiguration(new TrackingTableConfiguration());
            modelBuilder.ApplyConfiguration(new UserAllowedDocTypesSecurityLevelsConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationMessageConfiguration());
            modelBuilder.ApplyConfiguration(new EmailHistoryConfiguration());
            #endregion
            modelBuilder.Entity<UserRole>()
           .HasKey(ur => new { ur.RoleId, ur.UserId });
            modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.PermissionId, rp.RoleId });
        }
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.EnableSensitiveDataLogging(true);
                builder.UseSqlServer(AppConfig.UMS_DbContext, (opts) =>
                {

                });
            }
            base.OnConfiguring(builder);
        }

    }
}
