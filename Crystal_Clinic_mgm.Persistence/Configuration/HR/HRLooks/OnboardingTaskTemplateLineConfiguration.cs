using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class OnboardingTaskTemplateLineConfiguration : IEntityTypeConfiguration<OnboardingTaskTemplateLine>
    {
        public void Configure(EntityTypeBuilder<OnboardingTaskTemplateLine> entity)
        {
            entity.ToTable("OnboardingTaskTemplateLine", "HR");
            entity.HasKey(x => x.Id);

            entity.Property(c => c.Id).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();

            entity.Property(c => c.TemplateId).HasColumnName("TemplateId").HasColumnType("int").IsRequired(true);
            entity.HasOne(x => x.Template)
                .WithMany(x => x.TemplateLines)
                .HasForeignKey(f => f.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(c => c.TaskName).HasColumnName("TaskName").HasColumnType("nvarchar").HasMaxLength(200).IsRequired(true);
            entity.Property(c => c.TaskDescription).HasColumnName("TaskDescription").HasColumnType("nvarchar(max)").IsRequired(false);
            entity.Property(c => c.TaskOrder).HasColumnName("TaskOrder").HasColumnType("int").IsRequired(true);
            entity.Property(c => c.IsRequired).HasColumnName("IsRequired").HasColumnType("bit").IsRequired(true).HasDefaultValue(true);
            entity.Property(c => c.AssignedToDepartmentId).HasColumnName("AssignedToDepartmentId").HasColumnType("int").IsRequired(false);
            entity.Property(c => c.AssignedToRole).HasColumnName("AssignedToRole").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(false);

            entity.HasIndex(e => new { e.TemplateId, e.TaskOrder });

            EntityConfiguration<OnboardingTaskTemplateLine>.AuditableEntityConfigurations(entity);
        }
    }
}
