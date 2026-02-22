using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRProject
{
    public class HRTaskConfiguration : IEntityTypeConfiguration<HRTask>
    {
        public void Configure(EntityTypeBuilder<HRTask> entity)
        {
            entity.ToTable("HRTask", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.EmployeeId).HasColumnName("EmployeeId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.TemplateLineId).HasColumnName("TemplateLineId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.TemplateLine)
                .WithMany()
                .HasForeignKey(f => f.TemplateLineId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.TaskName).HasColumnName("TaskName").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);
            entity.Property(c => c.Category).HasColumnName("Category").HasColumnType("int").IsRequired(true).HasDefaultValue(HRTaskCategory.Onboarding);
            entity.Property(c => c.Status).HasColumnName("Status").HasColumnType("int").IsRequired(true).HasDefaultValue(HRTaskStatus.Pending); // Pending

            entity.Property(c => c.DueDate).HasColumnName("DueDate").HasColumnType("datetime").IsRequired(true);
            entity.Property(c => c.CompletedDate).HasColumnName("CompletedDate").HasColumnType("datetime").IsRequired(false);

            entity.Property(c => c.AssignedToEmployeeId).HasColumnName("AssignedToEmployeeId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.AssignedToEmployee)
                .WithMany()
                .HasForeignKey(f => f.AssignedToEmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.Description).HasColumnName("Description").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.Notes).HasColumnName("Notes").HasColumnType("nvarchar(max)").IsRequired(false);

            entity.HasIndex(e => new { e.EmployeeId, e.Category });
            entity.HasIndex(e => e.Status);

            EntityConfiguration<HRTask>.AuditableEntityConfigurations(entity);
        }
    }
}
