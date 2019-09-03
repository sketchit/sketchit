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

    public partial class Capture : IDisposable
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
                    //CvInvoke.Rectangle(_mat, new System.Drawing.Rectangle(10, 10, 100, 100), new Emgu.CV.Structure.MCvScalar(255, 0, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias);
                    //CvInvoke.PutText(_mat, "this is a test", new System.Drawing.Point(100, 100), Emgu.CV.CvEnum.FontFace.HersheyComplex, 2, new Emgu.CV.Structure.MCvScalar(0, 255, 0), 1, Emgu.CV.CvEnum.LineType.AntiAlias, false);
                    //CvInvoke.Ellipse(_mat, new Emgu.CV.Structure.RotatedRect(new System.Drawing.PointF(200, 200), new System.Drawing.SizeF(100, 50), 45), new Emgu.CV.Structure.MCvScalar(0, 255, 255, .5), -1, Emgu.CV.CvEnum.LineType.Filled);
                    //CvInvoke.Ellipse(_mat, new Emgu.CV.Structure.RotatedRect(new System.Drawing.PointF(200, 200), new System.Drawing.SizeF(100, 50), 45), new Emgu.CV.Structure.MCvScalar(0, 0, 255), 2, Emgu.CV.CvEnum.LineType.AntiAlias);
                    //CvInvoke.Polylines(_mat, new System.Drawing.Point[] { new System.Drawing.Point(0, 0), new System.Drawing.Point(100, 40), new System.Drawing.Point(40, 200) }, true, new Emgu.CV.Structure.MCvScalar(255));
                    
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
