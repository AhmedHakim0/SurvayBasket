using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SurvayBasket.API.Presistance;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> option,IHttpContextAccessor httpContextAccessor) : IdentityDbContext(option)
{
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    public DbSet<Poll> Polls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var CurrentUserId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var entires = ChangeTracker.Entries<AuditableEntity>();

        foreach(var EntityEntire in entires)
        {
            if (EntityEntire.State == EntityState.Added)
            {
                EntityEntire.Property(p => p.CreatedById).CurrentValue = CurrentUserId!;
            }
            else if (EntityEntire.State == EntityState.Modified)
            {
                EntityEntire.Property(p => p.UpdatedById).CurrentValue = CurrentUserId;
                EntityEntire.Property(p => p.UpdatedOn).CurrentValue = DateTime.UtcNow;
                        
            }
          
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
