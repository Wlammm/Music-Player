using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Music_Player
{
    class PlaylistListButton
    {
        private Playlist myPlaylist;
        private StackPanel myPanel;
        private MainWindow myMainWindow;
        private GUIHandler myHandler;

        private PlaylistListButton(Playlist aPlaylist, StackPanel aPanel, MainWindow aMainWindow, Button myButton, GUIHandler aHandler)
        {
            myPlaylist = aPlaylist;
            myPanel = aPanel;
            myMainWindow = aMainWindow;
            myHandler = aHandler;

            myButton.Click += Click; ;
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            myPlaylist.AddSong(MainWindow.AccessSelectedSong);
            myHandler.HideElement(GUIHandler.GUITab.PlaylistsList);
            myPlaylist.ShowPlaylist();
        }

        public static PlaylistListButton Create(Playlist aPlaylist, StackPanel aPanel, MainWindow aMainWindow, GUIHandler aHandler)
        {
            Button tempButton = new Button();
            tempButton.Width = 880;
            tempButton.Height = 25;
            tempButton.Content = aPlaylist.GetName;
            tempButton.VerticalAlignment = VerticalAlignment.Top;

            aPanel.Children.Add(tempButton);

            PlaylistListButton playlistButton = new PlaylistListButton(aPlaylist, aPanel, aMainWindow, tempButton, aHandler);
            return playlistButton;
        }
    }
}
