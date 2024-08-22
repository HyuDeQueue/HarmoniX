using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using NAudio.Wave;

namespace HarmoniX_View
{
    public partial class MainWindow : Window
    {
        private readonly Account _account;
        private IWavePlayer _wavePlayer;
        private AudioFileReader _audioFileReader;
        private SongService _songService = new();
        public event Action OnSongDetailClosed;

        public MainWindow()
        {
            InitializeComponent();
            LoadSongs();
        }

        // Add the Window_MouseDown method
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void LoadSongs()
        {
            var songs = await _songService.GetAllSongsAsync();
            SongsDataGrid.ItemsSource = songs;
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            AddSongDetail detail = new AddSongDetail(_account);
            detail.ShowDialog();
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SongsDataGrid.SelectedItem is Song selectedSong)
                {
                    var stream = await _songService.GetSongStreamAsync(selectedSong.SongMedia);

                    if (stream != null)
                    {
                        string tempFilePath = System.IO.Path.GetTempFileName() + ".mp3";

                        using (var fileStream = System.IO.File.Create(tempFilePath))
                        {
                            await stream.CopyToAsync(fileStream);
                        }

                        _wavePlayer?.Stop();
                        _wavePlayer?.Dispose();
                        _audioFileReader?.Dispose();

                        _wavePlayer = new WaveOutEvent();
                        _audioFileReader = new AudioFileReader(tempFilePath);
                        _wavePlayer.Init(_audioFileReader);
                        _wavePlayer.Play();
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve the song.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a song to play.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to play the song: {ex.Message}");
            }
        }

        private void btnCreatePlaylist_Click(object sender, RoutedEventArgs e)
        {
            CreatePlaylist createPlaylist = new CreatePlaylist();
            createPlaylist.Show();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
