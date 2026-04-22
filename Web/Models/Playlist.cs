using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Web.Models;

[PrimaryKey(nameof(ServerId), nameof(Id))]
public class Playlist
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<string> Items { get; set; }

    [ForeignKey("Server")]
    public string ServerId { get; set; }

    public Server Server { get; set; }
}
