//https://stackoverflow.com/questions/24701703/c-sharp-faster-alternatives-to-setpixel-and-getpixel-for-bitmaps-for-windows-f

using SketchIt.Api.Interfaces;
using Svg;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SketchIt.Api
{
    public class Image : IDisposable, ICloneable, IImage
    {
        public Bitmap Bitmap { get; private set; }
        public int[] Pixels { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public static Image FromSource(string source) => FromSource(source, null, null);
        public static Image FromSource(string source, int? width, int? height)
        {
            try
            {
                Uri uri = new Uri(source);
                string localPath = "";

                switch (uri.HostNameType)
                {
                    case UriHostNameType.Basic:
                        localPath = uri.LocalPath;
                        break;

                    default:
                        WebClient web = new WebClient();
                        localPath = Path.GetTempFileName();
                        web.DownloadFile(uri, localPath);
                        break;
                }

                if (File.Exists(localPath))
                {
                    Bitmap image;

                    switch (Path.GetExtension(localPath).ToLower())
                    {
                        case ".svg":
                            SvgDocument svg = SvgDocument.Open(localPath);
                            image = svg.Draw(width ?? (int)svg.Width.Value, height ?? (int)svg.Height.Value);

                            //using (TextReader tr = new StreamReader(localPath))
                            //{
                            //    var t = new NGraphics.SvgReader(tr);
                            //    var i = NGraphics.Platforms.Current.CreateImageCanvas(t.Graphic.Size);
                            //    t.Graphic.Draw(i);

                            //    using (MemoryStream ms = new MemoryStream())
                            //    {
                            //        i.GetImage().SaveAsPng(ms);
                            //        image = new Bitmap(Bitmap.FromStream(ms));
                            //    }
                            //}
                            break;

                        default:
                            image = System.Drawing.Image.FromFile(localPath) as Bitmap;
                            break;
                    }

                    if (image != null)
                    {
                        Image result = new Image(width ?? image.Width, height ?? image.Height);

                        using (Graphics g = Graphics.FromImage(result.Bitmap))
                        //if (result.Width != image.Width || result.Height != image.Height)
                        {
                            g.DrawImage(image, 0, 0, result.Width, result.Height);
                        }
                        //else
                        //{
                        //    g.DrawImageUnscaled(image, 0, 0);
                        //}

                        image.Dispose();

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public static Image FromSource(IImage image)
        {
            if (image == null)
            {
                return null;
            }
            else
            {
                Image result = new Image(image.Width, image.Height);

                using (Graphics g = Graphics.FromImage(result.Bitmap))
                {
                    g.DrawImageUnscaled(image.Bitmap, 0, 0);
                }

                return result;
            }
        }

        public object Clone() { return GetImage(); }
        public Image GetImage()
        {
            Image result = new Image(Width, Height);

            using (Graphics g = Graphics.FromImage(result.Bitmap))
            {
                g.DrawImageUnscaled(Bitmap, 0, 0);
            }

            return result;
        }

        public Image GetCroppedImage(int x, int y, int width, int height)
        {
            Image result = new Image(width, height);

            using (Graphics g = Graphics.FromImage(result.Bitmap))
            {
                g.DrawImage(Bitmap, new System.Drawing.Rectangle(0, 0, result.Width, result.Height), x, y, width, height, GraphicsUnit.Pixel);
            }

            return result;
        }

        public void Resize(int width, int height, int[] pixels)
        {
            Dispose();

            Width = width;
            Height = height;
            Pixels = pixels ?? new int[width * height];
            BitsHandle = GCHandle.Alloc(Pixels, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
            Disposed = false;
        }

        public Image(int width, int height)
            : this(width, height, null)
        {
        }

        public Image(int width, int height, int[] pixels)
        {
            Resize(width, height, pixels);
        }

        internal void ClearPixles()
        {
            Array.Clear(Pixels, 0, Pixels.Length);
        }

        public void SetPixelColor(int x, int y, Color color) => SetPixel(x, y, color.ToArgb());
        public void SetPixel(int x, int y, int argb)
        {
            int index = x + (y * Width);
            Pixels[index] = argb;
        }

        public int GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            return Pixels[index];
        }

        public Color GetPixelColor(int x, int y)
        {
            return new Color(GetPixel(x, y));
        }

        public void LoadPixels()
        {
            BitsHandle.Free();
            Bitmap.Dispose();
            //return (int[])Pixels.Clone();
        }

        public void UpdatePixels()
        {
            BitsHandle = GCHandle.Alloc(Pixels, GCHandleType.Pinned);
            Bitmap = new Bitmap(Width, Height, Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void UpdatePixels(int[] pixels)
        {
            Resize(Width, Height, pixels);
        }

        public Image ToTrapezoid(PointF topLeft, PointF topRight, PointF bottomLeft, PointF bottomRight)
        {
            //for (int x = 0; x < Width; x++)
            //{
            //    for (int y = 0; y < Height; y++)
            //    {
            //        float srcx = (float)x / Width;
            //        float srcy = (float)y / Height;
            //        SVector l = tl + (bl - tl) * srcy;
            //        SVector t = tl + (tr - tl) * srcx;
            //        SVector r = tr + (br - tr) * srcy;
            //        SVector b = bl + (br - bl) * srcx;

            //        float w = r.X - l.X;
            //        float slope1 = (r.Y - l.Y) / (r.X - l.X);

            //        float dstx = l.X + w * srcx;
            //        float dsty = l.Y + (w * srcx) * slope1;

            //        if (dstx >= 0 && dstx < trap.Width && dsty >= 0 && dsty < trap.Height)
            //        {
            //            trap.SetPixel((int)dstx, (int)dsty, this.GetPixel(x, y));
            //        }
            //    }
            //}

            float tlX = topLeft.X;
            float tlY = topLeft.Y;
            float trX = topRight.X;
            float trY = topRight.Y;
            float leftEdgeX = bottomLeft.X - topLeft.X;
            float leftEdgeY = bottomLeft.Y - topLeft.Y;
            float rightEdgeX = bottomRight.X - topRight.X;
            float rightEdgeY = bottomRight.Y - topRight.Y;

            //Parallel.For(0, Width, x =>
            //{
            //    Parallel.For(0, Height, y =>
            //    {
            //        float srcX = (float)x / Width;
            //        float srcY = (float)y / Height;
            //        float leftX = tlX + leftEdgeX * srcY;
            //        float leftY = tlY + leftEdgeY * srcY;
            //        float rightX = trX + rightEdgeX * srcY;
            //        float rightY = trY + rightEdgeY * srcY;

            //        float width = rightX - leftX;
            //        float slope = (rightY - leftY) / (rightX - leftX);
            //        float dstX = leftX + (width * srcX);
            //        float dstY = leftY + (width * srcX) * slope;

            //        int srcPixel = x + (y * Width);
            //        int dstPixel = (int)dstX + ((int)dstY * Width);

            //        trap.Bits[dstPixel] = Bits[srcPixel];
            //    });
            //});

            int[] sourcePixels = Pixels;
            float fWidth = Width;
            float fHeight = Height;
            int iWidth = Width;
            int length = sourcePixels.Length;
            int[] pixels = new int[length];
            //int[] count = new int[length];

            Parallel.For(0, length, i =>
            {
                try
                {
                    float normX = (i % fWidth) / fWidth;
                    float normY = (i / fWidth) / fHeight;
                    float leftX = tlX + leftEdgeX * normY;
                    float leftY = tlY + leftEdgeY * normY;
                    float rightX = trX + rightEdgeX * normY;
                    float rightY = trY + rightEdgeY * normY;
                    float width = rightX - leftX;
                    float pos = width * normX;
                    float slope = (rightY - leftY) / width;
                    int destX = Convert.ToInt32(leftX + pos);
                    int destY = (int)(leftY + pos * slope);
                    int index = destX + (destY * iWidth);

                    if (index > length) return;
                    if (pixels[index] != 0) return;

                    //count[index]++;

                    //int color = sourcePixels[i];
                    //int a = (color >> 24) & 255;
                    //int r = (color >> 16) & 255;
                    //int g = (color >> 8) & 255;
                    //int b = (color) & 255;

                    //color = pixels[index];
                    //a = (a + (color >> 24) & 255) / count[index];
                    //r = (r + (color >> 16) & 255) / count[index];
                    //g = (g + (color >> 8) & 255) / count[index];
                    //b = (b + (color) & 255) / count[index];

                    //color = (a & 255) << 24 | (r & 255) << 16 | (g & 255) << 8 | (b & 255);
                    //pixels[index] = color;

                    pixels[index] = sourcePixels[i];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            });

            return new Image(Width, Height, pixels);
        }

        public virtual void Dispose()
        {
            if (Disposed) return;
            Disposed = true;

            if (Bitmap != null)
            {
                Bitmap.Dispose();
            }

            if (BitsHandle.IsAllocated)
            {
                BitsHandle.Free();
            }
        }
    }
}
