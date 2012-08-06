using System.IO.IsolatedStorage;

namespace wp7_api_demos.ViewModel
{
    public enum ExamplePage
    {
        SimpleSync,
        FileSync,
        Permisions,
        Conflicts,
        RelationConflicts,
        PushNotification
    }

    public class PageSettings
    {
        public static bool PageOpened(ExamplePage page)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if(settings.Contains(page.ToString()))
            {
                return (bool)settings[page.ToString()];
            }

            return false;
        }

        public static void SetPageOpened(ExamplePage page)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(page.ToString()))
            {
                settings[page.ToString()] = true;
            }
            else
            {
                settings.Add(page.ToString(), true);
            }
        }
    }
}
