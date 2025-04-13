using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Shop.Areas.Identity.Pages.Account.Manage;
using Shop.Data;
using Shop.Models;
using Shop.Repos.Admin;
using Shop.Repos.Admin.Interfaces;
using Shop.Repos.Customer;
using Shop.Repos.Customer.Interfaces;
using Shop.Repositories;
using Shop.Repositories.Implementations;
using Shop.Repositories.Interfaces;
using Stripe;
using System.Threading.Tasks;

namespace Shop;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

		builder.Services.AddDbContext<ApplicationDbContext>(options =>
			options.UseSqlServer(connectionString));

		builder.Services.AddHttpContextAccessor();

		builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		builder.Services.AddTransient<IEmailSender, DummyEmailSender>();

		builder.Services.AddRazorPages();

		builder.Services.AddControllersWithViews();

		builder.Services.AddDistributedMemoryCache();

		builder.Services.AddSession(options =>
		{
			options.IdleTimeout = TimeSpan.FromMinutes(30);
			options.Cookie.IsEssential = true;
		});
		//Repos
		builder.Services.AddScoped<IProductTypesRepository, ProductTypesRepository>();
		builder.Services.AddScoped<ISpecialTagRepository, SpecialTagRepository>();
		builder.Services.AddScoped<IProductAdminRepository, ProductAdminRepository>();
		builder.Services.AddScoped<IOrderAdminRepository, OrderAdminRepository>();
		builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
		builder.Services.AddScoped<IProductRepository, ProductRepository>();
		builder.Services.AddScoped<IOrderRepository, OrderRepository>();
		builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();


		StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
		var app = builder.Build();

		// Seed roles/admin user
		await DbSeeder.SeedRolesAndAdminAsync(app.Services);

		if (app.Environment.IsDevelopment())
		{
			app.UseMigrationsEndPoint();
		}
		else
		{
			app.UseExceptionHandler("/Home/Error");
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseAuthentication();
		app.UseAuthorization();

		app.UseSession();

		app.MapControllerRoute(
			name: "default",
			pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

		// Razor Pages for Identity UI
		app.MapRazorPages();

		app.Run();
	}

}
