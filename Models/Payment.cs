using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceManager.Models
{
    public enum PaymentMethod
    {
        CASH,
        CARD,
        BANK_TRANSFER
    }

    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public DateTime PaidAt { get; set; } = DateTime.Now;

        [StringLength(60)]
        public string? ReferenceNo { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice? Invoice { get; set; }
    }
}
