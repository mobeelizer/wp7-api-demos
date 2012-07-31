using System;
using System.Text;
using Microsoft.Practices.Mobile.Configuration;
using Com.Mobeelizer.Mobile.Wp7.Configuration;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace wp7_api_demos.Model
{
    public class CreateSessionTask
    {
        private TaskFinishedCallback callback;

        public CreateSessionTask(TaskFinishedCallback callback)
        {
            this.callback = callback;
        }

        private const String META_URL = "mobeelizerUrl";

        private const String META_MODE = "mode";

        public delegate void TaskFinishedCallback(String sessionCode);

        private WebRequest request;

        public void Execute()
        {
            MobeelizerConfigurationSection section = (MobeelizerConfigurationSection)ConfigurationManager.GetSection("mobeelizer-configuration");
            String propUrl = null; 
            
            String mode = null;
            try
            {
                propUrl = section.AppSettings[META_URL].Value;
            }
            catch (KeyNotFoundException) { }

            try
            {
                mode = section.AppSettings[META_MODE].Value;
            }
            catch (KeyNotFoundException) { }
            
            StringBuilder baseUrlBuilder = new StringBuilder();
            if (propUrl == null)
            {
                baseUrlBuilder.Append(Resources.Config.c_apiURL_host);
            }
            else
            {
                baseUrlBuilder.Append(propUrl);
            }

            baseUrlBuilder.Append(Resources.Config.c_apiURL_path);

            if ("test".Equals(mode.ToLower()))
            {
                baseUrlBuilder.Append("?test=true");
            }
            else
            {
                baseUrlBuilder.Append("?test=false");
            }

            Uri url = new Uri(baseUrlBuilder.ToString());
            request = WebRequest.Create(url);
            request.BeginGetResponse(OnBeginGetResponse, null);
        }

        private void OnBeginGetResponse(IAsyncResult result)
        {
            String sessionCode = null;
            try
            {
                WebResponse response = request.EndGetResponse(result);
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        sessionCode = reader.ReadToEnd();
                    }
                }
            }
            catch { }

            callback(sessionCode);
        }
    }
}
