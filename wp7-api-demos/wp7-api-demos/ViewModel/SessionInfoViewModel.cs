using System.Windows.Input;
using wp7_api_demos.Model;

namespace wp7_api_demos.ViewModel
{
    public class SessionInfoViewModel : ViewModelBase
    {
        private int sessionCode;

        public SessionInfoViewModel(INavigationService navigationService, int sessionCode)
            : base(navigationService)
        {
            this.SessionCode = sessionCode;
        }

        public int SessionCode
        {
            get
            {
                return this.sessionCode;
            }

            set
            {
                this.sessionCode = value;
                base.RaisePropertyChanged("SessionCode");
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
            this.IsBusy = false;
        }

        private void SwitchingUser(object arg)
        {
            this.BusyMessage = "Logging in";
            this.IsBusy = true;
        }
    }
}
