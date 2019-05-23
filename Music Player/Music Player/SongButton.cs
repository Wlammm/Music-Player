using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Music_Player
{
    class SongButton
    {
        private Song mySong;
        private MainWindow myMainWindow;
        private GUIHandler myGUIHandler;
        private Playlist myPlaylist;

        private Button myButton;
        public Button GetButton
        {
            get { return myButton; }
        }

        private SongButton(Song aSong, StackPanel aPanel, MainWindow aMainWindow, GUIHandler aHandler, Playlist aPlaylist)
        {
            mySong = aSong;
            myPlaylist = aPlaylist;
            myGUIHandler = aHandler;
            myMainWindow = aMainWindow;
        }

        /// <summary>
        /// Creates a button for a song.
        /// </summary>
        /// <param name="aSong"></param>
        /// <param name="aPanel"></param>
        /// <param name="aMainWindow"></param>
        /// <param name="aHandler"></param>
        /// <param name="aPlaylist"></param>
        /// <returns></returns>
        public static SongButton Create(Song aSong, StackPanel aPanel, MainWindow aMainWindow, GUIHandler aHandler, Playlist aPlaylist)
        {
            SongButton songButton = new SongButton(aSong, aPanel, aMainWindow, aHandler, aPlaylist);

            Button tempButton = new Button();
            tempButton.Width = 880;
            tempButton.Height = 25;
            tempButton.Content = aSong.AccessName;
            tempButton.VerticalAlignment = VerticalAlignment.Top;

            ContextMenu menu = new ContextMenu();
            MenuItem items = new MenuItem();

            items.Header = "Add to playlist";
            menu.Items.Add(items);
            tempButton.ContextMenu = menu;
            items.Click += songButton.ContextAddToPlaylist;

            aPanel.Children.Add(tempButton);
            tempButton.Click += songButton.SongClicked;

            return songButton;
        }

        /// <summary>
        /// Called when a song is added to playlist via context menu. 
        /// Displays playlist list popup allowing user to decide what playlist to add song to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextAddToPlaylist(object sender, RoutedEventArgs e)
        {
            myGUIHandler.ShowElement(GUIHandler.GUITab.PlaylistsList);
            MainWindow.AccessSelectedSong = mySong;
        }

        /// <summary>
        /// Called when a song is clicked.
        /// Plays the selected song.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongClicked(object sender, RoutedEventArgs e)
        {
            myMainWindow.PlaySong(mySong, myPlaylist);
        }
    }
}
