using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace HarmoniX_Repository.Repositories
{
    public class PlaylistRepository
    {
        private HarmonixDbContext _context;

        public async Task Create(Playlist playlist)
        {
            _context = new HarmonixDbContext();
            await _context.Playlists.AddAsync(playlist);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Playlist>> GetAll()
        {
            _context = new();
            return await _context.Playlists.Include("Account").ToListAsync();
        }
        public async Task<List<Playlist>> GetAllByAccountId(int accountId)
        {
            _context = new();
            return await _context.Playlists.Include("Account").Where(s => s.AccountId == accountId).ToListAsync();
        }

        public async Task<List<Song>> GetSongsByPlaylistId(int playlistId)
        {
            return await _context.Playlistssongs
                                 .Where(ps => ps.PlaylistId == playlistId)
                                 .OrderBy(ps => ps.Position)
                                 .Include(ps => ps.Song)
                                 .Select(ps => ps.Song)
                                 .ToListAsync();
        }
    }
}
