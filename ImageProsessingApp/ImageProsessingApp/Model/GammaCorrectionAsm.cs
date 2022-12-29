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
        private float[] correctionArray;
        public GammaCorrectionAsm(int width, int prev, ref byte[] result, ref byte[] buffer, double gammaExponent,float[] corArray)
        {
            this.width = width;
            this.prev = prev;
            this.result = result;
            this.tempResult = buffer;
            this.gammaExponent = gammaExponent;
            this.correctionArray = corArray;
        }
        [DllImport(@"C:\Users\agnie\source\repos\JA\PROJ-5SEM\ImageProcessing\ImageProsessingApp\x64\Debug\AsmDLL.dll")]
        static extern int MyProc(float[] corArray, float[] resultPxArray, int picWidth);

        public void BlockCorrection(float c = 1f)
        { 
            float[] result = new float[width];
            MyProc(correctionArray, result,width);
            float[] result2 = result;
        }
    }
}
