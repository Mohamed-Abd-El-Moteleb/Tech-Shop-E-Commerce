using Shop.Models;
using X.PagedList;

namespace Shop.Repos.Customer.Interfaces
{
	public interface IProductRepository
	{
		IPagedList<Product> GetAllPaged(int pageNumber, int pageSize);
		Task<Product> GetByIdAsync(int id);
		Task<List<ProductTypes>> GetAllProductTypesAsync();
		IPagedList<Product> GetProductsByType(int typeId, int pageNumber, int pageSize);
		Task<ProductTypes> GetProductTypeByIdAsync(int id);
	}
}
