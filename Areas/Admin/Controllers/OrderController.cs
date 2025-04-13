using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repos;
using Shop.Repositories.Interfaces;

namespace Shop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
		private readonly IOrderAdminRepository _orderRepo;

		public OrderController(IOrderAdminRepository orderRepo)
		{
			_orderRepo = orderRepo;
		}

		public async Task<IActionResult> Index()
		{
			var orders = await _orderRepo.GetAllAsync();
			return View(orders);
		}

		public async Task<IActionResult> FilteredOrders(string name, string phone, string orderDate, string status)
		{
			var filtered = await _orderRepo.FilterAsync(name, phone, orderDate, status);

			var html = string.Join("", filtered.Select(order => $@"
                <tr>
                    <td>{order.OrderNo}</td>
                    <td>{order.Name}</td>
                    <td>{order.PhoneNo}</td>
                    <td>{order.OrderDate.ToShortDateString()}</td>
                    <td>{order.Status}</td>
                    <td>
                        <a href='/Admin/Order/Details/{order.Id}' class='btn btn-info btn-sm'>Details</a>
                        <a href='/Admin/Order/Edit/{order.Id}' class='btn btn-primary btn-sm'>Edit</a>
                        <a href='/Admin/Order/Delete/{order.Id}' class='btn btn-danger btn-sm'>Delete</a>
                    </td>
                </tr>
            "));

			return Content(html, "text/html");
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var order = await _orderRepo.GetByIdAsync(id.Value);
			if (order == null) return NotFound();

			return View(order);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var order = await _orderRepo.GetByIdAsync(id.Value);
			if (order == null) return NotFound();

			return View(order);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] Order updatedOrder)
		{
			if (id != updatedOrder.Id)
				return NotFound();

			var order = await _orderRepo.GetByIdAsync(id);
			if (order == null)
				return NotFound();

			order.Status = updatedOrder.Status;

			try
			{
				await _orderRepo.UpdateAsync(order);
				TempData["Success"] = "Order status updated!";
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				if (!await _orderRepo.ExistsAsync(id))
					return NotFound();
				else
					throw;
			}
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var order = await _orderRepo.GetByIdAsync(id.Value);
			if (order == null) return NotFound();

			return View(order);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var order = await _orderRepo.GetByIdAsync(id);
			if (order != null)
			{
				await _orderRepo.DeleteAsync(order);
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
