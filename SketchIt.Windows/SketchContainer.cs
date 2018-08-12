using SketchIt.Api;
using SketchIt.Api.Interfaces;
using System;
using System.Reflection;
using System.Windows.Forms;
using Drawing = System.Drawing;

namespace SketchIt.Windows
{
    public class SketchContainer : Control, ISketchContainer
    {
        private delegate object GenericInvocationHandler(string method, params object[] args);
        private Drawing.Size _screenSize;
        private bool _paintRequested = false;

        public SketchContainer()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.CacheText, true);
            SetStyle(ControlStyles.UserMouse, true);

            BackColor = Drawing.Color.FromArgb(100, 100, 100);
            Size = new Drawing.Size(200, 200);
        }

        public void Zoom(float factor)
        {
            Sketch.Zoom = factor;
            Scale(new Drawing.SizeF(factor, factor));
        }

        public Sketch Sketch
        {
            get;
            set;
        }

        public new bool DesignMode
        {
            get;
            set;
        }

        public Drawing.Icon Icon
        {
            get { return Properties.Resources.SketchIt; }
        }

        public IntPtr GetHandle()
        {
            return (IntPtr)InvokeMethod("Handle");
        }

        public int MouseX
        {
            get
            {
                if (InvokeRequired)
                {
                    return (int)(((System.Drawing.Point)InvokeMethod("PointToClient", MousePosition)).X / Sketch.Zoom);
                }
                else
                {
                    return (int)(PointToClient(MousePosition).X / Sketch.Zoom);
                }
            }
        }

        public int MouseY
        {
            get
            {
                if (InvokeRequired)
                {
                    return (int)(((System.Drawing.Point)InvokeMethod("PointToClient", MousePosition)).Y / Sketch.Zoom);
                }
                else
                {
                    return (int)(PointToClient(MousePosition).Y / Sketch.Zoom);
                }
            }
        }

        public int MouseButton
        {
            get
            {
                int button = 0;

                if ((MouseButtons & MouseButtons.Left) != 0) button |= SketchIt.Api.Static.Constants.LEFT;
                if ((MouseButtons & MouseButtons.Right) != 0) button |= SketchIt.Api.Static.Constants.RIGHT;
                if ((MouseButtons & MouseButtons.Middle) != 0) button |= SketchIt.Api.Static.Constants.MIDDLE;

                return button;
            }
        }

        public bool IsMousePressed
        {
            get { return MouseButtons != MouseButtons.None; }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            Invalidate();
            base.OnSizeChanged(e);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            Sketch.OnKeyPress(e, e.KeyChar);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Sketch.OnKeyDown(e, (int)e.KeyData);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            Sketch.OnKeyUp(e, (int)e.KeyData);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Sketch.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            Sketch.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Sketch.OnMouseMove(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            Sketch.OnMouseClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Sketch == null) return;
            if (!_paintRequested && Sketch.IsDrawing) return;

            Sketch.Paint(e.Graphics, this.Size.Equals(_screenSize) ? _screenSize : Drawing.Size.Empty);
            _paintRequested = false;
        }


        private object GenericMethodHandler(string name, params object[] args)
        {
            BindingFlags flags =
                BindingFlags.FlattenHierarchy |
                BindingFlags.Instance |
                BindingFlags.InvokeMethod |
                BindingFlags.NonPublic |
                BindingFlags.Public |
                BindingFlags.GetProperty |
                BindingFlags.Static;
            return GetType().InvokeMember(name, flags, null, this, args);
        }

        private object InvokeMethod(string method, params object[] args)
        {
            try
            {
                if (InvokeRequired)
                {
                    return Invoke(new GenericInvocationHandler(GenericMethodHandler), new object[] { method, args });
                }
                else
                {
                    return new GenericInvocationHandler(GenericMethodHandler).DynamicInvoke(new object[] { method, args });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSize()
        {
            Size = new Drawing.Size(Sketch.Width, Sketch.Height);
        }

        void ISketchContainer.Update()
        {
            _paintRequested = true;
            Invalidate();
        }

        public void CenterScreen()
        {
            Form form = FindForm();

            if (form != null)
            {
                Screen screen = Screen.FromHandle(form.Handle);
                form.Location = new Drawing.Point((screen.Bounds.Width - form.Width) / 2, (screen.Bounds.Height - form.Height) / 2);
            }
        }

        public void FullScreenRequested(bool stretch, int screenIndex)
        {
            Screen screen = screenIndex <= 0 ? Screen.FromControl(this) : Screen.AllScreens[screenIndex - 1];

            _screenSize = screen.Bounds.Size;

            if (!DesignMode)
            {
                if (FindForm() is Form form)
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = screen.WorkingArea.Location;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.WindowState = FormWindowState.Maximized;
                }
            }

            if (!stretch)
            {
                Sketch.SetSize(_screenSize.Width, _screenSize.Height);
            }
            else
            {
                Size = _screenSize;
            }
        }

        public IntPtr WindowHandle
        {
            get { return Handle; }
        }

        public void RendererChanging(RendererType rendererType)
        {
        }

        public void RendererChanged(RendererType rendererType)
        {
        }
    }
}
