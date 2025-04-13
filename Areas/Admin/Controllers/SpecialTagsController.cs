using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repos.Admin.Interfaces;

namespace Shop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class SpecialTagsController : Controller
	{
		private readonly ISpecialTagRepository _repository;

		public SpecialTagsController(ISpecialTagRepository repository)
		{
			_repository = repository;
		}

		public IActionResult Index()
		{
			var data = _repository.GetAll();
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
		public IActionResult Create(SpecialTag tag)
		{
			if (ModelState.IsValid)
			{
				_repository.Add(tag);
				_repository.Save();
				return RedirectToAction(nameof(Index));
			}
			return View(tag);
		}

		// Edit
		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null) return NotFound();

			var tag = _repository.GetById(id.Value);
			if (tag == null) return NotFound();

			return View(tag);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(SpecialTag tag)
		{
			if (ModelState.IsValid)
			{
				_repository.Update(tag);
				_repository.Save();
				return RedirectToAction(nameof(Index));
			}
			return View(tag);
		}

		// Details
		public IActionResult Details(int? id)
		{
			if (id == null) return NotFound();

			var tag = _repository.GetById(id.Value);
			if (tag == null) return NotFound();

			return View(tag);
		}

		// Delete
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null) return NotFound();

			var tag = _repository.GetById(id.Value);
			if (tag == null) return NotFound();

			return View(tag);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			_repository.Delete(id);
			_repository.Save();
			return RedirectToAction(nameof(Index));
		}
	}
}
