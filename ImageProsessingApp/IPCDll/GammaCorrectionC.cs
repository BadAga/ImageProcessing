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

        public void BlockCorrection(double c = 1d)
        {
            for (int i = 0; i < width; i += 4)
            {
                int currentCoordinates = this.prev+i;
                byte b = originalCopy[currentCoordinates];
                double range = (double)b / 255;
                double correction = c * Math.Pow(range, this.gammaExponent);

                this.result[currentCoordinates] = (byte)(correction * 255);

                currentCoordinates++;
                b = originalCopy[currentCoordinates];
                range = (double)b / 255;
                correction = c * Math.Pow(range, this.gammaExponent);

                this.result[currentCoordinates] = (byte)(correction * 255);

                currentCoordinates++;
                b = originalCopy[currentCoordinates];
                range = (double)b / 255;
                correction = c * Math.Pow(range, this.gammaExponent);

                this.result[currentCoordinates] = (byte)(correction * 255);

                currentCoordinates++;
                this.result[currentCoordinates] = 255;
            }
        }
    }
}
