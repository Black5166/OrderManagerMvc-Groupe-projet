using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;

namespace OrderManagerMvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext db, ILogger<ProductsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: /Products
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.ToListAsync();
            return View(products);
        }


        // GET: /Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _db.Add(product);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Product created successfully!";
                _logger.LogInformation("New product '{ProductName}' created at {Time}", product.Name, DateTime.UtcNow);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: /Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _db.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(product);
                    await _db.SaveChangesAsync();
                    TempData["Success"] = "Product updated successfully!";
                    _logger.LogInformation("Product '{ProductName}' (ID {Id}) updated at {Time}", product.Name, product.Id, DateTime.UtcNow);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Products.Any(e => e.Id == product.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: /Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: /Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Product deleted successfully!";
                _logger.LogWarning("Product '{ProductName}' (ID {Id}) deleted at {Time}", product?.Name, id, DateTime.UtcNow);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
