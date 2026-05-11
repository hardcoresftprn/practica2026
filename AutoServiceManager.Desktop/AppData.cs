using System.Text.Json;

namespace AutoServiceManager.Desktop;

public sealed class AppData
{
    public List<UserAccount> Users { get; set; } = new();
    public List<ClientRecord> Clients { get; set; } = new();
    public List<VehicleRecord> Vehicles { get; set; } = new();
    public List<ServiceOrderRecord> Orders { get; set; } = new();
    public List<PartRecord> Parts { get; set; } = new();
    public List<InvoiceRecord> Invoices { get; set; } = new();
    public bool DarkTheme { get; set; }
}

public sealed class UserAccount
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = UserRoles.User;
    public bool IsActive { get; set; } = true;
}

public static class UserRoles
{
    public const string Administrator = "Administrator";
    public const string User = "User";

    public static readonly string[] All = { Administrator, User };

    public static string Normalize(string? role)
    {
        return string.Equals(role, Administrator, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(role, "ADMIN", StringComparison.OrdinalIgnoreCase)
                ? Administrator
                : User;
    }
}

public sealed class ClientRecord
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public string Email { get; set; } = "";
    public string Address { get; set; } = "";
    public string Idnp { get; set; } = "";
}

public sealed class VehicleRecord
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string LicensePlate { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public int Year { get; set; }
    public string Vin { get; set; } = "";
    public int Mileage { get; set; }
    public string Status { get; set; } = "Activ";
}

public sealed class ServiceOrderRecord
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public int ClientId { get; set; }
    public int VehicleId { get; set; }
    public string Complaint { get; set; } = "";
    public string Diagnosis { get; set; } = "";
    public string Mechanic { get; set; } = "";
    public string Status { get; set; } = "Noua";
    public DateTime PlannedAt { get; set; } = DateTime.Today;
    public decimal LaborCost { get; set; }
    public List<int> PartIds { get; set; } = new();
}

public sealed class PartRecord
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Code { get; set; } = "";
    public string Supplier { get; set; } = "";
    public int Quantity { get; set; }
    public int MinQuantity { get; set; } = 2;
    public decimal Price { get; set; }
}

public sealed class InvoiceRecord
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public int OrderId { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.Today;
    public decimal Subtotal { get; set; }
    public decimal Vat { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "Neachitata";
}

public sealed class DataStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public AppData Data { get; private set; } = new();

    public DataStore()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AutoServiceManagerDesktop");
        Directory.CreateDirectory(folder);
        _filePath = Path.Combine(folder, "autoservice-data.json");
        Load();
    }

    public void Load()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            Data = JsonSerializer.Deserialize<AppData>(json, _jsonOptions) ?? new AppData();
        }

        SeedIfEmpty();
        NormalizeUserRoles();
        Save();
    }

    public void Save()
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(Data, _jsonOptions));
    }

    public int NextId<T>(IEnumerable<T> records, Func<T, int> idSelector)
    {
        return records.Any() ? records.Max(idSelector) + 1 : 1;
    }

    public string DataFilePath => _filePath;

    private void SeedIfEmpty()
    {
        if (!Data.Users.Any())
        {
            Data.Users.Add(new UserAccount
            {
                Id = 1,
                FullName = "Alexei Ciobanu",
                Email = "testare@gmail.com",
                Password = "password",
                Role = UserRoles.Administrator
            });
        }

        if (Data.Users.All(u => !string.Equals(u.Email, "user@gmail.com", StringComparison.OrdinalIgnoreCase)))
        {
            Data.Users.Add(new UserAccount
            {
                Id = NextId(Data.Users, u => u.Id),
                FullName = "Utilizator Demo",
                Email = "user@gmail.com",
                Password = "password",
                Role = UserRoles.User
            });
        }

        if (!Data.Clients.Any())
        {
            Data.Clients.AddRange(new[]
            {
                new ClientRecord { Id = 1, FullName = "Ion Popescu", Phone = "0722 123 456", Email = "ion.popescu@email.ro", Address = "Str. Victoriei 15", Idnp = "1234567890123" },
                new ClientRecord { Id = 2, FullName = "Maria Ionescu", Phone = "0733 456 789", Email = "maria.ionescu@email.ro", Address = "Bd. Stefan cel Mare 22" },
                new ClientRecord { Id = 3, FullName = "Andrei Dumitrescu", Phone = "0744 789 012", Email = "andrei.d@email.ro" }
            });
        }

        if (!Data.Vehicles.Any())
        {
            Data.Vehicles.AddRange(new[]
            {
                new VehicleRecord { Id = 1, ClientId = 1, LicensePlate = "B 101 ABC", Brand = "Toyota", Model = "Corolla", Year = 2018, Vin = "JTDBR32E123456789", Mileage = 124000, Status = "In Service" },
                new VehicleRecord { Id = 2, ClientId = 2, LicensePlate = "KAA 234", Brand = "Volkswagen", Model = "Golf", Year = 2017, Vin = "WVWZZZ1KZ8W000001", Mileage = 156300, Status = "Activ" }
            });
        }

        if (!Data.Parts.Any())
        {
            Data.Parts.AddRange(new[]
            {
                new PartRecord { Id = 1, Name = "Filtru ulei", Code = "FIL-U-01", Supplier = "AutoParts SRL", Quantity = 12, MinQuantity = 3, Price = 85 },
                new PartRecord { Id = 2, Name = "Placute frana fata", Code = "BRK-F-22", Supplier = "Parts Expert", Quantity = 2, MinQuantity = 4, Price = 340 },
                new PartRecord { Id = 3, Name = "Ulei 5W30", Code = "OIL-5W30", Supplier = "AutoParts SRL", Quantity = 18, MinQuantity = 6, Price = 120 }
            });
        }

        if (!Data.Orders.Any())
        {
            Data.Orders.Add(new ServiceOrderRecord
            {
                Id = 1,
                Number = "CMD-2026-001",
                ClientId = 1,
                VehicleId = 1,
                Complaint = "Schimb ulei si verificare frane",
                Diagnosis = "Necesita filtru ulei si placute frana",
                Mechanic = "Mihai Stanciu",
                Status = "In lucru",
                PlannedAt = DateTime.Today,
                LaborCost = 450,
                PartIds = new List<int> { 1, 2, 3 }
            });
        }

        if (!Data.Invoices.Any())
        {
            var order = Data.Orders[0];
            var partsTotal = Data.Parts.Where(p => order.PartIds.Contains(p.Id)).Sum(p => p.Price);
            var subtotal = order.LaborCost + partsTotal;
            Data.Invoices.Add(new InvoiceRecord
            {
                Id = 1,
                Number = "FAC-2026-001",
                OrderId = order.Id,
                Subtotal = subtotal,
                Vat = decimal.Round(subtotal * 0.20m, 2),
                Total = decimal.Round(subtotal * 1.20m, 2),
                Status = "Neachitata"
            });
        }
    }

    private void NormalizeUserRoles()
    {
        foreach (var user in Data.Users)
        {
            user.Role = UserRoles.Normalize(user.Role);
        }
    }
}
