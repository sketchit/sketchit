using System;

namespace SketchIt.Api.Interfaces
{
    /// <summary>
    /// The ISketchContainer interface would normally be implemented by a class where painting of the canvas
    /// would occur.
    /// </summary>
    public interface ISketchContainer
    {
        /// <summary>
        /// A reference to the <see cref="SketchIt.Api.Sketch"/>.
        /// </summary>
        Sketch Sketch { get; set; }

        /// <summary>
        /// Called by the sketch when the window needs to be painted.
        /// </summary>
        void Update();

        /// <summary>
        /// Should update the canvas to the sketch's size.
        /// </summary>
        void UpdateSize();

        /// <summary>
        /// Should display the canvas in fullscreen mode.
        /// </summary>
        void FullScreenRequested(bool stretch, int screenIndex);

        /// <summary>
        /// Should return the x-coordinate of the mouse cursor's current location relative
        /// to the canvas.
        /// </summary>
        int MouseX { get; }

        /// <summary>
        /// Should return the y-coordinate of the mouse cursor's current location relative
        /// to the canvas.
        /// </summary>
        int MouseY { get; }

        /// <summary>
        /// Should return the x-coordinate of the mouse cursor's previous location relative
        /// to the canvas.
        /// </summary>
        int PreviousMouseX { get; }

        /// <summary>
        /// Should return the y-coordinate of the mouse cursor's previous location relative
        /// to the canvas.
        /// </summary>
        int PreviousMouseY { get; }

        /// <summary>
        /// Should return the pressed Mouse Buttons.
        /// </summary>
        int MouseButton { get; }

        /// <summary>
        /// Should return true if a mouse button is currently pressed.
        /// </summary>
        bool IsMousePressed { get; }

        /// <summary>
        /// Should center the sketch in the center of the screen.
        /// </summary>
        void CenterScreen();

        /// <summary>
        /// Called by the sketch when the renderer is changing.
        /// </summary>
        void RendererChanging(Type rendererType);

        /// <summary>
        /// Called by the sketch after the renderer is changed.
        /// </summary>
        void RendererChanged(Type rendererType);

        IntPtr WindowHandle { get; }

        float Scale { get; }
    }
}
