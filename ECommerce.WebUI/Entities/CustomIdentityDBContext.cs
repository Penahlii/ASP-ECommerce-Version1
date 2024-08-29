using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.WebUI.Entities;

public class CustomIdentityDBContext : IdentityDbContext<CustomIdentityUser, CustomIdentityRole, string>
{
    public CustomIdentityDBContext(DbContextOptions<CustomIdentityDBContext> options) : base(options)
    {       
    }
}
