using SketchIt.Api.Internal;
using System;

namespace SketchIt.Api.Static
{
    public static class Functions
    {
        private static Random _random = new Random();
        private static Perlin _perlin = new Perlin();

        public static void ResetNoise()
        {
            _perlin = new Perlin();
        }

        public static float Noise(float x, float y, float z)
        {
            return (float)_perlin.noise(x, y, z);
        }

        public static float Noise(float x, float y)
        {
            return (float)_perlin.noise(x, y, 0);
        }

        public static float Noise(float x)
        {
            return (float)_perlin.noise(x, 0, 0);
        }

        public static float Random()
        {
            return (float)_random.NextDouble();
        }

        public static float Random(float high)
        {
            return (float)_random.NextDouble() * high;
        }

        public static float Random(float low, float high)
        {
            return (float)_random.NextDouble() * (high - low) + low;
        }

        public static float RandomGaussian()
        {
            return Map((float)_random.NextGaussian(), -3, 3, 0, 1);
        }

        public static int Floor(float value)
        {
            return (int)Math.Floor(value);
        }

        public static float GetDistance(float x1, float y1, float x2, float y2)
        {
            float x = x2 - x1;
            float y = y2 - y1;
            return (float)Math.Sqrt(x * x + y * y);
        }

        public static float Cos(float angle)
        {
            return (float)Math.Cos((double)angle);
        }

        public static float Sin(float angle)
        {
            return (float)Math.Sin((double)angle);
        }

        public static float Tan(float angle)
        {
            return (float)Math.Tan((double)angle);
        }

        public static float Sinh(float angle)
        {
            return (float)Math.Sinh((double)angle);
        }

        public static float Tanh(float angle)
        {
            return (float)Math.Tanh((double)angle);
        }

        public static float Map(float number, float start1, float stop1, float start2, float stop2)
        {
            float norm = Normalize(number, start1, stop1);
            return start2 + (norm * (stop2 - start2));
        }

        public static float Normalize(float number, float start, float stop)
        {
            return (number - start) / (stop - start);
        }

        public static float Max(float number1, float number2)
        {
            return Math.Max(number1, number2);
        }

        public static float Min(float number1, float number2)
        {
            return Math.Min(number1, number2);
        }

        public static float Round(float number) => Round(number, 0);
        public static float Round(float number, int decimals)
        {
            return (float)Math.Round(number, decimals);
        }

        public static float Ceiling(float number)
        {
            return (float)Math.Ceiling(number);
        }

        public static float Floot(float number)
        {
            return (float)Math.Floor(number);
        }

        public static Color ColorFromAhsb_(int a, float h, float s, float b)
        {
            //https://blogs.msdn.microsoft.com/cjacks/2006/04/12/converting-from-hsb-to-rgb-in-net/

            if (0 > a || 255 < a)
            {
                throw new ArgumentOutOfRangeException("a", a,
                  "Invalid Alpha value specified");
            }
            if (0f > h || 360f < h)
            {
                throw new ArgumentOutOfRangeException("h", h,
                  "Invalid Hue value specified");
            }
            if (0f > s || 1f < s)
            {
                throw new ArgumentOutOfRangeException("s", s,
                  "Invalid Saturation value specified");
            }
            if (0f > b || 1f < b)
            {
                throw new ArgumentOutOfRangeException("b", b,
                  "Invalid Brightness value specified");
            }

            if (0 == s)
            {
                return new Color(
                    Convert.ToInt32(b * 255),
                    Convert.ToInt32(b * 255),
                    Convert.ToInt32(b * 255),
                    a);
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < b)
            {
                fMax = b - (b * s) + s;
                fMin = b + (b * s) - s;
            }
            else
            {
                fMax = b + (b * s);
                fMin = b - (b * s);
            }

            iSextant = (int)Math.Floor(h / 60f);
            if (300f <= h)
            {
                h -= 360f;
            }
            h /= 60f;
            h -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = h * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - h * (fMax - fMin);
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return new Color(iMid, iMax, iMin, a);
                case 2:
                    return new Color(iMin, iMax, iMid, a);
                case 3:
                    return new Color(iMin, iMid, iMax, a);
                case 4:
                    return new Color(iMid, iMin, iMax, a);
                case 5:
                    return new Color(iMax, iMin, iMid, a);
                default:
                    return new Color(iMax, iMid, iMin, a);
            }
        }

        public static Color ColorFromAhsb(float a, float h, float s, float b)
        {
            return new Color(AHSBtoARGB(a, h, s, b));
        }

        /**
        * Converts the components of a color, as specified by the HSB
        * model, to an equivalent set of values for the default RGB model.
        * <p>
        * The <code>saturation</code> and <code>brightness</code> components
        * should be floating-point values between zero and one
        * (numbers in the range 0.0-1.0).  The <code>hue</code> component
        * can be any floating-point number.  The floor of this number is
        * subtracted from it to create a fraction between 0 and 1.  This
        * fractional number is then multiplied by 360 to produce the hue
        * angle in the HSB color model.
        * <p>
        * The integer that is returned by <code>HSBtoRGB</code> encodes the
        * value of a color in bits 0-23 of an integer value that is the same
        * format used by the method {@link #getRGB() <code>getRGB</code>}.
        * This integer can be supplied as an argument to the
        * <code>Color</code> constructor that takes a single integer argument.
        * @param     hue   the hue component of the color
        * @param     saturation   the saturation of the color
        * @param     brightness   the brightness of the color
        * @return    the RGB value of the color with the indicated hue,
        *                            saturation, and brightness.
        * @see       java.awt.Color#getRGB()
        * @see       java.awt.Color#Color(int)
        * @see       java.awt.image.ColorModel#getRGBdefault()
        * @since     JDK1.0
        */
        public static int AHSBtoARGB(float alpha, float hue, float saturation, float brightness)
        {
            int a = (int)alpha, r = 0, g = 0, b = 0;
            if (saturation == 0)
            {
                r = g = b = (int)(brightness * 255.0f + 0.5f);
            }
            else
            {
                float h = (hue - (float)Math.Floor(hue)) * 6.0f;
                float f = h - (float)Math.Floor(h);
                float p = brightness * (1.0f - saturation);
                float q = brightness * (1.0f - saturation * f);
                float t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(t * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (int)(q * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(brightness * 255.0f + 0.5f);
                        b = (int)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (int)(p * 255.0f + 0.5f);
                        g = (int)(q * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (int)(t * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (int)(brightness * 255.0f + 0.5f);
                        g = (int)(p * 255.0f + 0.5f);
                        b = (int)(q * 255.0f + 0.5f);
                        break;
                }
            }
            //return (int)(0xff000000 | (r << 16) | (g << 8) | (b << 0));
            return (a << 24) | (r << 16) | (g << 8) | (b << 0);
        }
    }
}
