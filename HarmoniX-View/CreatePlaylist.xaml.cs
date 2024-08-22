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
using HarmoniX_Repository.Models;
using HarmoniX_Service.Services;

namespace HarmoniX_View
{
    /// <summary>
    /// Interaction logic for CreatePlaylist.xaml
    /// </summary>
    public partial class CreatePlaylist : Window

    {
        private readonly PlaylistService _playlistService = new();
        private Account _account;

        //public CreatePlaylist()
        //{
        //}

        public CreatePlaylist(Account account)
        {
            InitializeComponent();
            _account = account;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(PlaylistNameTextBox.Text) || string.IsNullOrWhiteSpace(PlaylistDescriptionTextBox.Text))
                {
                    MessageBox.Show("Please fill Playlist Name & Playlist Description!", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Playlist playlist = new Playlist
                {
                    PlaylistName = PlaylistNameTextBox.Text,
                    PlaylistDescription = PlaylistDescriptionTextBox.Text,
                    AccountId = _account.AccountId
                };
                await _playlistService.CreatePlayList(playlist);

                MessageBox.Show("Playlist created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the playlist: {ex.Message}\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            this.Close();
        }


    }
}
