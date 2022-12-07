using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Media.Imaging;

namespace IPCDll
{
    public class GammaCorrectonC
    {
        private int width;
        private int prev;

        private byte[] result;
        private byte[] buffer;
        private double gammaExponent;

        public GammaCorrectonC(int width, int prev, ref byte[] result,ref byte[] buffer, double gammaExponent)
        {
            this.width = width;
            this.prev = prev;
            this.result = result;
            this.buffer = buffer;
            this.gammaExponent = gammaExponent;
        }

        public void BlockCorrection()
        {
            for (int x = 0; x < width; x++)
            {
                int currentCoordinates = this.prev + x * 4;
                PixelCorrection(currentCoordinates);
            }
        }

        private void PixelCorrection(int coordinates,double c=1d)
        {
            byte b = buffer[coordinates];
            double range = (double)b / 255;
            double correction = c * Math.Pow(range, this.gammaExponent);

            this.result[coordinates] = (byte)(correction * 255);

            coordinates += 1;
            b = buffer[coordinates];
            range = (double)b / 255;
            correction = c * Math.Pow(range, this.gammaExponent);
            this.result[coordinates] = (byte)(correction * 255);

            coordinates += 1;
            b = buffer[coordinates];
            range = (double)b / 255;
            correction = c * Math.Pow(range, this.gammaExponent);
            this.result[coordinates] = (byte)(correction * 255);

            coordinates += 1;
            this.result[coordinates] = 255;
        }
    }
}
