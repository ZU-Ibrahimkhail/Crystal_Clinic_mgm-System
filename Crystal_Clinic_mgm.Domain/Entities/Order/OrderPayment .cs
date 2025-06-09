using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;

public class OrderPayment : AuditableEntity
{
    [Key]
    public int PaymentId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public string TransactionReference { get; set; } = string.Empty;
    public bool IsAdvancePayment { get; set; }

    // Foreign Key
    public int OrderId { get; set; }

    // Navigation
    public Orders? Order { get; set; }
}

