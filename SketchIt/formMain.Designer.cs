namespace SketchIt
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNewSketch = new System.Windows.Forms.ToolStripMenuItem();
            this.newSourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRemoveFromProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.sketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRun = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.tabWindows = new SketchIt.Controls.TabButtonsControl();
            this.tlsMain = new System.Windows.Forms.ToolStrip();
            this.btnNewProject = new System.Windows.Forms.ToolStripButton();
            this.btnOpenSketch = new System.Windows.Forms.ToolStripButton();
            this.btnAddExistingFile = new System.Windows.Forms.ToolStripButton();
            this.btnAddNewSourceFile = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAll = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.btnStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnErrorList = new System.Windows.Forms.ToolStripButton();
            this.btnLivePreview = new System.Windows.Forms.ToolStripButton();
            this.pnlPreviewPane = new System.Windows.Forms.SplitContainer();
            this.pnlCanvasContainer = new System.Windows.Forms.Panel();
            this.ctlCanvas = new SketchIt.Windows.SketchContainer();
            this.lblPreparingPreview = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPausePreview = new System.Windows.Forms.ToolStripButton();
            this.tslFrameRate = new System.Windows.Forms.ToolStripLabel();
            this.btnRestartPreview = new System.Windows.Forms.ToolStripButton();
            this.txtConsoleOutput = new System.Windows.Forms.TextBox();
            this.lvwPreviewErrors = new SketchIt.Controls.ListViewEx();
            this.lvcLineIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvcMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splPreviewPane = new System.Windows.Forms.Splitter();
            this.pnlErrorList = new System.Windows.Forms.Panel();
            this.splErrorList = new System.Windows.Forms.Splitter();
            this.sspMain = new System.Windows.Forms.StatusStrip();
            this.tslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslParseTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWebsite = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnsMain.SuspendLayout();
            this.tlsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPreviewPane)).BeginInit();
            this.pnlPreviewPane.Panel1.SuspendLayout();
            this.pnlPreviewPane.Panel2.SuspendLayout();
            this.pnlPreviewPane.SuspendLayout();
            this.pnlCanvasContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlErrorList.SuspendLayout();
            this.sspMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnsMain
            // 
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.sketchToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Size = new System.Drawing.Size(994, 24);
            this.mnsMain.TabIndex = 1;
            this.mnsMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNewSketch,
            this.newSourceFileToolStripMenuItem,
            this.toolStripSeparator3,
            this.mnuRemoveFromProject,
            this.toolStripSeparator4,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            this.mnuFile.DropDownOpening += new System.EventHandler(this.mnuFile_DropDownOpening);
            // 
            // mnuNewSketch
            // 
            this.mnuNewSketch.Name = "mnuNewSketch";
            this.mnuNewSketch.Size = new System.Drawing.Size(255, 22);
            this.mnuNewSketch.Tag = "new-project";
            this.mnuNewSketch.Text = "&New Project...";
            this.mnuNewSketch.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // newSourceFileToolStripMenuItem
            // 
            this.newSourceFileToolStripMenuItem.Name = "newSourceFileToolStripMenuItem";
            this.newSourceFileToolStripMenuItem.Size = new System.Drawing.Size(255, 22);
            this.newSourceFileToolStripMenuItem.Tag = "add-new-source-file";
            this.newSourceFileToolStripMenuItem.Text = "New Source File";
            this.newSourceFileToolStripMenuItem.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(252, 6);
            // 
            // mnuRemoveFromProject
            // 
            this.mnuRemoveFromProject.AccessibleDescription = "Remove \'%filename%\' from Project";
            this.mnuRemoveFromProject.Name = "mnuRemoveFromProject";
            this.mnuRemoveFromProject.Size = new System.Drawing.Size(255, 22);
            this.mnuRemoveFromProject.Tag = "remove-from-project";
            this.mnuRemoveFromProject.Text = "Remove %filename% from Project";
            this.mnuRemoveFromProject.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(252, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(255, 22);
            this.mnuExit.Tag = "exit";
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // sketchToolStripMenuItem
            // 
            this.sketchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRun,
            this.mnuStop});
            this.sketchToolStripMenuItem.Name = "sketchToolStripMenuItem";
            this.sketchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.sketchToolStripMenuItem.Text = "&Sketch";
            // 
            // mnuRun
            // 
            this.mnuRun.Name = "mnuRun";
            this.mnuRun.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuRun.Size = new System.Drawing.Size(114, 22);
            this.mnuRun.Tag = "start";
            this.mnuRun.Text = "&Run";
            this.mnuRun.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // mnuStop
            // 
            this.mnuStop.Enabled = false;
            this.mnuStop.Name = "mnuStop";
            this.mnuStop.Size = new System.Drawing.Size(114, 22);
            this.mnuStop.Tag = "stop";
            this.mnuStop.Text = "Stop";
            this.mnuStop.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // tabWindows
            // 
            this.tabWindows.AutoSize = true;
            this.tabWindows.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabWindows.Location = new System.Drawing.Point(0, 51);
            this.tabWindows.Name = "tabWindows";
            this.tabWindows.Padding = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.tabWindows.Size = new System.Drawing.Size(496, 2);
            this.tabWindows.TabIndex = 3;
            this.tabWindows.TabButtonClick += new SketchIt.Controls.TabButtonsControl.TabButtonEventHandler(this.TabWindows_TabButtonClick);
            // 
            // tlsMain
            // 
            this.tlsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tlsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewProject,
            this.btnOpenSketch,
            this.btnAddExistingFile,
            this.btnAddNewSourceFile,
            this.btnSaveAll,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnRun,
            this.btnStop,
            this.toolStripSeparator2,
            this.btnErrorList,
            this.btnLivePreview});
            this.tlsMain.Location = new System.Drawing.Point(0, 24);
            this.tlsMain.Name = "tlsMain";
            this.tlsMain.Size = new System.Drawing.Size(994, 27);
            this.tlsMain.TabIndex = 5;
            this.tlsMain.Text = "toolStrip1";
            // 
            // btnNewProject
            // 
            this.btnNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNewProject.Image = ((System.Drawing.Image)(resources.GetObject("btnNewProject.Image")));
            this.btnNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(24, 24);
            this.btnNewProject.Tag = "new-project";
            this.btnNewProject.Text = "New Project";
            this.btnNewProject.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // btnOpenSketch
            // 
            this.btnOpenSketch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenSketch.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenSketch.Image")));
            this.btnOpenSketch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenSketch.Name = "btnOpenSketch";
            this.btnOpenSketch.Size = new System.Drawing.Size(24, 24);
            this.btnOpenSketch.Tag = "open-project";
            this.btnOpenSketch.Text = "Open";
            this.btnOpenSketch.ToolTipText = "Open Project";
            this.btnOpenSketch.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // btnAddExistingFile
            // 
            this.btnAddExistingFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddExistingFile.Image = ((System.Drawing.Image)(resources.GetObject("btnAddExistingFile.Image")));
            this.btnAddExistingFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddExistingFile.Name = "btnAddExistingFile";
            this.btnAddExistingFile.Size = new System.Drawing.Size(24, 24);
            this.btnAddExistingFile.Tag = "add-existing-file";
            this.btnAddExistingFile.Text = "New";
            this.btnAddExistingFile.ToolTipText = "Add Existing File";
            this.btnAddExistingFile.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // btnAddNewSourceFile
            // 
            this.btnAddNewSourceFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddNewSourceFile.Image = ((System.Drawing.Image)(resources.GetObject("btnAddNewSourceFile.Image")));
            this.btnAddNewSourceFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddNewSourceFile.Name = "btnAddNewSourceFile";
            this.btnAddNewSourceFile.Size = new System.Drawing.Size(24, 24);
            this.btnAddNewSourceFile.Tag = "add-new-source-file";
            this.btnAddNewSourceFile.Text = "Add";
            this.btnAddNewSourceFile.ToolTipText = "Add New Source File";
            this.btnAddNewSourceFile.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAll.Image")));
            this.btnSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(24, 24);
            this.btnSaveAll.Tag = "save-all";
            this.btnSaveAll.Text = "Save All";
            this.btnSaveAll.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(24, 24);
            this.btnSave.Tag = "save";
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnRun
            // 
            this.btnRun.Enabled = false;
            this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(52, 24);
            this.btnRun.Tag = "start";
            this.btnRun.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // btnStop
            // 
            this.btnStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStop.Enabled = false;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(24, 24);
            this.btnStop.Tag = "stop";
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // btnErrorList
            // 
            this.btnErrorList.Checked = true;
            this.btnErrorList.CheckOnClick = true;
            this.btnErrorList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnErrorList.Image = ((System.Drawing.Image)(resources.GetObject("btnErrorList.Image")));
            this.btnErrorList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnErrorList.Name = "btnErrorList";
            this.btnErrorList.Size = new System.Drawing.Size(77, 24);
            this.btnErrorList.Tag = "toggle-error-list";
            this.btnErrorList.Text = "Error List";
            this.btnErrorList.ToolTipText = "Toggle Error List";
            this.btnErrorList.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // btnLivePreview
            // 
            this.btnLivePreview.CheckOnClick = true;
            this.btnLivePreview.Image = ((System.Drawing.Image)(resources.GetObject("btnLivePreview.Image")));
            this.btnLivePreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLivePreview.Name = "btnLivePreview";
            this.btnLivePreview.Size = new System.Drawing.Size(96, 24);
            this.btnLivePreview.Text = "Live Preview";
            this.btnLivePreview.ToolTipText = "Live Preview";
            this.btnLivePreview.CheckStateChanged += new System.EventHandler(this.btnLivePreview_CheckStateChanged);
            // 
            // pnlPreviewPane
            // 
            this.pnlPreviewPane.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPreviewPane.Location = new System.Drawing.Point(500, 51);
            this.pnlPreviewPane.Name = "pnlPreviewPane";
            this.pnlPreviewPane.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pnlPreviewPane.Panel1
            // 
            this.pnlPreviewPane.Panel1.Controls.Add(this.pnlCanvasContainer);
            this.pnlPreviewPane.Panel1.Controls.Add(this.toolStrip1);
            // 
            // pnlPreviewPane.Panel2
            // 
            this.pnlPreviewPane.Panel2.Controls.Add(this.txtConsoleOutput);
            this.pnlPreviewPane.Size = new System.Drawing.Size(494, 562);
            this.pnlPreviewPane.SplitterDistance = 404;
            this.pnlPreviewPane.TabIndex = 7;
            this.pnlPreviewPane.Visible = false;
            // 
            // pnlCanvasContainer
            // 
            this.pnlCanvasContainer.Controls.Add(this.ctlCanvas);
            this.pnlCanvasContainer.Controls.Add(this.lblPreparingPreview);
            this.pnlCanvasContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvasContainer.Location = new System.Drawing.Point(0, 27);
            this.pnlCanvasContainer.Name = "pnlCanvasContainer";
            this.pnlCanvasContainer.Size = new System.Drawing.Size(494, 377);
            this.pnlCanvasContainer.TabIndex = 5;
            this.pnlCanvasContainer.SizeChanged += new System.EventHandler(this.ctlCanvas_SizeChanged);
            // 
            // ctlCanvas
            // 
            this.ctlCanvas.BackColor = System.Drawing.Color.Black;
            this.ctlCanvas.DesignMode = false;
            this.ctlCanvas.Location = new System.Drawing.Point(137, 85);
            this.ctlCanvas.Name = "ctlCanvas";
            this.ctlCanvas.Size = new System.Drawing.Size(200, 200);
            this.ctlCanvas.Sketch = null;
            this.ctlCanvas.TabIndex = 3;
            this.ctlCanvas.Text = "canvas1";
            this.ctlCanvas.Visible = false;
            // 
            // lblPreparingPreview
            // 
            this.lblPreparingPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPreparingPreview.Location = new System.Drawing.Point(0, 0);
            this.lblPreparingPreview.Name = "lblPreparingPreview";
            this.lblPreparingPreview.Size = new System.Drawing.Size(494, 377);
            this.lblPreparingPreview.TabIndex = 4;
            this.lblPreparingPreview.Text = "Preparing Live Preview...";
            this.lblPreparingPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPausePreview,
            this.tslFrameRate,
            this.btnRestartPreview});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(494, 27);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPausePreview
            // 
            this.btnPausePreview.Image = ((System.Drawing.Image)(resources.GetObject("btnPausePreview.Image")));
            this.btnPausePreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPausePreview.Name = "btnPausePreview";
            this.btnPausePreview.Size = new System.Drawing.Size(62, 24);
            this.btnPausePreview.Tag = "pause-preview";
            this.btnPausePreview.Text = "Pause";
            this.btnPausePreview.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // tslFrameRate
            // 
            this.tslFrameRate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslFrameRate.Name = "tslFrameRate";
            this.tslFrameRate.Size = new System.Drawing.Size(0, 24);
            // 
            // btnRestartPreview
            // 
            this.btnRestartPreview.Image = global::SketchIt.Properties.Resources.Restart;
            this.btnRestartPreview.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRestartPreview.Name = "btnRestartPreview";
            this.btnRestartPreview.Size = new System.Drawing.Size(67, 24);
            this.btnRestartPreview.Tag = "restart-preview";
            this.btnRestartPreview.Text = "Restart";
            this.btnRestartPreview.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // txtConsoleOutput
            // 
            this.txtConsoleOutput.BackColor = System.Drawing.Color.Black;
            this.txtConsoleOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtConsoleOutput.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConsoleOutput.ForeColor = System.Drawing.Color.White;
            this.txtConsoleOutput.Location = new System.Drawing.Point(0, 0);
            this.txtConsoleOutput.Multiline = true;
            this.txtConsoleOutput.Name = "txtConsoleOutput";
            this.txtConsoleOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConsoleOutput.Size = new System.Drawing.Size(494, 154);
            this.txtConsoleOutput.TabIndex = 0;
            // 
            // lvwPreviewErrors
            // 
            this.lvwPreviewErrors.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvwPreviewErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcLineIndex,
            this.lvcMessage});
            this.lvwPreviewErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwPreviewErrors.FullRowSelect = true;
            this.lvwPreviewErrors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwPreviewErrors.Location = new System.Drawing.Point(0, 0);
            this.lvwPreviewErrors.Name = "lvwPreviewErrors";
            this.lvwPreviewErrors.OwnerDraw = true;
            this.lvwPreviewErrors.Size = new System.Drawing.Size(496, 100);
            this.lvwPreviewErrors.TabIndex = 0;
            this.lvwPreviewErrors.UseCompatibleStateImageBehavior = false;
            this.lvwPreviewErrors.View = System.Windows.Forms.View.Details;
            this.lvwPreviewErrors.DoubleClick += new System.EventHandler(this.lvwPreviewErrors_DoubleClick);
            // 
            // lvcLineIndex
            // 
            this.lvcLineIndex.Text = "Line";
            // 
            // lvcMessage
            // 
            this.lvcMessage.Text = "Message";
            this.lvcMessage.Width = 436;
            // 
            // splPreviewPane
            // 
            this.splPreviewPane.Dock = System.Windows.Forms.DockStyle.Right;
            this.splPreviewPane.Location = new System.Drawing.Point(496, 51);
            this.splPreviewPane.Name = "splPreviewPane";
            this.splPreviewPane.Size = new System.Drawing.Size(4, 562);
            this.splPreviewPane.TabIndex = 8;
            this.splPreviewPane.TabStop = false;
            this.splPreviewPane.Visible = false;
            // 
            // pnlErrorList
            // 
            this.pnlErrorList.Controls.Add(this.lvwPreviewErrors);
            this.pnlErrorList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlErrorList.Location = new System.Drawing.Point(0, 513);
            this.pnlErrorList.Name = "pnlErrorList";
            this.pnlErrorList.Size = new System.Drawing.Size(496, 100);
            this.pnlErrorList.TabIndex = 10;
            // 
            // splErrorList
            // 
            this.splErrorList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splErrorList.Location = new System.Drawing.Point(0, 509);
            this.splErrorList.Name = "splErrorList";
            this.splErrorList.Size = new System.Drawing.Size(496, 4);
            this.splErrorList.TabIndex = 11;
            this.splErrorList.TabStop = false;
            // 
            // sspMain
            // 
            this.sspMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslStatus,
            this.tslParseTime});
            this.sspMain.Location = new System.Drawing.Point(0, 613);
            this.sspMain.Name = "sspMain";
            this.sspMain.Size = new System.Drawing.Size(994, 23);
            this.sspMain.SizingGrip = false;
            this.sspMain.TabIndex = 13;
            // 
            // tslStatus
            // 
            this.tslStatus.Image = ((System.Drawing.Image)(resources.GetObject("tslStatus.Image")));
            this.tslStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tslStatus.Name = "tslStatus";
            this.tslStatus.Padding = new System.Windows.Forms.Padding(1);
            this.tslStatus.Size = new System.Drawing.Size(979, 18);
            this.tslStatus.Spring = true;
            this.tslStatus.Text = "Ready";
            this.tslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tslParseTime
            // 
            this.tslParseTime.Name = "tslParseTime";
            this.tslParseTime.Size = new System.Drawing.Size(0, 18);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelp,
            this.mnuWebsite,
            this.toolStripSeparator5,
            this.mnuAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // mnuHelp
            // 
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(180, 22);
            this.mnuHelp.Tag = "help";
            this.mnuHelp.Text = "View Help";
            this.mnuHelp.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // mnuWebsite
            // 
            this.mnuWebsite.Name = "mnuWebsite";
            this.mnuWebsite.Size = new System.Drawing.Size(180, 22);
            this.mnuWebsite.Tag = "website";
            this.mnuWebsite.Text = "Visit website...";
            this.mnuWebsite.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(180, 22);
            this.mnuAbout.Tag = "about";
            this.mnuAbout.Text = "About SketchIt";
            this.mnuAbout.Click += new System.EventHandler(this.MenuItemClicked);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 636);
            this.Controls.Add(this.splErrorList);
            this.Controls.Add(this.pnlErrorList);
            this.Controls.Add(this.tabWindows);
            this.Controls.Add(this.splPreviewPane);
            this.Controls.Add(this.pnlPreviewPane);
            this.Controls.Add(this.tlsMain);
            this.Controls.Add(this.mnsMain);
            this.Controls.Add(this.sspMain);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mnsMain;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SketchIt";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.tlsMain.ResumeLayout(false);
            this.tlsMain.PerformLayout();
            this.pnlPreviewPane.Panel1.ResumeLayout(false);
            this.pnlPreviewPane.Panel1.PerformLayout();
            this.pnlPreviewPane.Panel2.ResumeLayout(false);
            this.pnlPreviewPane.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlPreviewPane)).EndInit();
            this.pnlPreviewPane.ResumeLayout(false);
            this.pnlCanvasContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlErrorList.ResumeLayout(false);
            this.sspMain.ResumeLayout(false);
            this.sspMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuNewSketch;
        private Controls.TabButtonsControl tabWindows;
        private System.Windows.Forms.ToolStrip tlsMain;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.Windows.Forms.ToolStripButton btnAddExistingFile;
        private System.Windows.Forms.ToolStripButton btnOpenSketch;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnStop;
        private System.Windows.Forms.ToolStripMenuItem newSourceFileToolStripMenuItem;
        private System.Windows.Forms.SplitContainer pnlPreviewPane;
        private Windows.SketchContainer ctlCanvas;
        private Controls.ListViewEx lvwPreviewErrors;
        private System.Windows.Forms.ColumnHeader lvcLineIndex;
        private System.Windows.Forms.ColumnHeader lvcMessage;
        private System.Windows.Forms.Splitter splPreviewPane;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnLivePreview;
        private System.Windows.Forms.ToolStripButton btnAddNewSourceFile;
        private System.Windows.Forms.ToolStripButton btnSaveAll;
        private System.Windows.Forms.ToolStripMenuItem sketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuRun;
        private System.Windows.Forms.Panel pnlErrorList;
        private System.Windows.Forms.TextBox txtConsoleOutput;
        private System.Windows.Forms.Splitter splErrorList;
        private System.Windows.Forms.StatusStrip sspMain;
        private System.Windows.Forms.ToolStripStatusLabel tslStatus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuStop;
        private System.Windows.Forms.ToolStripButton btnNewProject;
        private System.Windows.Forms.ToolStripButton btnErrorList;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveFromProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Panel pnlCanvasContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPausePreview;
        private System.Windows.Forms.ToolStripLabel tslFrameRate;
        private System.Windows.Forms.Label lblPreparingPreview;
        private System.Windows.Forms.ToolStripStatusLabel tslParseTime;
        private System.Windows.Forms.ToolStripButton btnRestartPreview;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuWebsite;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
    }
}