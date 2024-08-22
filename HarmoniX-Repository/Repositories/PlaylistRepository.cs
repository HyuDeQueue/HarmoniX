using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
