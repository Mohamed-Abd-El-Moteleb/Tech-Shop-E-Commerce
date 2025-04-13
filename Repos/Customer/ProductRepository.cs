using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Repos.Customer.Interfaces;
using X.PagedList;
using X.PagedList.Extensions;

namespace Shop.Repositories.Implementations
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public IPagedList<Product> GetAllPaged(int pageNumber, int pageSize)
		{
			return _context.Products
				.Include(p => p.ProductType)
				.Include(p => p.SpecialTag)
				.OrderBy(p => p.Name)
				.ToPagedList(pageNumber, pageSize);
		}


		public async Task<Product> GetByIdAsync(int id)
		{
			return await _context.Products
				.Include(p => p.ProductType)
				.Include(p => p.SpecialTag)
				.FirstOrDefaultAsync(p => p.Id == id);
		}

		public async Task<List<ProductTypes>> GetAllProductTypesAsync()
		{
			return await _context.ProductTypes.ToListAsync();
		}

		public IPagedList<Product> GetProductsByType(int typeId, int pageNumber, int pageSize)
		{
			return _context.Products
				.Where(p => p.ProductTypeId == typeId)
				.Include(p => p.ProductType)
				.Include(p => p.SpecialTag)
				.ToPagedList(pageNumber, pageSize);
		}

		public async Task<ProductTypes> GetProductTypeByIdAsync(int id)
		{
			return await _context.ProductTypes.FirstOrDefaultAsync(pt => pt.Id == id);
		}
	}
}
