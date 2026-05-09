using AshryverBot.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace AshryverBot.Database;

public class AshryverBotDbContext(DbContextOptions<AshryverBotDbContext> options) : DbContext(options)
{
    public DbSet<TwitchTokenEntity> TwitchTokens => Set<TwitchTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var token = modelBuilder.Entity<TwitchTokenEntity>();
        token.ToTable("twitch_tokens");
        token.HasKey(x => x.TwitchUserId);
        token.Property(x => x.TwitchUserId).HasMaxLength(64);
        token.Property(x => x.Login).HasMaxLength(64).IsRequired();
        token.Property(x => x.DisplayName).HasMaxLength(128);
        token.Property(x => x.Email).HasMaxLength(256);
        token.Property(x => x.AccessToken).IsRequired();
        token.Property(x => x.RefreshToken).IsRequired();
        token.Property(x => x.Scopes).HasMaxLength(4096);
        token.HasIndex(x => x.IsBotAccount);
    }
}
