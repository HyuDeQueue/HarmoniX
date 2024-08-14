    using HarmoniX_Repository.Models;
    using HarmoniX_Service.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Win32;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.IO;
using NAudio.Wave;


    namespace HarmoniX_View
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IWavePlayer _wavePlayer;
        private AudioFileReader _audioFileReader;
        private SongService _songService;

        public MainWindow()
        {
            InitializeComponent();
            _songService = new SongService(
                new HarmoniX_Controller.Controllers.S3Controller(),  // No arguments needed
                new HarmoniX_Repository.Repositories.SongRepository());
            LoadSongs();
        }

        private async void LoadSongs()
        {
            var songs = await _songService.GetAllSongsAsync();
            SongsListBox.ItemsSource = songs;
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 files (*.mp3)|*.mp3",
                Title = "Select a song to upload"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                var song = new Song
                {
                    SongTitle = System.IO.Path.GetFileNameWithoutExtension(filePath),
                    ArtistName = "Unknown Artist" // Replace with actual artist name or retrieve from metadata
                };

                await _songService.UploadSongAsync(song, filePath);
                MessageBox.Show("Song uploaded successfully!");
                LoadSongs();
            }
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SongsListBox.SelectedItem is Song selectedSong)
                {
                    var stream = await _songService.GetSongStreamAsync(selectedSong.SongMedia);

                    if (stream != null)
                    {
                        string tempFilePath = System.IO.Path.GetTempFileName() + ".mp3";

                        using (var fileStream = System.IO.File.Create(tempFilePath))
                        {
                            await stream.CopyToAsync(fileStream); // Ensure async copy to prevent blocking UI thread
                        }

                        // Stop previous playback
                        _wavePlayer?.Stop();
                        _wavePlayer?.Dispose();
                        _audioFileReader?.Dispose();

                        // Play the temporary file
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
    }
}