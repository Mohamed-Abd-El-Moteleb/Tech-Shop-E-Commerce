namespace Shop.ViewModels.Stripe
{
	public class StripePaymentViewModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public decimal Amount { get; set; }
	}

}
