﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ImageProsessingApp.Model
{
    public class GammaCorrection
    {
        public Bitmap SourceBitmap { get; set; }
        public Bitmap ResultBitmap { get; set; }

        public String BeforeImageSource { get; set; }
        public ImageSource AfterImageSource { get; set; }

        public String ResultsFilename { get; set; } 
        public double Gamma { get; set; }

        public GammaCorrection()
        {
            this.BeforeImageSource = string.Empty;
            Gamma = 0;
            SourceBitmap =null;
        }

        public GammaCorrection(string beforeImageSource,double gammaParam)
        {
            this.BeforeImageSource = beforeImageSource;
            this.SourceBitmap = (Bitmap)Image.FromFile(beforeImageSource);
            this.ResultsFilename = String.Empty;
            this.Gamma = gammaParam;
        }
        public void SetBitmap()
        {
            this.SourceBitmap = (Bitmap)Image.FromFile(this.BeforeImageSource);
        }

        public void ApplyGammaCorrection( double c = 1d)
        {
            int width = SourceBitmap.Width;
            int height = SourceBitmap.Height;
            BitmapData srcData = SourceBitmap.LockBits(new Rectangle(0, 0, width, height),
                                                 ImageLockMode.ReadOnly,
                                                 System.Drawing.Imaging.PixelFormat.Format32bppArgb);
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
                        double correction = c * Math.Pow(range, Gamma);
                        result[current + i] = (byte)(correction * 255);
                    }
                    result[current + 3] = 255;
                }
            }

            Bitmap resImg = new Bitmap(width, height);

            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                                                ImageLockMode.WriteOnly,
                                                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            this.ResultBitmap= resImg;

            using (MemoryStream memory = new MemoryStream())
            {
                resImg.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                AfterImageSource = bitmapImage;
            }
        }

        public ImageSource GetCorrectedImageSource()
        {
            return this.AfterImageSource;
        }
       
    }
}