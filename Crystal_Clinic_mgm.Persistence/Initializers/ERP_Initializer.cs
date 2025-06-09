using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;

namespace Crystal_Clinic_Mgm.Persistence.Initializers
{
    public class Crystal_Clinic_Initializer
    {
        public async static Task InitilizeCrystal_Clinic(ERP_DbContext context)
        {
            await SeedBranch(context);
            await SeedCurrencyType(context);
        }




        #region Branchs
        public static async Task SeedBranch(ERP_DbContext context)
        {
            var GUID = Guid.NewGuid();

            var branch = context.Branchs.ToList();
            if (branch.Count == 0)
            {
                try
                {
                    context.Branchs.Add(new Branch
                    {
                        EnglishName = "Main Branch",
                        PashtoName = "مرکزی نماینده ګی",
                        DariName = "نماینده گی مرکزی",
                        Code = "0001",
                        IsActive = true,
                        ParentId = null,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = GUID

                    }); ;
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        #region CurrencyType
        public static async Task SeedCurrencyType(ERP_DbContext context)
        {
            var GUID = Guid.NewGuid();

            var CurrencyType = context.CurrencyType.ToList();
            if (CurrencyType.Count == 0)
            {
                try
                {
                    context.CurrencyType.AddRange(new CurrencyType
                    {
                        EnglishName = "Dollar USD",
                        PashtoName = "امریکایی ډالر",
                        DariName = "دالر امریکایی",
                        Code = "USD",
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = GUID

                    },
                    new CurrencyType
                    {
                        EnglishName = "Afghani",
                        PashtoName = "افغانۍ",
                        DariName = "افغانی",
                        Code = "AFN",
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = GUID

                    }); ;
                    await context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

    }
}