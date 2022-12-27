using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageProsessingApp.Model
{
    public class GammaCorrectionAsm
    {
        private int width;
        private int prev;

        private byte[] result;
        private byte[] tempResult;
        private double gammaExponent;

        public GammaCorrectionAsm(int width, int prev, ref byte[] result, ref byte[] buffer, double gammaExponent)
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
                int currentCoordinates = this.prev + x;
                bool forth = false;
                if ((currentCoordinates + 1) % 4 == 0)
                {
                    forth = true;
                }
                SinglePixelCorrection(currentCoordinates, forth);
            }
        }

        [DllImport(@"C:\Users\agnie\source\repos\JA\PROJ-5SEM\ImageProcessing\ImageProsessingApp\x64\Debug\AsmDLL.dll")]
        static extern int MyProc(int a, int b);
        private void SinglePixelCorrection(int coordinates, bool forth, double c = 1d)
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
    }
}
