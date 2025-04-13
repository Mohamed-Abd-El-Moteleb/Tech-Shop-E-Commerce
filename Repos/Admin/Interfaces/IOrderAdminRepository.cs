using Shop.Models;

namespace Shop.Repositories.Interfaces
{
	public interface IOrderAdminRepository
	{
		Task<IEnumerable<Order>> GetAllAsync();
		Task<IEnumerable<Order>> FilterAsync(string name, string phone, string orderDate, string status);
		Task<Order?> GetByIdAsync(int id);
		Task AddAsync(Order order);
		Task UpdateAsync(Order order);
		Task DeleteAsync(Order order);
		Task<bool> ExistsAsync(int id);
	}
}
