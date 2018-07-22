using SketchIt.Api;
using SketchIt.Api.Interfaces;
using SketchIt.Api.Static;
using System;

namespace SketchIt.Windows
{
    /// <summary>
    /// <see cref="Application"/> is a static implementation of the <see cref="SketchIt.Api.Sketch"/> class, used from the SketchIt IDE and related compiled output. 
    /// Members of <see cref="Application"/> are, in most of the cases, mapped to matching members of the <see cref="SketchIt.Api.Sketch"/> class. <see cref="Application"/> is not 
    /// intented for use outside of the SketchIt IDE.
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// For internal use. Sets an instance of a <see cref="SketchIt.Api.Sketch"/> to <see cref="Application"/>.
        /// </summary>
        /// <param name="sketch"></param>
        public static void Set(Sketch sketch)
        {
            Application.Sketch = sketch;
            Functions.ResetNoise();
        }

        /// <summary>
        /// Creates a new canvas.
        /// </summary>
        /// <param name="width">The width, in pixels, of the new canvas.</param>
        /// <param name="height">The height, in pixels, of the new canvas.</param>
        /// <returns>Returns a new Canvas instance with the specified width and height.</returns>
        public static Canvas CreateCanvas(int width, int height) => new Canvas(Sketch, width, height);

        /// <summary>
        /// For internal use. Shorthand for <see cref="Sketch.Style"/>.
        /// </summary>
        public static Style Style { get => Sketch.Style; }

        /// <summary>
        /// The current layer of the sketch. When multi-layer mode is not used, the <see cref="CurrentLayer"/> and <see cref="OutputLayer"/> properties
        /// references the same object instance.
        /// </summary>
        public static Canvas CurrentLayer { get => Sketch.CurrentLayer; }

        /// <summary>
        /// For internal use. The instance to the <see cref="Api.Sketch"/> object.
        /// </summary>
        public static Sketch Sketch { get; private set; }

        /// <summary>
        /// Sets the renderer to use. The default renderer is GDIPLUS.
        /// </summary>
        /// <param name="renderer">GDIPLUS or OPENGL</param>
        public static void SetRenderer(int renderer) => Sketch.SetRenderer((RendererType)renderer);

        /// <summary>
        /// Initializes the sketch in multi-layer mode.
        /// </summary>
        public static void SetMultiLayer() => Sketch.SetMultiLayer();

        /// <summary>
        /// Adds an empty layer to the sketch.
        /// </summary>
        /// <returns>Returns the index of the newly added layer.</returns>
        public static int AddLayer() => Sketch.AddLayer();

        /// <summary>
        /// Sets the specified layer to be the active sketch layer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returns the previously active layer.</returns>
        public static int SetLayer(int index) => Sketch.SetLayer(index);

        /// <summary>
        /// Returns an array of all layers for the sketch.
        /// </summary>
        public static Canvas[] Layers => Sketch.Layers;

        /// <summary>
        /// Returns the output layer of the sketch. The output layer is the result of all layers drawn onto a single (flattened) layer.
        /// </summary>
        public static Canvas OutputLayer => Sketch.OutputLayer;

        /// <summary>
        /// Returns the width of the sketch, in pixels. In multi-layer mode, the current layer's width is returned.
        /// </summary>
        public static int Width { get { return CurrentLayer.Width; } }

        /// <summary>
        /// Returns the height of the sketch, in pixels. In multi-layer mode, the current layer's height is returned.
        /// </summary>
        public static int Height { get { return CurrentLayer.Height; } }

        /// <summary>
        /// Sets the size of the sketch. In multi-layer mode, the active layer's size is set.
        /// </summary>
        /// <param name="width">The width of the canvas, in pixels.</param>
        /// <param name="height">The height of the canvas, in pixels.</param>
        public static void SetSize(int width, int height) { CurrentLayer.SetSize(width, height); }

        /// <summary>
        /// Sets the location of the output canvas window.
        /// </summary>
        /// <param name="x">The x-coordinate of the window.</param>
        /// <param name="y">The y-coordinate of the window.</param>
        public static void SetLocation(float x, float y) { CurrentLayer.SetLocation(x, y); }

        /// <summary>
        /// Runs the sketch in fullscreen mode.
        /// </summary>
        public static void SetFullScreen() { Sketch.SetFullScreen(false, -1); }

        /// <summary>
        /// Rusn the sketch in fullscreen mode, but stretches the current size of the sketch to fullscreen.
        /// </summary>
        /// <param name="stretch">When true, the sketch will be streched fullscreen based on the current sketch size.</param>
        public static void SetFullScreen(bool stretch) { Sketch.SetFullScreen(stretch, -1); }

        /// <summary>
        /// Runs the sketch in fullscreen mode on the specified screen.
        /// </summary>
        /// <param name="screenIndex"></param>
        public static void SetFullScreen(int screenIndex) { Sketch.SetFullScreen(false, screenIndex); }

        /// <summary>
        /// Centers a non-fullscreen sketch on the screen.
        /// </summary>
        public static void CenterScreen() { Sketch.CenterScreen(); }

        /// <summary>
        /// Stops and exit the sketch.
        /// </summary>
        public static void Exit() { Sketch.Exit(); }

        /// <summary>
        /// Set the desired frame rate (frames per second) for the animation loop. The default is 60fps.
        /// </summary>
        /// <param name="rate">The desired frames per second.</param>
        public static void SetFrameRate(float rate) { Sketch.SetFrameRate(rate); }

        /// <summary>
        /// Stops the animation loop.
        /// </summary>
        public static void NoLoop() { Sketch.NoLoop(); }

        /// <summary>
        /// Starts the animation loop after a call to <see cref="NoLoop"/>.
        /// </summary>
        public static void Loop() { Sketch.Loop(); }

        /// <summary>
        /// Returns the current, actual frame rate (frames per second).
        /// </summary>
        public static float FrameRate { get { return Sketch.FrameRate; } }

        /// <summary>
        /// Returns the number of frames drawn since the start of the sketch.
        /// </summary>
        public static int FrameCount { get { return Sketch.FrameCount; } }

        /// <summary>
        /// Indicates if the sketch is currently looping.
        /// </summary>
        public static bool IsLooping { get { return Sketch.IsLooping; } }

        /// <summary>
        /// Returns the number of animation loops executed since the start of the sketch. If a different update interval (<see cref="SetUpdateInterval(int)"/>)
        /// is not specified, the <see cref="LoopCount"/> and <see cref="FrameCount"/> values should be the same.
        /// </summary>
        public static int LoopCount { get { return Sketch.LoopCount; } }

        public static int KeyCode { get { return Sketch.KeyCode; } }
        public static char KeyChar { get { return Sketch.KeyChar; } }

        /// <summary>
        /// Returns true of a key is pressed.
        /// </summary>
        public static bool IsKeyPressed { get { return Sketch.IsKeyPressed; } }

        /// <summary>
        /// Draws an arc to the canvas.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle that defines the arc's ellipse.</param>
        /// <param name="y">The y-coordinate of the rectangle that defines the arc's ellipse.</param>
        /// <param name="width">The width of the rectangle that defines the arc's ellipse.</param>
        /// <param name="height">The height of the rectangle that defines the arc's ellipse.</param>
        /// <param name="start">The angle (in degrees) measured clockwise from the x-axis to the starting point of the arc.</param>
        /// <param name="stop">The angle (in degrees) measured clockwise from the <paramref name="start"/> parameter to the ending point of the arc.</param>
        public static void DrawArc(float x, float y, float width, float height, float start, float stop) => DrawArc(x, y, width, height, start, stop, (int)ArcMode.Open);

        /// <summary>
        /// Draws an arc to the canvas.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle that defines the arc's ellipse.</param>
        /// <param name="y">The y-coordinate of the rectangle that defines the arc's ellipse.</param>
        /// <param name="width">The width of the rectangle that defines the arc's ellipse.</param>
        /// <param name="height">The height of the rectangle that defines the arc's ellipse.</param>
        /// <param name="start">The angle (in degrees) measured clockwise from the x-axis to the starting point of the arc.</param>
        /// <param name="stop">The angle (in degrees) measured clockwise from the <paramref name="start"/> parameter to the ending point of the arc.</param>
        /// <param name="mode">The shape mode.</param>
        public static void DrawArc(float x, float y, float width, float height, float start, float stop, int mode) { CurrentLayer.Renderer.DrawArc(new ArcParameters(x, y, width, height, start, stop) { Mode = (ArcMode)mode }); }

        public static void DrawBezier(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) { CurrentLayer.Renderer.DrawBezier(new BezierParameters(x1, y1, x2, y2, x3, y3, x4, y4)); }

        public static void DrawCurve(params float[] points) { CurrentLayer.Renderer.DrawCurve(new CurveParameters(points)); }
        public static void DrawCurve(params Point[] points) { CurrentLayer.Renderer.DrawCurve(new CurveParameters(points)); }

        /// <summary>
        /// Draws a circle to the canvas.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle that defines the circle.</param>
        /// <param name="y">The y-coordinate of the rectangle that defines the circle.</param>
        /// <param name="diameter">The diameter of the circle.</param>
        public static void DrawEllipse(float x, float y, float diameter) { CurrentLayer.Renderer.DrawEllipse(new EllipseParameters(x, y, diameter)); }

        /// <summary>
        /// Draws an ellipse to the canvas.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle that defines the ellipse.</param>
        /// <param name="y">The y-coordinate of the rectangle that defines the ellipse.</param>
        /// <param name="width">The width of the rectangle that defines the ellipse.</param>
        /// <param name="height">The height of the rectangle that defines the ellipse.</param>
        public static void DrawEllipse(float x, float y, float width, float height) { CurrentLayer.Renderer.DrawEllipse(new EllipseParameters(x, y, width, height)); }

        public static void DrawQuad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4) { CurrentLayer.Renderer.DrawQuad(new QuadParameters(x1, y1, x2, y2, x3, y3, x4, y4)); }

        /// <summary>
        /// Draws a square to the canvas.
        /// </summary>
        /// <param name="x">The x-coordinate of the square.</param>
        /// <param name="y">The y-coordinate of the square.</param>
        /// <param name="size">The width and height of the square.</param>
        public static void DrawRectangle(float x, float y, float size) { CurrentLayer.Renderer.DrawRectangle(new RectangleParameters(x, y, size, size)); }

        /// <summary>
        /// Draws a rectangle to the canvas.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle.</param>
        /// <param name="y">The y-coordinate of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public static void DrawRectangle(float x, float y, float width, float height) { CurrentLayer.Renderer.DrawRectangle(new RectangleParameters(x, y, width, height)); }

        /// <summary>
        /// Draws a point using the current stroke weight to the canvas.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public static void DrawPoint(float x, float y) { CurrentLayer.Renderer.DrawPoint(new PointParameters(x, y)); }

        /// <summary>
        /// Draws a shape to the canvas.
        /// </summary>
        /// <param name="shape">The shape to be drawn. See <see cref="Shape"/> for more information.</param>
        public static void DrawShape(Shape shape) { CurrentLayer.Renderer.DrawShape(new ShapeParameters(shape)); }

        /// <summary>
        /// Draws a line to the canvas.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static void DrawLine(float x1, float y1, float x2, float y2) { CurrentLayer.Renderer.DrawLine(new LineParameters(x1, y1, x2, y2)); }

        /// <summary>
        /// Draws an image to the canvas.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DrawImage(IImage image, float x, float y) { CurrentLayer.Renderer.DrawImage(new ImageParameters(image, x, y)); }

        /// <summary>
        /// Draws an image to the canvas.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawImage(IImage image, float x, float y, float width, float height) { CurrentLayer.Renderer.DrawImage(new ImageParameters(image, x, y, width, height)); }

        /// <summary>
        /// Draws the specified text to the canvas using the current font (see <see cref="SetFont(string, float)"/>).
        /// </summary>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="x">The x-coordinate of where the text will be drawn.</param>
        /// <param name="y">The y-coordinate of where the text will be drawn.</param>
        public static void DrawText(object text, float x, float y) { CurrentLayer.Renderer.DrawText(new TextParameters(text, x, y)); }

        /// <summary>
        /// Draws the specified text to the canvas inside the specified bounds using the current font (see <see cref="SetFont(string, float)"/>).
        /// </summary>
        /// <param name="text">The text to be draw.</param>
        /// <param name="x">The x-coordinate of the bounding rectangle.</param>
        /// <param name="y">The y-coorindate of the bounding rectangle.</param>
        /// <param name="width">The width of the bounding rectangle.</param>
        /// <param name="height">The height of the bounding rectangle.</param>
        public static void DrawText(object text, float x, float y, float width, float height) { CurrentLayer.Renderer.DrawText(new TextParameters(text, x, y, width, height)); }

        //public static Point[][] GetTextPoints(string text) { return Sketch.GetTextPoints(text); }

        public static void SetFont(string name, float size) => Style.SetFont(name, size);
        public static void SetFont(string name, float size, bool bold, bool italic) { Style.SetFont(new FontParameters(name, size, bold, italic)); }

        public static void Clear() { CurrentLayer.Renderer.Clear(); }

        public static void DrawBackground(IImage image) { CurrentLayer.Renderer.DrawBackground(new BackgroundParameters(image)); }

        /// <summary>
        /// Draws the background with the specified grayscale color.
        /// </summary>
        /// <param name="gray">The grayscale value (0 to 255 inclusive).</param>
        public static void DrawBackground(float gray) { CurrentLayer.Renderer.DrawBackground(new BackgroundParameters(Style.GetColor(gray))); }

        /// <summary>
        /// Draws the background with the specified grayscale color and alpha value.
        /// </summary>
        /// <param name="gray">The grayscale value (0 to 255 inclusive).</param>
        /// <param name="alpha">The alpha value (0 to 255 inclusive).</param>
        public static void DrawBackground(float gray, float alpha) { CurrentLayer.Renderer.DrawBackground(new BackgroundParameters(Style.GetColor(gray, alpha))); }

        /// <summary>
        /// Draws the background with the specified color values (either RGB or HSB).
        /// </summary>
        /// <param name="v1">Value 1. Red for RGB, Hue for HSB.</param>
        /// <param name="v2">Value 2. Green for RGB, Saturation for HSB.</param>
        /// <param name="v3">Value 3. Blue for RGB, Brightness for HSB.</param>
        public static void DrawBackground(float v1, float v2, float v3) { CurrentLayer.Renderer.DrawBackground(new BackgroundParameters(Style.GetColor(v1, v2, v3))); }

        /// <summary>
        /// Draws the background with the specified color values (either RGB or HSB).
        /// </summary>
        /// <param name="v1">Value 1. Red for RGB, Hue for HSB.</param>
        /// <param name="v2">Value 2. Green for RGB, Saturation for HSB.</param>
        /// <param name="v3">Value 3. Blue for RGB, Brightness for HSB.</param>
        /// <param name="alpha">Alpha value.</param>
        public static void DrawBackground(float v1, float v2, float v3, float alpha) { CurrentLayer.Renderer.DrawBackground(new BackgroundParameters(Style.GetColor(v1, v2, v3, alpha))); }
        public static void DrawBackground(Color color, float alpha) { CurrentLayer.Renderer.DrawBackground(new BackgroundParameters(new Color(color, alpha))); }
        public static void DrawBackground(Color color) { CurrentLayer.Renderer.DrawBackground(new BackgroundParameters(color)); }

        public static void SetFill(float gray) { Style.SetFill(new FillParameters(Style.GetColor(gray))); }
        public static void SetFill(float gray, float alpha) { Style.SetFill(new FillParameters(Style.GetColor(gray, alpha))); }
        public static void SetFill(float r, float g, float b) { Style.SetFill(new FillParameters(Style.GetColor(r, g, b))); }
        public static void SetFill(float r, float g, float b, float alpha) { Style.SetFill(new FillParameters(Style.GetColor(r, g, b, alpha))); }
        public static void SetFill(Color color, float alpha) { Style.SetFill(new FillParameters(new Color(color, alpha))); }
        public static void SetFill(Color color) { Style.SetFill(new FillParameters(color)); }
        public static void SetFill(IImage image) { Style.SetFill(new FillParameters(image)); }
        public static void SetNoFill() { Style.SetNoFill(); }

        public static void SetStrokeWeight(float weight) { Style.SetStrokeWeight(weight); }
        public static void SetStroke(float gray) { Style.SetStroke(new StrokeParameters(Style.GetColor(gray))); }
        public static void SetStroke(float gray, float alpha) { Style.SetStroke(new StrokeParameters(Style.GetColor(gray, alpha))); }
        public static void SetStroke(float r, float g, float b) { Style.SetStroke(new StrokeParameters(Style.GetColor(r, g, b))); }
        public static void SetStroke(float r, float g, float b, float alpha) { Style.SetStroke(new StrokeParameters(Style.GetColor(r, g, b, alpha))); }
        public static void SetStroke(Color color, float alpha) { Style.SetStroke(new StrokeParameters(new Color(color, alpha))); }
        public static void SetStroke(Color color) { Style.SetStroke(new StrokeParameters(color)); }
        public static void SetNoStroke() { Style.SetNoStroke(); }

        public static void SetTint(float gray) { Style.SetTint(new TintParameters(Style.GetColor(gray))); }
        public static void SetTint(float gray, float alpha) { Style.SetTint(new TintParameters(Style.GetColor(gray, alpha))); }
        public static void SetTint(float r, float g, float b) { Style.SetTint(new TintParameters(Style.GetColor(r, g, b))); }
        public static void SetTint(float r, float g, float b, float alpha) { Style.SetTint(new TintParameters(Style.GetColor(r, g, b, alpha))); }
        public static void SetTint(Color color, float alpha) { Style.SetTint(new TintParameters(new Color(color, alpha))); }
        public static void SetTint(Color color) { Style.SetTint(new TintParameters(color)); }
        public static void SetNoTint() { Style.SetNoTint(); }

        public static void LoadPixels() { Sketch.CurrentLayer.LoadPixels(); }
        public static Color GetPixelColor(int x, int y) { return CurrentLayer.GetPixelColor(x, y); }
        public static int GetPixel(int x, int y) { return CurrentLayer.GetPixel(x, y); }
        public static void SetPixelColor(int x, int y, Color color) { CurrentLayer.SetPixelColor(x, y, color); }
        public static void SetPixel(int x, int y, int argb) { CurrentLayer.SetPixel(x, y, argb); }
        public static int[] Pixels { get { return CurrentLayer.Pixels; } }
        public static void UpdatePixels() { Sketch.CurrentLayer.UpdatePixels(); }
        public static void UpdatePixels(int[] pixels) { Sketch.CurrentLayer.UpdatePixels(pixels); }

        /// <summary>
        /// Sets the way an ellipse is drawn to the canvas. The default mode is CENTER. See <see cref="SketchIt.Api.Static.EllipseMode"/> for more information.
        /// </summary>
        /// <param name="mode">CENTER, CORNER, RADIUS or CORNERS.</param>
        public static void SetEllipseMode(int mode) { Style.SetEllipseMode((EllipseMode)mode); }
        public static void SetRectangleMode(int mode) { Style.SetRectangleMode((RectangleMode)mode); }
        public static void SetImageMode(int mode) { Style.SetImageMode((ImageMode)mode); }
        public static void SetTextAlignment(int horizontal, int vertical) { Style.SetTextAlignment((HorizontalAlignment)horizontal, (VerticalAlignment)vertical); }

        public static void SetColorMode(int mode) { Style.SetColorMode(new ColorModeParameters((ColorMode)mode)); }
        public static void SetColorMode(int mode, float max) { Style.SetColorMode(new ColorModeParameters((ColorMode)mode, max)); }
        public static void SetColorMode(int mode, float max1, float max2, float max3) { Style.SetColorMode(new ColorModeParameters((ColorMode)mode, max1, max2, max3)); }
        public static void SetColorMode(int mode, float max1, float max2, float max3, float maxA) { Style.SetColorMode(new ColorModeParameters((ColorMode)mode, max1, max2, max3, maxA)); }

        public static Color GetColor(float gray) { return Style.GetColor(gray); }
        public static Color GetColor(float gray, float alpha) { return Style.GetColor(gray, alpha); }
        public static Color GetColor(float r, float g, float b) { return Style.GetColor(r, g, b); }
        public static Color GetColor(float r, float g, float b, float alpha) { return Style.GetColor(r, g, b, alpha); }

        /// <summary>
        /// Returns the current x-coordinate of the mouse location, relative to the canvas.
        /// </summary>
        public static int MouseX { get { return Sketch.Container.MouseX; } }

        /// <summary>
        /// Returns the current y-coordinate of the mouse location, relative to the canvas.
        /// </summary>
        public static int MouseY { get { return Sketch.Container.MouseY; } }

        /// <summary>
        /// Returns true if a mouse buttons is pressed.
        /// </summary>
        public static bool IsMousePressed { get { return Sketch.Container.IsMousePressed; } }

        /// <summary>
        /// Returns the version number of the SketchIt API.
        /// </summary>
        public static string ProductVersion { get { return Sketch.ProductVersion; } }

        public static void PushMatrix() { CurrentLayer.Renderer.PushMatrix(); }
        public static void PopMatrix() { CurrentLayer.Renderer.PopMatrix(); }
        public static void ResetMatrix() { CurrentLayer.Renderer.ResetMatrix(); }
        public static void Scale(float x, float y) { CurrentLayer.Renderer.Scale(x, y); }
        public static void Translate(float x, float y) { CurrentLayer.Renderer.Translate(x, y); }
        public static void Rotate(float angle) { CurrentLayer.Renderer.Rotate(angle); }
        public static void SetUpdateInterval(int interval) { Sketch.SetUpdateInterval(interval); }

        /// <summary>
        /// Returns the image of the output canvas.
        /// </summary>
        /// <returns>An <see cref="Image"/> object</returns>
        public static Image GetImage() { return CurrentLayer.GetImage(); }

        /// <summary>
        /// Loads an image from the specified location, and resizes it to the specified width and height.
        /// </summary>
        /// <param name="source">The location of the image. A local file or URL can be speicified.</param>
        /// <param name="width">The new width of the image.</param>
        /// <param name="height">The new height of the image.</param>
        /// <returns></returns>
        public static Image LoadImage(string source, int width, int height) { return Image.FromSource(source, width, height); }

        /// Loads an image from the specified location;
        /// </summary>
        /// <param name="source">The location of the image. A local file or URL can be speicified.</param>
        public static Image LoadImage(string source) { return Image.FromSource(source); }

        public static void BeginShape() { CurrentLayer.Renderer.BeginShape(ShapeKind.Polygon); }
        public static void BeginShape(int kind) { CurrentLayer.Renderer.BeginShape((ShapeKind)kind); }
        public static void Vertex(float x, float y) { CurrentLayer.Renderer.Vertex(x, y, 0, 0, 0); }
        public static void Vertex(float x, float y, float u, float v) { CurrentLayer.Renderer.Vertex(x, y, 0, u, v); }
        public static void Texture(IImage image) { CurrentLayer.Renderer.Texture(image); }
        public static void EndShape() { CurrentLayer.Renderer.EndShape(EndShapeMode.Open); }
        public static void EndShape(int mode) { CurrentLayer.Renderer.EndShape((EndShapeMode)mode); }

        /// <summary>
        /// Prints the specified value(s) to the console window.
        /// </summary>
        /// <param name="values">An array of values to print.</param>
        public static void Print(params object[] values) => Sketch.Print(values);

        /// <summary>
        /// Prints the specified value(s) to the console window, moving to the next line after printing all the values.
        /// </summary>
        /// <param name="values">An array of values to print.</param>
        public static void PrintLine(params object[] values) => Sketch.PrintLine(values);

        public static void RunThread(string methodName, params object[] args)
        {
            Sketch.RunThread(methodName, args);
        }
    }
}
