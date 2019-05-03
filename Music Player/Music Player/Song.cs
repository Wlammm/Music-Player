using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace Music_Player
{
    public class Song
    {
        private string myName = "";
        public string GetName
        {
            get { return myName; }
        }

        private Uri myUri;
        public Uri GetUri
        {
            get { return myUri; }
        }

        public Song(string aFilePath)
        {
            myUri = new Uri(aFilePath);
            myName = Path.GetFileNameWithoutExtension(aFilePath);
        }
    }
}
