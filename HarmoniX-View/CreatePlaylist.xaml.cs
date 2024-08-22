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
    /// Interaction logic for CreatePlaylist.xaml
    /// </summary>
    public partial class CreatePlaylist : Window
    {
        public CreatePlaylist()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (PlaylistNameTextBox.Text == null || PlaylistDescriptionTextBox.Text== null)
            {
                MessageBox.Show("Please fill Playlist Name & Playlist Description !", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
    }
}
