using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceManager.Models
{
    public enum OrderStatus
    {
        NEW,
        IN_PROGRESS,
        COMPLETED,
        CANCELLED
    }

    public class ServiceOrder
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public int? AssignedMechanicId { get; set; }

        [Required]
        [StringLength(30)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public string Complaint { get; set; } = string.Empty;

        public string? Diagnosis { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.NEW;

        [Column(TypeName = "decimal(10,2)")]
        public decimal LaborCost { get; set; } = 0.00m;

        public DateTime? PlannedStart { get; set; }
        public DateTime? PlannedEnd { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ClosedAt { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }

        [ForeignKey("VehicleId")]
        public virtual Vehicle? Vehicle { get; set; }

        [ForeignKey("AssignedMechanicId")]
        public virtual User? Mechanic { get; set; }
    }
}
