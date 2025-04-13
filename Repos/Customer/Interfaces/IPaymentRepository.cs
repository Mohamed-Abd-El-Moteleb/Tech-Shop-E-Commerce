using Shop.Models;

namespace Shop.Repos.Customer.Interfaces
{
	public interface IPaymentRepository
	{
		Task SaveOrderAsync(Order order, List<Product> products);
	}
}
