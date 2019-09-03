using SketchIt.Api.Interfaces;
using SketchIt.Api.Internal;
using SketchIt.Api.Renderers;
using SketchIt.Api.Static;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using System.Timers;
using Threading = System.Threading;

namespace SketchIt.Api
{
    public partial class Sketch
    {
        public delegate void LoopEventHandler(object sender, EventArgs e);

        public event LoopEventHandler LoopStopped;
        public event LoopEventHandler LoopStarted;
        public event EventHandler Exited;

        private static int _instanceCounter;
        private static string _version;

        public static string ProductVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_version))
                {
                    _version = FileVersionInfo.GetVersionInfo(typeof(Sketch).Assembly.Location).ProductVersion;
                }

                return _version;
            }
        }

        public static Type DefaultRendererType
        {
            get;
            set;
        }

        public Graphics Graphics { get; private set; }
        public Canvas OutputLayer { get; private set; }
        public Canvas CurrentLayer { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public DateTime StartTime { get; private set; }
        public Canvas[] Layers { get => _layers.ToArray(); }
        public int KeyCode { get; private set; }
        public char KeyChar { get; private set; }
        public bool IsKeyPressed { get; private set; }
        public bool UseDefaultConsole { get; set; } = true;
        public bool IsStatic { get; private set; } = false;
        public int InstanceId { get; private set; } = _instanceCounter = _instanceCounter + 1;

        private object _threadListLocker = new object();
        private List<Threading.Thread> _threadList = new List<Threading.Thread>();
        private Stack<object> _consoleStack = new Stack<object>();
        private Timer _consoleTimer;
        private List<Canvas> _layers = null;
        private IApplet _applet;
        private Timer _drawTimer;
        private DateTime _lastDrawnTime;
        private DateTime _startTime;
        private float _frameRate = 60;
        private float _drawInterval;
        private float _zoom = 1;
        private Stopwatch _frameRateStopWatch = new Stopwatch();
        private int _updateInterval = 1;
        private bool _stopped = false;
        private bool _redrawing = false;
        private bool _multiLayer = false;
        private MethodInfo _methodSetup;
        private MethodInfo _methodDraw;
        private MethodInfo _methodKeyUp;
        private MethodInfo _methodKeyDown;
        private MethodInfo _methodKeyPress;
        private MethodInfo _methodMouseClick;
        private MethodInfo _methodMouseMove;
        private MethodInfo _methodMouseDown;
        private MethodInfo _methodMouseUp;

        public Sketch()
            : this(200, 200)
        {
        }

        public Sketch(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool IsDrawing
        {
            get;
            private set;
        }

        public ISketchContainer Container
        {
            get;
            private set;
        }

        public Style Style
        {
            get => CurrentLayer.Style;
        }

        public RendererBase Renderer
        {
            get => CurrentLayer.Renderer;
        }

        public TimeSpan RunningTime
        {
            get => DateTime.Now.Subtract(StartTime);
        }

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        public void Start(ISketchContainer container) => Start(this, container);
        public void Start(IApplet applet, ISketchContainer canvas) => Start(applet, canvas, 0);
        private void Start(IApplet applet, ISketchContainer canvas, int retries)
        {
            using (ThreadLocker locker = ThreadLocker.Lock())
            {
                if (!locker.IsLocked)
                {
                    if (retries < 5)
                    {
                        Start(applet, canvas, retries + 1);
                        return;
                    }

                    throw new Exception("Unable to start the sketch. Lock not available. Please retry the operation.");
                }

                _applet = applet;
                _applet.SetSketch(this);

                Container = canvas;
                Container.Sketch = this;
                CurrentLayer = new Canvas(this, Width, Height);
                OutputLayer = CurrentLayer;
                Initialize();

                if (_methodSetup != null)
                {
                    InvokeSetup();
                    Container.Update();
                }

                if (_methodDraw != null && !_stopped)
                {
                    Loop();
                }
            }

            //Threading.ParameterizedThreadStart start = new Threading.ParameterizedThreadStart(StartSketch);
            //Threading.Thread thread = new Threading.Thread(start);
            //thread.Start(new object[] { applet, canvas, retries });
        }

        //private void StartSketch(object args)
        //{
        //    object[] parameters = (object[])args;
        //    IApplet applet = parameters[0] as IApplet;
        //    ISketchContainer canvas = parameters[1] as ISketchContainer;
        //    int retries = (int)parameters[2];

        //    using (ThreadLocker locker = ThreadLocker.Lock())
        //    {
        //        if (!locker.IsLocked)
        //        {
        //            if (retries < 5)
        //            {
        //                StartSketch(new object[] { applet, canvas, retries + 1 });
        //                return;
        //            }

        //            throw new Exception("Unable to start the sketch. Lock not available. Please retry the operation.");
        //        }

        //        _applet = applet;
        //        _applet.SetSketch(this);

        //        Container = canvas;
        //        Container.Sketch = this;
        //        CurrentLayer = new Canvas(this, Width, Height);
        //        OutputLayer = CurrentLayer;
        //        Initialize();

        //        if (_methodSetup != null)
        //        {
        //            InvokeSetup();
        //        }

        //        if (_methodDraw != null && !_stopped)
        //        {
        //            Loop();
        //        }
        //    }
        //}

        private void Initialize()
        {
            _drawInterval = 1000 / _frameRate;
            _startTime = DateTime.Now;
            _drawTimer = new Timer(5);
            _drawTimer.Elapsed += DrawTimerElapsed;
            _consoleTimer = new Timer(50);
            _consoleTimer.Elapsed += UpdateConsole;
            //_consoleTimer.Start();

            Type type = _applet.GetType();
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase;

            _methodSetup = type.GetMethod("Setup", bindingFlags);
            _methodDraw = type.GetMethod("Draw", bindingFlags);
            _methodKeyUp = type.GetMethod("KeyUp", bindingFlags);
            _methodKeyDown = type.GetMethod("KeyDown", bindingFlags);
            _methodKeyPress = type.GetMethod("KeyPress", bindingFlags);
            _methodMouseClick = type.GetMethod("MouseClick", bindingFlags);
            _methodMouseUp = type.GetMethod("MouseUp", bindingFlags);
            _methodMouseDown = type.GetMethod("MouseDown", bindingFlags);
            _methodMouseMove = type.GetMethod("MouseMove", bindingFlags);

            if (_methodSetup == null)
            {
                _methodSetup = type.GetMethod("RunStatic", bindingFlags);
                IsStatic = _methodSetup != null;
            }
        }

        public void SetRenderer(Type rendererType)
        {
            Container.RendererChanging(rendererType);
            OutputLayer.SetRenderer(rendererType);
            Container.RendererChanged(rendererType);
        }

        public int AddLayer()
        {
            _layers.Add(new Canvas(this, Width, Height));
            return _layers.Count - 1;
        }

        public int SetLayer(int index)
        {
            int previous = _layers.IndexOf(CurrentLayer);
            CurrentLayer = _layers[index];
            return previous;
        }

        public void SetFullScreen() => SetFullScreen(false, -1);
        public void SetFullScreen(bool stretch, int screenIndex)
        {
            Container.FullScreenRequested(stretch, screenIndex);
        }

        public void CenterScreen()
        {
            Container.CenterScreen();
        }

        public void SetMultiLayer()
        {
            _multiLayer = true;

            _layers = new List<Canvas>();
            _layers.Add(new Canvas(this, Width, Height));

            OutputLayer = new Canvas(this, Width, Height);
            SetLayer(0);
        }

        public void SetContainer(ISketchContainer container)
        {
            Container = container;
        }

        public void Paint(Graphics graphics, Size size)
        {
            try
            {
                Graphics = graphics;
                InvokeDraw();
                OutputLayer.Renderer.Flush();

                /*
                Graphics = graphics;

                switch (OutputLayer.RendererType)
                {
                    case RendererType.OpenGL:
                        InvokeDraw();
                        OutputLayer.Renderer.Flush();

                        if (_multiLayer)
                        {
                            for (int i = 1; i < _layers.Count; i++)
                            {
                                graphics.DrawImage(_layers[i].Bitmap, _layers[i].Location.X, _layers[i].Location.Y);
                            }
                        }
                        break;

                    default:
                        InvokeDraw();
                        OutputLayer.Renderer.Flush();

                        if (_multiLayer)
                        {
                            for (int i = 1; i < _layers.Count; i++)
                            {
                                graphics.DrawImage(_layers[i].Bitmap, _layers[i].Location.X, _layers[i].Location.Y);
                            }
                        }
                        break;

                        ////using (ThreadLocker locker = ThreadLocker.Lock())
                        //{
                        //    //if (locker.IsLocked)
                        //    {
                        //        //if (!_paintRequested) return;

                        //        //_paintRequested = false;

                        //        if (_zoom == 1 && size.IsEmpty)
                        //        {
                        //            Graphics.DrawImageUnscaled(OutputLayer.Bitmap, 0, 0);
                        //        }
                        //        else
                        //        {
                        //            Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                        //            Graphics.DrawImage(OutputLayer.Bitmap, size.IsEmpty ? Graphics.ClipBounds : new RectangleF(new PointF(), size));
                        //        }
                        //    }
                        //}
                        break;
                }
                */
            }
            catch (Exception ex)
            {
                PrintLine(ex.Message);
            }
            finally
            {
                Graphics = null;
                CanvasUpdated();
            }
        }

        private void DrawTimerElapsed(object sender, ElapsedEventArgs e)
        {
            HandleDraw();
        }

        private void HandleDraw()
        {
            using (ThreadLocker locker = ThreadLocker.Lock())
            {
                if (!locker.IsLocked) return;
                if (IsDrawing) return;
                if (Container == null) return;
                if (_stopped && !_redrawing) return;

                if (FrameRate <= _frameRate || _redrawing)
                {
                    Container.Update();
                }

                return;

                /*
                //if (DateTime.Now.Subtract(_lastDrawnTime).TotalMilliseconds >= _drawInterval)
                if (FrameRate <= _frameRate || _redrawing)
                {
                    switch (OutputLayer.RendererType)
                    {
                        case RendererType.OpenGL:
                            //for (int i = 0; i < _updateInterval - 1; i++)
                            //{
                            //    if (!redraw && (!IsLooping || _stopped))
                            //    {
                            //        break;
                            //    }

                            //    InvokeDraw(redraw);
                            //}

                            Container.Update();
                            break;

                        default:
                            IsDrawing = true;
                            _lastDrawnTime = DateTime.Now;

                            for (int i = 0; i < _updateInterval - 1; i++)
                            {
                                if (!_redrawing && (!IsLooping || _stopped))
                                {
                                    break;
                                }

                                InvokeDraw();
                            }

                            //if (_multiLayer)
                            //{
                            //    OutputLayer.IRenderer.Clear();
                            //}

                            if (_redrawing || (IsLooping && !_stopped))
                            {
                                InvokeDraw();

                                if (_multiLayer)
                                {
                                    OutputLayer.Renderer.Clear();

                                    foreach (Canvas layer in _layers)
                                    {
                                        OutputLayer.Renderer.DrawImage(new ImageParameters(layer, layer.Location.X, layer.Location.Y));
                                    }
                                }
                            }

                            //_paintRequested = true;
                            Container.Update();

                            break;
                    }

                    //IsDrawing = false;
                }
                */
            }
        }

        public int[] Pixels
        {
            get
            {
                return CurrentLayer.Pixels;
            }
        }

        public void CanvasUpdated()
        {
            if (IsLooping)
            {
                FrameCount++;
            }

            IsDrawing = false;
        }

        private void InvokeDraw()
        {
            if (_stopped && !_redrawing) return;

            if (_methodDraw != null)
            {
                if (_multiLayer)
                {
                    foreach (Canvas layer in _layers)
                    {
                        layer.Renderer.BeginDraw();
                    }
                }
                else
                {
                    CurrentLayer.Renderer.BeginDraw();
                }

                try
                {
                    for (int i = 0; i < _updateInterval; i++)
                    {
                        if (i > 0)
                        {
                            CurrentLayer.Renderer.ResetMatrix();
                        }

                        _methodDraw.Invoke(_applet, null);
                        LoopCount++;
                    }
                }
                catch (Exception ex)
                {
                    this.NoLoop();
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
                finally
                {
                    if (_multiLayer)
                    {
                        foreach (Canvas layer in _layers)
                        {
                            layer.Renderer.EndDraw();
                        }
                    }
                    else
                    {
                        CurrentLayer.Renderer.EndDraw();
                    }

                    _redrawing = false;
                }
            }
        }

        private void InvokeKeyUp(EventArgs e)
        {
            if (_methodKeyUp != null)
            {
                try
                {
                    _methodKeyUp.Invoke(_applet, _methodKeyUp.GetParameters().Length == 0 ? null : new object[] { e });
                }
                catch (Exception ex)
                {
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
            }
        }

        private void InvokeKeyDown(EventArgs e)
        {
            if (_methodKeyDown != null)
            {
                try
                {

                    _methodKeyDown.Invoke(_applet, _methodKeyDown.GetParameters().Length == 0 ? null : new object[] { e });
                }
                catch (Exception ex)
                {
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
            }
        }

        private void InvokeKeyPress(EventArgs e)
        {
            if (_methodKeyPress != null)
            {
                try
                {
                    _methodKeyPress.Invoke(_applet, _methodKeyPress.GetParameters().Length == 0 ? null : new object[] { e });
                }
                catch (Exception ex)
                {
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
            }
        }

        private void InvokeMouseClick(EventArgs e)
        {
            if (_methodMouseClick != null)
            {
                try
                {
                    _methodMouseClick.Invoke(_applet, _methodMouseClick.GetParameters().Length == 0 ? null : new object[] { e });
                }
                catch (Exception ex)
                {
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
            }
        }

        private void InvokeMouseMove(EventArgs e)
        {
            if (_methodMouseMove != null)
            {
                try
                {
                    _methodMouseMove.Invoke(_applet, _methodMouseMove.GetParameters().Length == 0 ? null : new object[] { e });
                }
                catch (Exception ex)
                {
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
            }
        }

        private void InvokeMouseDown(EventArgs e)
        {
            if (_methodMouseDown != null)
            {
                try
                {
                    _methodMouseDown.Invoke(_applet, _methodMouseDown.GetParameters().Length == 0 ? null : new object[] { e });
                }
                catch (Exception ex)
                {
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
            }
        }

        private void InvokeMouseUp(EventArgs e)
        {
            if (_methodMouseUp != null)
            {
                try
                {
                    _methodMouseUp.Invoke(_applet, _methodMouseUp.GetParameters().Length == 0 ? null : new object[] { e });
                }
                catch (Exception ex)
                {
                    PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                }
            }
        }

        private void InvokeSetup()
        {
            using (ThreadLocker.Lock())
            {
                if (_stopped) return;

                if (_methodSetup != null)
                {
                    try
                    {
                        //if (IsStatic)
                        {
                            CurrentLayer.Renderer.BeginDraw();
                        }

                        _methodSetup.Invoke(_applet, null);

                        //if (IsStatic)
                        {
                            CurrentLayer.Renderer.EndDraw();
                        }
                    }
                    catch (Exception ex)
                    {
                        PrintLine(ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace);
                    }
                }

                //this.Loop();
                //Container.Update();
            }
        }

        protected void OnLoop(EventArgs e)
        {
            LoopStarted?.Invoke(this, e);
        }

        public void Loop()
        {
            using (ThreadLocker.Lock())
            {
                if (_methodDraw != null)
                {
                    _drawTimer.Enabled = true;
                    _frameRateStopWatch.Start();

                    OnLoop(new EventArgs());
                }
            }

            _stopped = false;
        }

        protected void OnNoLoop(EventArgs e)
        {
            LoopStopped?.Invoke(this, e);
        }

        public bool NoLoop()
        {
            int retries = 0;

            _stopped = true;

            while (IsLooping && retries < 10)
            {
                using (ThreadLocker locker = ThreadLocker.Lock())
                {
                    if (locker.IsLocked)
                    {
                        if (_drawTimer != null)
                        {
                            _drawTimer.Enabled = false;
                            _frameRateStopWatch.Stop();
                        }

                        OnNoLoop(new EventArgs());

                        return true;
                    }
                }

                retries++;
            }

            _stopped = !IsLooping;

            return _stopped;
        }

        public int LoopCount
        {
            get;
            private set;
        }

        public int FrameCount
        {
            get;
            private set;
        }

        public float FrameRate
        {
            //get { return IsLooping ? FrameCount / ((float)DateTime.Now.Subtract(_loopStart).TotalMilliseconds / 1000) : 0; }
            get { return IsLooping ? FrameCount / ((float)_frameRateStopWatch.ElapsedMilliseconds / 1000) : 0; }
        }

        public void SetFrameRate(float rate)
        {
            _frameRate = rate;
        }

        public void SetUpdateInterval(int interval)
        {
            _updateInterval = interval;
        }

        public void SetSize(float width, float height)
        {
            Width = (int)width;
            Height = (int)height;
            CurrentLayer.SetSize(width, height);
            Container.UpdateSize();
        }

        public void SetAngleMode(AngleMode mode)
        {
            Functions.AngleMode = mode;
        }

        public bool IsLooping
        {
            get { return _drawTimer != null && _drawTimer.Enabled; }
        }

        public ColorMode ColorMode { get => ((IStyle)Style).ColorMode; set => ((IStyle)Style).ColorMode = value; }

        public ColorRange ColorRange => ((IStyle)Style).ColorRange;

        public EllipseMode EllipseMode { get => ((IStyle)Style).EllipseMode; set => ((IStyle)Style).EllipseMode = value; }

        public ImageMode ImageMode { get => ((IStyle)Style).ImageMode; set => ((IStyle)Style).ImageMode = value; }

        public FillParameters FillParameters => ((IStyle)Style).FillParameters;

        public Font Font => ((IStyle)Style).Font;

        public RectangleMode RectangleMode { get => ((IStyle)Style).RectangleMode; set => ((IStyle)Style).RectangleMode = value; }

        public StrokeParameters StrokeParameters => ((IStyle)Style).StrokeParameters;

        Canvas IRenderer.Canvas => ((IRenderer)Renderer).Canvas;

        public Point[][] GetTextPoints(string text)
        {
            List<Point[]> result = new List<Point[]>();
            Point pos = new Point();

            using (ThreadLocker locker = ThreadLocker.Lock())
            using (StringFormat sf = new StringFormat())
            {
                foreach (char c in text)
                {
                    using (GraphicsPath path = new GraphicsPath(FillMode.Alternate))
                    {
                        path.AddString(c.ToString(), Style.Font.FontFamily, (int)Style.Font.Style, Style.Font.Size, pos.ToSystemPointF(), sf);

                        RectangleF bounds = path.GetBounds();
                        List<Point> points = new List<Point>();
                        int start = 0;
                        int end = 0;

                        while (end < path.PointCount)
                        {
                            while ((path.PathData.Types[end] & (int)PathPointType.CloseSubpath) == 0)
                            {
                                end++;
                            }

                            points.AddRange(GetTextPoints(path, start, end));

                            end++;
                            start = end;
                        }

                        if (start != end)
                        {
                            start.ToString();
                        }

                        //points.AddRange(GetTextPoints(path, path.PointCount, 0));

                        result.Add(points.ToArray());
                        pos.X = bounds.Right - (bounds.Width * .2f);// + (_currentFont.SizeInPoints * .1f);
                        //pos.X += _graphics.MeasureString(c.ToString(), _currentFont, new SizeF(), sf).Width / 2f;
                    }
                }
            }

            return result.ToArray();
        }

        private Point[] GetTextPoints(GraphicsPath path, int start, int end)
        {
            List<Point> result = new List<Point>();

            for (int i = start + 1; i <= end; i++)
            {
                result.AddRange(InsertTextPoints(path.PathPoints[i - 1], path.PathPoints[i]));
            }

            result.AddRange(InsertTextPoints(path.PathPoints[end], path.PathPoints[start]));

            return result.ToArray();
        }

        private Point[] InsertTextPoints(PointF from, PointF to)
        {
            List<Point> result = new List<Point>();
            Vector pt1 = new Vector(from.X, from.Y);
            Vector pt2 = new Vector(to.X, to.Y);
            Vector dff = pt2 - pt1;
            float mag = 5;

            result.Add(new Point(pt1.X, pt1.Y));

            while (dff.GetMagnitude() > mag)
            {
                dff.SetMagnitude(mag);
                pt1 += dff;
                dff = pt2 - pt1;

                result.Add(new Point(pt1.X, pt1.Y));
            }

            return result.ToArray();
        }

        public Image GetImage()
        {
            return (Image)OutputLayer.Clone();
        }

        public void OnKeyUp(EventArgs e, int keyCode)
        {
            IsKeyPressed = false;
            KeyCode = keyCode;
            InvokeKeyUp(e);
        }

        public void OnKeyDown(EventArgs e, int keyCode)
        {
            IsKeyPressed = true;
            KeyCode = keyCode;
            InvokeKeyDown(e);
        }

        public void OnKeyPress(EventArgs e, char keyChar)
        {
            IsKeyPressed = true;
            KeyChar = keyChar;
            InvokeKeyPress(e);
        }

        public void OnMouseDown(EventArgs e)
        {
            InvokeMouseDown(e);
        }

        public void OnMouseMove(EventArgs e)
        {
            InvokeMouseMove(e);
        }

        public void OnMouseUp(EventArgs e)
        {
            InvokeMouseUp(e);
        }

        public void OnMouseClick(EventArgs e)
        {
            InvokeMouseClick(e);
        }

        protected void OnExit(EventArgs e)
        {
            Exited?.Invoke(this, e);
        }

        public void Exit()
        {
            foreach (Threading.Thread thread in _threadList)
            {
                if (thread.IsAlive)
                {
                    Console.WriteLine(string.Format("Aborting thread {0}", thread.ManagedThreadId));
                    thread.Abort();
                }
            }

            OnExit(new EventArgs());
        }

        private void UpdateConsole(object sender, ElapsedEventArgs e)
        {
            using (ThreadLocker locker = ThreadLocker.Lock(_consoleStack, 10))
            {
                if (locker.IsLocked)
                {
                    int count = 0;

                    while (_consoleStack.Count > 0)
                    {
                        Console.Write(_consoleStack.Pop());

                        count++;

                        if (count == 100)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public void Print(params object[] values)
        {
            using (ThreadLocker.Lock(this))
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                foreach (object value in values)
                {
                    if (Console.Out.NewLine.Equals(value))
                    {
                        Console.Write(builder.ToString());
                        Console.WriteLine();
                        builder.Clear();
                        count = 0;
                    }
                    else
                    {
                        if (count > 0) builder.Append(" ");
                        builder.Append(value);
                        count++;
                    }
                }

                if (builder.Length > 0)
                {
                    Console.Write(builder.ToString());
                }
            }
        }

        public void PrintLine(params object[] values)
        {
            using (ThreadLocker.Lock(this))
            //new System.Windows.Forms.MethodInvoker(
            //    delegate ()
            {
                List<object> valueList = new List<object>(values);
                valueList.Add(Console.Out.NewLine);
                Print(valueList.ToArray());
            }
            //).Invoke();
        }

        public void Redraw()
        {
            _redrawing = true;
            HandleDraw();
        }

        public void RunThread(string methodName, params object[] args)
        {
            ClearDeadThreads();

            lock (_threadListLocker)
            {
                MethodInfo mi = _applet.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                Threading.ThreadStart threadStart = new Threading.ThreadStart(delegate () { mi.Invoke(_applet, args); });
                Threading.Thread thread = new Threading.Thread(threadStart);

                _threadList.Add(thread);

                thread.Start();
            }
        }

        private void ClearDeadThreads()
        {
            lock (_threadListLocker)
            {
                List<Threading.Thread> deadThreads = new List<Threading.Thread>();

                foreach (Threading.Thread thread in _threadList)
                {
                    if (!thread.IsAlive)
                    {
                        deadThreads.Add(thread);
                    }
                }

                foreach (Threading.Thread deadThread in deadThreads)
                {
                    _threadList.Remove(deadThread);
                }
            }
        }
    }
}
