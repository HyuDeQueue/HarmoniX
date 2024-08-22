using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            Application.Current.Shutdown();
        }

        private void SubtractBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

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
