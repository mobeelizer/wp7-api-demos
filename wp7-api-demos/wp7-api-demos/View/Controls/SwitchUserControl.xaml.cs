using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View.Controls
{
    public partial class SwitchUserControl : UserControl
    {
        private SwitchUserControlViewModel viewModel = new SwitchUserControlViewModel();

        public static readonly DependencyProperty UserSwitchedCommandProperty = DependencyProperty.Register("UserSwitchedCommand", typeof(ICommand), typeof(SwitchUserControl), new PropertyMetadata(UserSwitchedCommandPropertyChanged));

        public static readonly DependencyProperty SwitchingUserCommandProperty = DependencyProperty.Register("SwitchingUserCommand", typeof(ICommand), typeof(SwitchUserControl), new PropertyMetadata(SwitchingUserCommandPropertyChanged));

        public static readonly DependencyProperty SessionCodeProperty = DependencyProperty.Register("SessionCode", typeof(int), typeof(SwitchUserControl), new PropertyMetadata(SessionCodeChanged));

        public SwitchUserControl()
        {
            InitializeComponent();
            this.LayoutRoot.DataContext = this.viewModel;
            this.Loaded += new RoutedEventHandler(SwitchUserControl_Loaded);
        }

        void SwitchUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.Refresh();
            //this.viewModel.CurrentUser = App.CurrentUser;
            //throw new System.NotImplementedException();
        }


        public ICommand UserSwitchedCommand
        {
            get
            {
                return this.GetValue(UserSwitchedCommandProperty) as ICommand;
            }

            set
            {
                this.SetValue(UserSwitchedCommandProperty, value);
            }
        }

        public int SessionCode
        {
            get
            {
                return (int)this.GetValue(SessionCodeProperty);
            }

            set
            {
                this.SetValue(SessionCodeProperty, value);
            }
        }

        public ICommand SwitchingUserCommand
        {
            get
            {
                return this.GetValue(SwitchingUserCommandProperty) as ICommand;
            }

            set
            {
                this.SetValue(SwitchingUserCommandProperty, value);
            }
        }

        private static void UserSwitchedCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs arg)
        {
            (d as SwitchUserControl).viewModel.UserSwitchedCommand = arg.NewValue as ICommand;
        }

        private static void SwitchingUserCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs arg)
        {
            (d as SwitchUserControl).viewModel.SwitchingUserCommand = arg.NewValue as ICommand;
        }

        private static void SessionCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs arg)
        {
            (d as SwitchUserControl).viewModel.SessionCode = (int)arg.NewValue;
        }
    }
}
