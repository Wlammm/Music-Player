using System;
using System.Collections.Generic;
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

        private Button myButton;
        public Button GetButton
        {
            get { return myButton; }
        }

        private SongButton(Song aSong, StackPanel aPanel, MainWindow aMainWindow)
        {
            mySong = aSong;
            myMainWindow = aMainWindow;
        }

        public static SongButton Create(Song aSong, StackPanel aPanel, MainWindow aMainWindow)
        {
            SongButton songButton = new SongButton(aSong, aPanel, aMainWindow);

            Button tempButton = new Button();
            tempButton.Width = 880;
            tempButton.Height = 25;
            tempButton.Content = aSong.GetName;
            tempButton.VerticalAlignment = VerticalAlignment.Top;

            aPanel.Children.Add(tempButton);
            tempButton.Click += songButton.SongClicked;

            return songButton;
        }

        private void SongClicked(object sender, RoutedEventArgs e)
        {
            myMainWindow.PlaySong(mySong);
        }
    }
}
