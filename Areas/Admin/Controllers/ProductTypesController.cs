using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repos.Admin.Interfaces;

namespace Shop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductTypesController : Controller
	{
		private readonly IProductTypesRepository _repository;

		public ProductTypesController(IProductTypesRepository repository)
		{
			_repository = repository;
		}

		// Index
		public async Task<IActionResult> Index()
		{
			var data = await _repository.GetAllAsync();
			return View(data);
		}

		// Create
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductTypes product)
		{
			if (ModelState.IsValid)
			{
				await _repository.AddAsync(product);
				TempData["Save"] = "Product Type has been saved.";
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}

		// Edit
		[HttpGet]
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();

			var productType = await _repository.GetByIdAsync(id.Value);
			if (productType == null) return NotFound();

			return View(productType);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductTypes product)
		{
			if (ModelState.IsValid)
			{
				await _repository.UpdateAsync(product);
				TempData["Edit"] = "Product Type has been edited.";
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}

		// Details
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();

			var productType = await _repository.GetByIdAsync(id.Value);
			if (productType == null) return NotFound();

			return View(productType);
		}

		// Delete
		[HttpGet]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();

			var productType = await _repository.GetByIdAsync(id.Value);
			if (productType == null) return NotFound();

			return View(productType);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			await _repository.DeleteAsync(id);
			TempData["Delete"] = "Product Type has been deleted.";
			return RedirectToAction(nameof(Index));
		}
	}
}
