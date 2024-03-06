using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;

namespace Ghostie
{
    public partial class MainWindow : Window
    {
        private bool _isStarted = false;

        private MediaPlayer _mediaPlayer = new MediaPlayer();

        private Stopwatch _stopwatch = new Stopwatch();

        private const short _OneSecondInterval = 1000;
        private System.Timers.Timer _timer;

        private Sprite _ghostie;

        public MainWindow()
        {
            InitializeComponent();
            _ghostie = new Sprite(GhostSprite, Dispatcher);
            ResizeGrid();
        }

        private void ResizeGrid()
        {
            MarginRight.Width = MarginLeft.Width = new GridLength(Width / 50);
            GhostColumn.Width = new GridLength(Width / 2);

            MarginBottom.Height = MarginTop.Height = new GridLength(Height / 50);
            GhostRow.Height = new GridLength(Height / 2);

            _ghostie.UpdateSpritePosition();
        }

        private void MainWindowClick(object sender, MouseButtonEventArgs e)
        {
            if (!_isStarted)
            {
                InitMusic();
                InitTime();
                _isStarted = true;
            }
        }

        private void InitMusic()
        {
            _mediaPlayer.Open(new Uri("../../Assets/BobaBossa.mp3", UriKind.Relative));
            _mediaPlayer.Play();
            _mediaPlayer.MediaEnded += MediaEndedEventHandler;
        }

        private void MediaEndedEventHandler(object sender, EventArgs e)
        {
            _mediaPlayer.Position = TimeSpan.Zero;
            _mediaPlayer.Play();
        }

        private void InitTime()
        {
            _stopwatch.Start();
            _timer = new System.Timers.Timer(_OneSecondInterval);
            _timer.Elapsed += TimerElapsedEventHandler;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void TimerElapsedEventHandler(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke((ThreadStart)delegate {
                TimeSpan time = TimeSpan.FromMilliseconds(_stopwatch.ElapsedMilliseconds);
                Counter.Text = GetCounterText(time);
            });
        }

        private string GetCounterText(TimeSpan time)
        {
            string message = "You've been distracting Ghostie for \n";
            if (time.Hours > 0)
            {
                message += $"{time.ToString(@"hh")} ";
                if (time.Hours > 1)
                {
                    message += "hours ";
                }
                else
                {
                    message += "hour ";
                }
            }
            if (time.Minutes > 0)
            {
                message += $"{time.ToString(@"mm")} ";
                if (time.Minutes > 1)
                {
                    message += "minutes and ";
                }
                else
                {
                    message += "minute and ";
                }
            }
            message += $"{time.ToString(@"ss")} ";
            if (time.Seconds > 1)
            {
                message += "seconds.";
            }
            else
            {
                message += "second.";
            }
            return message;
        }

        private void MainWindowResized(object sender, SizeChangedEventArgs e)
        {
            ResizeGrid();
        }

        private void MuteMusicClick(object sender, RoutedEventArgs e)
        {
            if (_isStarted)
                _mediaPlayer.IsMuted = !_mediaPlayer.IsMuted;
        }
                
    }
}
