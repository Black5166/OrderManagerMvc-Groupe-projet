using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;

namespace OrderManagerMvc.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ILogger<OrderItemsController> _logger;

        public OrderItemsController(AppDbContext db, ILogger<OrderItemsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: /OrderItems
        public async Task<IActionResult> Index()
        {
            var orderItems = await _db.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(o => o.Customer)
                .Include(oi => oi.Product)
                .ToListAsync();

            return View(orderItems);
        }

        // GET: /OrderItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _db.OrderItems
                .Include(oi => oi.Order)
                .ThenInclude(o => o.Customer)
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderItem == null) return NotFound();

            return View(orderItem);
        }

        // GET: /OrderItems/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_db.Orders.Include(o => o.Customer), "Id", "Id");
            ViewData["ProductId"] = new SelectList(_db.Products, "Id", "Name");
            return View();
        }

        // POST: /OrderItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderItem orderItem)
        {
            if (!ModelState.IsValid)
            {
                ViewData["OrderId"] = new SelectList(_db.Orders, "Id", "Id", orderItem.OrderId);
                ViewData["ProductId"] = new SelectList(_db.Products, "Id", "Name", orderItem.ProductId);
                return View(orderItem);
            }

            _db.OrderItems.Add(orderItem);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Order item created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _db.OrderItems.FindAsync(id);
            if (orderItem == null) return NotFound();

            ViewData["OrderId"] = new SelectList(_db.Orders, "Id", "Id", orderItem.OrderId);
            ViewData["ProductId"] = new SelectList(_db.Products, "Id", "Name", orderItem.ProductId);

            return View(orderItem);
        }

        // POST: /OrderItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderItem orderItem)
        {
            if (id != orderItem.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["OrderId"] = new SelectList(_db.Orders, "Id", "Id", orderItem.OrderId);
                ViewData["ProductId"] = new SelectList(_db.Products, "Id", "Name", orderItem.ProductId);
                return View(orderItem);
            }

            try
            {
                _db.Update(orderItem);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Order item updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_db.OrderItems.Any(e => e.Id == orderItem.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var orderItem = await _db.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderItem == null) return NotFound();

            return View(orderItem);
        }

        // POST: /OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderItem = await _db.OrderItems.FindAsync(id);
            if (orderItem == null) return NotFound();

            _db.OrderItems.Remove(orderItem);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Order item deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
