using ScintillaNET;
using SketchIt.Controls;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace SketchIt
{
    public partial class EditorForm : BaseForm
    {
        private Scintilla _editor;
        private string _fileName;

        public EditorForm()
        {
            InitializeComponent();
            Type = WindowType.SourceFile;
        }

        private void EditorForm_Load(object sender, EventArgs e)
        {
            _editor = new EditorControl();
            _editor.Dock = DockStyle.Fill;
            _editor.TextChanged += EditorTextChanged;
            pnlEditor.Controls.Add(_editor);
        }

        private void EditorTextChanged(object sender, EventArgs e)
        {
            EditorText = Editor.Text;
            Program.MainForm.CodeChanged(this);
        }

        public string EditorText
        {
            get;
            private set;
        }

        public Scintilla Editor
        {
            get
            {
                return _editor;
            }
        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                this.Text = new FileInfo(value).Name;
            }
        }

        public override void HandleCommand(string command)
        {
            switch (command)
            {
                case "save":
                    Save();
                    break;

                default:
                    base.HandleCommand(command);
                    break;
            }
        }

        public override bool SupportsCommand(string command)
        {
            switch (command)
            {
                case "save":
                    return true;

                default:
                    return base.SupportsCommand(command);
            }
        }

        public override ToolStrip ToolStrip => toolStrip1;

        public bool Save()
        {
            string fileName = FileName;

            if (string.IsNullOrEmpty(fileName))
            {
                using (SaveFileDialog d = new SaveFileDialog())
                {
                    d.Filter = "C# File|*.cs";
                    d.OverwritePrompt = true;

                    if (d.ShowDialog() == DialogResult.OK)
                    {
                        fileName = d.FileName;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            try
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Create))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(_editor.Text);
                    writer.Close();
                }

                _editor.SetSavePoint();
                FileName = fileName;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_editor.Modified)
            {
                switch (MessageBox.Show("\"" + this.Text + "\" has been modified. Would you like to save it before closing the window?", "File Modified", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        e.Cancel = !Save();
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

            base.OnClosing(e);
        }

        internal void UpdateEditorKeywords()
        {
            Editor.SetKeywords(1, Program.Parser.KnownKeywords + " " + Program.Parser.ParsedKeywords);
            Editor.SetKeywords(3, Program.Parser.KnownTypes + " " + Program.Parser.ParsedTypes);
        }
    }
}
