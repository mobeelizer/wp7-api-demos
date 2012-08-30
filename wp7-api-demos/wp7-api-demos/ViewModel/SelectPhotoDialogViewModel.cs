using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace wp7_api_demos.ViewModel
{
    public class SelectPhotoDialogViewModel : ViewModelBase
    {
        public SelectPhotoDialogViewModel() 
            : base(null)
        {
        }

        private ICommand takePhotoCommand;

        private ICommand choosePhotoCommand;

        private ICommand randomPhotoCommand;

        public ICommand TakePhotoCommand
        {
            get
            {
                return takePhotoCommand;
            }

            set
            {
                takePhotoCommand = value;
                RaisePropertyChanged("TakePhotoCommand");
            }
        }

        public ICommand ChosePhotoCommand
        {
            get
            {
                return choosePhotoCommand;
            }

            set
            {
                choosePhotoCommand= value;
                RaisePropertyChanged("ChosePhotoCommand");
            }
        }

        public ICommand RandomPhotoCommand
        {
            get
            {
                return randomPhotoCommand;
            }

            set
            {
                randomPhotoCommand = value;
                RaisePropertyChanged("RandomPhotoCommand");
            }
        }
    }
}
