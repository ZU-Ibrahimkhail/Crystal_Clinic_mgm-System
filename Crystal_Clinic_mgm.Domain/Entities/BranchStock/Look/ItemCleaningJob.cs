using System.ComponentModel.DataAnnotations;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look
{
    public class ItemCleaningJob
    {
        [Key]
        public Guid id { get; set; }
        public int itemId { get; set; }
        public Item? item { get; set; }
        public int unitId { get; set; }
        public ItemUnit? unit { get; set; }
        public int quantity { get; set; }
        public DateTime dateToBeRestocked { get; set; }
    }
}
