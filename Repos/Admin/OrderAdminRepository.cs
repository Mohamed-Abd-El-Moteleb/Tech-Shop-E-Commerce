using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Repositories.Interfaces;

namespace Shop.Repositories
{
	public class OrderAdminRepository : IOrderAdminRepository
	{
		private readonly ApplicationDbContext _context;

		public OrderAdminRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Order>> GetAllAsync()
		{
			return await _context.Orders
				.Include(o => o.OrderDetails)
				.ThenInclude(od => od.Product)
				.OrderByDescending(o => o.OrderDate)
				.ToListAsync();
		}

		public async Task<IEnumerable<Order>> FilterAsync(string name, string phone, string orderDate, string status)
		{
			var query = _context.Orders
				.Include(o => o.OrderDetails)
				.ThenInclude(od => od.Product)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(name))
				query = query.Where(o => o.Name.Contains(name));
			if (!string.IsNullOrWhiteSpace(phone))
				query = query.Where(o => o.PhoneNo.Contains(phone));
			if (!string.IsNullOrWhiteSpace(orderDate) && DateTime.TryParse(orderDate, out var parsedDate))
				query = query.Where(o => o.OrderDate.Date == parsedDate.Date);
			if (!string.IsNullOrWhiteSpace(status))
				query = query.Where(o => o.Status == status);

			return await query.ToListAsync();
		}

		public async Task<Order?> GetByIdAsync(int id)
		{
			return await _context.Orders
				.Include(o => o.OrderDetails)
				.ThenInclude(od => od.Product)
				.FirstOrDefaultAsync(o => o.Id == id);
		}

		public async Task AddAsync(Order order)
		{
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Order order)
		{
			_context.Orders.Update(order);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Order order)
		{
			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ExistsAsync(int id)
		{
			return await _context.Orders.AnyAsync(o => o.Id == id);
		}
	}
}
