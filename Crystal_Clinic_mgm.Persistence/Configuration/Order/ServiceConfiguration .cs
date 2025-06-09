using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Order
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> entity)
        {
            entity.ToTable(nameof(Service), "Services");
            entity.HasKey(x => x.ServiceId);

            entity.Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar(100)").IsRequired();
            entity.Property(x => x.Description).HasColumnName("Description").HasColumnType("nvarchar(500)");
            entity.Property(x => x.IsRentable).HasColumnName("IsRentable").HasColumnType("bit").IsRequired();
            entity.Property(x => x.DailyRate).HasColumnName("DailyRate").HasColumnType("decimal(18,2)");
            entity.Property(x => x.HourlyRate).HasColumnName("HourlyRate").HasColumnType("decimal(18,2)");
            entity.Property(x => x.ImagePath).HasColumnName("ImagePath").HasColumnType("nvarchar(max)");
        }
    }
}
