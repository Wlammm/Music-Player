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

        private Button myButton;
        public Button GetButton
        {
            get { return myButton; }
        }

        private SongButton(Song aSong, StackPanel aPanel, MainWindow aMainWindow, GUIHandler aHandler)
        {
            mySong = aSong;
            myGUIHandler = aHandler;
            myMainWindow = aMainWindow;
        }

        public static SongButton Create(Song aSong, StackPanel aPanel, MainWindow aMainWindow, GUIHandler aHandler)
        {
            SongButton songButton = new SongButton(aSong, aPanel, aMainWindow, aHandler);

            Button tempButton = new Button();
            tempButton.Width = 880;
            tempButton.Height = 25;
            tempButton.Content = aSong.GetName;
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

        private void ContextAddToPlaylist(object sender, RoutedEventArgs e)
        {
            myGUIHandler.ShowElement(GUIHandler.GUITab.PlaylistsList);
            MainWindow.AccessSelectedSong = mySong;
        }

        private void SongClicked(object sender, RoutedEventArgs e)
        {
            myMainWindow.PlaySong(mySong);
        }
    }
}
