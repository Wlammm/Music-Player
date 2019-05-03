using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.IO;

namespace Music_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();

        private int currentTimelineValue = 0;
        private int maxTimelineValue = 100;

        private bool isPaused = true;
        private bool timelineIsChanging = false;

        private string myAppdataFolder;
        private string myLocalFolder;

        Song currentSong;

        public MainWindow()
        {
            InitializeComponent();

            string appdata= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            myAppdataFolder = System.IO.Path.Combine(appdata, ".MusicPlayer");
            myLocalFolder = System.IO.Path.Combine(myAppdataFolder, "Music", "Local/");
            if (!Directory.Exists(myLocalFolder))
            {
                Directory.CreateDirectory(myLocalFolder);
            }

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            mediaPlayer.MediaOpened += MediaOpened;
        }

        private void MediaOpened(object sender, EventArgs e)
        {
            maxTimelineValue = (int)mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
            currentTimelineValue = 0;

            sldrTimeline.Minimum = 0;
            sldrTimeline.Maximum = maxTimelineValue;
            txtMaxTime.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
            txtSongTitle.Text = currentSong.GetName;

            if (!isPaused)
            {
                mediaPlayer.Play();
            }
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {

        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                txtCurrentTime.Text = mediaPlayer.Position.ToString(@"mm\:ss");
                if (!timelineIsChanging)
                {
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

            if(tempFileDialog.ShowDialog() == true)
            {
                // Copying selected files to local folder (%appdata%/.MusicPlayer/Music/Local)
                for(int i = 0; i < tempFileDialog.FileNames.Length; i++)
                {
                    string tempSourceFilePath = tempFileDialog.FileNames[i];
                    string tempNewFilePath = myLocalFolder + System.IO.Path.GetFileName(tempSourceFilePath);

                    File.Copy(tempSourceFilePath, tempNewFilePath, true);
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
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            btnPause.Visibility = Visibility.Collapsed;
            btnPlay.Visibility = Visibility.Visible;
            isPaused = true;
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
    }
}
