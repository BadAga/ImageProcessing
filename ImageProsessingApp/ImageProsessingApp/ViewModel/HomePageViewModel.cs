using ImageProsessingApp.Commands;
using ImageProsessingApp.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

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
                    IsAnyDDLChosen = true;
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
                    IsAnyDDLChosen = true;
                }
                asmDDLChosen = value;
                OnPropertyChanged(nameof(AsmDDLChosen));
            }
        }

        private bool isAnyDDLChosen = false;
        public bool IsAnyDDLChosen
        {
            get { return isAnyDDLChosen; }
            set { isAnyDDLChosen = value; OnPropertyChanged(nameof(IsAnyDDLChosen)); }
        }

        public GammaCorrection GCorecction { get; set; }

        public HomePageViewModel()
        {
            GCorecction = new GammaCorrection();
            LoadImageCommand = new RelayCommand(LoadImage);
            RunCommand = new RelayCommand(RunCorraction);
            NumberOfThreadsChosen = Environment.ProcessorCount;
            NumberOfThreads = 64;
        }

        //commands

        public RelayCommand RunCommand { get; }
        public RelayCommand LoadImageCommand { get; }

        public RelayCommand SaveImageCommand { get; }        

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
                GCorecction = new GammaCorrection(this.BeforeImagePath,this.GammaParam,this.NumberOfThreadsChosen);
                GCorecction.ApplyGammaCorrection();
                this.AfterImagePath = GCorecction.GetCorrectedImageSource();
            }
        }

    }
}
