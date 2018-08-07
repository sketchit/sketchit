namespace SketchIt.Api
{
    public class Rectangle
    {
        private float _x;
        private float _y;
        private float _width;
        private float _height;
        private System.Drawing.Rectangle _systemRectangle;
        private System.Drawing.RectangleF _systemRectangleF;

        public Rectangle()
            : this(0, 0, 0, 0)
        {
        }

        public Rectangle(float x, float y, float width, float height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
        }

        public float X
        {
            get => _x;
            set
            {
                _x = value;
                ResetCache();
            }
        }

        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                ResetCache();
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                ResetCache();
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                ResetCache();
            }
        }

        public float Top
        {
            get => _x;
        }

        public float Left
        {
            get => _y;
        }

        public float Right
        {
            get => _x + _width;
        }

        public float Bottom
        {
            get => _y + _height;
        }

        private void ResetCache()
        {
            _systemRectangle = System.Drawing.Rectangle.Empty;
            _systemRectangleF = System.Drawing.RectangleF.Empty;
        }

        public System.Drawing.Rectangle SystemRectangle
        {
            get
            {
                if (_systemRectangle.IsEmpty)
                {
                    _systemRectangle = new System.Drawing.Rectangle((int)_x, (int)_y, (int)_width, (int)_height);
                }

                return _systemRectangle;
            }
        }

        public System.Drawing.RectangleF SystemRectangleF
        {
            get
            {
                if (_systemRectangleF.IsEmpty)
                {
                    _systemRectangleF = new System.Drawing.RectangleF(_x, _y, _width, _height);
                }

                return _systemRectangleF;
            }
        }
    }
}
