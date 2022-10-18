using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ImageProsessingApp.Model
{
    public class GammaCorrection
    {
        public Bitmap SourceBitmap { get; set; }
        public Bitmap ResultBitmap { get; set; }
        public double RedComponent { get; set; }
        public double GreenComponent { get; set; }
        public double BlueComponent { get; set; }
        public String BeforeImageSource { get; set; }
        public String AfterImageSource { get; set; }
        public String ResultsFilename { get; set; } 
        public GammaCorrection()
        {
            this.BeforeImageSource = string.Empty;
            this.AfterImageSource = string.Empty;
            this.RedComponent=0.0;
            this.GreenComponent =0.0;
            this.BlueComponent =0.0;
            SourceBitmap=null;
        }

        public GammaCorrection(double redComponent, double greenComponent, double blueComponent, string beforeImageSource,string afterImageSource, String resultsFilename)
        {
            this.BeforeImageSource = beforeImageSource;
            this.AfterImageSource = afterImageSource;

            this.SourceBitmap = (Bitmap)Image.FromFile(beforeImageSource);
            this.RedComponent = redComponent;
            this.GreenComponent = greenComponent;
            this.BlueComponent = blueComponent;     
            this.ResultsFilename = resultsFilename;
        }
        public void SetBitmap()
        {
            this.SourceBitmap = (Bitmap)Image.FromFile(this.BeforeImageSource);
        }

        public void ApplyRGBGamma()
        {
            if (RedComponent < 0.2 || RedComponent > 5) return;
            if (GreenComponent < 0.2 || GreenComponent > 5) return;
            if (BlueComponent < 0.2 || BlueComponent > 5) return;

            BitmapData bmpData = SourceBitmap.LockBits(new Rectangle(0, 0, SourceBitmap.Width, SourceBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                int stopAddress = (int)ptr + bmpData.Stride * bmpData.Height;

                while ((int)ptr != stopAddress)
                {
                    ptr[0] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(ptr[0] / 255.0, 1.0 / BlueComponent)) + 0.5));
                    ptr[1] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(ptr[1] / 255.0, 1.0 / GreenComponent)) + 0.5));
                    ptr[2] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(ptr[2] / 255.0, 1.0 / RedComponent)) + 0.5));

                    ptr += 3;
                }
            }            
            SourceBitmap.UnlockBits(bmpData);
        }

        public void ApplyGammaCorrection(double gamma, double c = 1d)
        {
            int width = SourceBitmap.Width;
            int height = SourceBitmap.Height;
            BitmapData srcData = SourceBitmap.LockBits(new Rectangle(0, 0, width, height),
                                                 ImageLockMode.ReadOnly,
                                                 PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;

            byte[] buffer = new byte[bytes];
            byte[] result = new byte[bytes];
            Marshal.Copy(srcData.Scan0, buffer, 0, bytes);

            SourceBitmap.UnlockBits(srcData);

            int current = 0;
            int cChannels = 3;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    current = y * srcData.Stride + x * 4;
                    for (int i = 0; i < cChannels; i++)
                    {
                        double range = (double)buffer[current + i] / 255;
                        double correction = c * Math.Pow(range, gamma);
                        result[current + i] = (byte)(correction * 255);
                    }
                    result[current + 3] = 255;
                }
            }

            Bitmap resImg = new Bitmap(width, height);

            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                                                ImageLockMode.WriteOnly, 
                                                PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            this.ResultBitmap= resImg;
        }
       
    }
}
