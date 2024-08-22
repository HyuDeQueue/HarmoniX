using HarmoniX_Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Repository.Repositories
{
    public class PlaylistRepository
    {
        private HarmonixDbContext _context;

        public async void Create(Playlist playlist)
        {
            _context = new HarmonixDbContext();
            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();
        }

    }
}
