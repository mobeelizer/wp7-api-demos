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
using System.IO;
using ExifLib;
using wp7_api_demos.Model;

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

            this.Loaded += new RoutedEventHandler(PageLoaded);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!PageSettings.PageOpened(ExamplePage.FileSync))
            {
                OnInfoClicked(this, null);
                PageSettings.SetPageOpened(ExamplePage.FileSync);
            }
        }

        private void GetPhotoTaskCompleted(object sender, PhotoResult e)
        {
            if (getPhotoCallback != null)
            {
                if (e.TaskResult == TaskResult.OK)
                {
                    JpegInfo info = ExifReader.ReadJpeg(e.ChosenPhoto, e.OriginalFileName);
                    e.ChosenPhoto.Seek(0, SeekOrigin.Begin);
                    ExifOrientation _orientation = info.Orientation;
                    int _angle = 0;
                    switch (info.Orientation)
                    {
                        case ExifOrientation.TopLeft:
                        case ExifOrientation.Undefined:
                            _angle = 0;
                            break;
                        case ExifOrientation.TopRight:
                            _angle = 90;
                            break;
                        case ExifOrientation.BottomRight:
                            _angle = 180;
                            break;
                        case ExifOrientation.BottomLeft:
                            _angle = 270;
                            break;
                    }
                  
                    Stream capturedImage;
                    if (_angle > 0d)
                    {
                        capturedImage = RotateStream(e.ChosenPhoto, _angle);
                    }
                    else
                    {
                        capturedImage = e.ChosenPhoto;
                    }
                    IMobeelizerFile file = Mobeelizer.CreateFile("photo", capturedImage);
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
            questionMessage.Title = "Choose source:";
            SelectPhotoDialog dialogContent = new SelectPhotoDialog();
            dialogContent.TakePhotoCommand = new DelegateCommand((o) =>
                {
                    questionMessage.Hide();
                    this.photoCameraCapture.Show();
                });
            dialogContent.ChosePhotoCommand = new DelegateCommand((o) =>
                {
                    questionMessage.Hide();
                    this.photoChooserTask.Show();
                });
            dialogContent.RandomPhotoCommand = new DelegateCommand((o) =>
                {
                    questionMessage.Hide();
                    callback(null);
                });
            questionMessage.Body = dialogContent;
            RoundButton cancelButton = new RoundButton();
            cancelButton.ImageSource = new BitmapImage(new Uri("/Resources/icons/appbar.stop.rest.png", UriKind.Relative));
            cancelButton.Click += (sender, args) =>
                {
                    questionMessage.Hide();
                };
            questionMessage.ActionPopUpButtons.Clear();
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
            this.viewModel.LogoutCommand.Execute(null);
        }

        private void OnNext(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri(String.Format("/View/PermisionsPage.xaml?SessionCode={0}", viewModel.SessionCode), UriKind.Relative));
        }


        public void GoBackToRoot()
        {
            int howMany = 0;
            foreach (var item in this.NavigationService.BackStack)
            {
                ++howMany;
            }

            for (int i = 0; i < howMany - 1; ++i)
            {
                this.NavigationService.RemoveBackEntry();
            }

            this.NavigationService.GoBack();
        }

        private Stream RotateStream(Stream stream, int angle)
        {
            stream.Position = 0;
            if (angle % 90 != 0 || angle < 0) throw new ArgumentException();
            if (angle % 360 == 0) return stream;

            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            WriteableBitmap wbSource = new WriteableBitmap(bitmap);

            WriteableBitmap wbTarget = null;
            if (angle % 180 == 0)
            {
                wbTarget = new WriteableBitmap(wbSource.PixelWidth, wbSource.PixelHeight);
            }
            else
            {
                wbTarget = new WriteableBitmap(wbSource.PixelHeight, wbSource.PixelWidth);
            }

            for (int x = 0; x < wbSource.PixelWidth; x++)
            {
                for (int y = 0; y < wbSource.PixelHeight; y++)
                {
                    switch (angle % 360)
                    {
                        case 90:
                            wbTarget.Pixels[(wbSource.PixelHeight - y - 1) + x * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 180:
                            wbTarget.Pixels[(wbSource.PixelWidth - x - 1) + (wbSource.PixelHeight - y - 1) * wbSource.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                        case 270:
                            wbTarget.Pixels[y + (wbSource.PixelWidth - x - 1) * wbTarget.PixelWidth] = wbSource.Pixels[x + y * wbSource.PixelWidth];
                            break;
                    }
                }
            }
            MemoryStream targetStream = new MemoryStream();
            wbTarget.SaveJpeg(targetStream, wbTarget.PixelWidth, wbTarget.PixelHeight, 0, 100);
            targetStream.Seek(0, SeekOrigin.Begin);
            return targetStream;
        }

        public static JpegInfo ReadJpeg(Stream FileStream, string FileName)
        {
            DateTime now = DateTime.Now;
            JpegInfo info = ExifReader.ReadJpeg(FileStream, FileName);
            info.FileSize = (int)FileStream.Length;
            info.FileName = string.Format("{0}.jpg", FileName);
            info.LoadTime = (TimeSpan)(DateTime.Now - now);
            return info;
        }
    }
}