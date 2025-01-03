using Microsoft.EntityFrameworkCore;
using Urlshortener.Api.Entities;
using Urlshortener.Api.Services;

namespace Urlshortener.Api;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(entity =>
        {
            entity.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);
            entity.HasIndex(s => s.Code).IsUnique();
        });
    }
}