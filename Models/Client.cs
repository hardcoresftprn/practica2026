using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceManager.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(120)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        [StringLength(20)]
        public string? Idnp { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();
    }
}
