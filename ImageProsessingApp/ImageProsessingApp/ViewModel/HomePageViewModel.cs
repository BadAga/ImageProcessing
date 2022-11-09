using ImageProsessingApp.Commands;
using ImageProsessingApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageProsessingApp.ViewModel
{
    public class HomePageViewModel:ViewModelBase
    {
        private int numberOfThreadsChosen;
        public int NumberOfThreadsChosen
        {
            get { return numberOfThreadsChosen; }
            set 
            {
                numberOfThreadsChosen = value;
                OnPropertyChanged(nameof(NumberOfThreadsChosen));
            }
        }

        private int numberOfThreads;
        public int NumberOfThreads
        {
            get { return numberOfThreads; }
            set { numberOfThreads = value; OnPropertyChanged(nameof(NumberOfThreads)); }
        }

        private double gammaParam=2.2;
        public double GammaParam
        {
            get { return gammaParam; }
            set { gammaParam = value; OnPropertyChanged(nameof(GammaParam)); }
        }

        private String beforeImagePath;
        public String BeforeImagePath
        {
            get { return beforeImagePath;}
            set { beforeImagePath = value; OnPropertyChanged(nameof(BeforeImagePath)); }
        }

        private ImageSource afterImagePath;
        public ImageSource AfterImagePath
        {
            get { return afterImagePath; }
            set { afterImagePath = value; OnPropertyChanged(nameof(AfterImagePath)); }
        }

        private bool cDDLChosen=false;//as default     
        public bool CDDLChosen
        {
            get { return cDDLChosen; }
            set
            {
                if (value) //onlu=y one of the ddl's can be chosen.
                {
                    AsmDDLChosen = false;
                    CanRun = true;
                }
                cDDLChosen = value; 
                OnPropertyChanged(nameof(CDDLChosen));
            }
        }

        private bool asmDDLChosen=false;//as default
        public bool AsmDDLChosen
        {
            get { return asmDDLChosen; }
            set
            {
                if (value)
                {
                    CDDLChosen = false;
                    CanRun = true;
                }
                asmDDLChosen = value;
                OnPropertyChanged(nameof(AsmDDLChosen));
            }
        }

        private bool canRun = false;
        public bool CanRun
        {
            get { return canRun; }
            set { canRun = value; OnPropertyChanged(nameof(CanRun)); }
        }

        private bool canSaveResult = false;
        public bool CanSaveResult
        {
            get { return canSaveResult; }
            set { canSaveResult = value; OnPropertyChanged(nameof(CanSaveResult)); }
        }

        private string execTime = "-";
        public string ExecTime
        {
            get { return execTime; }
            set { execTime = value; OnPropertyChanged(nameof(ExecTime)); }
        }

        public GammaCorrection GCorecction { get; set; }
        /// ///////////////////////////////
        public HomePageViewModel()
        {
            GCorecction = new GammaCorrection();
            LoadImageCommand = new RelayCommand(LoadImage);
            RunCommand = new RelayCommand(RunCorraction);
            SaveImageCommand = new RelayCommand(SaveResultImage);
            TestCommand = new RelayCommand(RunTests);
            NumberOfThreadsChosen = Environment.ProcessorCount;
            NumberOfThreads = 64;
        }

        //commands

        public RelayCommand RunCommand { get; }
        public RelayCommand LoadImageCommand { get; }
        public RelayCommand SaveImageCommand { get; }
        public RelayCommand TestCommand { get; }
        private void LoadImage(object o)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = (".png");
            open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

            if (open.ShowDialog() == true)
                BeforeImagePath = open.FileName;
        }       
        private void RunCorraction(object o)
        {
            if (this.beforeImagePath != null)
            {
                CanRun = false;
                GCorecction = new GammaCorrection(this.BeforeImagePath,this.GammaParam,this.NumberOfThreadsChosen);
                //GCorecction.ApplyGammaCorrection();
                //GCorecction.ApplyGammaCorrectionInThreads();
                GCorecction.ApplyGammaCorrectionInThreadsVersion2();
                this.ExecTime = GCorecction.ExecutionTime.ToString() + " s";
                this.AfterImagePath = GCorecction.GetCorrectedImageSource();
                CanSaveResult = true;
                CanRun=true;
            }
        }
        private void RunTests(object o)
        {
            List<List<string>> list = new List<List<string>>();
            if (this.beforeImagePath != null)
            {
                
                for (int i = 1; i < 65; i=i*2)
                {
                    List<string> listReultsPerThread = new List<string>();
                    for (int j = 1; j <= 10; j++)
                    {
                        GCorecction = new GammaCorrection(this.BeforeImagePath, this.GammaParam, i);
                        GCorecction.ApplyGammaCorrectionInThreadsVersion2();
                        listReultsPerThread.Add(GCorecction.ExecutionTime.ToString() + " s");
                    }
                    list.Add(listReultsPerThread);
                }
            }
            TextWriter tw = new StreamWriter("TestForTestVersion2.txt");
            int counter = 1;
            foreach (var block in list)
            {
                tw.WriteLine("Number of threads:" + counter.ToString());
                foreach (var result in block)
                {
                    tw.Write(result.ToString());
                    tw.Write("  ");
                }
                tw.WriteLine();
                counter = counter * 2;
            }
            tw.Close();

        }
        private void SaveResultImage(object o)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "Save picture as ";
            save.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (GCorecction.ResultImage != null)
            {
                if (save.ShowDialog() == true)
                {
                    JpegBitmapEncoder jpg = new JpegBitmapEncoder();
                    jpg.Frames.Add(BitmapFrame.Create(GCorecction.ResultImage));
                    using (Stream stm = File.Create(save.FileName))
                    {
                        jpg.Save(stm);
                    }
                }
            }
        }
    }
}
