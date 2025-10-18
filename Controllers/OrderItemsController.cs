using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;

namespace OrderManagerMvc.Controllers
{
    public class OrderItemsController(AppDbContext db)
    {
        private AppDbContext _db = db;

        // Controller actions for OrderItems would go here

        public async Task<IActionResult> Index()
        {
            List<OrderItem> orderItems = await _db.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .ToListAsync();
            return View(orderItems);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            OrderItem? orderItem = await _db.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderItem == null) return NotFound();
            return View(orderItem);
        }

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

        public async Task<IActionResult> Create(int? id)
        {
            if (id == null) return NotFound();
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            var orderItem = new OrderItem { OrderId = order.Id };
            return View(orderItem);
        }


        private IActionResult View(object orderItem)
        {
            // Assuming orderItem is of type OrderItem, you can cast it accordingly.
            if (orderItem is OrderItem item)
            {
                // Return the view with the order item model.
                return View(item);
            }
            // If the orderItem is not of the expected type, return a NotFound result.
            return NotFound();
        }

        private IActionResult NotFound()
        {
            NotFoundResult notFound = new();
            return notFound;
        }
    }
}
