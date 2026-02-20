using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> entity)
        {
            entity.ToTable("LeaveRequest", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee)
                .WithMany(x => x.LeaveRequests)
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.LeaveTypeId).HasColumnName("LeaveTypeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.LeaveType)
                .WithMany(x => x.LeaveRequests)
                .HasForeignKey(f => f.LeaveTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.StartDate).HasColumnName("StartDate").HasColumnType("date").IsRequired(true);
            entity.Property(c => c.EndDate).HasColumnName("EndDate").HasColumnType("date").IsRequired(true);
            entity.Property(c => c.TotalDays).HasColumnName("TotalDays").HasColumnType("decimal(5,2)").IsRequired(true);

            entity.Property(c => c.Reason).HasColumnName("Reason").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.Status).HasColumnName("Status").HasColumnType("int").IsRequired(true).HasDefaultValue(1); // Pending

            entity.Property(c => c.ApprovedById).HasColumnName("ApprovedById").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.ApprovedBy)
                .WithMany()
                .HasForeignKey(f => f.ApprovedById)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.ApprovalDate).HasColumnName("ApprovalDate").HasColumnType("datetime").IsRequired(false);
            entity.Property(c => c.ApprovalNotes).HasColumnName("ApprovalNotes").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.AttachmentPath).HasColumnName("AttachmentPath").HasColumnType("nvarchar(max)").IsRequired(false);

            entity.HasIndex(e => e.EmployeeId);
            entity.HasIndex(e => e.Status);

            EntityConfiguration<LeaveRequest>.AuditableEntityConfigurations(entity);
        }
    }
}
