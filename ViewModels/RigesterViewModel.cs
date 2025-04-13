using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
	public class RigesterViewModel
	{
		[Required(ErrorMessage = "First Name Is Required")]
		[Display(Name ="First Name")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name Is Required")]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email Is Required")]
		[EmailAddress]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password Is Required")]
		[DataType(DataType.Password)]
		[StringLength(40,MinimumLength =8,ErrorMessage = "Password Minimum Length 8 and Maximum Length is 40 ")]
		[Compare("ConfirmPassword",ErrorMessage ="Password Does Not Match")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password Is Required")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }

	}
}
