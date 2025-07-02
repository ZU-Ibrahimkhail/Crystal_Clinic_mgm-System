using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crystal_Clinic_Mgm.Persistence.Configuration.Look
{
    public class CurrencyExchangeRateConfiguration : IEntityTypeConfiguration<CurrencyExchangeRate>
    {
        [Obsolete]
        public void Configure(EntityTypeBuilder<CurrencyExchangeRate> builder)
        {
            builder.ToTable("CurrencyExchangeRates");


            builder.HasKey(e => e.CurrencyExchangeRateId);

            builder.Property(e => e.ExchangeRate)
                .IsRequired()
                .HasPrecision(18, 6); // Allows for precise exchange rates

            builder.HasOne(e => e.FromCurrency)
                .WithMany()
                .HasForeignKey(e => e.FromCurrencyId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of CurrencyType in use

            builder.HasOne(e => e.ToCurrency)
                .WithMany()
                .HasForeignKey(e => e.ToCurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Ensure FromCurrencyId != ToCurrencyId
            builder.ToTable(x => x.HasCheckConstraint("CK_CurrencyExchangeRate_DifferentCurrencies", "[FromCurrencyId] != [ToCurrencyId]"));
            //builder.HasCheckConstraint("CK_CurrencyExchangeRate_DifferentCurrencies", "[FromCurrencyId] != [ToCurrencyId]");
        }
    }
}