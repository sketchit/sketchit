using SketchIt.Api;
using SketchIt.Api.Static;
using SketchIt.Utilities;
using System.Windows.Forms;
using static SketchIt.Api.Static.Functions;
using Drawing = System.Drawing;

namespace SketchIt
{
    public partial class SplashScreenForm : BaseForm
    {
        private SplashAnimation _animation;

        public SplashScreenForm()
        {
            InitializeComponent();
        }

        private void SplashScreenForm_Load(object sender, System.EventArgs e)
        {
            _animation = new SplashAnimation();
            _animation.Start(ctlCanvas);
            Size = new Drawing.Size(_animation.Width, _animation.Height);
            CenterToScreen();
            ActiveControl = ctlCanvas;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Drawing.Pen pen = new Drawing.Pen(AppearanceSettings.ActiveCaptionBackColor, 4))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, Width, Height);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                return;
            }

            base.OnKeyDown(e);
        }

        private void ctlCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            Close();
        }

        private new void Close()
        {
            //Program.MainForm.Activate();
            base.Close();
        }
    }

    public class SplashAnimation : Sketch
    {
        class Dot
        {
            Vector pos = new Vector(Random(minx, maxx), Random(miny, maxy));
            Vector dest = new Vector(Random(minx, maxx), Random(miny, maxy));
            int sleepMax = 1;//(int)Random(50, 250);
            int sleepCount = 0;

            public float X => pos.X;
            public float Y => pos.Y;

            public Dot()
            {
            }

            public void Update()
            {
                Vector diff = dest - pos;

                if (diff.GetMagnitude() == 0)
                {
                    sleepCount++;

                    if (sleepCount == sleepMax)
                    {
                        sleepCount = 0;
                        dest = new Vector(Random(minx, maxx), Random(miny, maxy));
                    }
                }
                else
                {
                    diff.Limit(2);
                    pos += diff;
                }
            }
        }

        Canvas bg;
        Dot[] points = new Dot[4];
        float maxdist;
        static float minx;
        static float maxx;
        static float miny;
        static float maxy;

        void Setup()
        {
            SetSize(600, 400);
            bg = new Canvas(this, Width, Height);

            minx = -100;
            maxx = Width + 100;
            miny = -100;
            maxy = Height * .75f;
            maxdist = GetDistance(minx, miny, maxx, maxy) * .7f;

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Dot();
            }

            ClearBackground();
            bg.SetColorMode(Constants.HSB);
            bg.SetStrokeWeight(1);
        }

        float a = 10f;
        float textColor = 0f;

        void ClearBackground()
        {
            bg.DrawBackground(GetColor(ControlPaint.Dark(AppearanceSettings.BaseBackColor, .9f)));
        }

        void Draw()
        {
            if (IsKeyPressed && KeyCode == 32)
            {
                ClearBackground();
                a = 10;
            }

            if (textColor < 255f)
            {
                textColor += 5f;
            }

            if (a == 0)
            {
                NoLoop();
                return;
            }

            if (FrameCount % 60 == 0)
            {
                if (a > 0)
                {
                    a -= .5f;
                    if (a == 0)
                    {
                        NoLoop();
                        return;
                    }
                }
            }

            for (int i = 0; i < points.Length - 1; i++)
            {
                Dot pt1 = points[i];

                for (int j = i + 1; j < points.Length; j++)
                {
                    Dot pt2 = points[j];

                    if (!pt1.Equals(pt2))
                    {
                        float distance = Map(GetDistance(pt1.X, pt1.Y, pt2.X, pt2.Y), 0f, maxdist, 0f, 255f);
                        bg.SetStroke(distance, 255f, 255f, a);
                        bg.DrawLine(pt1.X, pt1.Y, pt2.X, pt2.Y);
                    }
                }
            }

            foreach (Dot pt in points)
            {
                pt.Update();
            }

            DrawBackground(bg);

            SetTextAlignment(Constants.LEFT, Constants.TOP);
            SetStroke(GetColor(AppearanceSettings.ApplicationBackColor));
            SetFont("Segoe UI Light", 40, false, false);
            //DrawText("SketchIt", 7, 12, Width - 10, Height - 20);
            SetFont("Segoe UI Light", 10, true, false);
            //DrawText("An open source project created to have fun while learning to code, or simply to sketch together a visual idea using code.", 11, 81, Width - 20, Height - 160);
            SetStroke(GetColor(ControlPaint.Light(AppearanceSettings.ApplicationTextColor, .9f)));
            SetFont("Segoe UI Light", 40, false, false);
            DrawText("SketchIt", 5, 10, Width - 10, Height - 20);
            SetFont("Segoe UI Light", 10, true, false);
            DrawText("An open source project created to have fun while learning to code, or simply to sketch together a visual idea using code.", 10, 80, Width - 20, Height - 160);

            SetFont("Segoe UI Light", 9, true, false);
            SetTextAlignment(Constants.LEFT, Constants.BOTTOM);
            DrawText("Version " + ProductVersion + "\r\nCopyright © Ian Kloppers\r\nwww.sketchit.org", 10, 10, Width - 20, Height - 20);

            SetTextAlignment(Constants.RIGHT, Constants.BOTTOM);
            DrawText(string.Format("{2}\r\n{0}\r\n{1:0.00}", FrameCount, FrameRate, IsLooping ? "" : "Press ESC to close\r\nPress SPACEBAR for new animation"), 10, 10, Width - 20, Height - 20);
        }

        void KeyPress()
        {
            if (!IsLooping)
            {
                ClearBackground();
                a = 10;
                Loop();
            }
        }
    }
}
