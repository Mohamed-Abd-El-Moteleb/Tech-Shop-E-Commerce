using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext()
	{
	}

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
	public DbSet<ProductTypes> ProductTypes { get; set; }
	public DbSet<SpecialTag> SpecialTags { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<OrderDetails> OrderDetails { get; set; }
	public DbSet<ApplicationUser> ApplicationUsers { get; set; }

}
