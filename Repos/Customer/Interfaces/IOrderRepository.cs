using Shop.Models;

namespace Shop.Repos.Customer.Interfaces
{
	public interface IOrderRepository
	{
		Task<List<Order>> GetUserOrdersAsync(string userId);
		Task<string> GenerateOrderNumberAsync();
		Task SaveOrderAsync(Order order);
	}
}
