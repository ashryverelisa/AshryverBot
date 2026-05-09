using AshryverBot.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshryverBot.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TwitchTokenEntity> TwitchTokens => Set<TwitchTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var token = modelBuilder.Entity<TwitchTokenEntity>();
        token.ToTable("twitch_tokens");
        token.HasKey(x => x.TwitchUserId);
        token.HasIndex(x => x.IsBotAccount);
    }
}
