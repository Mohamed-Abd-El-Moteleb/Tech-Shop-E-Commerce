using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repos.Customer.Interfaces;
using Shop.Utility;
using Shop.ViewModels.Stripe;
using Stripe.Checkout;

namespace Shop.Areas.Customer.Controllers
{
	[Area("Customer")]

	public class PaymentController : Controller
	{
		private readonly IConfiguration _config;
		private readonly IPaymentRepository _paymentRepository;

		public PaymentController(IConfiguration config, IPaymentRepository paymentRepository)
		{
			_config = config;
			_paymentRepository = paymentRepository;
		}

		public IActionResult Index(decimal amount)
		{
			var viewModel = new StripePaymentViewModel
			{
				Amount = amount
			};
			return View(viewModel);
		}

		[HttpPost]
		public IActionResult CreateCheckoutSession(StripePaymentViewModel model)
		{
			var domain = "https://localhost:44367/";
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = "usd",
							UnitAmount = (long)(model.Amount * 100),
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = "Order Payment"
							}
						},
						Quantity = 1
					}
				},
				Mode = "payment",
				SuccessUrl = domain + "Customer/Payment/Success",
				CancelUrl = domain + "Customer/Payment/Cancel"
			};

			var service = new SessionService();
			Session session = service.Create(options);
			return Redirect(session.Url);
		}

		public async Task<IActionResult> Success()
		{
			var order = HttpContext.Session.Get<Order>("PendingOrder");
			var products = HttpContext.Session.Get<List<Product>>("PendingProducts");

			if (order == null || products == null)
				return RedirectToAction("Index", "Home");

			await _paymentRepository.SaveOrderAsync(order, products);

			HttpContext.Session.Remove("Products");
			HttpContext.Session.Remove("PendingOrder");
			HttpContext.Session.Remove("PendingProducts");

			return View();
		}

		public IActionResult Cancel()
		{
			return View();
		}
	}
}
