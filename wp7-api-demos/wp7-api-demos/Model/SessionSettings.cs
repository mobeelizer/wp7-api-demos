using System;
using System.IO.IsolatedStorage;

namespace wp7_api_demos.Model
{
    public class SessionSettings
    {
        private const String SESSION_KEY = "CurrentSession";

        public static void SaveSessionCode(int sessionCode)
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (!settings.Contains(SESSION_KEY))
            {
                settings.Add(SESSION_KEY, sessionCode);
            }
            else
            {
                settings[SESSION_KEY] = sessionCode;
            }

            settings.Save();
        }

        public static bool IsSessionCreated
        {
            get
            {
                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                return settings.Contains(SESSION_KEY);
            }
        }

        public static int? GetSessionCode()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            return (int?)settings[SESSION_KEY];
        }

        public static void RemoveSessionCode()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains(SESSION_KEY))
            {
                settings.Remove(SESSION_KEY);
            }

            settings.Save();
        }
    }
}
