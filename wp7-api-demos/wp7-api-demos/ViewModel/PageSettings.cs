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
        public static void ResetSettings()
        {
            SetPageOpened(ExamplePage.SimpleSync, false);
            SetPageOpened(ExamplePage.FileSync, false);
            SetPageOpened(ExamplePage.Permisions, false);
            SetPageOpened(ExamplePage.Conflicts, false);
            SetPageOpened(ExamplePage.RelationConflicts, false);
            SetPageOpened(ExamplePage.PushNotification, false);
        }

        public static bool PageOpened(ExamplePage page)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if(settings.Contains(page.ToString()))
            {
                return (bool)settings[page.ToString()];
            }

            return false;
        }

        public static void SetPageOpened(ExamplePage page, bool isOpened = true)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(page.ToString()))
            {
                settings[page.ToString()] = isOpened;
            }
            else
            {
                settings.Add(page.ToString(), isOpened);
            }
        }
    }
}
