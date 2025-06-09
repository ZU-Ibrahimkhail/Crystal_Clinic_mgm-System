using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class EmployeeProfileConfiguration : IEntityTypeConfiguration<EmployeeProfile>
    {
        public void Configure(EntityTypeBuilder<EmployeeProfile> entity)
        {
            entity.ToTable("EmployeeProfile", "HR");
            entity.HasKey(x => x.ID);
            entity.Property(c => c.EnglishFirstName).HasColumnName("EnglishFirstName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.PashtoFirstName).HasColumnName("PashtoFirstName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.EnglishSurName).HasColumnName("EnglishSurName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.PashtoSurName).HasColumnName("PashtoSurName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.EnglishFatherName).HasColumnName("EnglishFatherName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.PashtoFatherName).HasColumnName("PashtoFatherName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.EnglishGrandFatherName).HasColumnName("EnglishGrandFatherName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.PashtoGrandFatherName).HasColumnName("PashtoGrandFatherName").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.Gender).HasColumnName("Gender").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            //---Tazkira Info
            entity.Property(c => c.TazkiraTypeId).HasColumnName("TazkiraTypeId").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.TazkiraNo).HasColumnName("TazkiraNo").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);

            entity.Property(c => c.JoldNo).HasColumnName("JoldNo").HasMaxLength(20).IsRequired(false).HasColumnType("nvarchar");
            entity.Property(c => c.PageNo).HasColumnName("PageNo").HasMaxLength(20).IsRequired(false).HasColumnType("nvarchar");
            entity.Property(c => c.RegNo).HasColumnName("RegNo").HasMaxLength(20).IsRequired(false).HasColumnType("nvarchar");

            entity.Property(c => c.DateOfBirth).HasColumnName("DateOfBirth").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.TemporaryAddress).HasColumnName("TemporaryAddress").HasMaxLength(50).IsRequired(true).HasColumnType("nvarchar");
            entity.Property(c => c.PermenantAddress).HasColumnName("PermenantAddress").HasMaxLength(50).IsRequired(true).HasColumnType("nvarchar");

            entity.Property(c => c.BranchId).HasColumnName("BranchId").HasColumnType("int").IsRequired(true);
            entity.HasOne<Branch>("Branch").WithMany().HasForeignKey(f => f.BranchId).OnDelete(DeleteBehavior.NoAction);
            
            entity.Property(c => c.CurrencyTypeId).HasColumnName("CurrencyTypeId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x=>x.CurrencyType).WithMany().HasForeignKey(f => f.CurrencyTypeId).OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.BloodGroup).HasColumnName("BloodGroup").HasMaxLength(50).IsRequired(false).HasColumnType("nvarchar");
            entity.Property(c => c.JoinDate).HasColumnName("JoinDate").HasColumnType("DateTime").IsRequired(true);
            entity.Property(c => c.LeaveDate).HasColumnName("LeaveDate").HasColumnType("DateTime").IsRequired(false).HasDefaultValue(null);
            entity.Property(c => c.LeaveRemark).HasColumnName("LeaveRemark").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);
            //--Eamil-------------
            entity.Property(c => c.PersonalEmail).HasColumnName("PersonalEmail").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);

            entity.Property(c => c.PhoneNumber).HasColumnName("PhoneNumber").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.EmergencyPhoneNumber).HasColumnName("EmergencyPhoneNumber").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(true);
            entity.Property(c => c.PhotoPath).HasColumnName("PhotoPath").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            entity.Property(c => c.IsActive).HasColumnName("IsCurrent").HasColumnType("bit").IsRequired(true);
            entity.Property(c => c.HasAccount).HasColumnName("HasAccount").HasColumnType("bit").IsRequired(true).HasDefaultValue(false);

            EntityConfiguration<EmployeeProfile>.AuditableEntityConfigurations(entity);
        }
    }
}
