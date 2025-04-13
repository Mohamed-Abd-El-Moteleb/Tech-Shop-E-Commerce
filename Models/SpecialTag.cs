using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
	public class SpecialTag
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Special Tag")]
		public String? SpecialTagName { get; set; }
	}
}
