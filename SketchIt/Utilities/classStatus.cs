using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SketchIt.Utilities
{
    public class StatusAction
    {
        public string Message { get; set; }
        public Cursor Cursor { get; set; }
        public bool PreventBusyDialog { get; set; }
        public bool ProgressAvailable { get; set; }
        public float Progress { get; set; }

        public StatusAction(string message, Cursor cursor, bool preventBusyDialog)
        {
            Message = message;
            Cursor = cursor;
            PreventBusyDialog = preventBusyDialog;
        }
    }

    public class Status : IDisposable
    {
        public static Status Set(string message)
        {
            return new Status(message, Cursors.Default, false);
        }

        private string _message;
        private bool _progressAvailable;
        private float _progress;
        private bool _hidden = true;

        public bool PreventBusyDialog { get; private set; }
        public Cursor Cursor { get; private set; }
        public StatusAction Action { get; private set; }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;

                if (Action != null)
                    Action.Message = _message;
            }
        }

        public bool ProgressAvailable
        {
            get { return _progressAvailable; }
            set
            {
                _progressAvailable = value;

                if (Action != null)
                    Action.ProgressAvailable = value;
            }
        }

        public float Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;

                if (Action != null)
                    Action.Progress = value;
            }
        }

        public Status(string message, Cursor cursor, bool preventBusyDialog)
        {
            Message = message;
            Cursor = cursor;
            PreventBusyDialog = preventBusyDialog;
            Action = new StatusAction(message, cursor, preventBusyDialog);
            Show();
        }

        public void Dispose()
        {
            Hide();
        }

        public void Show()
        {
            if (_hidden)
            {
                _hidden = false;
                Program.SetStatusMessage(Action);
            }
        }

        public void Hide()
        {
            if (!_hidden)
            {
                _hidden = true;
                Program.RemoveStatusMessage();
            }
        }
    }
}
