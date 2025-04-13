using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repos.Customer.Interfaces;
using Shop.Utility;
using System.Security.Claims;

namespace Shop.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class OrderController : Controller
	{
		private readonly IOrderRepository _orderRepository;
		private readonly UserManager<ApplicationUser> _userManager;

		public OrderController(IOrderRepository orderRepository, UserManager<ApplicationUser> userManager)
		{
			_orderRepository = orderRepository;
			_userManager = userManager;
		}

		public async Task<IActionResult> Checkout()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);

			var order = new Order
			{
				OrderDate = DateTime.Now
			};

			if (user != null)
			{
				order.Name = $"{user.FirstName} {user.LastName}";
				order.Email = user.Email;
				order.PhoneNo = user.PhoneNumber;
				order.Address = user.Address;
			}

			return View(order);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Checkout(Order order)
		{
			var products = HttpContext.Session.Get<List<Product>>("Products");

			if (products == null || !products.Any())
			{
				ModelState.AddModelError("", "Your cart is empty.");
				return View(order);
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = await _userManager.FindByIdAsync(userId);

			if (user != null)
			{
				order.UserId = user.Id;
				if (string.IsNullOrWhiteSpace(order.PhoneNo)) order.PhoneNo = user.PhoneNumber;
				if (string.IsNullOrWhiteSpace(order.Address)) order.Address = user.Address;
				if (string.IsNullOrWhiteSpace(order.Name)) order.Name = $"{user.FirstName} {user.LastName}";
				if (string.IsNullOrWhiteSpace(order.Email)) order.Email = user.Email;
			}

			order.OrderNo = await _orderRepository.GenerateOrderNumberAsync();
			order.OrderDate = DateTime.Now;
			order.OrderDetails = products.Select(p => new OrderDetails { ProductId = p.Id }).ToList();

			HttpContext.Session.Set("PendingOrder", order);
			HttpContext.Session.Set("PendingProducts", products);

			var totalAmount = products.Sum(p => p.Price);

			return RedirectToAction("Index", "Payment", new { amount = totalAmount });
		}

		[HttpGet]
		public async Task<IActionResult> MyOrders()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userId))
			{
				return RedirectToAction("Login", "Account", new { area = "Customer" });
			}

			var orders = await _orderRepository.GetUserOrdersAsync(userId);
			return View(orders);
		}
	}
}
