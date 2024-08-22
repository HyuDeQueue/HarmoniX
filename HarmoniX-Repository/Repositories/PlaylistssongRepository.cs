using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Repository.Repositories
{
    public class PlaylistssongRepository
    {
        private HarmonixDbContext _context;

        public async Task Create(Playlistssong playlistssong)
        {
            var existingEntry = await _context.Playlistssongs
                .FirstOrDefaultAsync(ps => ps.SongId == playlistssong.SongId && ps.PlaylistId == playlistssong.PlaylistId);

            if (existingEntry == null)
            {
                _context.Playlistssongs.Add(playlistssong);
                await _context.SaveChangesAsync();
            }
            else
            {
                existingEntry.Position = playlistssong.Position;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Song>> GetSongsByPlaylistIdAsync(int playlistId)
        {
            _context = new();
            var songIds = await _context.Playlistssongs
                .Where(ps => ps.PlaylistId == playlistId)
                .Select(ps => ps.SongId)
                .ToListAsync();

            var songs = await _context.Songs
                .Where(s => songIds.Contains(s.SongId))
                .ToListAsync();

            return songs;
        }

    }

    

}
