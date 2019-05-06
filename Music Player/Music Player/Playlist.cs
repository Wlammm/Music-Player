using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

        public Playlist(string aName, string aSavePath)
        {
            GetName = aName;
            mySavePath = aSavePath;

            mySongs = new List<Song>();

            // Playlist already exists.
            if(File.Exists(aSavePath + "/" + aName + ".json"))
            {
                //ReadPlaylistFile();
            }
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

        public void ReadPlaylistFile()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));

            using (TextReader reader = new StreamReader(mySavePath + "/" + GetName + ".xml"))
            {
                mySongs = (List<Song>)serializer.Deserialize(reader);
            }

            Debug.Print("");
        }

        public void SavePlaylist()
        {
            if (mySongs.Count == 0)
                return;

            XmlSerializer serializer = new XmlSerializer(typeof(List<Song>));

            using (TextWriter writer = new StreamWriter(mySavePath + "/" + GetName + ".xml"))
            {
                serializer.Serialize(writer, mySongs);
            }
        }
    }
}
