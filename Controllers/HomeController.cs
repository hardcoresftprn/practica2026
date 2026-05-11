using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using AutoServiceManager.Data;
using AutoServiceManager.Models;

namespace AutoServiceManager.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            await _context.Database.EnsureCreatedAsync();

            ViewBag.TotalOrders = await _context.ServiceOrders.CountAsync();
            ViewBag.ActiveRepairs = await _context.ServiceOrders.CountAsync(order => order.Status == OrderStatus.IN_PROGRESS);
            ViewBag.TotalRevenue = await _context.Invoices
                .Where(invoice => invoice.PaymentStatus == PaymentStatus.PAID || invoice.PaymentStatus == PaymentStatus.PARTIAL)
                .SumAsync(invoice => (decimal?)invoice.TotalAmount) ?? 0m;
            ViewBag.ClientsCount = await _context.Clients.CountAsync();
        }
        catch (Exception ex) when (IsDatabaseUnavailable(ex))
        {
            ViewBag.DatabaseWarning = "MySQL nu este conectat sau schema nu este importată. Verifică baza practica pe 127.0.0.1:3306.";
            ViewBag.TotalOrders = 0;
            ViewBag.ActiveRepairs = 0;
            ViewBag.TotalRevenue = 0m;
            ViewBag.ClientsCount = 0;
        }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private static bool IsDatabaseUnavailable(Exception ex)
    {
        return ex is MySqlException
            || ex is InvalidOperationException
            || ex is TimeoutException
            || ex is DbUpdateException;
    }
}
