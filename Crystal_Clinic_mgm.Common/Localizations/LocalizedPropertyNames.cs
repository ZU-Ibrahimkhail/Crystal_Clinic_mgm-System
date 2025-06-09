namespace MEW_ERP.Common.Localizations
{
    public class LocalizedPropertyNames
    {
        public readonly Dictionary<string, string[]> LocalizeProperty = new();

        public LocalizedPropertyNames()
        {
            LocalizeProperty.Add("Language", new string[] { "زبان", "ژبه" });
            LocalizeProperty.Add("EnglishName", new string[] { "نام انگلیسی", "انګلیسی نوم" });
            LocalizeProperty.Add("DariName", new string[] { "نام دری", "دري نوم" });
            LocalizeProperty.Add("PashtoName", new string[] { "نام پشتو", "پښتو نوم" });
            LocalizeProperty.Add("Code", new string[] { "کود", "کود" });
            LocalizeProperty.Add("IsCCDepartment", new string[] { "اداره قابل کاپی گرفتن", "کاپی اخیستونکې اداره" });
        }
    }
}
