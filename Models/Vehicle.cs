using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceManager.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string Model { get; set; } = string.Empty;

        public short? ProductionYear { get; set; }

        [Required]
        [StringLength(30)]
        public string Vin { get; set; } = string.Empty;

        public int Mileage { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("ClientId")]
        public virtual Client? Client { get; set; }
    }
}
