using Microsoft.EntityFrameworkCore;
using Priority1.ToDo.Core.Domain;

namespace Priority1.ToDo.Core.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        BeforeSaveChanges();
        return await base.SaveChangesAsync(ct);
    }

    public override int SaveChanges()
    {
        BeforeSaveChanges();
        return base.SaveChanges();
    }

    public void BeforeSaveChanges()
    {
        var entries = ChangeTracker.Entries().ToList();

        foreach(var entry in entries)
        {
            if (entry.Entity is EntityBase entity)
            {
                var now = DateTime.UtcNow;

                // TODO: Set CreatedBy and UpdatedBy using the current username from UserService.
                var currentUser = "System";

                if (entry.State == EntityState.Added)
                {
                    entity.CreateDate = now;
                    entity.UpdateDate = now;
                    entity.CreatedBy = currentUser;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdateDate = now;
                    entity.UpdatedBy = currentUser;
                }
            }
        }
    }
}
