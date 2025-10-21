using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;

namespace OrderManagerMvc.Controllers
{
    public class CustomersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(AppDbContext db, ILogger<CustomersController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: /Customers
        public async Task<IActionResult> Index()
        {
            var customers = await _db.Customers
                .Include(c => c.Orders)
                .ToListAsync();
            return View(customers);
        }

        // GET: /Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _db.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        // GET: /Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            try
            {
                _db.Add(customer);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Customer created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                ModelState.AddModelError("", "An error occurred while saving.");
                return View(customer);
            }
        }

        // GET: /Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _db.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: /Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(customer);

            try
            {
                _db.Update(customer);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Customer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Customers.AnyAsync(e => e.Id == id))
                    return NotFound();

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer");
                ModelState.AddModelError("", "An unexpected error occurred while saving.");
                return View(customer);
            }
        }

        // GET: /Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _db.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: /Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _db.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return NotFound();

            if (customer.Orders.Any())
            {
                ModelState.AddModelError("", "❌ Cannot delete a customer who has existing orders.");
                return View("Delete", customer);
            }

            try
            {
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Customer deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                ModelState.AddModelError("", "An unexpected error occurred while deleting.");
                return View("Delete", customer);
            }
        }
    }
}
