using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using AutoServiceManager.Data;
using AutoServiceManager.Models;

namespace AutoServiceManager.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();

                // Seed sample data if empty for demonstration
                if (!await _context.Clients.AnyAsync())
                {
                    _context.Clients.AddRange(GetSampleClients());
                    await _context.SaveChangesAsync();
                }

                var clients = await _context.Clients
                    .OrderBy(c => c.FullName)
                    .ToListAsync();

                return View(clients);
            }
            catch (Exception ex) when (IsDatabaseUnavailable(ex))
            {
                ViewBag.DatabaseWarning = "MySQL nu este conectat. Se afișează date demo; modificările nu se salvează.";
                return View(GetSampleClients());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!CurrentUserIsAdministrator())
            {
                TempData["ErrorMessage"] = "Nu ai drepturi pentru adăugare. Contul tău permite doar vizualizare.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Completează câmpurile obligatorii pentru client.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Clientul a fost adăugat.";
            }
            catch (Exception ex) when (IsDatabaseUnavailable(ex))
            {
                TempData["ErrorMessage"] = "Clientul nu a fost salvat deoarece MySQL nu este conectat.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Client client)
        {
            if (!CurrentUserIsAdministrator())
            {
                TempData["ErrorMessage"] = "Nu ai drepturi pentru editare. Contul tău permite doar vizualizare.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Clientul a fost actualizat.";
                }
                catch (Exception ex) when (IsDatabaseUnavailable(ex))
                {
                    TempData["ErrorMessage"] = "Clientul nu a fost actualizat deoarece MySQL nu este conectat.";
                }

                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Datele clientului nu sunt valide.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (!CurrentUserIsAdministrator())
            {
                TempData["ErrorMessage"] = "Nu ai drepturi pentru ștergere. Contul tău permite doar vizualizare.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var client = await _context.Clients.FindAsync(id);

                if (client is null)
                {
                    TempData["ErrorMessage"] = "Clientul nu a fost găsit.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Clientul a fost șters.";
            }
            catch (Exception ex) when (IsDatabaseUnavailable(ex))
            {
                TempData["ErrorMessage"] = "Clientul nu a fost șters deoarece MySQL nu este conectat.";
            }

            return RedirectToAction(nameof(Index));
        }

        private static bool IsDatabaseUnavailable(Exception ex)
        {
            return ex is MySqlException
                || ex is InvalidOperationException
                || ex is TimeoutException
                || ex is DbUpdateException;
        }

        private bool CurrentUserIsAdministrator()
        {
            return string.Equals(
                HttpContext.Session.GetString("UserRole"),
                "ADMINISTRATOR",
                StringComparison.OrdinalIgnoreCase);
        }

        private static List<Client> GetSampleClients()
        {
            return new List<Client>
            {
                new Client { ClientId = 1, FullName = "Ion Popescu", Phone = "0722 123 456", Email = "ion.popescu@email.ro", Address = "Str. Victoriei 15, Bucuresti", Idnp = "1234567890123" },
                new Client { ClientId = 2, FullName = "Maria Ionescu", Phone = "0733 456 789", Email = "maria.ionescu@email.ro" },
                new Client { ClientId = 3, FullName = "Andrei Dumitrescu", Phone = "0744 789 012", Email = "andrei.d@email.ro" },
                new Client { ClientId = 4, FullName = "Ana Maria Petrescu", Phone = "0777 678 901", Email = "ana.petrescu@email.ro" },
                new Client { ClientId = 5, FullName = "George Vasile", Phone = "0788 901 234", Email = "george.v@email.ro" },
                new Client { ClientId = 6, FullName = "Elena Georgescu", Phone = "0755 012 345", Email = "elena.g@email.ro" },
                new Client { ClientId = 7, FullName = "Mihai Stanciu", Phone = "0766 345 678", Email = "mihai.s@email.ro" }
            };
        }
    }
}
