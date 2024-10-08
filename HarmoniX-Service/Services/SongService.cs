﻿using HarmoniX_Controller.Controllers;
using HarmoniX_Repository.Models;
using HarmoniX_Repository.Repositories;

namespace HarmoniX_Service.Services
{
    public class SongService
    {
        //private SongRepository _repo = new();

        private readonly S3Controller _s3Controller = new();
        private readonly SongRepository _songRepository = new();

        public SongService() { }

        public async Task UploadSongAsync(Song song, string filePath)
        {
            string key = $"{song.SongTitle}-{Guid.NewGuid()}.mp3";

            using (var fileStream = File.OpenRead(filePath))
            {
                await _s3Controller.UploadFileAsync(key, fileStream);
            }

            song.SongMedia = key;
            song.AccountId = song.AccountId;

            await _songRepository.CreateSongAsync(song);
            return;
        }

        public async Task<Stream> GetSongStreamAsync(string songMedia)
        {
            var song = await _songRepository.GetSongAsync(songMedia);

            if (song != null)
            {
                return await _s3Controller.RetrieveFileAsync(song.SongMedia);
            }

            return null;
        }

        public async Task<List<Song>> GetAllSongsAsync()
        {
            return await _songRepository.GetAllSongsForGridAsync();
        }

        public async Task<List<Song>> GetSongsByIdAsync(int id)
        {
            return await _songRepository.GetSongsByAccountIdAsync(id);
        }

        public async Task UpdateSongAsync(Song song)
        {
            await _songRepository.UpdateSongAsync(song);
            return;
        }

        public async Task<List<Song>> SearchSongsAsync(string songTitle, string artistName)
        {
            return await _songRepository.SearchSongsAsync(songTitle, artistName);
        }

    }
}
