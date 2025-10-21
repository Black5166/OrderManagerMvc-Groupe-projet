using OrderManagerMvc.Models;

namespace OrderManager.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IList<OrderItem>> GetAllAsync();
        Task<OrderItem> GetDetailsAsync(int id);
        Task<OrderItem> CreateAsync(OrderItem orderItem);
        Task<bool> UpdateAsync(OrderItem orderItem);
        Task<bool> DeleteAsync(int id);
    }
}
