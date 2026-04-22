using Web.Models;

namespace Web.Data;

public class EpisodeRepository : RepositoryBase<Episode>
{
    public EpisodeRepository(CustomDbContext customDbContext) : base(customDbContext)
    {
    }

    public override Task Upsert(IEnumerable<Episode> t)
    {
        var episodesInDb = CustomDbContext.Episodes.ToHashSet();
        var episodesToUpsert = t.ToHashSet();
        var keySelector = (Episode episode) => (episode.ServerId, episode.RatingKey);

        var episodesToDelete = episodesInDb.ExceptBy(episodesToUpsert.Select(keySelector), keySelector);
        var episodesToInsert = episodesToUpsert.ExceptBy(episodesInDb.Select(keySelector), keySelector);
        var episodesToUpdate = episodesInDb.IntersectBy(episodesToUpsert.Select(keySelector), keySelector);
        CustomDbContext.Episodes.RemoveRange(episodesToDelete);
        CustomDbContext.Episodes.AddRange(episodesToInsert);
        CustomDbContext.Episodes.UpdateRange(episodesToUpdate);

        Upsert(episodesToUpsert.SelectMany(x => x.MediaFiles));

        return Task.CompletedTask;
    }

    private Task Upsert(IEnumerable<MediaFile> t)
    {
        var inDb = CustomDbContext.MediaFiles.ToHashSet();
        var toUpsert = t.ToHashSet();
        var keySelector = (MediaFile mediaFile) => (mediaFile.ServerId, mediaFile.DownloadUri);

        var toDelete = inDb.ExceptBy(toUpsert.Select(keySelector), keySelector);
        var toInsert = toUpsert.ExceptBy(inDb.Select(keySelector), keySelector);
        var toUpdate = inDb.IntersectBy(toUpsert.Select(keySelector), keySelector);
        CustomDbContext.MediaFiles.RemoveRange(toDelete);
        CustomDbContext.MediaFiles.AddRange(toInsert);
        CustomDbContext.MediaFiles.UpdateRange(toUpdate);

        return Task.CompletedTask;
    }
}
