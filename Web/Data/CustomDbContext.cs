using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Web.Models;

namespace Web.Data;

public class CustomDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<TvShow> TvShows { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }
    public DbSet<BusyTask> Tasks { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<DownloadElement> Downloads { get; set; }
    public DbSet<Playlist> Playlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Playlist>()
            .Property(b => b.Items)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null));

        modelBuilder.Entity<TvShow>()
            .HasMany(show => show.Episodes)
            .WithOne(episode => episode.TvShow)
            .HasForeignKey(episode => new { episode.ServerId, episode.TvShowId })
            .HasPrincipalKey(show => new { show.ServerId, show.RatingKey });

        modelBuilder.Entity<Movie>()
            .HasMany(movie => movie.MediaFiles)
            .WithOne()
            .HasForeignKey(mediaFile => new { mediaFile.ServerId, mediaFile.MovieRatingKey })
            .HasPrincipalKey(movie => new { movie.ServerId, movie.RatingKey });

        modelBuilder.Entity<Episode>()
            .HasMany(episode => episode.MediaFiles)
            .WithOne()
            .HasForeignKey(mediaFile => new { mediaFile.ServerId, mediaFile.EpisodeRatingKey })
            .HasPrincipalKey(episode => new { episode.ServerId, episode.RatingKey });
    }

    public CustomDbContext(DbContextOptions<CustomDbContext> options)
        : base(options)
    {
    }
}
