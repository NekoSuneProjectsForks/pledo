using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class ServerScopedMediaKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "TvShows");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => new { x.ServerId, x.RatingKey });
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Items = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => new { x.ServerId, x.Id });
                    table.ForeignKey(
                        name: "FK_Playlists_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TvShows",
                columns: table => new
                {
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Guid = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShows", x => new { x.ServerId, x.RatingKey });
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TvShowId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => new { x.ServerId, x.RatingKey });
                    table.ForeignKey(
                        name: "FK_Episodes_TvShows_ServerId_TvShowId",
                        columns: x => new { x.ServerId, x.TvShowId },
                        principalTable: "TvShows",
                        principalColumns: new[] { "ServerId", "RatingKey" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    DownloadUri = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    ServerFilePath = table.Column<string>(type: "TEXT", nullable: false),
                    MovieRatingKey = table.Column<string>(type: "TEXT", nullable: true),
                    EpisodeRatingKey = table.Column<string>(type: "TEXT", nullable: true),
                    TotalBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<long>(type: "INTEGER", nullable: true),
                    Bitrate = table.Column<long>(type: "INTEGER", nullable: true),
                    Width = table.Column<int>(type: "INTEGER", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    AspectRatio = table.Column<double>(type: "REAL", nullable: true),
                    AudioChannels = table.Column<int>(type: "INTEGER", nullable: true),
                    AudioCodec = table.Column<string>(type: "TEXT", nullable: true),
                    VideoCodec = table.Column<string>(type: "TEXT", nullable: true),
                    VideoResolution = table.Column<string>(type: "TEXT", nullable: true),
                    Container = table.Column<string>(type: "TEXT", nullable: true),
                    VideoFrameRate = table.Column<string>(type: "TEXT", nullable: true),
                    AudioProfile = table.Column<string>(type: "TEXT", nullable: true),
                    VideoProfile = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => new { x.DownloadUri, x.ServerId });
                    table.ForeignKey(
                        name: "FK_MediaFiles_Episodes_ServerId_EpisodeRatingKey",
                        columns: x => new { x.ServerId, x.EpisodeRatingKey },
                        principalTable: "Episodes",
                        principalColumns: new[] { "ServerId", "RatingKey" });
                    table.ForeignKey(
                        name: "FK_MediaFiles_Movies_ServerId_MovieRatingKey",
                        columns: x => new { x.ServerId, x.MovieRatingKey },
                        principalTable: "Movies",
                        principalColumns: new[] { "ServerId", "RatingKey" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_ServerId_TvShowId",
                table: "Episodes",
                columns: new[] { "ServerId", "TvShowId" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ServerId_EpisodeRatingKey",
                table: "MediaFiles",
                columns: new[] { "ServerId", "EpisodeRatingKey" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ServerId_MovieRatingKey",
                table: "MediaFiles",
                columns: new[] { "ServerId", "MovieRatingKey" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "TvShows");

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.RatingKey);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Items = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlists_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TvShows",
                columns: table => new
                {
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Guid = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TvShows", x => x.RatingKey);
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: true),
                    SeasonNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    EpisodeNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    TvShowId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.RatingKey);
                    table.ForeignKey(
                        name: "FK_Episodes_TvShows_TvShowId",
                        column: x => x.TvShowId,
                        principalTable: "TvShows",
                        principalColumn: "RatingKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    DownloadUri = table.Column<string>(type: "TEXT", nullable: false),
                    ServerId = table.Column<string>(type: "TEXT", nullable: false),
                    RatingKey = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    ServerFilePath = table.Column<string>(type: "TEXT", nullable: false),
                    MovieRatingKey = table.Column<string>(type: "TEXT", nullable: true),
                    EpisodeRatingKey = table.Column<string>(type: "TEXT", nullable: true),
                    TotalBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    LibraryId = table.Column<string>(type: "TEXT", nullable: false),
                    Duration = table.Column<long>(type: "INTEGER", nullable: true),
                    Bitrate = table.Column<long>(type: "INTEGER", nullable: true),
                    Width = table.Column<int>(type: "INTEGER", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    AspectRatio = table.Column<double>(type: "REAL", nullable: true),
                    AudioChannels = table.Column<int>(type: "INTEGER", nullable: true),
                    AudioCodec = table.Column<string>(type: "TEXT", nullable: true),
                    VideoCodec = table.Column<string>(type: "TEXT", nullable: true),
                    VideoResolution = table.Column<string>(type: "TEXT", nullable: true),
                    Container = table.Column<string>(type: "TEXT", nullable: true),
                    VideoFrameRate = table.Column<string>(type: "TEXT", nullable: true),
                    AudioProfile = table.Column<string>(type: "TEXT", nullable: true),
                    VideoProfile = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => new { x.DownloadUri, x.ServerId });
                    table.ForeignKey(
                        name: "FK_MediaFiles_Episodes_EpisodeRatingKey",
                        column: x => x.EpisodeRatingKey,
                        principalTable: "Episodes",
                        principalColumn: "RatingKey");
                    table.ForeignKey(
                        name: "FK_MediaFiles_Movies_MovieRatingKey",
                        column: x => x.MovieRatingKey,
                        principalTable: "Movies",
                        principalColumn: "RatingKey");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_TvShowId",
                table: "Episodes",
                column: "TvShowId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_EpisodeRatingKey",
                table: "MediaFiles",
                column: "EpisodeRatingKey");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_MovieRatingKey",
                table: "MediaFiles",
                column: "MovieRatingKey");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_ServerId",
                table: "Playlists",
                column: "ServerId");
        }
    }
}
