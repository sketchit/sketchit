using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace SketchIt.Utilities
{
    public static class Drawing
    {
        public static Image InvertImage(Image image)
        {
            Image result = new Bitmap(image.Width, image.Height);
            ColorMatrix clrMatrix = new ColorMatrix(new float[][] {
                                                    new float[] { -1, 0, 0, 0, 0},
                                                    new float[] { 0, -1, 0, 0, 0},
                                                    new float[] { 0, 0, -1, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {1, 1, 1, 0, 1}
                            });

            using (Graphics g = Graphics.FromImage(result))
            using (ImageAttributes attr = new ImageAttributes())
            {
                attr.SetColorMatrix(clrMatrix);
                g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, result.Width, result.Height, GraphicsUnit.Pixel, attr);
            }

            return result;
        }

        public static Image GrayscaleImage(Image image)
        {
            Image result = new Bitmap(image.Width, image.Height);
            ColorMatrix clrMatrix = new ColorMatrix(new float[][] {
                                                    new float[] { .5f, .5f, .5f, 0, 0},
                                                    new float[] { .1f, .1f, .1f, 0, 0},
                                                    new float[] { .3f, .3f, .3f, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                            });

            using (Graphics g = Graphics.FromImage(result))
            using (ImageAttributes attr = new ImageAttributes())
            {
                attr.SetColorMatrix(clrMatrix);
                g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, result.Width, result.Height, GraphicsUnit.Pixel, attr);
            }

            return result;
        }

        //public static HSB RGBtoHSB(int red, int green, int blue)
        //{
        //    // normalize red, green and blue values
        //    double r = ((double)red / 255.0);
        //    double g = ((double)green / 255.0);
        //    double b = ((double)blue / 255.0);

        //    // conversion start
        //    double max = Math.Max(r, Math.Max(g, b));
        //    double min = Math.Min(r, Math.Min(g, b));

        //    double h = 0.0;
        //    if (max == r && g >= b)
        //    {
        //        h = 60 * (g - b) / (max - min);
        //    }
        //    else if (max == r && g < b)
        //    {
        //        h = 60 * (g - b) / (max - min) + 360;
        //    }
        //    else if (max == g)
        //    {
        //        h = 60 * (b - r) / (max - min) + 120;
        //    }
        //    else if (max == b)
        //    {
        //        h = 60 * (r - g) / (max - min) + 240;
        //    }

        //    double s = (max == 0) ? 0.0 : (1.0 - (min / max));

        //    return new HSB(h, s, (double)max);
        //}
    }
}
