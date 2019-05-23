using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace Music_Player
{
    [Serializable]
    public class Song
    {
        public string AccessName { get; set; }
        public string AccessSongPath { get; set; }

        public Song()
        {
            // Run it down mid.s
        }

        public Song(string somePath)
        {
            AccessSongPath = somePath;
            AccessName = Path.GetFileNameWithoutExtension(somePath);
        }

        /// <summary>
        /// Returns the Uri of the song path.
        /// </summary>
        /// <returns></returns>
        public Uri GetUri()
        {
            return new Uri(AccessSongPath);
        }
    }
}
