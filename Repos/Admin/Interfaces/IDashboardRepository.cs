namespace Shop.Repositories.Interfaces
{
	public interface IDashboardRepository
	{
		Task<int> GetTotalOrdersAsync();
		Task<int> GetTotalCustomersAsync();
		Task<decimal> GetTotalRevenueAsync();
	}
}
