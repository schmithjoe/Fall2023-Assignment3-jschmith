using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Fall2023_Assignment3_jschmith.Models;

namespace Fall2023_Assignment3_jschmith.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Fall2023_Assignment3_jschmith.Models.Movie> Movie { get; set; } = default!;
    public DbSet<Fall2023_Assignment3_jschmith.Models.Actor> Actor { get; set; } = default!;
    public DbSet<Fall2023_Assignment3_jschmith.Models.MovieActor> MovieActor { get; set; } = default!;
    public DbSet<Fall2023_Assignment3_jschmith.Models.Review> Review { get; set; } = default!;
}
