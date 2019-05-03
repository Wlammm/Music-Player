using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.IO;
using DiscordRPC;
using System.Collections.Generic;

namespace Music_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();

        private int maxTimelineValue = 100;

        private bool isPaused = true;
        private bool timelineIsChanging = false;

        private string myAppdataFolder;
        private string myLocalFolder;

        Song currentSong;
        TimeSpan currentSongDuration;

        DiscordRpcClient client;

        private enum Tab { AllSongs, Youtube, Local, Downloads, Playlists, Settings };
        private Tab myActiveTab = Tab.AllSongs;
        private Dictionary<Tab, Grid> myTabs;

        public MainWindow()
        {
            InitializeComponent();

            // Discord initialization.
            client = new DiscordRpcClient("573842291517685821");
            client.Initialize();
            DiscordRPC();

            // Getting file paths.
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            myAppdataFolder = System.IO.Path.Combine(appdata, ".MusicPlayer");
            myLocalFolder = System.IO.Path.Combine(myAppdataFolder, "Music", "Local/");

            // Creating new file paths.
            if (!Directory.Exists(myLocalFolder))
            {
                Directory.CreateDirectory(myLocalFolder);
            }

            // Adding timeline timer.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1f);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Subscribing to event.
            mediaPlayer.MediaOpened += MediaOpened;

            myTabs = new Dictionary<Tab, Grid>
            {
                {Tab.AllSongs, AllSongsGrid },
                {Tab.Youtube, YoutubeGrid },
                {Tab.Local, LocalGrid },
                {Tab.Downloads, DownloadsGrid },
                {Tab.Playlists, PlaylistsGrid },
                {Tab.Settings, SettingsGrid }
            };
        }

        public void PlaySong(Song aSong)
        {
            currentSong = aSong;
            mediaPlayer.Open(aSong.GetUri);
        }

        private void MediaOpened(object sender, EventArgs e)
        {
            // Setting max length of song. 
            maxTimelineValue = (int)mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            sldrTimeline.Maximum = maxTimelineValue;
            txtMaxTime.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

            // Reseting timeline.
            sldrTimeline.Minimum = 0;

            // Changing song title. 
            txtSongTitle.Text = currentSong.GetName;

            currentSongDuration = mediaPlayer.NaturalDuration.TimeSpan;

            if (!isPaused)
            {
                mediaPlayer.Play();
            }
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

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            DiscordRPC();
            if (mediaPlayer.Source != null)
            {
                // Updating current timeline text.
                txtCurrentTime.Text = mediaPlayer.Position.ToString(@"mm\:ss");
                if (!timelineIsChanging)
                {
                    // Updating timeline if user is not changing.
                    sldrTimeline.Value = (int)mediaPlayer.Position.TotalSeconds;
                }
            }
        }

        private void SelectSong_Click(object sender, RoutedEventArgs e)
        {
            currentSong = ImportFiles();

            mediaPlayer.Open(currentSong.GetUri);
        }

        /// <summary>
        /// Opens a FileDialog and imports songs to music player.
        /// </summary>
        Song ImportFiles()
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
                    if (!File.Exists(tempNewFilePath))
                    {
                        File.Copy(tempSourceFilePath, tempNewFilePath, false);
                    }
                    SongButton b = new SongButton(new Song(tempNewFilePath), pnlSongList, this);
                }
            }

            return new Song(tempFileDialog.FileNames.Last());
        }

        private void Volume_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = sldrVolume.Value;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            btnPause.Visibility = Visibility.Visible;
            btnPlay.Visibility = Visibility.Collapsed;
            isPaused = false;
            DiscordRPC();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            btnPause.Visibility = Visibility.Collapsed;
            btnPlay.Visibility = Visibility.Visible;
            isPaused = true;
            DiscordRPC();
        }

        private void MyTimeline_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            TimeSpan newSongPosition = TimeSpan.FromSeconds(Convert.ToInt32(sldrTimeline.Value));
            mediaPlayer.Position = newSongPosition;
            timelineIsChanging = false;
        }

        private void MyTimeline_DragStarted(object sender, DragStartedEventArgs e)
        {
            timelineIsChanging = true;
        }


        private void SetActiveTab(Tab aTab)
        {
            myTabs[myActiveTab].Visibility = Visibility.Collapsed;
            myTabs[aTab].Visibility = Visibility.Visible;

            myActiveTab = aTab;
        }

        private void AllSongsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetActiveTab(Tab.AllSongs);
        }

        private void YoutubeTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetActiveTab(Tab.Youtube);
        }

        private void LocalTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetActiveTab(Tab.Local);
        }

        private void DownloadsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetActiveTab(Tab.Downloads);
        }

        private void PlaylistTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetActiveTab(Tab.Playlists);
        }

        private void SettingsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SetActiveTab(Tab.Settings);
        }
    }
}
