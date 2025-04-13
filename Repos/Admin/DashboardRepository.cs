using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Repositories.Interfaces;

namespace Shop.Repositories.Implementations
{
	public class DashboardRepository : IDashboardRepository
	{
		private readonly ApplicationDbContext _context;

		public DashboardRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int> GetTotalOrdersAsync()
		{
			return await _context.Orders.CountAsync();
		}

		public async Task<int> GetTotalCustomersAsync()
		{
			return await _context.Orders
				.Select(o => o.UserId)
				.Distinct()
				.CountAsync();
		}

		public async Task<decimal> GetTotalRevenueAsync()
		{
			return await _context.OrderDetails
				.Include(od => od.Product)
				.SumAsync(od => od.Product.Price);
		}
	}
}
