using DiscordBotDotNet.Domain;
using DSharpPlus.Entities;
using Microsoft.EntityFrameworkCore;

namespace DiscordBotDotNet.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<DiscordRole> PublicGuildRoles { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=discordbot.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DiscordRole>()
                .HasKey(x => x.Id);

            builder.Entity<Quote>()
                .HasKey(x => x.Id);
        }
    }
}
