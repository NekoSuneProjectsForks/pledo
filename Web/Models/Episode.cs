using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Web.Models.Interfaces;

namespace Web.Models;

[PrimaryKey(nameof(ServerId), nameof(RatingKey))]
public class Episode : IMediaElement, ISearchable
{
    public string RatingKey { get; set; }
    public string Key { get; set; }
    public string Title { get; set; }
    public string LibraryId { get; set; }
    public string ServerId { get; set; }
    public int? Year { get; set; }
    public List<MediaFile> MediaFiles { get; set; } = new();
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
    public string TvShowId { get; set; }

    [JsonIgnore]
    public TvShow TvShow { get; set; }
}
