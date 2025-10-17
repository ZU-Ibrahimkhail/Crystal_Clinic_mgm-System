using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.CrystalClinic
{
    public class CallListConfiguration : IEntityTypeConfiguration<CallList>
    {
        public void Configure(EntityTypeBuilder<CallList> entity)
        {
            entity.ToTable(nameof(CallList), "CrystalClinic");

            // Primary Key configuration
            entity.HasKey(d => d.Id);

            entity.Property(d => d.CallingReason)
                  .HasColumnType("int")
                  .IsRequired();

            entity.Property(d => d.Name)
                  .HasColumnName("Name")
                  .HasColumnType("nvarchar(100)")
                  .IsRequired();

            entity.Property(d => d.Description)
                  .HasColumnName("Description")
                  .HasColumnType("nvarchar(max)")
                  .IsRequired();


            entity.Property(d => d.PhoneNumber)
                  .HasColumnName("PhoneNumber")
                  .HasColumnType("nvarchar(255)")
                  .IsRequired(false);

            entity.Property(d => d.ToBeCalledDate)
                  .HasColumnType("DateTime")
                  .IsRequired();

            entity.Property(d => d.ActualCalledDate)
                  .HasColumnType("DateTime")
                  .IsRequired(false);

            entity.Property(d => d.CallResponse)
                  .HasColumnType("int")
                  .IsRequired();

            entity.Property(d => d.ResponseReasult)
                  .HasColumnName("ResponseReasult")
                  .HasColumnType("nvarchar(max)")
                  .IsRequired();

            entity.Property(d => d.AssignedEmployeeId)
                  .HasColumnName("AssignedEmployeeId")
                  .HasColumnType("int")
                  .IsRequired(false);

            entity.HasOne(x => x.AssignedEmployee)
                  .WithMany()
                  .HasForeignKey(x => x.AssignedEmployeeId)
                  .OnDelete(DeleteBehavior.NoAction);

            // Auditable properties (if required, you can configure them as well)
            EntityConfiguration<CallList>.AuditableEntityConfigurations(entity);
        }
    }
}
