﻿using Microsoft.AspNetCore.Identity;

namespace Shop.Models
{
	public class ApplicationUser:IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public virtual List<Order> Orders { get; set; }

	}
}
