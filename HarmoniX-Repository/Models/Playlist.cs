namespace HarmoniX_Repository.Models;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public string PlaylistName { get; set; } = null!;

    public string PlaylistDescription { get; set; } = null!;

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Playlistssong> Playlistssongs { get; set; } = new List<Playlistssong>();
}
