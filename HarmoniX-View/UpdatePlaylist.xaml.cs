using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;
using System.Windows;

namespace HarmoniX_View
{
    /// <summary>
    /// Interaction logic for UpdatePlaylist.xaml
    /// </summary>
    public partial class UpdatePlaylist : Window
    {
        private readonly SongService _songService = new();
        private readonly PlaylistssongService _playlistssongService = new();
        private readonly Playlist _playlist;
        public UpdatePlaylist(Playlist playlist)
        {
            InitializeComponent();
            _playlist = playlist;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = (Song)SongsDataGrid.SelectedItem;

            if (selectedSong != null)
            {
                var currentPlaylistSongs = (List<Song>)PlaylistSongDataGrid.ItemsSource ?? new List<Song>();

                if (!currentPlaylistSongs.Any(s => s.SongId == selectedSong.SongId))
                {
                    currentPlaylistSongs.Add(selectedSong);
                    PlaylistSongDataGrid.ItemsSource = null;
                    PlaylistSongDataGrid.ItemsSource = currentPlaylistSongs;
                }
            }
        }

        private void SubtractBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = (Song)PlaylistSongDataGrid.SelectedItem;

            if (selectedSong != null)
            {
                var currentPlaylistSongs = (List<Song>)PlaylistSongDataGrid.ItemsSource;

                currentPlaylistSongs.Remove(selectedSong);
                PlaylistSongDataGrid.ItemsSource = null;
                PlaylistSongDataGrid.ItemsSource = currentPlaylistSongs;
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var currentPlaylistSongs = (List<Song>)PlaylistSongDataGrid.ItemsSource;

            if (currentPlaylistSongs != null && currentPlaylistSongs.Count > 0)
            {
                var existingSongs = await _playlistssongService.GetSongsByPlaylistIdAsync(_playlist.PlaylistId);
                foreach (var song in existingSongs)
                {
                    var playlistssongToRemove = new Playlistssong
                    {
                        PlaylistId = _playlist.PlaylistId,
                        SongId = song.SongId
                    };
                    await _playlistssongService.RemovePlaylistSong(playlistssongToRemove);
                }

                foreach (var song in currentPlaylistSongs)
                {
                    var playlistssong = new Playlistssong
                    {
                        PlaylistId = _playlist.PlaylistId,
                        SongId = song.SongId,
                        Position = currentPlaylistSongs.IndexOf(song) + 1
                    };

                    await _playlistssongService.CreatePlayListSong(playlistssong);
                }

                MessageBox.Show("Playlist updated successfully!");
            }
            else
            {
                MessageBox.Show("Please add at least one song to the playlist.");
            }
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var allSongs = await _songService.GetAllSongsAsync();
            SongsDataGrid.ItemsSource = allSongs;

            var allSongsInPlaylist = await _playlistssongService.GetSongsByPlaylistIdAsync(_playlist.PlaylistId);
            PlaylistSongDataGrid.ItemsSource = allSongsInPlaylist;
            ThisPlaylist.Text = $"Playlist name: {_playlist.PlaylistName}";

        }
    }
}
