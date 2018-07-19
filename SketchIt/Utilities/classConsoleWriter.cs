using SketchIt.Api.Internal;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SketchIt.Utilities
{
    public class ConsoleWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.ASCII;
        delegate void AppendTextHandler(string text);

        private Queue<string> _buffer = new Queue<string>();

        public bool Buffered
        {
            get;
            private set;
        }

        public TextBoxBase OutputWindow
        {
            get;
            private set;
        }

        public ConsoleWriter(TextBoxBase output, bool buffered)
        {
            OutputWindow = output;
            Buffered = buffered;
        }

        public override void Write(string value)
        {
            AppendText(value);
        }

        public override void WriteLine(string value)
        {
            AppendText(value);
            AppendText(NewLine);
        }

        public override void WriteLine()
        {
            AppendText(NewLine);
        }

        public override void WriteLine(string format, object arg0)
        {
            AppendText(string.Format(format, arg0));
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            AppendText(string.Format(format, arg0, arg1));
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            AppendText(string.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine(string format, params object[] arg)
        {
            AppendText(string.Format(format, arg));
        }

        private void AppendText(string value)
        {
            if (Buffered)
            {
                //using (ThreadLocker locker = ThreadLocker.Lock(_buffer, -1))
                {
                    _buffer.Enqueue(value);
                }
            }
            else
            {
                OutputText(value);
            }
        }

        private void OutputText(string text)
        {
            OutputWindow.Invoke(new AppendTextHandler(OutputWindow.AppendText), new object[] { text });
        }

        public override void Flush()
        {
            if (Buffered)
            {
                //using (ThreadLocker locker = ThreadLocker.Lock(_buffer, 5))
                {
                    //if (locker.IsLocked)
                    {
                        StringBuilder builder = new StringBuilder();

                        while (_buffer.Count > 0)
                        {
                            builder.Append(_buffer.Dequeue());

                            if (builder.Length > 100000)
                            {
                                OutputText(builder.ToString());
                            }
                        }

                        if (builder.Length > 0)
                        {
                            OutputText(builder.ToString());
                        }

                        base.Flush();
                    }
                }
            }
            else
            {
                base.Flush();
            }
        }

        public void Clear()
        {
            if (Buffered)
            {
                //using (ThreadLocker locker = ThreadLocker.Lock(_buffer, 10))
                {
                    //if (locker.IsLocked)
                    {
                        _buffer.Clear();
                    }
                    //else
                    //{
                    //    _buffer = new Queue<string>();
                    //}
                }
            }
        }
    }
}