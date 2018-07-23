using Emgu.CV;
using SketchIt.Api;
using System;

namespace SketchIt.Video
{
    public static class ExtensionMethods
    {
        public static Capture CreateCapture(this Applet applet)
        {
            return new Capture(applet);
        }
    }

    public partial class Capture
    {
        private Applet _applet;
        private Sketch _sketch;
        private VideoCapture _videoCapture;
        private Mat _mat;
        private long _lastFrameRead;
        private long _lastFrameGrabbed;
        private System.Drawing.Bitmap _nextFrame;
        private bool _started = false;

        public Capture(Applet applet)
        {
            _applet = applet;
            _sketch = applet.GetSketch();
            _sketch.Exited += _sketch_Exited;
        }

        private void _sketch_Exited(object sender, EventArgs e)
        {
            if (_videoCapture != null)
            {
                _videoCapture.ImageGrabbed -= _videoCapture_ImageGrabbed;
                _videoCapture.Stop();
                _videoCapture.Dispose();
            }
        }

        public bool Start()
        {
            _started = true;
            _mat = new Mat();
            _videoCapture = new VideoCapture();
            _videoCapture.ImageGrabbed += _videoCapture_ImageGrabbed;
            _videoCapture.Read(_mat);
            _nextFrame = new System.Drawing.Bitmap(_mat.Bitmap);

            Read();
            Width = Bitmap.Width;
            Height = Bitmap.Height;

            _videoCapture.Start();

            return true;
        }

        public void Stop()
        {
            if (_started)
            {
                _videoCapture.Stop();
                _started = false;
            }
        }

        private void _videoCapture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_videoCapture != null)
            {
                lock (_videoCapture)
                {
                    _videoCapture.Read(_mat);

                    if (!_mat.IsEmpty)
                    {
                        _nextFrame = new System.Drawing.Bitmap(_mat.Bitmap);
                        _lastFrameGrabbed = DateTime.Now.Ticks;
                    }
                }
            }
        }

        public bool Available()
        {
            return _started && !_lastFrameGrabbed.Equals(_lastFrameRead);
        }

        public void Read()
        {
            Bitmap = _nextFrame;
            _lastFrameRead = _lastFrameGrabbed;
        }

        public void Dispose()
        {
            if (_videoCapture != null)
            {
                _videoCapture.Dispose();
            }
        }
    }
}
