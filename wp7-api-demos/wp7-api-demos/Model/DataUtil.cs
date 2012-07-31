using System;
using System.Xml.Linq;
using System.Collections.Generic;
using Com.Mobeelizer.Mobile.Wp7;

namespace wp7_api_demos.Model
{
    public class DataUtil
    {
        private static IList<String> titles = null, directors = null;

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
            return new Movie() { Title = titles[index], Director = directors[index], Rating = random.Next(1,6) };
        }
    }
}
