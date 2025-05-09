namespace SurvayBasket.API.Presistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : DbContext(option)
{
    public DbSet<Poll> Polls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
