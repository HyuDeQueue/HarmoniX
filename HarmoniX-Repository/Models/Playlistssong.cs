using System;
using System.Collections.Generic;

namespace HarmoniX_Repository.Models;

public partial class Playlistssong
{
    public int Position { get; set; }

    public int SongId { get; set; }

    public int PlaylistId { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;
}
