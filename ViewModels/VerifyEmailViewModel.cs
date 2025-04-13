using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
	public class VerifyEmailViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress]
		public string Email { get; set; }
	}
}
