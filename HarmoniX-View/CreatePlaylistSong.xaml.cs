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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HarmoniX_View
{
    /// <summary>
    /// Interaction logic for CreatePlaylistSong.xaml
    /// </summary>
    public partial class CreatePlaylistSong : Window
    {
        private PlaylistssongService _playlistsongService = new();
        public CreatePlaylistSong()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPlayListID.Text == null || txtSongID.Text == null)
                {
                    MessageBox.Show("Please fill Playlist ID & Song ID !", "Add Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Playlistssong playlistssong = new Playlistssong
                {
                    PlaylistId = int.Parse(txtPlayListID.Text),
                    SongId = int.Parse(txtSongID.Text),
                    
                };
                _playlistsongService.CreatePlayListSong(playlistssong);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the song: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
