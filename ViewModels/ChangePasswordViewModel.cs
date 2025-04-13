using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
	public class ChangePasswordViewModel
	{
		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "New Password Is Required")]
		[DataType(DataType.Password)]
		[StringLength(40, MinimumLength = 8, ErrorMessage = "Password Minimum Length 8 and Maximum Length is 40 ")]
		[Compare("ConfirmPassword", ErrorMessage = "Password Does Not Match")]
		[Display(Name = "New Password")]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }
	}
}
