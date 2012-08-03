using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Coding4Fun.Phone.Controls;
using Com.Mobeelizer.Mobile.Wp7;
using Com.Mobeelizer.Mobile.Wp7.Api;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using wp7_api_demos.View.Controls;
using wp7_api_demos.View.Controls.info;
using wp7_api_demos.ViewModel;

namespace wp7_api_demos.View
{
    public partial class FilesPage : PhoneApplicationPage, IFilesPageNavigationService
    {
        private PhotoChooserTask photoChooserTask;

        private CameraCaptureTask photoCameraCapture;

        private FilesPageViewModel viewModel;

        private GetPhotoCallback getPhotoCallback;

        public FilesPage()
        {
            InitializeComponent();

            this.photoChooserTask = new PhotoChooserTask();
            this.photoChooserTask.Completed += new EventHandler<PhotoResult>(GetPhotoTaskCompleted);

            this.photoCameraCapture = new CameraCaptureTask();
            this.photoCameraCapture.Completed += new EventHandler<PhotoResult>(GetPhotoTaskCompleted);
        }

        private void GetPhotoTaskCompleted(object sender, PhotoResult e)
        {
            if (getPhotoCallback != null)
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    IMobeelizerFile file = Mobeelizer.CreateFile("photo", e.ChosenPhoto);
                    this.getPhotoCallback(file);
                    this.getPhotoCallback = null;
                }
                else
                {
                    this.GetPhoto(getPhotoCallback);
                }
            }
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (this.NavigationContext.QueryString.ContainsKey("SessionCode"))
            {
                String sessioCode = this.NavigationContext.QueryString["SessionCode"];
                this.viewModel = new FilesPageViewModel(this, Int32.Parse(sessioCode));
                this.DataContext = this.viewModel;
            }

            base.OnNavigatedTo(e);
        }

        public void Navigate(System.Uri path)
        {
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                this.NavigationService.Navigate(path);
            }));
        }

        public void GoBack()
        {
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                this.NavigationService.GoBack();
            }));
        }


        public void ShowMessage(string title, string message)
        {
            this.Dispatcher.BeginInvoke(new System.Action(() =>
            {
                MessageBox.Show(message, title, MessageBoxButton.OK);
            }));
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            if (this.viewModel.AddCommand != null)
            {
                this.viewModel.AddCommand.Execute(null);
            }
        }

        private void OnSyncClicked(object sender, EventArgs e)
        {
            if (this.viewModel.SyncCommand != null)
            {
                this.viewModel.SyncCommand.Execute(null);
            }
        }

        private void OnInfoClicked(object sender, EventArgs e)
        {
            MessagePrompt messagePrompt = new MessagePrompt();
            messagePrompt.Title = wp7_api_demos.Resources.ResourceDictionary.dialogFilesSyncTitle;
            messagePrompt.Body = new PhotoInfoMessage();
            messagePrompt.Show();
        }

        public void GetPhoto(GetPhotoCallback callback)
        {
            this.getPhotoCallback = callback;
            MessagePrompt questionMessage = new MessagePrompt();
            questionMessage.Title = "Source";
            questionMessage.Body = "Choose source:";
            RoundButton takePhotoButton = new RoundButton();
            takePhotoButton.ImageSource = new BitmapImage(new Uri("/Resources/icons/photo.png", UriKind.Relative));
            takePhotoButton.Click += (sender, args) =>
                {
                    questionMessage.Hide();
                    this.photoCameraCapture.Show();
                };
            RoundButton selectPhotoButton = new RoundButton();
            selectPhotoButton.ImageSource = new BitmapImage(new Uri("/Resources/icons/filesys.png", UriKind.Relative));
            selectPhotoButton.Click += (sender, args) =>
                {
                    questionMessage.Hide();
                    this.photoChooserTask.Show();
                };
            RoundButton randomButton = new RoundButton();
                randomButton.Click += (sender, args) =>
                {
                    questionMessage.Hide();
                    callback(null);
                };
            RoundButton cancelButton = new RoundButton();
            cancelButton.ImageSource = new BitmapImage(new Uri("/Resources/icons/appbar.stop.rest.png", UriKind.Relative));
            cancelButton.Click += (sender, args) =>
                {
                    questionMessage.Hide();
                };
            questionMessage.ActionPopUpButtons.Clear();
            questionMessage.ActionPopUpButtons.Add(takePhotoButton);
            questionMessage.ActionPopUpButtons.Add(selectPhotoButton);
            questionMessage.ActionPopUpButtons.Add(randomButton);
            questionMessage.ActionPopUpButtons.Add(cancelButton);
            questionMessage.Show();
        }

        public void ShowPhoto(IMobeelizerFile photo)
        {
            MessagePrompt showPhoto = new MessagePrompt();
            showPhoto.ActionPopUpButtons.Clear();
            showPhoto.Body = new PhotoControl(photo);
            showPhoto.Show();
        }


        private void OnLogout(object sender, EventArgs e)
        {
        }

        private void OnNext(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(String.Format("/View/PermisionsPage.xaml?SessionCode={0}", viewModel.SessionCode), UriKind.Relative));
        }
    }
}