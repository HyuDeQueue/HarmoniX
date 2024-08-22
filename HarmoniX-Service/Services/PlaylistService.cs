using HarmoniX_Repository.Models;
using HarmoniX_Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Service.Services
{
    public class PlaylistService
    {
        private PlaylistRepository _repo = new();

        public void CreatePlayList(Playlist playlist)
        {
            _repo.Create(playlist);
        }
    }
}
