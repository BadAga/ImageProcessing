using ImageProsessingApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProsessingApp.Commands
{
    public class RunCommand : CommandBase
    {
        private HomePageViewModel hpViewModel;

        public RunCommand(HomePageViewModel hpViewModel)
        {
            this.hpViewModel = hpViewModel;
            hpViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override void Execute(object parameter)
        {
            hpViewModel.GCorecction.BeforeImageSource = hpViewModel.BeforeImagePath;
            hpViewModel.GCorecction.ResultsFilename = "Result.jpg";

            hpViewModel.GCorecction.SetBitmap();

            hpViewModel.GCorecction.RedComponent = 2;
            hpViewModel.GCorecction.GreenComponent= 1;
            hpViewModel.GCorecction.BlueComponent= 0.9;

            hpViewModel.GCorecction.ApplyRGBGamma();       

        }

        public override bool CanExecute(object parameter)
        {
            return (hpViewModel.CDDLChosen||hpViewModel.AsmDDLChosen)&&base.CanExecute(parameter); ;
        }
        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(hpViewModel.CDDLChosen) ||
                e.PropertyName == nameof(hpViewModel.AsmDDLChosen))
            {
                OnCanExecutedChanged();
            }
        }

    }
}
