using SketchIt.Controls;
using SketchIt.Utilities;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SketchIt
{
    public enum WindowType
    {
        Unknown = 0,
        SourceFile = 1
    }

    public partial class BaseForm : Form
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private const int WS_BORDER = 0x800000;
        private const int GWL_EXSTYLE = -20;
        private const int GWL_STYLE = -16;
        private const int WS_EX_CLIENTEDGE = 0x200;
        private const int WS_EX_WINDOWEDGE = 0x100;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREDRAW = 0x0008;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;

        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_NOSENDCHANGING = 0x0400;

        public event ProcessCommandKeysHandler ProcessCommandKeys;

        public WindowType Type
        {
            get;
            protected set;
        }

        public BaseForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Program.IsRunning) return;

            this.Font = new Font("Segoe UI", 9f);
            base.OnLoad(e);

            //if (!this.Modal)
            {
                UpdateAppearance();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (FormBorderStyle == FormBorderStyle.Sizable) AutoSize = false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ProcessCommandKeys != null)
                ProcessCommandKeys(new ProcessCommandKeysEventArgs(keyData, ModifierKeys));

            return base.ProcessCmdKey(ref msg, keyData);
        }

        internal protected virtual void UpdateAppearance()
        {
            this.BackColor = AppearanceSettings.ApplicationBackColor;
            this.ForeColor = AppearanceSettings.ApplicationTextColor;
            SetControlProperties(this.Controls);
            this.Invalidate();
        }

        private void SetControlProperties(Control.ControlCollection controls)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                Control control = controls[i];

                if (control is SplitterPanel splitterPanel)
                {
                    splitterPanel.BackColor = AppearanceSettings.BaseBackColor;
                }
                else if (control is MenuStrip menuStrip)
                {
                    menuStrip.Renderer = new CustomToolStripRenderer();
                }
                else if (control is ToolStrip toolStrip)
                {
                    toolStrip.Renderer = new CustomToolStripRenderer();
                }
                else if (control is StatusStrip statusStrip)
                {
                    statusStrip.Renderer = new CustomToolStripRenderer();
                }
                else if (control is MdiClient && this.IsMdiContainer)
                {
                    control.BackColor = AppearanceSettings.ApplicationBackColor;
                }
                else if (control is EditorControl editorControl)
                {
                    editorControl.UpdateAppearance();
                }
                else if (control is ProgressBar progressBar)
                {
                    progressBar.BackColor = AppearanceSettings.ApplicationBackColor;
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = AppearanceSettings.TextBoxBackColor;
                    comboBox.ForeColor = AppearanceSettings.TextBoxForeColor;
                    if (VisualWrapper.Apply(control) != null) i--;
                }
                else if (control is TextBoxBase textBoxBase)
                {
                    textBoxBase.BackColor = AppearanceSettings.TextBoxBackColor;
                    textBoxBase.ForeColor = AppearanceSettings.TextBoxForeColor;
                    if (VisualWrapper.Apply(control) != null) i--;
                }
                else if (control is PropertyGrid propertyGrid)
                {
                    propertyGrid.ViewBackColor = this.BackColor;
                    propertyGrid.ViewForeColor = this.ForeColor;
                    propertyGrid.HelpBackColor = AppearanceSettings.BaseBackColor;
                    propertyGrid.HelpForeColor = AppearanceSettings.BaseForeColor;
                    propertyGrid.LineColor = AppearanceSettings.BorderColor;
                    propertyGrid.CategoryForeColor = AppearanceSettings.ActiveCaptionTextColor;
                }
                else if (control is DataGridView dataGridView)
                {
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = this.BackColor;
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = this.ForeColor;
                    dataGridView.GridColor = this.BackColor;
                    dataGridView.BackgroundColor = AppearanceSettings.BaseBackColor;
                    dataGridView.RowHeadersDefaultCellStyle = ((DataGridView)control).ColumnHeadersDefaultCellStyle;
                    dataGridView.DefaultCellStyle.BackColor = AppearanceSettings.BaseBackColor;
                    dataGridView.DefaultCellStyle.ForeColor = AppearanceSettings.BaseForeColor;
                }
                else if (control is ListView listView)
                {
                    listView.BackColor = AppearanceSettings.BaseBackColor;
                    listView.ForeColor = AppearanceSettings.BaseForeColor;
                    control.Refresh();
                }
                else if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat;
                    button.BackColor = AppearanceSettings.BaseBackColor;
                    button.ForeColor = AppearanceSettings.BaseForeColor;
                    button.FlatAppearance.BorderColor = AppearanceSettings.HoverItemBackColor;
                    button.FlatAppearance.MouseOverBackColor = AppearanceSettings.HoverItemBackColor;
                    button.FlatAppearance.MouseDownBackColor = AppearanceSettings.ActiveCaptionBackColor;
                    button.FlatAppearance.BorderSize = 0;
                }
                else if (control is TreeView treeView)
                {
                    treeView.BackColor = AppearanceSettings.BaseBackColor;
                    treeView.ForeColor = AppearanceSettings.BaseForeColor;
                }

                control.Invalidate();

                if (control.HasChildren) SetControlProperties(control.Controls);
            }
        }

        public bool SetBevel(bool show)
        {
            foreach (Control c in this.Controls)
            {
                MdiClient client = c as MdiClient;

                if (client != null)
                {
                    int windowLong = GetWindowLong(c.Handle, GWL_EXSTYLE);

                    if (show)
                    {
                        windowLong |= WS_EX_CLIENTEDGE;
                    }
                    else
                    {
                        windowLong &= ~WS_EX_CLIENTEDGE;
                    }

                    SetWindowLong(c.Handle, GWL_EXSTYLE, windowLong);

                    // Update the non-client area.
                    SetWindowPos(client.Handle, IntPtr.Zero, 0, 0, 0, 0,
                        SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER |
                        SWP_NOOWNERZORDER | SWP_FRAMECHANGED);

                    return true;
                }
            }

            return false;
        }

        public virtual void HandleCommand(string command)
        {
        }

        public virtual bool SupportsCommand(string command)
        {
            return false;
        }

        public virtual ToolStrip ToolStrip
        {
            get;
        }
    }

    public class ProcessCommandKeysEventArgs : EventArgs
    {
        public Keys KeyData { get; set; }
        public Keys ModifierKeys { get; set; }

        public ProcessCommandKeysEventArgs(Keys keyData, Keys modifierKeys)
        {
            KeyData = keyData;
            ModifierKeys = modifierKeys;
        }
    }

    public delegate void ProcessCommandKeysHandler(ProcessCommandKeysEventArgs e);
}
