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
using System.Threading;
using System.Security.AccessControl;
using ImageProsessingApp.Model.Mulithraeding;
using ImageProsessingApp.Model.Multithreading;
using System.Collections.Concurrent;
using System.Windows;
using IPCDll;

namespace ImageProsessingApp.Model
{
    public class GammaCorrection
    {
        public Bitmap SourceBitmap { get; set; }
        public String BeforeImageSource { get; set; }
        public ImageSource AfterImageSource { get; set; }

        public String ResultsFilename { get; set; } 

        public BitmapImage ResultImage { get; set; }

        public double Gamma { get; set; }
        public int NumberOfThreads { get; set; }
        public double ExecutionTime { get; set; }

        private byte[] result;

        private double c = 1d;

        private byte b = new byte();
        private byte[] buffer;
        private int coordinates = 0;

        public GammaCorrection()
        {
            this.BeforeImageSource = string.Empty;
            Gamma = 0;
            SourceBitmap =null;
        }

        public GammaCorrection(string beforeImageSource,double gammaParam,int numberOfThreads)
        {
            this.BeforeImageSource = beforeImageSource;
            this.SourceBitmap = (Bitmap)Image.FromFile(beforeImageSource);
            this.ResultsFilename = String.Empty;
            this.Gamma = gammaParam;
            this.NumberOfThreads = numberOfThreads;           
        }
        public void SetBitmap()
        {
            this.SourceBitmap = (Bitmap)Image.FromFile(this.BeforeImageSource);
        }
        public ImageSource GetCorrectedImageSource()
        {
            return this.AfterImageSource;
        }

        public void ApplyGammaCorrectionInThreads(double c = 1d)
        {
            int width = SourceBitmap.Width;
            int height = SourceBitmap.Height;
            BitmapData srcData = SourceBitmap.LockBits(new Rectangle(0, 0, width, height),
                                                 ImageLockMode.ReadOnly,
                                                 System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int bytes = srcData.Stride * srcData.Height;

            this.buffer = new byte[bytes];
            this.result = new byte[bytes];

            Marshal.Copy(srcData.Scan0, this.buffer, 0, bytes);
            SourceBitmap.UnlockBits(srcData);

            var watch = new System.Diagnostics.Stopwatch();
            int stride = srcData.Stride;

            List<WaitHandle> waitingRoomList = new List<WaitHandle>();
            MultithreadingManager manager = MultithreadingManager.Instance;
            manager.UpdateThreadCount(this.NumberOfThreads);
            watch.Start();

            for (int y = 0; y < height; y++)
            {
                int prev = y * stride;

                GammaCorrectonC gmccBlock = new GammaCorrectonC(width, prev, ref this.result, ref this.buffer, this.Gamma);
                Action action =()=> gmccBlock.BlockCorrection();
                // PixelBlockChange pChange = new PixelBlockChange(action, width,prev);
                PixelBlockChange pChange = new PixelBlockChange(action);

                WaitHandle currentWaitHandle = manager.AddPixelChange(pChange);
                waitingRoomList.Add(currentWaitHandle);
                if (waitingRoomList.Count == 56)
                {
                    foreach (var wh in waitingRoomList)
                    {
                        wh.WaitOne();
                    }
                    waitingRoomList.Clear();
                }
            }

            if (waitingRoomList.Count != 0)
            {
                foreach (var wh in waitingRoomList)
                {
                    wh.WaitOne();
                }
                waitingRoomList.Clear();
            }

            watch.Stop();
            ExecutionTime = (double)watch.ElapsedMilliseconds / 1000;
            Bitmap resImg = new Bitmap(width, height);

            BitmapData resData = resImg.LockBits(new Rectangle(0, 0, width, height),
                                                ImageLockMode.WriteOnly,
                                                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Marshal.Copy(result, 0, resData.Scan0, bytes);
            resImg.UnlockBits(resData);
            this.SetCorrectedAfterImage(resImg);
        }

        private void ApplyGammaToBlockOfPx(int width, int prev)
        {
            int current = 0;
            for (int x = 0; x < width; x++)
            {
                current = prev + x * 4;
                ApplyGammaToPixelThreads(current);                    
            }
        }
        private void ApplyGammaToPixelThreads(int coordinates)
        { 
            byte b = buffer[coordinates];
            double range = (double)b / 255;
            double correction = this.c * Math.Pow(range, this.Gamma);

            this.result[coordinates] = (byte)(correction * 255);

            coordinates += 1;
            b = buffer[coordinates];
            range = (double)b / 255;
            correction = this.c * Math.Pow(range, this.Gamma);
           this.result[coordinates] = (byte)(correction * 255);

            coordinates += 1;
            b = buffer[coordinates];
            range = (double)b / 255;
            correction = this.c * Math.Pow(range, this.Gamma);
            this.result[coordinates] = (byte)(correction * 255);

            coordinates += 1;
            this.result[coordinates] = 255;
            //return changed;
        }
        private void SetCorrectedAfterImage(Bitmap correctedBitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                correctedBitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                this.ResultImage = bitmapImage;
                AfterImageSource = bitmapImage;
            }
        }
       
    }
}
