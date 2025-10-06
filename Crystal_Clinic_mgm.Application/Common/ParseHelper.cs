namespace Crystal_Clinic_Mgm.Application.Common
{
    public class ParseHelper
    {
        public static List<int> ParseServices(string services)
        {
            if (string.IsNullOrWhiteSpace(services))
            {
                return new List<int>();
            }

            return services.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                           .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
                           .Where(id => id.HasValue)
                           .Select(id => id.Value)
                           .ToList();
        }
    }
}
