using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace Music_Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath = "C:/Users/William Arnback/Desktop/Rammstein - Du Hast (Official Video).mp3";

        private MediaPlayer mediaPlayer = new MediaPlayer();

        private int currentTimelineValue = 0;
        private int maxTimelineValue = 100;

        private bool isPaused = true;

        Song rammstein;

        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
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
            txtSongTitle.Text = rammstein.GetName;

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

        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                txtCurrentTime.Text = mediaPlayer.Position.ToString(@"mm\:ss");
                sldrTimeline.Value = (int)mediaPlayer.Position.TotalSeconds;
            }
        }

        private void SelectSong_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                rammstein = new Song(openFileDialog.FileName);
                mediaPlayer.Open(rammstein.GetUri);
            }
        }

        private void Volume_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = sldrVolume.Value;
        }

        private void Timeline_Clicked(object sender, DragEventArgs e)
        {
            
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
    }
}
