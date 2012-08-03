using System;
using System.Xml.Linq;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7;

namespace wp7_api_demos.Model
{
    public class DataUtil
    {
        private static IList<String> titles = null, directors = null;

        private static String[] photos = new String[] { "landscape_01.jpg", "landscape_02.jpg", "landscape_03.jpg", 
            "landscape_04.jpg", "landscape_05.jpg", "landscape_06.jpg", "landscape_07.jpg", "landscape_08.jpg",
            "landscape_09.jpg", "landscape_10.jpg" };

        private static Random random = new Random();

        public static Movie GetRandomMovie()
        {
            if (titles == null || directors == null)
            {
                XDocument document = XDocument.Parse(Resources.ResourceDictionary.movies);
                XElement xTitles = document.Root.Element("movieTitles");
                XElement xDirectors = document.Root.Element("movieDirectors");
                titles = new List<String>();
                foreach (var item in xTitles.Elements("item"))
                {
                    titles.Add(item.Value);
                }

                directors = new List<String>();
                foreach (var item in xDirectors.Elements("item"))
                {
                    directors.Add(item.Value);
                }
            }

            int index = random.Next(0, titles.Count - 1);
            return new Movie() { Title = titles[index], Director = directors[index], Rating = random.Next(1, 6) };
        }

        public static String GetRandomPhoto()
        {
            int index = random.Next(0, photos.Length - 1);
            return photos[index];
        }

    }
}
