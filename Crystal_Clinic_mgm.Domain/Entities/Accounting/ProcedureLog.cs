using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class ProcedureLog : AuditableEntity
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public Visit? Visit { get; set; }
        public int FixedAssetId { get; set; }
        public FixedAsset? FixedAsset { get; set; }
        public int? RoomId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal DurationMinutes { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
