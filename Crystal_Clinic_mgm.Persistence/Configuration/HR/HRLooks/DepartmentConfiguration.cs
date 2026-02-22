using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.HR.HRLooks
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.ToTable("Department", "HR");
            entity.HasKey(x => x.ID);

            entity.Property(c => c.ID).HasColumnName("Id").HasColumnType("int").ValueGeneratedOnAdd();
            entity.Property(c => c.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(100).IsRequired(true);
            entity.Property(c => c.DeptCode).HasColumnName("DeptCode").HasColumnType("nvarchar").HasMaxLength(50).IsRequired(false);

            entity.Property(c => c.ParentDepartmentId).HasColumnName("ParentDepartmentId").HasColumnType("int").IsRequired(false);
            entity.HasOne(x => x.ParentDepartment)
                .WithMany(x => x.ChildDepartments)
                .HasForeignKey(f => f.ParentDepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.Property(c => c.HeadEmployeeId).HasColumnName("HeadEmployeeId").HasColumnType("int").IsRequired(false);

            entity.HasMany(x => x.Employees)
                .WithOne(x => x.Department)
                .HasForeignKey(f => f.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            EntityConfiguration<Department>.AuditableEntityConfigurations(entity);
        }
    }
}
