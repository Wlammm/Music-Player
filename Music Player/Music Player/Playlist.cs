using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace Music_Player
{
    class Playlist
    {
        private List<Song> mySongs;
        public List<Song> GetSongs
        {
            get { return mySongs; }
        }

        public string GetName { get; }
        private string mySavePath;
        private StackPanel mySongPanel;
        private StackPanel myPlaylistPanel;
        private MainWindow myMainWindow;
        private GUIHandler myGUIHander;

        public Playlist(string aName, string aSavePath, StackPanel aSongPanel, StackPanel aPlaylistPanel, StackPanel aPlaylistPickPanel,  MainWindow someMainWindow, GUIHandler aHandler)
        {
            GetName = aName;
            mySavePath = aSavePath;
            mySongPanel = aSongPanel;
            myPlaylistPanel = aPlaylistPanel;
            myMainWindow = someMainWindow;
            myGUIHander = aHandler;
            mySongs = new List<Song>();

            // Playlist already exists.
            if(File.Exists(aSavePath + "/" + aName + ".xml"))
            {
                ReadPlaylistFile();
            }

            if(aName != "Local")
            {
                PlaylistButton.Create(this, aPlaylistPanel, someMainWindow, aHandler);
                Button tempButton = new Button();
                tempButton.Click += PlaylistPick_Click;
                tempButton.Content = aName;
                
                aPlaylistPickPanel.Children.Add(tempButton);
            }
        }

        private void PlaylistPick_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddSong(MainWindow.AccessSelectedSong);
        }

        public void AddSong(Song aSong)
        {
            mySongs.Add(aSong);

            SavePlaylist();
        }

        public void AddSongs(IEnumerable<Song> someSongsToAdd)
        {
            mySongs.AddRange(someSongsToAdd);

            SavePlaylist();
        }

        public Song NextSong(Song someCurrentSong)
        {
            int currentSongIndex = mySongs.IndexOf(someCurrentSong);
            
            if(currentSongIndex == mySongs.Count - 1)
                return mySongs[0];

            return mySongs[currentSongIndex + 1];
        }

        public void ShowPlaylist()
        {
            mySongPanel.Children.Clear();

            foreach(Song song in mySongs)
            {
                SongButton.Create(song, mySongPanel, myMainWindow, myGUIHander);
            }
        }

        public void ReadPlaylistFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));

            List<Song> songsToAdd = new List<Song>();

            using (TextReader reader = new StreamReader(mySavePath + "/" + GetName + ".xml"))
            {
                songsToAdd = (List<Song>)serializer.Deserialize(reader);
            }

            AddSongs(songsToAdd);

            ShowPlaylist();
        }

        public void SavePlaylist()
        {
            /*if (mySongs.Count == 0)
                return;*/

            XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));

            using (TextWriter writer = new StreamWriter(mySavePath + "/" + GetName + ".xml"))
            {
                serializer.Serialize(writer, mySongs);
            }
        }
    }
}
