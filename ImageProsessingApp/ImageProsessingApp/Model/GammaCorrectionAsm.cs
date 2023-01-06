using ImageProsessingApp.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace ImageProsessingApp.Model
{
    public class GammaCorrectionAsm
    {
        private int width;
        private int prev;

        private float[] result;
        private int[] originalCopy;
        private double gammaExponent;
        private float[] correctionArray;
        public GammaCorrectionAsm(int width, int prev, ref float[] result, ref int[] buffer, double gammaExponent,float[] corArray)
        {
            this.width = width;
            this.prev = prev;
            this.result = result;
            this.originalCopy=buffer;
            this.gammaExponent = gammaExponent;
            this.correctionArray = corArray;
        }

        private float[] ConvertByteArray(byte[] bArray)
        {
            float[] fArray=new float[bArray.Length];
            for(int i=0;i<bArray.Length;i++)
            {
                fArray[i]=bArray[i];
            }
            return fArray;
        }
        [DllImport(@"C:\Users\agnie\source\repos\JA\PROJ-5SEM\ImageProcessing\ImageProsessingApp\x64\Debug\AsmDLL.dll")]
        static extern void MyProc(float[] corArray, int[] originalToCopy,float[] resultPxArray, int picWidth);


        public void BlockCorrection(float c = 1f)
        {
            if(this.prev> 490000)
            {
                Console.WriteLine("asd");
            }
            float[] resultRow = new float[width];
            int[] originalCopyRow = new int[width];
            for (int i = 0; i < width; i++)
            {
                originalCopyRow[i] = this.originalCopy[i + this.prev];
            }

            MyProc(this.correctionArray, originalCopyRow, resultRow, width*4);

            for (int i = 0; i < width; i++)
            {
                this.result[this.prev + i] = resultRow[i];
            }
            Console.WriteLine(4);
        }
    }
}
