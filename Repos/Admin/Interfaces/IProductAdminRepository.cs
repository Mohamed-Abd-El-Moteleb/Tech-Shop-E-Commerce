using Shop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Repos.Admin.Interfaces
{
	public interface IProductAdminRepository
	{
		Task<IEnumerable<Product>> GetAllAsync();
		Task<Product?> GetByIdAsync(int id);
		Task<IEnumerable<Product>> FilteredProductsAsync(string? search, string? sortBy, string? sortOrder, int? productTypeId, int? specialTagId, bool? isAvailable);
		Task AddAsync(Product product);
		Task UpdateAsync(Product product);
		Task DeleteAsync(Product product);
		Task<bool> ExistsAsync(string name);
	}
}
