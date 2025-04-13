using Microsoft.AspNetCore.Identity;
using Shop.Models;

namespace Shop.Data
{
	public class DbSeeder
	{

		public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
		{


			using var scope = service.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();


			try
			{
				logger.LogInformation("Ensuring Database Is Created");
				await context.Database.EnsureCreatedAsync();

				logger.LogInformation("Seeding Role");
				await AddRoleAsync(roleManager, "Admin");
				await AddRoleAsync(roleManager, "Customer");

				logger.LogInformation("Seeding Admin User");
				var adminEmail = "Midoshaaban95@gmail.com";
				if(await userManager.FindByEmailAsync(adminEmail) == null)
				{
					var adminUser = new ApplicationUser
					{
						UserName = adminEmail,
						Email = adminEmail,
						FirstName = "Mohamed",
						LastName = "Shaban",
						EmailConfirmed = true,
						PhoneNumberConfirmed = true,
						SecurityStamp = Guid.NewGuid().ToString()
					};
					var result = await userManager.CreateAsync(adminUser, "Admin123##");
					if (result.Succeeded)
					{
						logger.LogInformation("Assigning Admin Role To Admin User");
						await userManager.AddToRoleAsync(adminUser, "Admin");
					}
					else
					{
						logger.LogError("Failed To Create Admin User");
					}
				}

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error While Seeding The Database");
			}

		}

		private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string role)
		{
			if(!await roleManager.RoleExistsAsync(role))
			{
				var result = await roleManager.CreateAsync(new IdentityRole(role));
				if (!result.Succeeded)
				{
					throw new Exception($"Failed To Create Role{role} :");
				}
			}
		}

	}
}
