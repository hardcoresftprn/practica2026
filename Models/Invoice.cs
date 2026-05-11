using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceManager.Models
{
    public enum PaymentStatus
    {
        UNPAID,
        PARTIAL,
        PAID
    }

    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(30)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public DateTime IssueDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; } = 0.00m;

        [Column(TypeName = "decimal(5,2)")]
        public decimal VatPercent { get; set; } = 20.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal VatAmount { get; set; } = 0.00m;

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; } = 0.00m;

        [Required]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.UNPAID;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("OrderId")]
        public virtual ServiceOrder? Order { get; set; }
    }
}
