using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Web.Data;
using Web.Models;

namespace Tests.Core;

public class ServerScopedMediaKeysTests
{
    [Fact]
    public async Task Migrate_FromPreviousSchema_AllowsSamePlexKeysOnDifferentServers()
    {
        var databasePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.db");

        try
        {
            await using (var oldSchemaContext = CreateContext(databasePath))
            {
                var migrator = oldSchemaContext.GetService<IMigrator>();
                await migrator.MigrateAsync("20231020211437_AddTransientTokenToServer");
            }

            await using (var context = CreateContext(databasePath))
            {
                await context.Database.MigrateAsync();

                context.Servers.AddRange(
                    CreateServer("server-1", "Server One"),
                    CreateServer("server-2", "Server Two"));

                context.Movies.AddRange(
                    CreateMovie("server-1", "1", "Movie One"),
                    CreateMovie("server-2", "1", "Movie Two"));

                context.TvShows.AddRange(
                    CreateTvShow("server-1", "10", "Show One"),
                    CreateTvShow("server-2", "10", "Show Two"));

                context.Episodes.AddRange(
                    CreateEpisode("server-1", "11", "10", "Episode One"),
                    CreateEpisode("server-2", "11", "10", "Episode Two"));

                context.MediaFiles.AddRange(
                    CreateMovieMediaFile("server-1", "1", "/library/parts/1/file.mkv"),
                    CreateMovieMediaFile("server-2", "1", "/library/parts/1/file.mkv"),
                    CreateEpisodeMediaFile("server-1", "11", "/library/parts/11/file.mkv"),
                    CreateEpisodeMediaFile("server-2", "11", "/library/parts/11/file.mkv"));

                context.Playlists.AddRange(
                    CreatePlaylist("server-1", "1", "Playlist One"),
                    CreatePlaylist("server-2", "1", "Playlist Two"));

                await context.SaveChangesAsync();
            }

            await using (var verificationContext = CreateContext(databasePath))
            {
                Assert.Equal(2, await verificationContext.Movies.CountAsync(x => x.RatingKey == "1"));
                Assert.Equal(2, await verificationContext.TvShows.CountAsync(x => x.RatingKey == "10"));
                Assert.Equal(2, await verificationContext.Episodes.CountAsync(x => x.RatingKey == "11"));
                Assert.Equal(2, await verificationContext.Playlists.CountAsync(x => x.Id == "1"));
                Assert.Equal(4, await verificationContext.MediaFiles.CountAsync());
            }
        }
        finally
        {
            if (File.Exists(databasePath))
            {
                try
                {
                    File.Delete(databasePath);
                }
                catch (IOException)
                {
                    // SQLite can hold the file handle briefly after the final context is disposed.
                }
            }
        }
    }

    private static CustomDbContext CreateContext(string databasePath)
    {
        var options = new DbContextOptionsBuilder<CustomDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;

        return new CustomDbContext(options);
    }

    private static Server CreateServer(string id, string name)
    {
        return new Server
        {
            Id = id,
            Name = name,
            Connections = new List<ServerConnection>()
        };
    }

    private static Movie CreateMovie(string serverId, string ratingKey, string title)
    {
        return new Movie
        {
            ServerId = serverId,
            RatingKey = ratingKey,
            Key = $"/library/metadata/{ratingKey}",
            Title = title,
            LibraryId = $"{serverId}-movie-library",
            Year = 2024
        };
    }

    private static TvShow CreateTvShow(string serverId, string ratingKey, string title)
    {
        return new TvShow
        {
            ServerId = serverId,
            RatingKey = ratingKey,
            Key = $"/library/metadata/{ratingKey}",
            Guid = $"plex://show/{serverId}/{ratingKey}",
            Title = title,
            LibraryId = $"{serverId}-show-library",
            Episodes = new List<Episode>()
        };
    }

    private static Episode CreateEpisode(string serverId, string ratingKey, string tvShowId, string title)
    {
        return new Episode
        {
            ServerId = serverId,
            RatingKey = ratingKey,
            Key = $"/library/metadata/{ratingKey}",
            Title = title,
            LibraryId = $"{serverId}-show-library",
            Year = 2024,
            SeasonNumber = 1,
            EpisodeNumber = 1,
            TvShowId = tvShowId
        };
    }

    private static MediaFile CreateMovieMediaFile(string serverId, string movieRatingKey, string downloadUri)
    {
        return new MediaFile
        {
            ServerId = serverId,
            RatingKey = movieRatingKey,
            Key = $"/library/metadata/{movieRatingKey}",
            ServerFilePath = $"X:/Movies/{serverId}/{movieRatingKey}.mkv",
            MovieRatingKey = movieRatingKey,
            DownloadUri = downloadUri,
            TotalBytes = 1024,
            LibraryId = $"{serverId}-movie-library"
        };
    }

    private static MediaFile CreateEpisodeMediaFile(string serverId, string episodeRatingKey, string downloadUri)
    {
        return new MediaFile
        {
            ServerId = serverId,
            RatingKey = episodeRatingKey,
            Key = $"/library/metadata/{episodeRatingKey}",
            ServerFilePath = $"X:/Shows/{serverId}/{episodeRatingKey}.mkv",
            EpisodeRatingKey = episodeRatingKey,
            DownloadUri = downloadUri,
            TotalBytes = 1024,
            LibraryId = $"{serverId}-show-library"
        };
    }

    private static Playlist CreatePlaylist(string serverId, string id, string name)
    {
        return new Playlist
        {
            ServerId = serverId,
            Id = id,
            Name = name,
            Items = new List<string> { "1" }
        };
    }
}
