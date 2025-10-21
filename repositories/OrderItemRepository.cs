using Microsoft.EntityFrameworkCore;
using OrderManager.Interfaces;
using OrderManagerMvc.Data;
using OrderManagerMvc.Models;

namespace OrderManager.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly AppDbContext _dbContext;

        public OrderItemRepository(IDbContextFactory<AppDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<OrderItem> CreateAsync(OrderItem orderItem)
        {
            var newOrderItem = await _dbContext.OrderItems.AddAsync(orderItem);
            await _dbContext.SaveChangesAsync();
            return newOrderItem.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var orderItem = await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.Id == id);

            if (orderItem == null) return false;

            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IList<OrderItem>> GetAllAsync()
        {
            return await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .ToListAsync();
        }

        public Task<OrderItem> GetDetailsAsync(int id)
        {
            return _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .FirstOrDefaultAsync(oi => oi.Id == id)!;
        }

        public async Task<bool> UpdateAsync(OrderItem orderItem)
        {
            var existingOrderItem = await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.Id == orderItem.Id);

            if (existingOrderItem == null) return false;

            existingOrderItem.OrderId = orderItem.OrderId;
            existingOrderItem.ProductId = orderItem.ProductId;
            existingOrderItem.Quantity = orderItem.Quantity;
            existingOrderItem.UnitPrice = orderItem.UnitPrice;

            _dbContext.OrderItems.Update(existingOrderItem);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}