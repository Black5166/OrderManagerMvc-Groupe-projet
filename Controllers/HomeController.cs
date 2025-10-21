using Microsoft.AspNetCore.Mvc;
using OrderManagerMvc.Models;
using System.Diagnostics;

namespace OrderManagerMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Home page accessed at {Time}", DateTime.UtcNow);
            ViewData["Title"] = "Home";
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Privacy page accessed at {Time}", DateTime.UtcNow);
            ViewData["Title"] = "Privacy";
            return View();
        }

        // Handles both generic and custom error messages
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message = null)
        {
            _logger.LogError("Error occurred at {Time}: {Message}", DateTime.UtcNow, message ?? "Unknown error");

            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = message ?? "An unexpected error occurred. Please try again later."
            };

            return View(model);
        }
    }
}
