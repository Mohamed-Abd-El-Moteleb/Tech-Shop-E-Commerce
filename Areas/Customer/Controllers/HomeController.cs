using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repos.Customer.Interfaces;
using Shop.Utility;

namespace Shop.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly IProductRepository _productRepository;

		public HomeController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public IActionResult Index(int? page)
		{
			int pageNumber = page ?? 1;
			int pageSize = 8;

			var products = _productRepository.GetAllPaged(pageNumber, pageSize);

			return View(products);
		}

		public async Task<IActionResult> Details(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		[HttpGet]
		public async Task<IActionResult> AddToCart(int id)
		{
			var product = await _productRepository.GetByIdAsync(id);
			if (product == null)
			{
				return NotFound();
			}

			var products = HttpContext.Session.Get<List<Product>>("Products") ?? new List<Product>();
			products.Add(product);
			HttpContext.Session.Set("Products", products);

			return View("Details", product);
		}

		public IActionResult RemoveFromCart(int id)
		{
			var products = HttpContext.Session.Get<List<Product>>("Products");
			if (products != null)
			{
				var product = products.FirstOrDefault(p => p.Id == id);
				if (product != null)
				{
					products.Remove(product);
					HttpContext.Session.Set("Products", products);
				}
			}
			return RedirectToAction("Cart");
		}

		public IActionResult RemoveFromCartInPage(int id)
		{
			var products = HttpContext.Session.Get<List<Product>>("Products");
			if (products != null)
			{
				var product = products.FirstOrDefault(p => p.Id == id);
				if (product != null)
				{
					products.Remove(product);
					HttpContext.Session.Set("Products", products);
				}
			}
			return RedirectToAction("Index");
		}

		public IActionResult Cart()
		{
			var products = HttpContext.Session.Get<List<Product>>("Products") ?? new List<Product>();
			return View(products);
		}

		public async Task<IActionResult> ProductTypes()
		{
			var productTypes = await _productRepository.GetAllProductTypesAsync();
			return View(productTypes);
		}

		public IActionResult ProductsByType(int id, int? page)
		{
			int pageNumber = page ?? 1;
			int pageSize = 8;

			var products = _productRepository.GetProductsByType(id, pageNumber, pageSize);
			var productType = _productRepository.GetProductTypeByIdAsync(id).Result;

			ViewBag.ProductTypeName = productType?.ProductType;
			ViewBag.TypeId = id;

			return View(products);
		}
	}
}
