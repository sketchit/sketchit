using Newtonsoft.Json;
using SketchIt.Api;
using SketchIt.Controls;
using SketchIt.Utilities;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SketchIt
{
    public partial class MainForm : BaseForm
    {
        private Stack<StatusAction> _status = new Stack<StatusAction>();
        private List<ToolStripItem> _enabledButtons;
        private ConsoleWriter _consoleWriter;
        private int NewFileCounter = 0;
        private Process _process;
        private System.Timers.Timer _previewTimer;
        private System.Timers.Timer _updateTimer;
        private float _currentScale = 1f;
        private Sketch _sketch;
        private string _projectFile = null;
        private Status _startStatus;
        private List<ProjectFileReference> _resourceFiles;

        public MainForm()
        {
            InitializeComponent();
            Compiler = new Compiler();
            BackgroundCompiler = new Compiler(true);
            Parser = new SimpleParser();
            ResetPreviewTimer(false);
            Console.SetOut(_consoleWriter = new ConsoleWriter(txtConsoleOutput, true));
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            Program.InvokeMethod(Program.BusyDialog, new MethodInvoker(Program.BusyDialog.Close));
            base.OnFormClosed(e);
        }

        public EditorForm AddNewSourceFile() { return NewSourceFile(false); }
        public EditorForm NewSourceFile(bool isOpeningFile)
        {
            EditorForm form = new EditorForm();
            form.MdiParent = this;
            form.FormBorderStyle = FormBorderStyle.None;
            form.ControlBox = false;
            form.Show();
            form.WindowState = FormWindowState.Maximized;
            form.TextChanged += MdiChildTextChanged;

            if (!isOpeningFile)
            {
                NewFileCounter++;
                form.Text = "New Sketch " + NewFileCounter;
            }

            tabWindows.TabButtons.Add(new TabButton()
            {
                Text = form.Text,
                Tag = form
            });

            form.Activate();

            return form;
        }

        public ProjectFileReference[] GetResourceFiles()
        {
            return _resourceFiles.ToArray();
        }

        public SimpleParser Parser
        {
            get;
            private set;
        }

        public Compiler BackgroundCompiler
        {
            get;
            private set;
        }

        public Compiler Compiler
        {
            get;
            private set;
        }

        public string ProjectName
        {
            get
            {
                return _projectFile == null ? null : Path.GetFileNameWithoutExtension(new FileInfo(_projectFile).Name);
            }
        }

        public void AddExistingFile()
        {
            using (OpenFileDialog d = new OpenFileDialog())
            {
                d.Filter = "C# File|*.cs|Sketchbook Project|*.sbp";
                d.Title = "Open File";

                if (d.ShowDialog() == DialogResult.OK)
                {
                    EditorForm form = GetActiveWindow() as EditorForm;

                    if (form == null || form.Editor.Modified || !string.IsNullOrEmpty(form.EditorText))
                    {
                        form = NewSourceFile(true);
                    }

                    using (TextReader reader = new StreamReader(d.OpenFile()))
                    {
                        form.FileName = d.FileName;
                        form.Editor.Text = reader.ReadToEnd();
                        form.Editor.SetSavePoint();
                        reader.Close();
                    }

                    form.Activate();
                }
            }
        }

        public bool NewProject(bool addEmptyFile)
        {
            ResetPreviewTimer(btnLivePreview.Enabled);

            foreach (BaseForm form in this.MdiChildren)
            {
                form.Close();
                if (form.Visible) return false;

                foreach (TabButton button in tabWindows.TabButtons)
                {
                    if (button.Tag.Equals(form))
                    {
                        tabWindows.TabButtons.Remove(button);
                        break;
                    }
                }
            }

            CloseLivePreview();

            if (addEmptyFile)
            {
                AddNewSourceFile();
            }

            _resourceFiles = new List<ProjectFileReference>();
            _projectFile = null;

            Text = "SketchIt - New Project";

            return true;
        }

        public BaseForm GetActiveWindow()
        {
            return ActiveMdiChild as BaseForm;
        }

        public void UpdateToolbarButtons()
        {
            foreach (ToolStripItem item in tlsMain.Items)
            {
                EnableToolItem(item);
            }

            foreach (ToolStripMenuItem item in mnsMain.Items)
            {
                EnableToolItem(item);
            }
        }

        private void EnableToolItem(ToolStripItem item)
        {
            BaseForm activeForm = GetActiveWindow();

            if (item.Tag != null)
            {
                switch (item.Tag.ToString())
                {
                    case "start":
                        item.Enabled = activeForm != null && _process == null;
                        break;

                    case "stop":
                        item.Enabled = _process != null;
                        break;

                    default:
                        if (!_enabledButtons.Contains(item))
                        {
                            item.Enabled = activeForm != null && activeForm.SupportsCommand(item.Tag.ToString());
                        }
                        break;
                }
            }

            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem childItem in menuItem.DropDownItems)
                {
                    EnableToolItem(childItem);
                }
            }
        }

        private void UpdateWindowTabs()
        {
            Form activeForm = GetActiveWindow();

            if (activeForm != null)
            {
                tabWindows.SelectTab(activeForm);
            }
        }

        private void MenuItemClicked(object sender, EventArgs e)
        {
            try
            {
                switch (((ToolStripItem)sender).Tag.ToString())
                {
                    case "resources":
                        ResourcesForm.ShowResources(_resourceFiles);
                        break;

                    case "restart-preview":
                        RestartPreview();
                        break;

                    case "pause-preview":
                        PausePreview();
                        break;

                    case "save-all":
                        SaveProject();
                        break;

                    case "add-new-source-file":
                        AddNewSourceFile();
                        break;

                    case "add-existing-file":
                        AddExistingFile();
                        break;

                    case "open-project":
                        OpenProject();
                        break;

                    case "start":
                        StartApp();
                        break;

                    case "stop":
                        Stop();
                        break;

                    case "new-project":
                        NewProject(true);
                        break;

                    case "toggle-error-list":
                        splErrorList.Visible = pnlErrorList.Visible = ((ToolStripButton)sender).Checked;
                        break;

                    case "remove-from-project":
                        if (GetActiveWindow() is BaseForm form)
                        {
                            form.Close();

                            if (!form.Equals(GetActiveWindow()))
                            {
                                foreach (TabButton button in tabWindows.TabButtons)
                                {
                                    if (form.Equals(button.Tag))
                                    {
                                        tabWindows.TabButtons.Remove(button);
                                        break;
                                    }

                                }
                            }
                        }
                        break;

                    case "exit":
                        this.Close();
                        break;

                    case "about":
                        new SplashScreenForm().ShowDialog();
                        break;

                    case "website":
                        Process.Start("http://www.sketchit.org");
                        break;

                    case "help":
                        Process.Start("sketchit.chm");
                        break;

                    case "create-screen-saver":
                        CreateScreenSaver();
                        break;

                    default:
                        if (GetActiveWindow() != null)
                        {
                            GetActiveWindow().HandleCommand(((ToolStripItem)sender).Tag.ToString());
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SketchIt", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateToolbarButtons();
            }
        }

        private void CreateScreenSaver()
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.FileName = "SketchIt Screen Saver.scr";
                dialog.Filter = "Screen Saver|*.scr";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (Compiler.Compile(Properties.Resources.AppScreenSaver))
                    {
                        File.Copy(Compiler.Output.Location, dialog.FileName, true);
                    }
                    else
                    {
                        MessageBox.Show("Unable to compile the sketch. Please check the error list.", "SketchIt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RestartPreview()
        {
            UpdateLivePreview();
        }

        private void PausePreview()
        {
            if (_sketch != null)
            {
                if (_sketch.IsLooping)
                {
                    _sketch.NoLoop();
                }
                else
                {
                    _sketch.Loop();
                }

                btnPausePreview.Checked = !_sketch.IsLooping;
                btnPausePreview.Image = _sketch.IsLooping ? Properties.Resources.Pause : Properties.Resources.Resume;
                btnPausePreview.Text = _sketch.IsLooping ? "Pause" : "Resume";
            }
        }

        private void MdiChildTextChanged(object sender, EventArgs e)
        {
            foreach (TabButton button in tabWindows.TabButtons)
            {
                if (sender.Equals(button.Tag))
                {
                    button.Text = ((Form)sender).Text;
                }
            }
        }

        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);
            UpdateToolbarButtons();

            ToolStripManager.RevertMerge(tlsMain);

            if (GetActiveWindow() != null && GetActiveWindow().ToolStrip != null)
            {
                ToolStripManager.Merge(GetActiveWindow().ToolStrip, tlsMain);
            }

            UpdateWindowTabs();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetBevel(false);

            _enabledButtons = new List<ToolStripItem>();

            foreach (ToolStripItem item in tlsMain.Items)
            {
                CheckEnabledButton(item);
            }

            foreach (ToolStripItem item in mnsMain.Items)
            {
                CheckEnabledButton(item);
            }

            NewProject(true);

            _updateTimer = new System.Timers.Timer(100);
            _updateTimer.Elapsed += UpdateTimerElapsed;
            _updateTimer.Start();

            string latestVersion = Program.IsUpdateAvailable();
            mnuInfo.Visible = !string.IsNullOrEmpty(latestVersion);
            if (!string.IsNullOrEmpty(latestVersion))
            {
                if (MessageBox.Show(string.Format("An update is available (version {0}). Would you like to visit the website to download the latest version?", latestVersion), "SketchIt Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Process.Start("http://www.sketchit.org");
                }
            }

            Settings.User.EnableFileWatcher();
        }

        private void UpdateTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _updateTimer.Stop();

                tslParseTime.Text = string.Format("{0:0.00}s", Program.Parser.ParseTime.TotalSeconds);

                if (IsLivePreviewEnabled)
                {
                    if (_sketch != null)
                    {
                        tslFrameRate.Text = string.Format("Frame Rate: {0:0.00}", _sketch.FrameRate);
                    }

                    _consoleWriter.Flush();
                }
            }
            finally
            {
                _updateTimer.Start();
            }
        }

        private void CheckEnabledButton(ToolStripItem item)
        {
            if (item.Enabled)
            {
                _enabledButtons.Add(item);
            }

            if (item is ToolStripMenuItem menuItem)
            {
                foreach (ToolStripItem child in menuItem.DropDownItems)
                {
                    CheckEnabledButton(child);
                }
            }
        }

        private void TabWindows_TabButtonClick(object sender, TabButtonEventArgs e)
        {
            BaseForm form = e.Button.Tag as BaseForm;

            if (form != null)
            {
                form.Activate();
            }
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
        }

        private void ctlCanvas_SizeChanged(object sender, EventArgs e)
        {
            if (ctlCanvas.Tag != null) return;
            if (!pnlPreviewPane.Visible) return;

            ctlCanvas.Tag = DateTime.Now;
            ctlCanvas.Zoom(1 / _currentScale);
            //ctlCanvas.Zoom(1 / _sketch.Zoom);

            int maxWidth = pnlCanvasContainer.Width;
            int maxHeight = pnlCanvasContainer.Height;
            float width = ctlCanvas.Width;
            float height = ctlCanvas.Height;

            if (width > maxWidth)
            {
                width = maxWidth / width;
                height *= width;
                width = maxWidth;
            }

            if (height > maxHeight)
            {
                height = maxHeight / height;
                width *= height;
                height = maxHeight;
            }

            _currentScale = width / ctlCanvas.Width;
            ctlCanvas.Zoom(_currentScale);

            ctlCanvas.Location = new System.Drawing.Point((pnlCanvasContainer.Width - (int)width) / 2, (pnlCanvasContainer.Height - (int)height) / 2);
            ctlCanvas.Tag = null;
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            _process = null;
            BeginInvoke(new MethodInvoker(Program.MainForm.UpdateToolbarButtons));
        }

        private bool IsLivePreviewEnabled
        {
            get { return btnLivePreview.Checked; }
        }

        private void UpdateLivePreview()
        {
            lvwPreviewErrors.Items.Clear();

            try
            {
                if (IsLivePreviewEnabled && BackgroundCompiler.Output != null)
                {
                    btnPausePreview.Checked = false;
                    btnPausePreview.Text = "Pause";
                    btnPausePreview.Image = Properties.Resources.Pause;

                    ctlCanvas.Hide();
                    lblPreparingPreview.Show();

                    _currentScale = 1;

                    if (_sketch != null)
                    {
                        if (!_sketch.NoLoop())
                        {
                            //BuildPreview();
                            //return;
                        }

                        _sketch.Exit();
                    }

                    _consoleWriter.Clear();
                    txtConsoleOutput.Clear();

                    pnlCanvasContainer.Controls.Remove(ctlCanvas);
                    ctlCanvas.Dispose();
                    ctlCanvas = new Windows.SketchContainer();
                    ctlCanvas.CreateControl();
                    ctlCanvas.DesignMode = true;
                    ctlCanvas.SizeChanged += ctlCanvas_SizeChanged;
                    pnlCanvasContainer.Controls.Add(ctlCanvas);

                    _sketch = new Sketch();
                    _sketch.UseDefaultConsole = false;

                    Windows.Application.Set(_sketch);
                    Applet applet = BackgroundCompiler.Output.CreateInstance("App") as Applet;
                    _sketch.Start(applet, ctlCanvas);

                    ctlCanvas_SizeChanged(null, null);

                    //ProcessStartInfo startInfo = new ProcessStartInfo(BackgroundCompiler.Output.Location, $"preview={ctlCanvas.Handle}");
                    //Process.Start(startInfo);

                    ctlCanvas.Show();
                    lblPreparingPreview.Hide();
                }
                else if (IsLivePreviewEnabled && _sketch != null)
                {
                    //_sketch.NoLoop();
                }

                if (BackgroundCompiler.CompilerErrors != null)
                {
                    foreach (CompilerError error in BackgroundCompiler.CompilerErrors)
                    {
                        string line = error.Line == 0 ? "" : error.Line.ToString();

                        if (lvwPreviewErrors.Items.ContainsKey(line))
                        {
                            lvwPreviewErrors.Items[line].SubItems[1].Text += " " + error.ErrorText + ".";
                        }
                        else
                        {
                            ListViewItem lvi = lvwPreviewErrors.Items.Add(string.IsNullOrEmpty(line) ? null : line, line, 0);
                            lvi.SubItems.Add(error.ErrorText + ".");
                        }
                    }
                }

                if (BackgroundCompiler.Exception != null)
                {
                    lvwPreviewErrors.Items.Add("").SubItems.Add(BackgroundCompiler.Exception.Message).Tag = BackgroundCompiler.Exception;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Live Preview", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //delegate void AddCanvas(Control control);
        //private void StartSketch()
        //{
        //    ctlCanvas = new Windows.SketchContainer();
        //    ctlCanvas.DesignMode = true;
        //    pnlCanvasContainer.Invoke(new AddCanvas(delegate (Control c) { pnlCanvasContainer.Controls.Add(c); }), new object[] { ctlCanvas });
        //    //pnlCanvasContainer.Controls.Add(ctlCanvas);

        //    _sketch = new Sketch();
        //    _sketch.UseDefaultConsole = false;

        //    Windows.Application.Set(_sketch);
        //    Applet applet = BackgroundCompiler.Output.CreateInstance("App") as Applet;
        //    _sketch.Start(applet, ctlCanvas);
        //}

        private void btnLivePreview_CheckStateChanged(object sender, EventArgs e)
        {
            pnlPreviewPane.Visible = splPreviewPane.Visible = btnLivePreview.Checked;

            ResetPreviewTimer(false);

            if (_sketch != null)
            {
                _sketch.NoLoop();
                _sketch.Exit();
            }

            if (btnLivePreview.Checked)
            {
                ResetPreviewTimer(true);
            }
        }

        internal void CodeChanged(EditorForm editor)
        {
            _previewTimer?.Stop();
            _previewTimer?.Start();
        }

        private void BuildPreview()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += BuildPreviewStart;
            worker.RunWorkerCompleted += BuildPreviewCompleted;
            worker.RunWorkerAsync();
        }

        private void BuildPreviewCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BeginInvoke(new MethodInvoker(UpdateLivePreview));
        }

        private void PreviewTimerElapsed(object sender, EventArgs e)
        {
            _previewTimer.Stop();
            Program.Parser.Parse(!IsLivePreviewEnabled);
            BuildPreview();
        }

        private void ResetPreviewTimer(bool previewEnabled)
        {
            ctlCanvas.Hide();
            lblPreparingPreview.Show();

            if (previewEnabled && _previewTimer == null)
            {
                _previewTimer = new System.Timers.Timer();
                _previewTimer.Interval = 500;
                _previewTimer.Elapsed += PreviewTimerElapsed;
            }

            if (_previewTimer != null)
            {
                _previewTimer.Stop();

                if (_sketch != null)
                {
                    _sketch.NoLoop();
                }
            }

            if (previewEnabled)
            {
                _previewTimer.Start();
            }
        }

        private void BuildPreviewStart(object sender, DoWorkEventArgs e)
        {
            BackgroundCompiler.Compile();
        }

        public void Stop()
        {
            if (_process != null)
            {
                if (!_process.HasExited)
                {
                    _process.Kill();
                    //_process.WaitForExit(1000);
                }

                _process = null;
            }
        }

        private void StartApp(int retries = 0)
        {
            if (_startStatus == null)
            {
                _startStatus = Status.Set("Preparing to launch sketch...");
            }

            try
            {
                CloseLivePreview();
                Stop();
                Program.Parser.Parse(false);

                if (Compiler.Compile())
                {
                    _process = Compiler.LaunchOutput();
                    _process.EnableRaisingEvents = true;
                    _process.Exited += ProcessExited;
                }
                else
                {
                    //if (retries < 3)
                    //{
                    //    StartApp(retries + 1);
                    //    return;
                    //}

                    if (Compiler.CompilerErrors != null && Compiler.CompilerErrors.Count > 0)
                    {
                        //MessageBox.Show(Compiler.CompilerErrors[0].ErrorText, "Compiler Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw new Exception(Compiler.CompilerErrors[0].ErrorText);
                    }
                    else if (Compiler.Exception != null)
                    {
                        //MessageBox.Show(Compiler.Exception.Message, "Compiler Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        throw Compiler.Exception;
                    }
                }
            }
            catch (Exception ex)
            {
                if (retries < 3)
                {
                    StartApp(retries + 1);
                }
                else
                {
                    if (_startStatus != null)
                    {
                        _startStatus.Dispose();
                        _startStatus = null;
                    }

                    MessageBox.Show(ex.Message, "Launch App", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                UpdateToolbarButtons();
            }

            if (_startStatus != null)
            {
                _startStatus.Dispose();
            }
        }

        private void CloseLivePreview()
        {
            btnLivePreview.Checked = false;
            UpdateToolbarButtons();
        }

        private void OpenProject()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "SketchIt Project|*.sip";

                if (dialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                NewProject(false);
                Project project = null;

                using (Status.Set(string.Format("Loading project '{0}...'", dialog.FileName)))
                {
                    using (TextReader reader = new StreamReader(dialog.FileName))
                    {
                        string json = reader.ReadToEnd();
                        project = JsonConvert.DeserializeObject(json, typeof(Project)) as Project;
                        _projectFile = dialog.FileName;
                        _resourceFiles = new List<ProjectFileReference>();
                    }

                    Directory.SetCurrentDirectory(new FileInfo(_projectFile).DirectoryName);

                    foreach (ProjectFileReference reference in project.Files)
                    {
                        EditorForm form = NewSourceFile(true);

                        using (TextReader reader = new StreamReader(reference.Name))
                        {
                            form.FileName = new FileInfo(reference.Name).FullName;
                            form.Editor.Text = reader.ReadToEnd();
                            form.Editor.SetSavePoint();
                            reader.Close();
                        }
                    }

                    if (project.Resources != null)
                    {
                        _resourceFiles = new List<ProjectFileReference>(project.Resources);
                    }

                    Text = "SketchIt - " + project.Name;
                    Directory.SetCurrentDirectory(System.Windows.Forms.Application.StartupPath);

                    ResetPreviewTimer(false);
                }
            }
        }

        private void SaveProject()
        {
            if (string.IsNullOrEmpty(_projectFile))
            {
                using (SaveFileDialog dialog = new SaveFileDialog())
                {
                    dialog.Filter = "SketchIt Project|*.sip";

                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    _projectFile = dialog.FileName;
                }
            }

            using (Status status = Status.Set(string.Format("Saving project '{0}'...", _projectFile)))
            {
                Directory.SetCurrentDirectory(new FileInfo(_projectFile).DirectoryName);
                Project proj = new Project();
                List<ProjectFileReference> files = new List<ProjectFileReference>();
                Uri projectUri = new Uri(_projectFile);
                FileInfo projectFileInfo = new FileInfo(_projectFile);

                proj.Name = Path.GetFileNameWithoutExtension(projectFileInfo.Name);

                foreach (BaseForm form in Application.OpenForms)
                {
                    if (form == null) continue;
                    if (form.Type == WindowType.SourceFile)
                    {
                        status.Hide();

                        if (!((EditorForm)form).Save()) return;
                        ProjectFileReference f = new ProjectFileReference();
                        string filename = ((EditorForm)form).FileName;
                        Uri fileUri = new Uri(new FileInfo(filename).FullName);
                        f.Name = Uri.UnescapeDataString(projectUri.MakeRelativeUri(fileUri).ToString());
                        files.Add(f);

                        status.Show();
                    }
                }

                foreach (ProjectFileReference resource in _resourceFiles)
                {
                    FileInfo fileInfo = new FileInfo(resource.Name);
                    Uri fileUri = new Uri(fileInfo.FullName);

                    if (projectFileInfo.DirectoryName.StartsWith(fileInfo.DirectoryName))
                    {
                        resource.Name = Uri.UnescapeDataString(projectUri.MakeRelativeUri(fileUri).ToString());
                    }
                    else
                    {
                        resource.Name = fileUri.AbsolutePath;
                    }
                }

                proj.Files = files.ToArray();
                proj.Resources = _resourceFiles.ToArray();

                string json = JsonConvert.SerializeObject(proj, Formatting.Indented);

                using (TextWriter writer = new StreamWriter(_projectFile, false))
                {
                    writer.Write(json);
                }

                Text = "SketchIt - " + proj.Name;
                Directory.SetCurrentDirectory(Application.StartupPath);
            }
        }

        internal void SetStatusMessage(StatusAction action)
        {
            _status.Push(action);
            UpdateStatus();
            BusyDialog.PushAction(action);
        }

        internal void RemoveStatusMessage()
        {
            _status.Pop();
            UpdateStatus();
            BusyDialog.PopAction();
        }

        private void UpdateStatus()
        {
            Program.InvokeMethod(this, new MethodInvoker(delegate ()
            {
                StatusAction action = _status.Count == 0 ? null : _status.Peek();
                this.StatusText = action == null ? "Ready" : action.Message;
                Cursor = action == null ? Cursors.Default : action.Cursor;
            }
            ));
        }

        public string StatusText
        {
            get { return tslStatus.Text; }
            set
            {
                tslStatus.Text = value;
                sspMain.Refresh();
            }
        }

        private void lvwPreviewErrors_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (lvwPreviewErrors.SelectedItems.Count > 0)
                {
                    int line = -1;

                    if (int.TryParse(lvwPreviewErrors.SelectedItems[0].Text, out line))
                    {
                        if (GetActiveWindow() is EditorForm form)
                        {
                            form.Editor.Lines[line - 1].Goto();
                            form.Editor.Focus();
                        }
                    }
                    else if (lvwPreviewErrors.SelectedItems[0].SubItems[1].Tag is Exception exception)
                    {
                        MessageBox.Show(exception.StackTrace, "Stack Trace", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            finally
            {
            }
        }

        private void mnuFile_DropDownOpening(object sender, EventArgs e)
        {
            if (GetActiveWindow() is EditorForm form)
            {
                mnuRemoveFromProject.Text = mnuRemoveFromProject.AccessibleDescription.Replace("%filename%", form.Text);
                mnuRemoveFromProject.Enabled = true;
            }
            else
            {
                mnuRemoveFromProject.Text = "Remove from project";
                mnuRemoveFromProject.Enabled = false;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _sketch?.NoLoop();
            _updateTimer.Stop();
            _previewTimer.Stop();
        }

        protected internal override void UpdateAppearance()
        {
            lblPreparingPreview.ForeColor = AppearanceSettings.MenuTextColor;
            base.UpdateAppearance();
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public ProjectFileReference[] Files { get; set; }
        public ProjectFileReference[] Resources { get; set; }
    }

    public class ProjectFileReference
    {
        public string Name { get; set; }
    }

}
