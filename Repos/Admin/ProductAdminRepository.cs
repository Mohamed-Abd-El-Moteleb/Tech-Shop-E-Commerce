using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Repos.Admin.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Repos.Admin
{
	public class ProductAdminRepository : IProductAdminRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductAdminRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Product>> GetAllAsync()
		{
			return await _context.Products
				.Include(p => p.ProductType)
				.Include(p => p.SpecialTag)
				.ToListAsync();
		}

		public async Task<Product?> GetByIdAsync(int id)
		{
			return await _context.Products
				.Include(p => p.ProductType)
				.Include(p => p.SpecialTag)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<IEnumerable<Product>> FilteredProductsAsync(string? search, string? sortBy, string? sortOrder, int? productTypeId, int? specialTagId, bool? isAvailable)
		{
			var query = _context.Products
				.Include(p => p.ProductType)
				.Include(p => p.SpecialTag)
				.AsQueryable();

			if (!string.IsNullOrWhiteSpace(search))
				query = query.Where(p => p.Name.Contains(search));

			if (productTypeId.HasValue)
				query = query.Where(p => p.ProductTypeId == productTypeId);

			if (specialTagId.HasValue)
				query = query.Where(p => p.SpecialTagId == specialTagId);

			if (isAvailable.HasValue)
				query = query.Where(p => p.IsAvailable == isAvailable);

			query = (sortBy?.ToLower(), sortOrder?.ToLower()) switch
			{
				("price", "desc") => query.OrderByDescending(p => p.Price),
				("price", _) => query.OrderBy(p => p.Price),
				("name", "desc") => query.OrderByDescending(p => p.Name),
				("name", _) => query.OrderBy(p => p.Name),
				("color", "desc") => query.OrderByDescending(p => p.ProductColor),
				("color", _) => query.OrderBy(p => p.ProductColor),
				_ => query.OrderBy(p => p.Name)
			};

			return await query.ToListAsync();
		}

		public async Task AddAsync(Product product)
		{
			_context.Products.Add(product);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Product product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Product product)
		{
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> ExistsAsync(string name)
		{
			return await _context.Products.AnyAsync(p => p.Name == name);
		}
	}
}
