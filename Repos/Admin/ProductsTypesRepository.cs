using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Repos.Admin.Interfaces;

namespace Shop.Repos.Admin
{
	public class ProductTypesRepository : IProductTypesRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductTypesRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<ProductTypes>> GetAllAsync()
		{
			return await _context.ProductTypes.ToListAsync();
		}

		public async Task<ProductTypes?> GetByIdAsync(int id)
		{
			return await _context.ProductTypes.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task AddAsync(ProductTypes productType)
		{
			await _context.ProductTypes.AddAsync(productType);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(ProductTypes productType)
		{
			_context.ProductTypes.Update(productType);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var productType = await GetByIdAsync(id);
			if (productType != null)
			{
				_context.ProductTypes.Remove(productType);
				await _context.SaveChangesAsync();
			}
		}
	}
}
