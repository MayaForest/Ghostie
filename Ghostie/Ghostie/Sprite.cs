using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Timers;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Threading;

namespace Ghostie
{
    internal class Sprite
    {
        private Image _spriteSheet;

        private const short _AnimationFps = 1000 / 20;
        private System.Timers.Timer _timer;

        private const byte _Height = 48;
        private const byte _Width = 225;
        private const byte _NbrOfFrames = 9;
        private const byte _FrameWidth = 225 / _NbrOfFrames;

        private double xOffset = 100;
        private byte _frame = 0;

        private RectangleGeometry[] _rectangleGeometries;

        private Dispatcher _mainDispatcher;

        public Sprite(Image spriteSheet, Dispatcher dispatcher)
        {
            this._spriteSheet = spriteSheet;

            _mainDispatcher = dispatcher;

            _rectangleGeometries = new RectangleGeometry[_NbrOfFrames];
            for(int frame = 0; frame < _NbrOfFrames; frame++)
            {
                _rectangleGeometries[frame] = new RectangleGeometry();
                _rectangleGeometries[frame].Rect =  new System.Windows.Rect(frame * _FrameWidth, 0, _FrameWidth, _Height);
            }
            UpdateRect();

            _timer = new System.Timers.Timer(_AnimationFps);
            _timer.Elapsed += TimerElapsedEventHandler;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void UpdateRect()
        {
            _spriteSheet.Clip = _rectangleGeometries[GetFrame()];
            UpdateSpritePosition();
        }

        public byte GetFrame()
        {
            return _frame;
        }

        public void UpdateSpritePosition()
        {
            xOffset = (_Width / 2) - (_FrameWidth / 2) - (_FrameWidth * _frame);
            _spriteSheet.RenderTransform = new TranslateTransform(xOffset, 0);
        }

        private void TimerElapsedEventHandler(object sender, EventArgs e)
        {
            _mainDispatcher.BeginInvoke((ThreadStart)delegate {
                NextFrame();
                UpdateRect();
            });
        }

        public void NextFrame()
        {
            _frame++;
            _frame %= _NbrOfFrames;
        }

    }
}
