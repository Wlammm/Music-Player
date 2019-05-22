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

            // Discord initialization.
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

            myPlaylists.Add(new Playlist("Local", myPlaylistsFolder, pnlLocalSongs, pnlPlaylists,pnlPlaylists, this, myGUIHandler));

            // Adding timeline timer.
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1f);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Subscribing to event.
            mediaPlayer.MediaOpened += MediaOpened;

            LoadPlaylists();

            myActivePlaylist = myPlaylists[0];
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

            SetPaused(false);
            mediaPlayer.Play();
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

        private void Volume_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = sldrVolume.Value;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            SetPaused(false);
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            SetPaused(true);
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

        private void AllSongsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.AllSong);
        }

        private void YoutubeTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Youtube);
        }

        private void LocalTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Local);
        }

        private void DownloadsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Download);
        }

        private void PlaylistTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Playlist);
        }

        private void SettingsTab_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myGUIHandler.SwapTab(GUIHandler.GUITab.Setting);
        }

        private void AddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            txtPlaylistName.Text = "";
            myGUIHandler.ShowElement(GUIHandler.GUITab.PlaylistName);
        }

        private void ExitApplication_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Environment.Exit(0);
        }

        private void CreatePlaylistName_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(myPlaylistsFolder + "/" + txtPlaylistName.Text + ".xml"))
                return;

            myGUIHandler.HideElement(GUIHandler.GUITab.PlaylistName);

            Playlist list = new Playlist(txtPlaylistName.Text, myPlaylistsFolder, listPlaylistsStack, pnlPlaylists, pnlPlaylists, this, myGUIHandler);
            myPlaylists.Add(list);

            list.SavePlaylist();
        }

        private void FadeBackground_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BackgroundClick();
        }

        private void AddLocalSong_Click(object sender, RoutedEventArgs e)
        {
            ImportFiles();
        }

        private void Application_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}