using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
	public class Order
	{
		public int Id { get; set; }

		[Display(Name ="Order Number")]
		public string OrderNo { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		[Display(Name ="Phone Number")]
		public string PhoneNo { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public DateTime OrderDate { get; set; }

		public string Status { get; set; } = "Pending";

		public string? UserId { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User { get; set; }
		public virtual List<OrderDetails> OrderDetails { get; set; }

	}
}
