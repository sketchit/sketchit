using SketchIt.Utilities;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SketchIt.Controls
{
    public class ListViewEx : ListView
    {
        private bool _updateingColumnWidths = false;

        public ListViewEx()
        {
            OwnerDraw = true;
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter;

            e.DrawBackground();
            e.Graphics.FillRectangle(new SolidBrush(AppearanceSettings.HeaderBackColor), e.Bounds);
            TextRenderer.DrawText(e.Graphics, e.Header.Text, Font, e.Bounds, AppearanceSettings.HeaderForeColor, flags);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
        {
            base.OnColumnWidthChanging(e);
            UpdateColumnWidths();
        }

        protected override void OnColumnWidthChanged(ColumnWidthChangedEventArgs e)
        {
            base.OnColumnWidthChanged(e);
            UpdateColumnWidths();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateColumnWidths();
        }

        private void UpdateColumnWidths()
        {
            if (_updateingColumnWidths)
            {
                return;
            }

            _updateingColumnWidths = true;

            ColumnHeader resizeHeader = Columns[Columns.Count - 1];
            int width = 0;

            foreach (ColumnHeader header in Columns)
            {
                if (!header.Equals(resizeHeader))
                {
                    width += header.Width;
                }
            }

            resizeHeader.Width = Width - width;

            _updateingColumnWidths = false;
        }
    }
}
