using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SketchIt.Utilities
{
    public class CustomToolStripRenderer : ToolStripRenderer
    {
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripButton button = e.Item as ToolStripButton;
            SizeF size = new SizeF(e.Item.Size.Width - 2, e.Item.Size.Height - 2);

            if (button.Pressed)
            {
                using (Pen pen = new Pen(AppearanceSettings.SelectedItemBorderColor))
                using (SolidBrush brush = new SolidBrush(AppearanceSettings.ActiveCaptionBackColor))
                {
                    e.Graphics.FillRectangle(brush, new RectangleF(new PointF(1, 1), size));
                    e.Graphics.DrawRectangle(pen, new Rectangle(new Point(1, 1), size.ToSize()));
                }
            }
            else if (button.Selected)
            {
                using (Pen pen = new Pen(AppearanceSettings.HoverItemBorderColor))
                using (SolidBrush brush = new SolidBrush(AppearanceSettings.HoverItemBackColor))
                {
                    e.Graphics.FillRectangle(brush, new RectangleF(new PointF(1, 1), size));
                    e.Graphics.DrawRectangle(pen, new Rectangle(new Point(1, 1), size.ToSize()));
                }
            }
            else if (button.Checked)
            {
                using (Pen pen = new Pen(AppearanceSettings.SelectedItemBorderColor))
                using (SolidBrush brush = new SolidBrush(AppearanceSettings.ActiveCaptionBackColor))
                {
                    e.Graphics.FillRectangle(brush, new RectangleF(new PointF(1, 1), size));
                    e.Graphics.DrawRectangle(pen, new Rectangle(new Point(1, 1), size.ToSize()));
                }
            }
        }

        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            using (Pen pen = new Pen(AppearanceSettings.BorderColor))
            {
                pen.DashStyle = DashStyle.Dot;
                e.Graphics.DrawLine(pen, e.GripBounds.X, e.GripBounds.Y + 5, e.GripBounds.X, e.GripBounds.Bottom - 7);
                e.Graphics.DrawLine(pen, e.GripBounds.X + 2, e.GripBounds.Y + 5, e.GripBounds.X + 2, e.GripBounds.Bottom - 7);
            }
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            //e.Graphics.FillRectangle(new SolidBrush(ControlPaint.Light(ColorTable.BaseBackColor, .05f)), e.AffectedBounds);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip.IsDropDown)
            {
                e.Graphics.Clear(AppearanceSettings.BaseBackColor);

                using (GraphicsPath path = new GraphicsPath())
                using (Pen pen = new Pen(AppearanceSettings.BorderColor))
                {
                    path.AddLine(e.AffectedBounds.Left, e.AffectedBounds.Top, e.AffectedBounds.Left, e.AffectedBounds.Bottom - 1);
                    path.AddLine(e.AffectedBounds.Left, e.AffectedBounds.Bottom - 1, e.AffectedBounds.Right - 1, e.AffectedBounds.Bottom - 1);
                    path.AddLine(e.AffectedBounds.Right - 1, e.AffectedBounds.Bottom - 1, e.AffectedBounds.Right - 1, e.AffectedBounds.Top);
                    path.AddLine(e.AffectedBounds.Right - 1, e.AffectedBounds.Top, e.ConnectedArea.Right, e.AffectedBounds.Top);

                    e.Graphics.DrawPath(pen, path);
                }
            }
            else if (e.ToolStrip is StatusStrip)
            {
                e.Graphics.Clear(AppearanceSettings.StatusDefaultBackColor);
            }
            else
            {
                e.Graphics.Clear(AppearanceSettings.MenuBackColor);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.ToolStrip is StatusStrip)
            {
                e.TextColor = AppearanceSettings.StatusDefaultForeColor;
            }
            else
            {
                e.TextColor = AppearanceSettings.MenuTextColor;
            }

            base.OnRenderItemText(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripMenuItem menu = e.Item as ToolStripMenuItem;

            if (menu != null && menu.DropDown.Visible)
            {
                e.Graphics.Clear(AppearanceSettings.BaseBackColor);
                Rectangle rect = Rectangle.Round(e.Graphics.VisibleClipBounds);
                rect.Width = menu.Width;
                using (GraphicsPath path = new GraphicsPath())
                using (Pen pen = new Pen(AppearanceSettings.BorderColor))
                {
                    path.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Top);
                    path.AddLine(rect.Left, rect.Top, rect.Right - 1, rect.Top);
                    path.AddLine(rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom);
                    e.Graphics.DrawPath(pen, path);
                }
            }
            else if (e.Item.Selected)
            {
                Rectangle rect = Rectangle.Round(e.Graphics.VisibleClipBounds);
                rect.Width = menu.Width;

                using (Pen pen = new Pen(AppearanceSettings.HoverItemBorderColor))
                using (SolidBrush brush = new SolidBrush(AppearanceSettings.HoverItemBackColor))
                {
                    if (!menu.IsOnDropDown)
                    {
                        rect.Width--;
                        rect.Height--;
                        e.Graphics.FillRectangle(brush, rect);
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(brush, e.Item.ContentRectangle);
                    }
                }
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            using (Pen pen = new Pen(e.ToolStrip is ToolStripDropDown ? AppearanceSettings.BorderColor : AppearanceSettings.BaseBackColor))
            using (Pen pen2 = new Pen(e.ToolStrip is ToolStripDropDown ? AppearanceSettings.BaseBackColor : AppearanceSettings.BorderColor))
            {
                if (e.Vertical)
                {
                    e.Graphics.DrawLine(pen, e.Item.ContentRectangle.Left, e.Item.ContentRectangle.Top, e.Item.ContentRectangle.Left, e.Item.ContentRectangle.Bottom);
                    e.Graphics.DrawLine(pen2, e.Item.ContentRectangle.Left + 1, e.Item.ContentRectangle.Top, e.Item.ContentRectangle.Left + 1, e.Item.ContentRectangle.Bottom);
                }
                else
                {
                    e.Graphics.DrawLine(pen, e.Item.ContentRectangle.Left, e.Item.ContentRectangle.Top, e.Item.ContentRectangle.Right, e.Item.ContentRectangle.Top);
                }
            }
        }
    }
}
