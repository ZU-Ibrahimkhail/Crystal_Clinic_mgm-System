namespace Crystal_Clinic_Mgm.Application.Common.ViewModels
{
    public class GetDocumentCCBranchModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        //public int DocumentNumber { get; set; } 
        public string Comments { get; set; } = string.Empty;
        public int? OutNumber { get; set; }
        public int? OutBookRecievedNumber { get; set; }
        public DateTime? OutDate { get; set; }
        public int? OutBranchId { get; set; }
        public string? OutBranchName { get; set; } = string.Empty;
    }
}
