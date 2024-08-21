﻿using HarmoniX_Controller.Controllers;
using HarmoniX_Repository.Models;
using HarmoniX_Repository.Repositories;
using HarmoniX_Service.Services;
using Microsoft.Win32;
using NAudio.Wave;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace HarmoniX_View
{
    /// <summary>
    /// Interaction logic for AddSongDetail.xaml
    /// </summary>
    public partial class AddSongDetail : Window
    {
        private Account _account;
        private DispatcherTimer timer;
        private TimeSpan songDuration;
        private TimeSpan currentTime;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private string fileName;
        private bool isPlaying = false;
        private CategoryService categoryService = new();
        public Song SelectedSong { get; set; } = null;
        public event Action OnSongDetailClosed;

        public AddSongDetail(Account account)
        {
            InitializeComponent();
            InitializeTimer();
            _account = account;
        }
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (audioFile != null)
            {
                songDuration = audioFile.TotalTime;
                currentTime = audioFile.CurrentTime;
                if (currentTime < songDuration)
                {
                    TimelineProgressBar.Maximum = songDuration.TotalSeconds;
                    TimelineProgressBar.Value = currentTime.TotalSeconds;
                    ElapsedTimeTextBlock.Text = currentTime.ToString(@"mm\:ss");
                    RemainingTimeTextBlock.Text = (songDuration - currentTime).ToString(@"mm\:ss");
                }
                else
                {
                    timer.Stop();
                    isPlaying = false;
                    PlayButton.Content = "▶️";
                }
            }
        }
        private void BT_Click_Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                DefaultExt = ".mp3",
                Filter = "Audio Files (*.mp3;*.wav)|*.mp3;*.wav"
            };
            bool? dialogOK = fileDialog.ShowDialog();
            if (dialogOK == true)
            {
                fileName = fileDialog.FileName;
                FileName.Text = fileDialog.SafeFileName;

                if (audioFile != null)
                {
                    audioFile.Dispose();
                }

                if (outputDevice != null)
                {
                    outputDevice.Dispose();
                }

                audioFile = new AudioFileReader(fileName);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
            }
        }

        private void BT_Click_PlayPause(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(fileName) && audioFile != null)
            {
                if (isPlaying)
                {
                    outputDevice.Pause();
                    isPlaying = false;
                    PlayButton.Content = "▶️";
                    timer.Stop();
                }
                else
                {
                    outputDevice.Play();
                    isPlaying = true;
                    PlayButton.Content = "⏸";
                    timer.Start();
                }
            }
            else
            {
                MessageBox.Show("Please open a song file first.", "No File Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
        }



        private void ClearForm()
        {
            SongNameTextBox.Clear();
            AuthorTextBox.Clear();
            SongCategoryIdComboBox.SelectedIndex = -1;
            FileName.Text = string.Empty;

            fileName = string.Empty;

            TimelineProgressBar.Value = 0;
            ElapsedTimeTextBlock.Text = "00:00";
            RemainingTimeTextBlock.Text = "00:00";
            outputDevice.Stop();
            isPlaying = false;
            PlayButton.Content = "▶️";
            timer.Stop();
        }


        public byte[] FileToByteArray(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                outputDevice.Stop();
                isPlaying = false;
                PlayButton.Content = "▶️";
                timer.Stop();
            }

            audioFile?.Dispose();
            outputDevice?.Dispose();

            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (isPlaying)
            {
                outputDevice.Stop();
                isPlaying = false;
                PlayButton.Content = "▶️";
                timer.Stop();
            }

            audioFile?.Dispose();
            outputDevice?.Dispose();
            OnSongDetailClosed?.Invoke();
            base.OnClosed(e);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SongLabel.Content = "Create a Song";
            SongCategoryIdComboBox.ItemsSource = await categoryService.GetAllCategories();
            SongCategoryIdComboBox.DisplayMemberPath = "CategoryName";
            SongCategoryIdComboBox.SelectedValuePath = "CategoryId";

            SongCategoryIdComboBox.SelectedValue = 1;

            if (SelectedSong != null)
            {
                SongLabel.Content = "Update a Song";
                SongNameTextBox.Text = SelectedSong.SongTitle.ToString();
                AuthorTextBox.Text = SelectedSong.ArtistName.ToString();
                SongCategoryIdComboBox.SelectedValue = SelectedSong.CategoryId.ToString();
            }
        }
    }
}