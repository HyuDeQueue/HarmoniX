using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using NAudio.Wave;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HarmoniX_View
{
    public partial class MainWindow : Window
    {
        private IWavePlayer _wavePlayer;
        private AudioFileReader _audioFileReader;
        private readonly SongService _songService = new();
        private readonly PlaylistService _playlistService = new();
        public event Action OnSongDetailClosed;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Account _account;
        private readonly QueueService _queueService;
        private TimeSpan _totalDuration;
        private DispatcherTimer _timer;


        public MainWindow(Account account)
        {
            InitializeComponent();
            _account = account;
            LoadSongs();
            _queueService = new QueueService();

            _wavePlayer = new WaveOutEvent();
            _wavePlayer.PlaybackStopped += OnWavePlayerPlaybackStopped;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            SetVolume(VolumeSlider.Value);

        }

        // Add the Window_MouseDown method
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void AddToQueueButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Song selectedSong)
            {
                bool isAdded = _queueService.AddSongToQueue(selectedSong);
                if (isAdded)
                {
                    if (!_queueService.IsPlaying)
                    {
                        await PlayNextSong();
                    }
                }
                else
                {
                    MessageBox.Show("Failed to add the song to the queue.");
                }
            }
            UpdateQueueDataGrid();
        }


        private async void LoadSongs()
        {
            var songs = await _songService.GetAllSongsAsync();
            SongsDataGrid.ItemsSource = songs;
        }
        private async void LoadPlayLists()
        {
            var playlists = await _playlistService.GetAllPlaylist();
            PlaylistDataGrid.ItemsSource = playlists;
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            AddSongDetail detail = new AddSongDetail(_account);
            detail.OnSongDetailClosed += () =>
            {
                LoadSongs();
            };
            detail.ShowDialog();
        }

        //private async void PlayButton_Click(object sender, RoutedEventArgs e)
        //{
        //    await PlaySong();
        //}

        private void btnCreatePlaylist_Click(object sender, RoutedEventArgs e)
        {
            CreatePlaylist createPlaylist = new CreatePlaylist(_account);
            createPlaylist.ShowDialog();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task PlaySong(Song song)
        {
            try
            {
                var stream = await _songService.GetSongStreamAsync(song.SongMedia);
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
                    _wavePlayer.PlaybackStopped += OnWavePlayerPlaybackStopped;
                    _audioFileReader = new AudioFileReader(tempFilePath);
                    _wavePlayer.Init(_audioFileReader);

                    //MessageBox.Show("Playing song now.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                    _wavePlayer.Play();
                    Dispatcher.Invoke(() =>
                    {
                        NowPlayingTextBlock.Text = $"Now Playing: {song.SongTitle}";
                        SongInfoTextBlock.Text = $"{song.SongTitle} - {song.ArtistName}";
                    });
                }
                else
                {
                    MessageBox.Show("Failed to retrieve the song.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while trying to play the song: {ex.Message}");
            }
        }



        private async Task PlayNextSong()
        {
            try
            {
                Song nextSong = _queueService.PlayNextSong();
                if (nextSong != null)
                {
                    await PlaySong(nextSong);
                }
                else
                {
                    Dispatcher.Invoke(() =>
                    {
                        NowPlayingTextBlock.Text = "Queue is empty.";
                        _wavePlayer?.Stop();  // Stop player if queue is empty
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while playing the next song: {ex.Message}");
            }
            finally
            {
                UpdateQueueDataGrid();
            }
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (SongsDataGrid.SelectedItem is Song selectedSong)
            {
                await PlaySong(selectedSong);
            }
            else
            {
                MessageBox.Show("Please select a song to play.");
            }
            UpdateQueueDataGrid();
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            var playPauseButtonTemplate = PlayPauseButton.Template;

            var playPauseTextBlock = (TextBlock)playPauseButtonTemplate.FindName("PlayPauseTextBlock", PlayPauseButton);

            if (_wavePlayer?.PlaybackState == PlaybackState.Playing)
            {
                _wavePlayer.Pause();
                playPauseTextBlock.Text = "▶️";
            }
            else if (_wavePlayer?.PlaybackState == PlaybackState.Paused)
            {
                _wavePlayer.Play();
                playPauseTextBlock.Text = "⏸️"; 
            }
            else
            {
                MessageBox.Show("No song is currently loaded.");
            }
        }



        private void OnWavePlayerPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_audioFileReader != null)
            {
                _audioFileReader.Dispose();
                _audioFileReader = null;
            }

            Dispatcher.Invoke(async () =>
            {
                //MessageBox.Show("The song has finished playing.", "Song Finished", MessageBoxButton.OK, MessageBoxImage.Information);
                await PlayNextSong();
            });
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_audioFileReader != null)
            {
                _totalDuration = _audioFileReader.TotalTime;
                SongProgressBar.Maximum = _totalDuration.TotalSeconds;
                SongProgressBar.Value = _audioFileReader.CurrentTime.TotalSeconds;

                CurrentTimeTextBlock.Text = _audioFileReader.CurrentTime.ToString(@"mm\:ss");

                TimeSpan timeRemaining = _totalDuration - _audioFileReader.CurrentTime;
                TotalTimeTextBlock.Text = timeRemaining.ToString(@"mm\:ss");
            }
        }

        private void UpdateQueueDataGrid()
        {
            QueueDataGrid.ItemsSource = null;
            QueueDataGrid.ItemsSource = _queueService.GetCurrentQueue();
        }

        private void SetVolume(double volume)
        {
            if (_wavePlayer != null && _audioFileReader != null)
            {
                _audioFileReader.Volume = (float)volume;
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetVolume(e.NewValue);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Song selected = SongsDataGrid.SelectedItem as Song;
            if (selected == null)
            {
                MessageBox.Show("Please select a song!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (selected.AccountId != _account.AccountId)
            {
                MessageBox.Show("You have no permission to do this action!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            AddSongDetail d = new AddSongDetail(_account);
            d.SelectedSong = selected;
            d.ShowDialog();
            LoadSongs();
        }

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            _queueService.ShuffleQueue();
            UpdateQueueDataGrid();
        }

        private void UpQueue_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Song selectedSong)
            {
                _queueService.MoveSongUpInQueue(selectedSong);
                UpdateQueueDataGrid();
            }
        }
        private void DownQueue_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Song selectedSong)
            {
                _queueService.MoveSongDownInQueue(selectedSong);
                UpdateQueueDataGrid();
            }
        }
        private void RemoveQueue_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Song selectedSong)
            {
                _queueService.RemoveSongFromQueue(selectedSong);
                UpdateQueueDataGrid();
            }
        }

        private async void AllSongs_Click(object sender, RoutedEventArgs e)
        {
            var songs = await _songService.GetAllSongsAsync();
            SongsDataGrid.ItemsSource = null;
            SongsDataGrid.ItemsSource = songs;
        }

        private async void YourSongs_Click(object sender, RoutedEventArgs e)
        {
            var songs = await _songService.GetSongsByIdAsync(_account.AccountId);
            SongsDataGrid.ItemsSource = null;
            SongsDataGrid.ItemsSource = songs;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string songTitle = SongNameTextBox.Text;
                string artistName = ArtistNameTextBox.Text;

                List<Song> searchResults = await _songService.SearchSongsAsync(songTitle, artistName);

                if (searchResults.Any())
                {
                    SongsDataGrid.ItemsSource = null;
                    SongsDataGrid.ItemsSource = searchResults;
                }
                else
                {
                    SongsDataGrid.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while searching: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

