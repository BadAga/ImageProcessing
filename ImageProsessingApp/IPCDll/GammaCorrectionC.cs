using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Media.Imaging;

namespace IPCDll
{
    public class GammaCorrectionC
    {
        private int width;
        private int prev;

        private byte[] result;
        private byte[] originalCopy;
        private double gammaExponent;

        public GammaCorrectionC(int width, int prev, ref byte[] result,ref byte[] buffer, double gammaExponent)
        {
            this.width = width;
            this.prev = prev;
            this.result = result;
            this.originalCopy = buffer;
            this.gammaExponent = gammaExponent;
        }

        public void BlockCorrection()
        {
            for (int x = 0; x < width; x++)
            {
                int currentCoordinates = this.prev + x;
                bool forth = false;
                if ((currentCoordinates + 1) % 4 == 0)
                {
                    forth = true;
                }
                SinglePixelCorrection(currentCoordinates, forth);
            }
        }

        private void SinglePixelCorrection(int coordinates, bool forth,double c = 1d)
        {
            if (!forth)
            {
                byte b = originalCopy[coordinates];
                double range = (double)b / 255;
                double correction = c * Math.Pow(range, this.gammaExponent);

                this.result[coordinates] = (byte)(correction * 255);
            }
            else
            {
                this.result[coordinates] = 255;
            }
        }
    }
}
