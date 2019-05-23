using DiscordRPC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Music_Player
{
    partial class MainWindow : Window
    {
        private static Song mySelctedSong;
        public static Song AccessSelectedSong
        {
            get { return mySelctedSong; }
            set { mySelctedSong = value; }
        }


        public void PlaySong(Song aSong, Playlist aPlaylist)
        {
            currentSong = aSong;
            myActivePlaylist = aPlaylist;
            mediaPlayer.Open(aSong.GetUri());
        }

        void BackgroundClick()
        {
            myGUIHandler.HideElement(GUIHandler.GUITab.PlaylistsList);
            myGUIHandler.HideElement(GUIHandler.GUITab.PlaylistName);
        }

        void SetPaused(bool pause)
        {
            isPaused = pause;

            if (pause)
            {
                mediaPlayer.Pause();
                btnPause.Visibility = Visibility.Collapsed;
                btnPlay.Visibility = Visibility.Visible;
                return;
            }

            mediaPlayer.Play();
            btnPause.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Collapsed;
        }

        void DiscordRPC()
        {
            client.SetPresence(new RichPresence()
            {
                Details = currentSong == null ? "Paused." : "Playing: " + currentSong.GetName,
                State = mediaPlayer.Position.ToString(@"mm\:ss") + "/" + currentSongDuration.ToString(@"mm\:ss"),
                Assets = new Assets()
                {
                    LargeImageKey = "image_large",
                    LargeImageText = "Lachee's Discord IPC Library",
                    SmallImageKey = "image_small"
                }
            });
        }

        /// <summary>
        /// Opens a FileDialog and imports songs to music player.
        /// </summary>
        void ImportFiles()
        {
            OpenFileDialog tempFileDialog = new OpenFileDialog();
            tempFileDialog.Filter = "MP3 files (*.mp3)|*.mp3| All files (*.*)|*.*";
            tempFileDialog.Multiselect = true;

            if (tempFileDialog.ShowDialog() == true)
            {
                // Copying selected files to local folder (%appdata%/.MusicPlayer/Music/Local)
                for (int i = 0; i < tempFileDialog.FileNames.Length; i++)
                {
                    MediaPlayer tempMediaPlayer = new MediaPlayer();
                    tempMediaPlayer.Open(new Uri(tempFileDialog.FileNames[i]));

                    string tempSourceFilePath = tempFileDialog.FileNames[i];
                    string tempNewFilePath = myLocalFolder + System.IO.Path.GetFileName(tempSourceFilePath);

                    List<Song> songsAdded = new List<Song>();

                    if (!File.Exists(tempNewFilePath))
                    {
                        File.Copy(tempSourceFilePath, tempNewFilePath, false);
                        songsAdded.Add(new Song(tempNewFilePath));
                    }

                    myPlaylists[0].AddSongs(songsAdded);
                    myPlaylists[0].ShowPlaylist();
                }
            }
        }
        
        private void LoadPlaylists()
        {
            string[] playlistPaths = Directory.GetFiles(myPlaylistsFolder);

            for (int i = 0; i < playlistPaths.Length; i++)
            {
                myPlaylists.Add(new Playlist(Path.GetFileNameWithoutExtension(playlistPaths[i]), myPlaylistsFolder, pnlPlaylistsSongs, pnlPlaylists, listPlaylistsStack, this, myGUIHandler));
            }
        }
    }
}
