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

        private byte[] result;
        private float[] originalCopy;
        private double gammaExponent;
        private float[] correctionArray;
        public GammaCorrectionAsm(int width, int prev, ref byte[] result, ref byte[] buffer, double gammaExponent,float[] corArray)
        {
            this.width = width;
            this.prev = prev;
            this.result = result;
            this.originalCopy=ConvertByteArray(buffer);
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
            float[] lutArray ={ 1.1f, 1.2f ,1.3f, 1.4f, 1.5f, 1.6f , 1.7f, 1.8f, 1.9f , 2.0f, 2.1f, 2.2f };
            int[] row = { 2,3,9,4,6,7,8,10,12,9,1,11 };
            float[] result=new float[row.Length];

            MyProc(lutArray, row, result, 12*4);
            Omg();
        }

        //public void BlockCorrection(float c = 1f)
        //{
        //    float[] result = new float[width];
        //    int[] originalCopyRow = new int[width];
        //    for(int i=0;i<width;i++)
        //    {
        //        originalCopyRow[i]=(int)this.originalCopy[i+this.prev];
        //    }

        //    MyProc(this.correctionArray, originalCopyRow, result, width);
        //    Omg();

        //    List<float> fcheck = new List<float>();
        //    foreach (float f in result)
        //    {
        //        if (f > 50)
        //        {
        //            fcheck.Add(f);
        //        }
        //    }

        //}
        private void Omg()
        {
            Console.WriteLine("hi");
        }
    }
}
