using AshryverBot.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshryverBot.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TwitchTokenEntity> TwitchTokens => Set<TwitchTokenEntity>();
    public DbSet<CommandEntity> Commands => Set<CommandEntity>();
    public DbSet<WatchtimeEntity> Watchtimes => Set<WatchtimeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TwitchTokenEntity>(entity =>
        {
            entity.ToTable("twitch_tokens");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.TwitchUserId).IsUnique();
            entity.HasIndex(x => x.IsBotAccount);
        });

        modelBuilder.Entity<CommandEntity>(entity =>
        {
            entity.ToTable("commands");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.Name).IsUnique();
            entity.Property(x => x.RequiredRole).HasConversion<string>().HasMaxLength(32);
        });

        modelBuilder.Entity<WatchtimeEntity>(entity =>
        {
            entity.ToTable("watchtimes");
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.TwitchUserId).IsUnique();
        });
    }
}
