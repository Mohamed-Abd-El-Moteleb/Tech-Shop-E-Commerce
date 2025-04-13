using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Repos.Customer.Interfaces;

namespace Shop.Repositories.Implementations
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly ApplicationDbContext _context;

		public PaymentRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task SaveOrderAsync(Order order, List<Product> products)
		{
			order.OrderDetails = products.Select(p => new OrderDetails
			{
				ProductId = p.Id
			}).ToList();

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
		}
	}
}
