using SketchIt.Api.Internal;
using SketchIt.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SketchIt
{
    public partial class BusyDialog : BaseForm
    {
        private static Stack<StatusAction> _actions = new Stack<StatusAction>();
        private static DateTime _startTime = DateTime.Now;

        public BusyDialog()
        {
            InitializeComponent();
        }

        public static void PushAction(StatusAction action)
        {
            using (ThreadLocker.Lock(_actions))
            {
                if (_actions.Count == 0)
                    _startTime = DateTime.Now;

                _actions.Push(action);
            }
        }

        private static void Start()
        {
            Program.BusyDialog = new BusyDialog();
            Program.SplashScreen = new SplashScreenForm();
            Program.SplashScreen.Show();

            Application.Run(Program.BusyDialog);
        }

        internal static void Initialize()
        {
            Thread thread = new Thread(Start);
            thread.TrySetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Font = new Font(Font.FontFamily, Font.Size * .9f);
        }

        private void ShowBusy()
        {
            Program.InvokeMethod(this, new MethodInvoker(
                delegate ()
                {
                    TimeSpan elapsedTime = DateTime.Now.Subtract(_startTime);
                    StatusAction action = _actions.Peek();

                    lblMessage.Text = action.Message;

                    if (action.ProgressAvailable)
                    {
                        pbrProgress.Style = ProgressBarStyle.Continuous;
                        pbrProgress.Value = Convert.ToInt32(action.Progress);
                        lblProgress.Text = action.Progress.ToString("0.00") + "%";
                        lblProgress.Visible = true;
                    }
                    else
                    {
                        pbrProgress.Value = 100;
                        pbrProgress.Style = ProgressBarStyle.Marquee;
                        lblProgress.Text = "";
                        lblProgress.Visible = false;
                    }

                    if (!this.Visible && !action.PreventBusyDialog)
                    {
                        this.Location = Program.MainForm.Location;
                        this.CenterToScreen();
                    }
                    else
                    {
                        if (elapsedTime.TotalSeconds >= 6 /*&& seconds < 15*/)
                        {
                            if (elapsedTime.Hours >= 1)
                                lblElapsedTime.Text = string.Format("{4:0} hour{5}, {0:0} minute{2}, {1:0} second{3}", elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Minutes == 1 ? "" : "s", elapsedTime.Seconds == 1 ? "" : "s", elapsedTime.Hours, elapsedTime.Hours == 1 ? "" : "s");
                            else if (elapsedTime.Minutes >= 1)
                                lblElapsedTime.Text = string.Format("{0:0} minute{2}, {1:0} second{3}", elapsedTime.Minutes, elapsedTime.Seconds, elapsedTime.Minutes == 1 ? "" : "s", elapsedTime.Seconds == 1 ? "" : "s");
                            else
                                lblElapsedTime.Text = string.Format("{0:0} second", elapsedTime.TotalSeconds) + (elapsedTime.TotalSeconds == 1 ? "" : "s");

                            lblElapsedTime.Show();
                        }
                        //else if (seconds >= 15 && seconds < 30)
                        //{
                        //    lblElapsedTime.Text = string.Format("there might be a connection problem - {0:0} seconds", seconds);
                        //}
                        //else if (seconds >= 30)
                        //{
                        //    lblElapsedTime.Text = string.Format("hmmm, this is taking too long - {0:0} seconds", seconds);
                        //}
                    }

                    if (!action.PreventBusyDialog && elapsedTime.TotalSeconds >= 3 && !this.Visible)
                    {
                        this.Show();
                        this.BringToFront();
                        this.TopMost = true;
                        this.TopLevel = true;
                    }
                }
            ));
        }

        private void HideBusy()
        {
            Program.InvokeMethod(this, new MethodInvoker(
                delegate ()
                {
                    this.SendToBack();
                    this.Hide();
                    this.TopLevel = false;
                    this.TopMost = false;
                    lblElapsedTime.Hide();
                    lblElapsedTime.Text = "";
                    pbrProgress.Style = ProgressBarStyle.Blocks;
                    pbrProgress.Style = ProgressBarStyle.Marquee;
                }
            ));
        }

        public static void PopAction()
        {
            using (ThreadLocker.AttemptLock(_actions))
                _actions.Pop();
        }

        private void tmrCheck_Tick(object sender, EventArgs e)
        {
            using (ThreadLocker locker = ThreadLocker.AttemptLock(_actions))
            {
                if (locker.IsLocked && _actions.Count > 0)
                {
                    ShowBusy();
                }
                else
                {
                    HideBusy();
                }
            }
        }
    }
}
