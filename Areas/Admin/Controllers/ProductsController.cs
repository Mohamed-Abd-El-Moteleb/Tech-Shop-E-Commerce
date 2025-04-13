using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Models;
using Shop.Repos.Admin.Interfaces;
using Shop.ViewModels;

namespace Shop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class ProductsController : Controller
	{
		private readonly IProductAdminRepository _productRepo;
		private readonly IProductTypesRepository _productTypesRepo;
		private readonly ISpecialTagRepository _specialTagRepo;

		private readonly List<string> allowedExtensions = new() { ".png", ".jpg" };
		private readonly long maxFileSize = 1 * 1024 * 1024;

		public ProductsController(IProductAdminRepository productRepo, IProductTypesRepository productTypesRepo, ISpecialTagRepository specialTagRepo)
		{
			_productRepo = productRepo;
			_productTypesRepo = productTypesRepo;
			_specialTagRepo = specialTagRepo;
		}

		public async Task<IActionResult> Index()
		{
			ViewBag.ProductTypes = new SelectList(await _productTypesRepo.GetAllAsync(), "Id", "ProductType");
			ViewBag.SpecialTags = new SelectList(await _specialTagRepo.GetAllAsync(), "Id", "SpecialTagName");

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> FilteredProducts(string search, string sortBy, string sortOrder, int? productTypeId, int? specialTagId, bool? isAvailable)
		{
			var products = await _productRepo.FilteredProductsAsync(search, sortBy, sortOrder, productTypeId, specialTagId, isAvailable);
			return PartialView("PartialTableFilter", products);
		}

		public async Task<IActionResult> Create()
		{
			ViewBag.ProductTypes = new SelectList(await _productTypesRepo.GetAllAsync(), "Id", "ProductType");
			ViewBag.SpecialTags = new SelectList(await _specialTagRepo.GetAllAsync(), "Id", "SpecialTagName");
			return View("ProductForm", new ProductFormVM());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(ProductFormVM productfromvm)
		{
			var files = Request.Form.Files;
			if (!files.Any())
			{
				await LoadViewBagsAsync();
				ModelState.AddModelError("Image", "Please select a Product Image!");
				return View("ProductForm", productfromvm);
			}

			var image = files.FirstOrDefault();
			var ext = Path.GetExtension(image.FileName).ToLower();

			if (!allowedExtensions.Contains(ext))
			{
				await LoadViewBagsAsync();
				ModelState.AddModelError("Image", "Only PNG and JPG images are allowed!");
				return View("ProductForm", productfromvm);
			}

			if (image.Length > maxFileSize)
			{
				await LoadViewBagsAsync();
				ModelState.AddModelError("Image", "Image cannot be more than 1 MB!");
				return View("ProductForm", productfromvm);
			}

			using var dataStream = new MemoryStream();
			await image.CopyToAsync(dataStream);

			if (await _productRepo.ExistsAsync(productfromvm.Name))
			{
				await LoadViewBagsAsync();
				ModelState.AddModelError("Name", "This Product already exists!");
				return View("ProductForm", productfromvm);
			}

			var product = new Product
			{
				Name = productfromvm.Name,
				Price = productfromvm.Price,
				ProductColor = productfromvm.ProductColor,
				IsAvailable = productfromvm.IsAvailable,
				Image = dataStream.ToArray(),
				ProductTypeId = productfromvm.ProductTypeId,
				SpecialTagId = productfromvm.SpecialTagId
			};

			await _productRepo.AddAsync(product);
			TempData["Save"] = "Product has been saved";
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var product = await _productRepo.GetByIdAsync(id);
			if (product == null) return NotFound();

			var vm = new ProductFormVM
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				ProductColor = product.ProductColor,
				ImageEdit = product.Image,
				ProductTypeId = product.ProductTypeId,
				SpecialTagId = product.SpecialTagId,
				IsAvailable = product.IsAvailable
			};

			await LoadViewBagsAsync();
			return View("ProductForm", vm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(ProductFormVM productVM)
		{
			if (!ModelState.IsValid)
			{
				await LoadViewBagsAsync();
				return View("ProductForm", productVM);
			}

			var product = await _productRepo.GetByIdAsync(productVM.Id);
			if (product == null) return NotFound();

			var files = Request.Form.Files;
			if (files.Any())
			{
				var image = files.FirstOrDefault();
				var ext = Path.GetExtension(image.FileName).ToLower();

				if (!allowedExtensions.Contains(ext))
				{
					await LoadViewBagsAsync();
					ModelState.AddModelError("Image", "Only PNG and JPG images are allowed!");
					return View("ProductForm", productVM);
				}

				if (image.Length > maxFileSize)
				{
					await LoadViewBagsAsync();
					ModelState.AddModelError("Image", "Image cannot be more than 1 MB!");
					return View("ProductForm", productVM);
				}

				using var dataStream = new MemoryStream();
				await image.CopyToAsync(dataStream);
				product.Image = dataStream.ToArray();
			}

			product.Name = productVM.Name;
			product.Price = productVM.Price;
			product.ProductColor = productVM.ProductColor;
			product.IsAvailable = productVM.IsAvailable;
			product.ProductTypeId = productVM.ProductTypeId;
			product.SpecialTagId = productVM.SpecialTagId;

			await _productRepo.UpdateAsync(product);
			TempData["Edit"] = "Product has been updated";
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Details(int id)
		{
			var product = await _productRepo.GetByIdAsync(id);
			return product == null ? NotFound() : View(product);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var product = await _productRepo.GetByIdAsync(id);
			return product == null ? NotFound() : View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirm(int id)
		{
			var product = await _productRepo.GetByIdAsync(id);
			if (product == null) return NotFound();

			await _productRepo.DeleteAsync(product);
			TempData["Delete"] = "Product has been deleted";
			return RedirectToAction(nameof(Index));
		}

		private async Task LoadViewBagsAsync()
		{
			ViewBag.ProductTypes = new SelectList(await _productTypesRepo.GetAllAsync(), "Id", "ProductType");
			ViewBag.SpecialTags = new SelectList(await _specialTagRepo.GetAllAsync(), "Id", "SpecialTagName");
		}
	}
}
