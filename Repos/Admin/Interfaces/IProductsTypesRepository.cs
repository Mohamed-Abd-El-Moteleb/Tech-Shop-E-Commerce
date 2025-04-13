using Shop.Models;

namespace Shop.Repos.Admin.Interfaces
{
	public interface IProductTypesRepository
	{
		Task<List<ProductTypes>> GetAllAsync();
		Task<ProductTypes?> GetByIdAsync(int id);
		Task AddAsync(ProductTypes productType);
		Task UpdateAsync(ProductTypes productType);
		Task DeleteAsync(int id);
	}
}
