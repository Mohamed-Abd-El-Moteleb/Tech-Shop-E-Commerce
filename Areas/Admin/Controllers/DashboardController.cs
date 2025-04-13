using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Repositories.Interfaces;
namespace Shop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class DashboardController : Controller
	{
		private readonly IDashboardRepository _dashboardRepo;

		public DashboardController(IDashboardRepository dashboardRepo)
		{
			_dashboardRepo = dashboardRepo;
		}

		public async Task<IActionResult> Index()
		{
			var model = new DashboardViewModel
			{
				TotalOrders = await _dashboardRepo.GetTotalOrdersAsync(),
				TotalCustomers = await _dashboardRepo.GetTotalCustomersAsync(),
				TotalRevenue = await _dashboardRepo.GetTotalRevenueAsync()
			};

			return View(model);
		}
	}
}
