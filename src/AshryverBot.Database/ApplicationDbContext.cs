using AshryverBot.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshryverBot.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TwitchTokenEntity> TwitchTokens => Set<TwitchTokenEntity>();
    public DbSet<CommandEntity> Commands => Set<CommandEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var token = modelBuilder.Entity<TwitchTokenEntity>();
        token.ToTable("twitch_tokens");
        token.HasKey(x => x.Id);
        token.HasIndex(x => x.TwitchUserId).IsUnique();
        token.HasIndex(x => x.IsBotAccount);

        var command = modelBuilder.Entity<CommandEntity>();
        command.ToTable("commands");
        command.HasKey(x => x.Id);
        command.HasIndex(x => x.Name).IsUnique();
        command.Property(x => x.RequiredRole).HasConversion<string>().HasMaxLength(32);
    }
}
