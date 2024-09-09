namespace HarmoniX_Repository.Models;

public partial class Song
{
    public int SongId { get; set; }

    public string SongTitle { get; set; } = null!;

    public string ArtistName { get; set; } = null!;

    public string SongMedia { get; set; } = null!;

    public int CategoryId { get; set; }

    public int AccountId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Playlistssong> Playlistssongs { get; set; } = new List<Playlistssong>();
}
