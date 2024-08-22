using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Repository.Repositories
{
    public class SongRepository
    {
         private HarmonixDbContext _context;

        public async Task CreateSongAsync(Song song)
        {
            _context = new();
                _context.Songs.Add(song);
                await _context.SaveChangesAsync();
        }

        public async Task UpdateSongAsync(Song song)
        {
            _context = new();
            var existingSong = await _context.Songs
                .FirstOrDefaultAsync(s => s.SongTitle == song.SongTitle);

            if (existingSong != null)
            {
                existingSong.ArtistName = song.ArtistName;
                existingSong.SongMedia = song.SongMedia;
                existingSong.CategoryId = song.CategoryId;
                existingSong.AccountId = song.AccountId;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Song not found for update.");
            }
        }

        //public async Task SaveSongAsync(Song song)
        //{
        //    _context = new();
        //    var existingSong = await _context.Songs
        //        .FirstOrDefaultAsync(s => s.SongTitle == song.SongTitle);

        //    if (existingSong != null)
        //    {
        //        existingSong.ArtistName = song.ArtistName;
        //        existingSong.SongMedia = song.SongMedia;
        //        existingSong.CategoryId = song.CategoryId;
        //        existingSong.AccountId = song.AccountId;
        //    }
        //    else
        //    {
        //        _context.Songs.Add(song);
        //    }

        //    await _context.SaveChangesAsync();
        //}

        public async Task<Song> GetSongAsync(string songMedia)
        {
            _context = new();
            return await _context.Songs
                .FirstOrDefaultAsync(s => s.SongMedia == songMedia);
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            _context = new();
            return await _context.Songs.ToListAsync();
        }

        public async Task<List<Song>> GetAllSongsForGridAsync()
        {
            _context = new();
            return await _context.Songs
                .Include("Category")
                .Include("Account")
                .ToListAsync();
        }

    }
}
