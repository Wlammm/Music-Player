using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace Music_Player
{
    [Serializable]
    public class Song
    {
        public string GetName { get; set; }
        public string GetSongPath { get; set; }

        public Song()
        {
            // Run it down mid.
        }

        public Song(string somePath)
        {
            GetSongPath = somePath;
            GetName = Path.GetFileNameWithoutExtension(somePath);
        }

        public Uri GetUri()
        {
            return new Uri(GetSongPath);
        }
    }
}
