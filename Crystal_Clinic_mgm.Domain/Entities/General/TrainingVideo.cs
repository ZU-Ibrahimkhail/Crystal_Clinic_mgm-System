namespace Crystal_Clinic_Mgm.Domain.Entities.General
{
    public class TrainingVideo : AuditableEntity
    {
        public int Id { get; set; }
        public string DariTitle { get; set; } = string.Empty;
        public string PashtoTitle { get; set; } = string.Empty;
        public string Application { get; set; } = string.Empty;
        public string Poster { get; set; } = string.Empty;
        public string DariVideoPath { get; set; } = string.Empty;
        public string PashtoVideoPath { get; set; } = string.Empty;
    }
}
