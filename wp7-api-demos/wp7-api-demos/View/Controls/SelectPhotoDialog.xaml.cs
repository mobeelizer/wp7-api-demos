using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View.Controls
{
    public partial class SelectPhotoDialog : UserControl
    {
        public static readonly DependencyProperty TakePhotoCommandProperty = DependencyProperty.Register("TakePhotoCommand", typeof(ICommand), typeof(SelectPhotoDialog), new PropertyMetadata(null, TakePhotoCommandChanged));

        public static readonly DependencyProperty ChosePhotoCommandProperty = DependencyProperty.Register("ChosePhotoCommand", typeof(ICommand), typeof(SelectPhotoDialog), new PropertyMetadata(null, ChosePhotoCommandChanged));

        public static readonly DependencyProperty RandomPhotoCommandProperty = DependencyProperty.Register("RandomPhotoCommand", typeof(ICommand), typeof(SelectPhotoDialog), new PropertyMetadata(null, RandPhotoCommandChanged));

        private SelectPhotoDialogViewModel viewModel = new SelectPhotoDialogViewModel();

        public SelectPhotoDialog()
        {
            InitializeComponent();
            LayoutRoot.DataContext = viewModel;
        }

        public ICommand TakePhotoCommand
        {
            get
            {
                return (ICommand)this.GetValue(TakePhotoCommandProperty);
            }

            set
            {
                SetValue(TakePhotoCommandProperty, value);
            }
        }

        public ICommand ChosePhotoCommand
        {
            get
            {
                return (ICommand)this.GetValue(ChosePhotoCommandProperty);
            }

            set
            {
                SetValue(ChosePhotoCommandProperty, value);
            }
        }

        public ICommand RandomPhotoCommand
        {
            get
            {
                return (ICommand)this.GetValue(RandomPhotoCommandProperty);
            }

            set
            {
                SetValue(RandomPhotoCommandProperty, value);
            }
        }

        private static void TakePhotoCommandChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args)
        {
            (owner as SelectPhotoDialog).viewModel.TakePhotoCommand = args.NewValue as ICommand;
        }

        private static void ChosePhotoCommandChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args)
        {
            (owner as SelectPhotoDialog).viewModel.ChosePhotoCommand = args.NewValue as ICommand;
        }

        private static void RandPhotoCommandChanged(DependencyObject owner, DependencyPropertyChangedEventArgs args)
        {
            (owner as SelectPhotoDialog).viewModel.RandomPhotoCommand = args.NewValue as ICommand;
        }
    }
}
