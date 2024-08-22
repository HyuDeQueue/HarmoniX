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

        public async void Create(Playlistssong playlistssong)
        {
            _context = new HarmonixDbContext();
            _context.Playlistssongs.Add(playlistssong);
            await _context.SaveChangesAsync();
        }
    }

    

}
