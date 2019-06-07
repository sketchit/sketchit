using SketchIt.Api.Static;
using System;

namespace SketchIt.Api
{
    public struct Color : ICloneable
    {
        public static readonly Color Empty = new Color();

        public Color(Color color, float alpha)
            : this(color.Value1, color.Value2, color.Value3, alpha)
        {
        }

        public Color(float value1, float value2, float value3, float alpha)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Alpha = alpha;
        }

        public Color(int argb)
        {
            System.Drawing.Color color = System.Drawing.Color.FromArgb(argb);

            Value1 = color.R;
            Value2 = color.G;
            Value3 = color.B;
            Alpha = color.A;
        }

        public float Value1 { get; private set; }
        public float Value2 { get; private set; }
        public float Value3 { get; private set; }
        public float Alpha { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return !(Value1 != 0 || Value2 != 0 || Value3 != 0 || Alpha != 0);
            }
        }

        public float[] GetNormalizedValues()
        {
            float[] result = new float[4];

            result[0] = Functions.Map(Value1, 0, 255, 0, 1);
            result[1] = Functions.Map(Value2, 0, 255, 0, 1);
            result[2] = Functions.Map(Value3, 0, 255, 0, 1);
            result[3] = Functions.Map(Alpha, 0, 255, 0, 1);

            return result;
        }

        public Color Clone()
        {
            return new Color(this, Alpha);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public System.Drawing.Color ToSystemColor()
        {
            return System.Drawing.Color.FromArgb((int)Alpha, (int)Value1, (int)Value2, (int)Value3);
        }

        public int ToArgb()
        {
            return ToSystemColor().ToArgb();
        }
    }
}
