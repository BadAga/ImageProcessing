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
    public class GammaCorrectonC
    {
        private int width;
        private int prev;

        private byte[] result;
        private byte[] tempResult;
        private double gammaExponent;

        public GammaCorrectonC(int width, int prev, ref byte[] result,ref byte[] buffer, double gammaExponent)
        {
            this.width = width;
            this.prev = prev;
            this.result = result;
            this.tempResult = buffer;
            this.gammaExponent = gammaExponent;
        }

        public void BlockCorrection()
        {
            for (int x = 0; x < width; x++)
            {
                //int currentCoordinates = this.prev + x * 4;
                int currentCoordinates = this.prev + x;
                bool forth = false;
                if ((currentCoordinates + 1) % 4 == 0)
                {
                    forth = true;
                }
                //PixelCorrection(currentCoordinates);
                SinglePixelCorrection(currentCoordinates, forth);
            }
        }

        private void SinglePixelCorrection(int coordinates, bool forth,double c = 1d)
        {
            if (!forth)
            {
                byte b = tempResult[coordinates];
                double range = (double)b / 255;
                double correction = c * Math.Pow(range, this.gammaExponent);

                this.result[coordinates] = (byte)(correction * 255);
            }
            else
            {
                this.result[coordinates] = 255;
            }
        }
        private void PixelCorrection(int coordinates,double c=1d)
        {
            for(int i=0;i<4;i++)
            {
                if (i == 3)
                {
                    this.result[coordinates] = 255;
                }
                else
                {
                    byte b = tempResult[coordinates];
                    double range = (double)b / 255;
                    double correction = c * Math.Pow(range, this.gammaExponent);

                    this.result[coordinates] = (byte)(correction * 255);
                    coordinates++;
                }
            }
            //byte b = tempResult[coordinates];
            //double range = (double)b / 255;
            //double correction = c * Math.Pow(range, this.gammaExponent);

            //this.result[coordinates] = (byte)(correction * 255);

            //coordinates += 1;
            //b = tempResult[coordinates];
            //range = (double)b / 255;
            //correction = c * Math.Pow(range, this.gammaExponent);
            //this.result[coordinates] = (byte)(correction * 255);

            //coordinates += 1;
            //b = tempResult[coordinates];
            //range = (double)b / 255;
            //correction = c * Math.Pow(range, this.gammaExponent);
            //this.result[coordinates] = (byte)(correction * 255);

            //coordinates += 1;
            //this.result[coordinates] = 255;
        }
    }
}
