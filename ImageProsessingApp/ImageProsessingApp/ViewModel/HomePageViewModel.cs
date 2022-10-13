using ImageProsessingApp.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int numberOfThreads=16;//default
        public int NumberOfThreads
        {
            get { return numberOfThreads; }
            set { numberOfThreads = value; OnPropertyChanged(nameof(NumberOfThreads)); }
        }

        private String beforeImagePath;
        public String BeforeImagePath
        {
            get { return beforeImagePath;}
            set { beforeImagePath = value; OnPropertyChanged(nameof(BeforeImagePath)); }
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
                }
                asmDDLChosen = value;
                OnPropertyChanged(nameof(AsmDDLChosen));
            }
        }

        //commands

        public RelayCommand LoadImageCommand { get; }

        public RelayCommand SaveImageCommand { get; }

        public HomePageViewModel()
        {
            BeforeImagePath = "/Images/lea.jpg"; //as a default image
            LoadImageCommand = new RelayCommand(LoadImage);
        }

        private void LoadImage(object o)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = (".png");
            open.Filter = "Pictures (*.jpg;*.gif;*.png)|*.jpg;*.gif;*.png";

            if (open.ShowDialog() == true)
                BeforeImagePath = open.FileName;
        }

    }
}
