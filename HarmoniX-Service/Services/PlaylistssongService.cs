using HarmoniX_Repository.Models;
using HarmoniX_Repository.Repositories;

namespace HarmoniX_Service.Services
{
    public class PlaylistssongService
    {
        private PlaylistssongRepository _repo = new();

        public async Task CreatePlayListSong(Playlistssong playlistsong)
        {
            await _repo.Create(playlistsong);
        }

        public async Task<List<Song>> GetSongsByPlaylistIdAsync(int playlistId)
        {
            return await _repo.GetSongsByPlaylistIdAsync(playlistId);
        }

        public async Task RemovePlaylistSong(Playlistssong playlistsong)
        {
            await _repo.Remove(playlistsong);
        }

    }

}
