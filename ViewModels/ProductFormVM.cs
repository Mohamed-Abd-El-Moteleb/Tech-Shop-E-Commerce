using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
	public class ProductFormVM
	{
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[Precision(18, 2)]
		public decimal Price { get; set; }

		public IFormFile? Image { get; set; }
		public byte[]? ImageEdit { get; set; }

		[Display(Name = "Product Color")]
		public string ProductColor { get; set; }

		[Required]
		[Display(Name = "Available")]
		public bool IsAvailable { get; set; }

		[Required]
		[Display(Name = "Product Type")]
		public int ProductTypeId { get; set; }

		[Required]
		[Display(Name = "Special Tag")]
		public int SpecialTagId { get; set; }

	}
}
