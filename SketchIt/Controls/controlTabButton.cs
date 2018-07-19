using SketchIt.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SketchIt.Controls
{
    public class TabButtonEventArgs : EventArgs
    {
        public TabButton Button { get; private set; }
        public TabButtonEventArgs(TabButton button)
        {
            Button = button;
        }
    }

    public partial class TabButtonsControl : FlowLayoutPanel
    {
        public delegate void TabButtonEventHandler(object sender, TabButtonEventArgs e);
        public event TabButtonEventHandler TabButtonClick;

        private TabButton _selectedButton;

        public TabButtonsControl()
        {
            TabButtons = new TabButtonCollection(this);
            Padding = new Padding(2, 2, 2, 0);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabButtonCollection TabButtons
        {
            get;
            private set;
        }

        private Label GetTabButtonLabel(TabButton button)
        {
            foreach (Control ctl in this.Controls)
                if (ctl.Tag == button)
                    return ctl as Label;

            return null;
        }

        public void SelectNextTab()
        {
            Label current = GetTabButtonLabel(_selectedButton);

            if (current != null)
            {
                int index = Controls.IndexOf(current);

                if (index == Controls.Count - 1)
                    current = Controls[0] as Label;
                else
                    current = Controls[index + 1] as Label;

                HandleTabClick(current, new EventArgs());
            }
        }

        internal void UpdateButtons()
        {
            this.Controls.Clear();

            foreach (TabButton button in TabButtons)
            {
                Label label = new Label();
                label.Text = button.Text;
                label.Tag = button;
                label.AutoSize = true;
                label.Margin = new Padding(0, 0, 2, 2);
                label.Padding = new Padding(6, 3, 6, 3);
                label.MouseDown += HandleTabClick;
                label.Visible = button.Visible;
                button.Label = label;
                this.Controls.Add(label);
            }

            if (this.Controls.Count > 0)
                HandleTabClick(this.Controls[0], new EventArgs());
        }

        private void HandleTabClick(object sender, EventArgs e)
        {
            TabButton button = null;
            Label lbl = sender as Label;

            lbl.BackColor = AppearanceSettings.ActiveCaptionBackColor; //ColorTable.BaseBackColor
            lbl.ForeColor = AppearanceSettings.ActiveCaptionTextColor; //ColorTable.ActiveCaptionBackColor
            //lbl.Padding = new Padding(6, 3, 20, 3);

            foreach (Label label in this.Controls)
                if (label != null && !label.Equals(sender))
                {
                    label.BackColor = this.BackColor;
                    label.ForeColor = AppearanceSettings.ApplicationTextColor;
                    //label.Padding = new Padding(6, 3, 6, 3);
                }
                else if (label.Equals(lbl))
                {
                    button = lbl.Tag as TabButton;
                }

            if (button != null)
            {
                _selectedButton = button;
                OnTabButtonClick(new TabButtonEventArgs(button));
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.FromArgb(41, 57, 86));

            using (Pen pen = new Pen(AppearanceSettings.ActiveCaptionBackColor))
            {
                pen.Width = 1;
                e.Graphics.DrawLine(pen, 0, e.ClipRectangle.Bottom - 1, e.ClipRectangle.Right, e.ClipRectangle.Bottom - 1);
                e.Graphics.DrawLine(pen, 0, e.ClipRectangle.Bottom - 2, e.ClipRectangle.Right, e.ClipRectangle.Bottom - 2);
            }

            base.OnPaint(e);
        }

        protected void OnTabButtonClick(TabButtonEventArgs e)
        {
            TabButtonClick?.Invoke(this, e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            UpdateButtons();
            base.OnBackColorChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            UpdateButtons();
            base.OnForeColorChanged(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        public void SelectTab(object tag)
        {
            foreach (Control ctl in this.Controls)
            {
                if (ctl.Tag is TabButton && ((TabButton)ctl.Tag).Tag == tag)
                {
                    HandleTabClick(ctl, new EventArgs());
                    break;
                }
            }
        }
    }

    [Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
    public class TabButtonCollection : IList<TabButton>, ICollection<TabButton>, IEnumerable<TabButton>, IList, ICollection, IEnumerable
    {
        private TabButtonsControl _control;
        private List<TabButton> _list;

        internal TabButtonCollection(TabButtonsControl control)
        {
            _control = control;
            _list = new List<TabButton>();
        }

        public TabButton this[int index]
        {
            get { return _list[index]; }
            set
            {
                _list[index] = value;
                _control.UpdateButtons();
            }
        }

        object IList.this[int index]
        {
            get { return ((IList)_list)[index]; }
            set
            {
                ((IList)_list)[index] = value;
                _control.UpdateButtons();
            }
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsFixedSize
        {
            get { return ((IList)_list).IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsSynchronized
        {
            get { return ((ICollection)_list).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((ICollection)_list).SyncRoot; }
        }

        public int Add(object value)
        {
            int result = ((IList)_list).Add(value);
            _control.UpdateButtons();
            return result;
        }

        public void Add(TabButton item)
        {
            _list.Add(item);
            _control.UpdateButtons();
        }

        public void AddRange(IEnumerable<TabButton> items)
        {
            _list.AddRange(items);
            _control.UpdateButtons();
        }

        public void Clear()
        {
            _list.Clear();
            _control.UpdateButtons();
        }

        public bool Contains(object value)
        {
            return ((IList)_list).Contains(value);
        }

        public bool Contains(TabButton item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_list).CopyTo(array, index);
        }

        public void CopyTo(TabButton[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TabButton> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            return ((IList)_list).IndexOf(value);
        }

        public int IndexOf(TabButton item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, object value)
        {
            ((IList)_list).Insert(index, value);
            _control.UpdateButtons();
        }

        public void Insert(int index, TabButton item)
        {
            _list.Insert(index, item);
            _control.UpdateButtons();
        }

        public void Remove(object value)
        {
            ((IList)_list).Remove(value);
            _control.UpdateButtons();
        }

        public bool Remove(TabButton item)
        {
            bool result = _list.Remove(item);
            _control.UpdateButtons();
            return result;
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            _control.UpdateButtons();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }

    public class TabButton
    {
        private string _text;
        internal Label Label { get; set; }

        public TabButton()
        {
            Visible = true;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (Label != null) Label.Text = _text;
            }
        }

        [TypeConverter(typeof(StringConverter))]
        public object Tag { get; set; }

        [DefaultValue(true)]
        public bool Visible { get; set; }
    }
}
