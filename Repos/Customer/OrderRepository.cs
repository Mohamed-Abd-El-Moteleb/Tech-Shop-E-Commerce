using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Repos.Customer.Interfaces;


namespace Shop.Repos.Customer
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Order>> GetUserOrdersAsync(string userId)
		{
			return await _context.Orders
				.Include(o => o.OrderDetails)
					.ThenInclude(od => od.Product)
				.Where(o => o.UserId == userId)
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();
		}

		public async Task<string> GenerateOrderNumberAsync()
		{
			int count = await _context.Orders.CountAsync() + 1;
			return count.ToString("000");
		}

		public async Task SaveOrderAsync(Order order)
		{
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
		}
	}
}
