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
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel
{
    public class PermisionsPageViewModel : ViewModelBase
    {
        public PermisionsPageViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
        }

        public int SessionCode { get; private set; }

        public ICommand AddCommand
        {
            get
            {
                return new DelegateCommand(this.OnAdd);
            }
        }

        public ICommand SyncCommand
        {
            get
            {
                return new DelegateCommand(this.OnSync);
            }
        }

        public ICommand UserSwitchedCommand
        {
            get
            {
                return new DelegateCommand(UserSwitched);
            }
        }

        public ICommand SwitchingUserCommand
        {
            get
            {
                return new DelegateCommand(SwitchingUser);
            }
        }

        private void UserSwitched(object arg)
        {
        }

        private void SwitchingUser(object arg)
        {
        }

        private void OnAdd(object arg)
        {
        }

        private void OnSync(object arg)
        {
        }
    }
}
