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
using System.ComponentModel;

namespace wp7_api_demos.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected INavigationService navigationService;

        private bool isBusy;

        private String busyMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public String BusyMessage
        {
            get
            {
                return this.busyMessage;
            }

            set
            {
                this.busyMessage = value;
                RaisePropertyChanged("BusyMessage");
            }
        }

        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
