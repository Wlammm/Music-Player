using DiscordRPC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows.Controls;

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
        private string myPlaylistsFolder;

        Song currentSong;
        TimeSpan currentSongDuration;

        // Value 0 är alltid local. 
        List<Playlist> myPlaylists = new List<Playlist>();
        Playlist myActivePlaylist;

        DiscordRpcClient client;

        private GUIHandler myGUIHandler;

        public MainWindow()
        { 
            InitializeComponent();

            // Discord Rich Presence initialization.
            client = new DiscordRpcClient("573842291517685821");
            client.Initialize();
            DiscordRPC();

            myGUIHandler = new GUIHandler(this);

            // Getting file paths.
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            myAppdataFolder = System.IO.Path.Combine(appdata, ".MusicPlayer");
            myLocalFolder = System.IO.Path.Combine(myAppdataFolder, "Music", "Local/");
            myPlaylistsFolder = System.IO.Path.Combine(myAppdataFolder, "Playlists");

            // Creating new file paths.
            if (!Directory.Exists(myLocalFolder))
            {
                Directory.CreateDirectory(myLocalFolder);
            }

            if (!Directory.Exists(myPlaylistsFolder))
            {
                Directory.CreateDirectory(myPlaylistsFolder);
            }

            // Loads local playlist before all other playlists making local always have index 0.
            myPlaylists.Add(new Playlist("Local", myPlaylistsFolder, pnlLocalSongs, pnlPlaylists,pnlPlaylists, this, myGUIHandler));

            // Adding timeline timer.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1f);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Subscribing to event.
            mediaPlayer.MediaOpened += MediaOpened;

            LoadPlaylists();

            // Sets local playlist to default playlist.
            myActivePlaylist = myPlaylists[0];
        }
        
        /// <summary>
        /// Called whenever a new media is loaded into the player. 
        /// Changing timeline and setting title of song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaOpened(object sender, EventArgs e)
        {
            // Setting max length of song. 
            maxTimelineValue = (int)mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            sldrTimeline.Maximum = maxTimelineValue;
            txtMaxTime.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");

            // Reseting timeline.
            sldrTimeline.Minimum = 0;

            // Changing song title. 
            txtSongTitle.Text = currentSong.AccessName;

            currentSongDuration = mediaPlayer.NaturalDuration.TimeSpan;

            SetPaused(false);
            mediaPlayer.Play();
        }
        
        /// <summary>
        /// Called when the previous song button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            PlaySong(myActivePlaylist.PreviousSong(currentSong), myActivePlaylist);
        }

        /// <summary>
        /// Called when the next song button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            PlaySong(myActivePlaylist.NextSong(currentSong), myActivePlaylist);
        }

        /// <summary>
        /// Called every second to update timeline of the song. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Called when the volume slider is changed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Volume_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = sldrVolume.Value;
        }

        /// <summary>
        /// Called when play song button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            SetPaused(false);
        }

        /// <summary>
        /// Called when pause song button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            SetPaused(true);
        }

        /// <summary>
        /// Called when user stops interacting with song timeline. 
        /// Allows timeline to continue ticking forward with music and setting new song location.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTimeline_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            TimeSpan newSongPosition = TimeSpan.FromSeconds(Convert.ToInt32(sldrTimeline.Value));
            mediaPlayer.Position = newSongPosition;
            timelineIsChanging = false;
        }

        /// <summary>
        /// Stops timeline ticking to prevent timeline from moving while user is interacting.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTimeline_DragStarted(object sender, DragStartedEventArgs e)
        {
            timelineIsChanging = true;
        }

        /// <summary>
        /// Switching to all songs tab. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllSongsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.AllSong);
        }

        /// <summary>
        /// Switching to Youtube tab. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YoutubeTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Youtube);
        }

        /// <summary>
        /// Switching to local tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Local);
        }

        /// <summary>
        /// Switching to downloads tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Download);
        }

        /// <summary>
        /// Switching to playlists tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaylistTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Playlist);
        }

        /// <summary>
        /// Switching to settings tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Setting);
        }

        /// <summary>
        /// Called when Add playlist button is clicked.
        /// Shows popup box for playlist name. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            txtPlaylistName.Text = "";
            myGUIHandler.ShowElement(GUIHandler.GUITab.PlaylistName);
        }

        /// <summary>
        /// Exits application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApplication_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Called when user clicks Create playlist in playlist name popup box.
        /// Hides the playlist name popup and creates new playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePlaylistName_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(myPlaylistsFolder + "/" + txtPlaylistName.Text + ".xml"))
                return;

            myGUIHandler.HideElement(GUIHandler.GUITab.PlaylistName);

            Playlist list = new Playlist(txtPlaylistName.Text, myPlaylistsFolder, pnlPlaylistsSongs, pnlPlaylists, listPlaylistsStack, this, myGUIHandler);
            myPlaylists.Add(list);

            list.SavePlaylist();
        }

        /// <summary>
        /// Called when the fade background in popups are clicked.
        /// Closing the popup window that is currently active.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FadeBackground_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BackgroundClick();
        }

        /// <summary>
        /// Called when add local songs button is clicked.
        /// Opens file explorer for user to add songs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLocalSong_Click(object sender, RoutedEventArgs e)
        {
            ImportFiles();
        }
    }
}