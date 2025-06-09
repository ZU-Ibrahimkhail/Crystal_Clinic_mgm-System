namespace Crystal_Clinic_Mgm.Domain.Entities.UMS
{
    public class Applications : AuditableEntity
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Abbrevation { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public string DefaultRoute { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public static implicit operator Applications(int v)
        {
            throw new NotImplementedException();
        }
    }
}
