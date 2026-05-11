using Microsoft.EntityFrameworkCore;
using AutoServiceManager.Models;

namespace AutoServiceManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("clients");
                entity.HasKey(c => c.ClientId);
                entity.Property(c => c.ClientId).HasColumnName("client_id");
                entity.Property(c => c.FullName).HasColumnName("full_name");
                entity.Property(c => c.Phone).HasColumnName("phone");
                entity.Property(c => c.Email).HasColumnName("email");
                entity.Property(c => c.Address).HasColumnName("address");
                entity.Property(c => c.Idnp).HasColumnName("idnp");
                entity.Property(c => c.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("vehicles");
                entity.HasKey(v => v.VehicleId);
                entity.Property(v => v.VehicleId).HasColumnName("vehicle_id");
                entity.Property(v => v.ClientId).HasColumnName("client_id");
                entity.Property(v => v.LicensePlate).HasColumnName("license_plate");
                entity.Property(v => v.Brand).HasColumnName("brand");
                entity.Property(v => v.Model).HasColumnName("model");
                entity.Property(v => v.ProductionYear).HasColumnName("production_year");
                entity.Property(v => v.Vin).HasColumnName("vin");
                entity.Property(v => v.Mileage).HasColumnName("mileage");
                entity.Property(v => v.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.UserId).HasColumnName("user_id");
                entity.Property(u => u.FullName).HasColumnName("full_name");
                entity.Property(u => u.Email).HasColumnName("email");
                entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
                entity.Property(u => u.Role).HasColumnName("role").HasConversion<string>();
                entity.Property(u => u.IsActive).HasColumnName("is_active");
                entity.Property(u => u.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<ServiceOrder>(entity =>
            {
                entity.ToTable("service_orders");
                entity.HasKey(so => so.OrderId);
                entity.Property(so => so.OrderId).HasColumnName("order_id");
                entity.Property(so => so.ClientId).HasColumnName("client_id");
                entity.Property(so => so.VehicleId).HasColumnName("vehicle_id");
                entity.Property(so => so.AssignedMechanicId).HasColumnName("assigned_mechanic_id");
                entity.Property(so => so.OrderNumber).HasColumnName("order_number");
                entity.Property(so => so.Complaint).HasColumnName("complaint");
                entity.Property(so => so.Diagnosis).HasColumnName("diagnosis");
                entity.Property(so => so.Status).HasColumnName("status").HasConversion<string>();
                entity.Property(so => so.LaborCost).HasColumnName("labor_cost").HasColumnType("decimal(10,2)");
                entity.Property(so => so.PlannedStart).HasColumnName("planned_start");
                entity.Property(so => so.PlannedEnd).HasColumnName("planned_end");
                entity.Property(so => so.CreatedAt).HasColumnName("created_at");
                entity.Property(so => so.ClosedAt).HasColumnName("closed_at");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("invoices");
                entity.HasKey(i => i.InvoiceId);
                entity.Property(i => i.InvoiceId).HasColumnName("invoice_id");
                entity.Property(i => i.OrderId).HasColumnName("order_id");
                entity.Property(i => i.InvoiceNumber).HasColumnName("invoice_number");
                entity.Property(i => i.IssueDate).HasColumnName("issue_date");
                entity.Property(i => i.DueDate).HasColumnName("due_date");
                entity.Property(i => i.Subtotal).HasColumnName("subtotal").HasColumnType("decimal(10,2)");
                entity.Property(i => i.VatPercent).HasColumnName("vat_percent").HasColumnType("decimal(5,2)");
                entity.Property(i => i.VatAmount).HasColumnName("vat_amount").HasColumnType("decimal(10,2)");
                entity.Property(i => i.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(10,2)");
                entity.Property(i => i.PaymentStatus).HasColumnName("payment_status").HasConversion<string>();
                entity.Property(i => i.CreatedAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payments");
                entity.HasKey(p => p.PaymentId);
                entity.Property(p => p.PaymentId).HasColumnName("payment_id");
                entity.Property(p => p.InvoiceId).HasColumnName("invoice_id");
                entity.Property(p => p.Amount).HasColumnName("amount").HasColumnType("decimal(10,2)");
                entity.Property(p => p.PaymentMethod).HasColumnName("payment_method").HasConversion<string>();
                entity.Property(p => p.PaidAt).HasColumnName("paid_at");
                entity.Property(p => p.ReferenceNo).HasColumnName("reference_no");
            });

            // Configure unique keys and relationships based on the SQL schema.
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Phone)
                .IsUnique();
            
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.LicensePlate)
                .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.Vin)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<ServiceOrder>()
                .HasIndex(so => so.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNumber)
                .IsUnique();

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.OrderId)
                .IsUnique();

            // Set up cascading deletes/restrictions
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Client)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Client)
                .WithMany(c => c.ServiceOrders)
                .HasForeignKey(so => so.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Vehicle)
                .WithMany()
                .HasForeignKey(so => so.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceOrder>()
                .HasOne(so => so.Mechanic)
                .WithMany()
                .HasForeignKey(so => so.AssignedMechanicId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithOne()
                .HasForeignKey<Invoice>(i => i.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Invoice)
                .WithMany()
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
