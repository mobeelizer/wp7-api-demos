using Com.Mobeelizer.Mobile.Wp7.Api;

namespace wp7_api_demos.ViewModel
{
    public delegate void GetPhotoCallback(IMobeelizerFile photo);

    public interface IFilesPageNavigationService : INavigationService
    {
        void GetPhoto(GetPhotoCallback callback);

        void ShowPhoto(IMobeelizerFile file);
    }
}
