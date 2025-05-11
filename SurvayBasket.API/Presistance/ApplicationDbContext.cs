using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SurvayBasket.API.Presistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : IdentityDbContext(option)
{
    public DbSet<Poll> Polls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
