using System;
using System.Drawing;
using System.Windows.Forms;

namespace SketchIt.Utilities
{
    public class VisualWrapper : Control
    {
        private Control _control;
        private TextBox _comboTextBox;

        public static VisualWrapper Apply(Control control)
        {
            if (control.Parent is Form || control.Parent is TableLayoutPanel || control.Parent is Panel ||
                control.Parent is FlowLayoutPanel || control.Parent is SplitContainer)
            {
                return new Utilities.VisualWrapper(control);
            }

            if (control.Parent is VisualWrapper wrapper)
            {
                wrapper.BackColor = wrapper._control.BackColor;
            }

            return null;
        }

        private VisualWrapper(Control control)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            Control parent = control.Parent;
            TableLayoutPanel layoutPanel = parent as TableLayoutPanel;

            _control = control;

            this.Size = control.Size;
            this.Location = control.Location;
            this.Padding = control.Padding;
            this.Margin = control.Margin;
            this.Anchor = control.Anchor;
            this.Dock = control.Dock;
            this.TabIndex = control.TabIndex;

            int index = parent.Controls.IndexOf(control);
            int row = layoutPanel?.GetRow(control) ?? 0;
            int col = layoutPanel?.GetColumn(control) ?? 0;
            int rowSpan = layoutPanel?.GetRowSpan(control) ?? 0;
            int colSpan = layoutPanel?.GetColumnSpan(control) ?? 0;

            parent.Controls.Remove(control);

            if (layoutPanel != null)
            {
                layoutPanel.Controls.Add(this, col, row);
                layoutPanel.SetRowSpan(this, rowSpan);
                layoutPanel.SetColumnSpan(this, colSpan);
            }
            else
            {
                parent.Controls.Add(this);
                parent.Controls.SetChildIndex(this, index);
            }

            control.Padding = new Padding();
            control.Margin = new Padding();
            control.Dock = DockStyle.None;
            control.Anchor = AnchorStyles.None;
            control.GotFocus += ControlFocusChanged;
            control.LostFocus += ControlFocusChanged;
            control.TextChanged += ControlTextChanged;

            if (control is TextBoxBase)
            {
                this.SetStyle(ControlStyles.Selectable, false);
                ((TextBoxBase)control).BorderStyle = BorderStyle.None;
            }
            else if (control is ComboBox)
            {
                control.Visible = false;

                if (((ComboBox)control).DropDownStyle == ComboBoxStyle.DropDown)
                {
                    _comboTextBox = new TextBox();
                    _comboTextBox.Text = control.Text;
                    _comboTextBox.BorderStyle = BorderStyle.None;
                    _comboTextBox.KeyDown += ComboTextBoxKeyDown;
                    _comboTextBox.PreviewKeyDown += ComboTextBoxPreviewKeyDown;
                    _comboTextBox.TextChanged += ComboTextBoxTextChanged;
                    this.Controls.Add(_comboTextBox);
                    this.SetStyle(ControlStyles.Selectable, false);
                }

                ((ComboBox)control).ItemHeight = control.Height;
                ((ComboBox)control).SelectedIndexChanged += ComboSelectedIndexChanged;
                ((ComboBox)control).DropDown += ComboDropDownChanged;
                ((ComboBox)control).DropDownClosed += ComboDropDownChanged;
            }

            this.Controls.Add(control);
            this.BackColor = control.BackColor;

            OnSizeChanged(new EventArgs());
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (_control is ComboBox && _comboTextBox == null)
            {
                ComboTextBoxPreviewKeyDown(this, e);
                ComboTextBoxKeyDown(this, new KeyEventArgs(e.KeyData));
            }
            base.OnPreviewKeyDown(e);
        }

        private void ComboTextBoxTextChanged(object sender, EventArgs e)
        {
            _control.Text = _comboTextBox.Text;
        }

        private void ComboTextBoxPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            ComboBox combo = _control as ComboBox;
            if (combo == null) return;

            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (combo.DroppedDown)
                    {
                        combo.DroppedDown = false;
                        e.IsInputKey = true;
                    }
                    break;

                case Keys.Enter:
                    if (combo.DroppedDown)
                    {
                        combo.DroppedDown = false;
                        e.IsInputKey = true;
                    }
                    break;
            }
        }

        private void ComboTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            ComboBox combo = _control as ComboBox;
            if (combo == null) return;

            switch (e.KeyCode)
            {
                case Keys.F4:
                    combo.DroppedDown = !combo.DroppedDown;
                    e.Handled = true;
                    break;

                case Keys.Down:
                    if (combo.SelectedIndex < combo.Items.Count - 1) combo.SelectedIndex++;
                    e.Handled = true;
                    break;

                case Keys.Up:
                    if (combo.SelectedIndex > 0) combo.SelectedIndex--;
                    e.Handled = true;
                    break;
            }
        }

        private void ComboDropDownChanged(object sender, EventArgs e)
        {
            _control.Enabled = !((ComboBox)_control).DroppedDown;

            if (_comboTextBox != null)
            {
                _comboTextBox.Focus();
                _comboTextBox.SelectAll();
            }
        }

        private void ComboSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_comboTextBox != null)
            {
                _comboTextBox.Text = _control.Text;
                _comboTextBox.SelectAll();
            }
        }

        private void ControlTextChanged(object sender, EventArgs e)
        {
            this.Invalidate();

            if (_comboTextBox != null)
            {
                _comboTextBox.Text = _control.Text;
            }
        }

        private void ControlFocusChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            if (_control is ComboBox) ((ComboBox)_control).DroppedDown = false;
            base.OnLeave(e);
            this.Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (_control is ComboBox)
            {
                this.Focus();

                if (_comboTextBox != null) _comboTextBox.Focus();
                else _control.Focus();

                ((ComboBox)_control).DroppedDown = true;
                this.Invalidate();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (_control == null) return;

            TextBoxBase textBox = (_comboTextBox ?? _control) as TextBoxBase;

            if (textBox != null)
            {
                if (textBox.Multiline)
                {
                    textBox.Top = 4;
                    textBox.Height = this.Height - 8;
                }
                else
                {
                    textBox.Top = (int)Math.Round((this.Height - textBox.Height) / 2f);
                }

                textBox.Width = this.Width - 8 - (_control is ComboBox ? SystemInformation.VerticalScrollBarWidth : 0);
                textBox.Left = 4;
            }

            if (_control is ComboBox)
            {
                _control.Top = 0;
                _control.Width = 0;
                _control.Left = 0;
                ((ComboBox)_control).DropDownWidth = this.Width;
            }

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            Color borderColor = this.ContainsFocus ? AppearanceSettings.ActiveCaptionBackColor : this.ForeColor;
            Rectangle rect = this.ClientRectangle;
            rect.Width--;
            rect.Height--;
            e.Graphics.DrawRectangle(new Pen(borderColor), rect);

            if (_control is ComboBox)
            {
                if (_comboTextBox == null)
                {
                    if (this.ContainsFocus) e.Graphics.FillRectangle(new SolidBrush(AppearanceSettings.ActiveCaptionBackColor), rect);
                    TextRenderer.DrawText(e.Graphics, _control.Text, _control.Font, rect, this.ContainsFocus ? AppearanceSettings.ActiveCaptionTextColor : _control.ForeColor, TextFormatFlags.VerticalCenter);
                }

                Color foreColor = this.ContainsFocus ? AppearanceSettings.ActiveCaptionTextColor : _control.ForeColor;
                rect.Width = SystemInformation.VerticalScrollBarWidth;
                rect.X = this.Width - rect.Width;

                if (_comboTextBox != null)
                {
                    rect.Inflate(2, 0);
                    if (this.ContainsFocus) e.Graphics.FillRectangle(new SolidBrush(AppearanceSettings.ActiveCaptionBackColor), rect);
                    rect.Inflate(-2, 0);

                    using (Pen pen = new Pen(borderColor))
                        e.Graphics.DrawLine(pen, rect.X - 2, 1, rect.X - 2, rect.Bottom - 1);
                }
                else
                {
                }

                TextRenderer.DrawText(e.Graphics, "▼", new Font(_control.Font.FontFamily, _control.Font.Size * .65f), rect, foreColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            }
        }
    }
}
