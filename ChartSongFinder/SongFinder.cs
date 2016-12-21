using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HtmlAgilityPack;

namespace ChartSongFinder
{
    class SongFinder
    {
        private string radio1Top40;
        private List<Tuple<string,string>> top40SongList;

        public SongFinder()
        {
            radio1Top40 = "http://www.bbc.co.uk/radio1/chart/singles";
            HtmlDocument doc = GetHtml(radio1Top40);
            top40SongList = new List<Tuple<string, string>>();

            //narrow html nodes down to a collection of cht-entry-details divs
            HtmlNodeCollection nodes =  doc.DocumentNode.SelectNodes("//*[contains(@class,'cht-entry-details')]");

            foreach (HtmlNode n in nodes)
            {
                HtmlNodeCollection childNodes = n.ChildNodes;

                top40SongList.Add(GetSongTuple(childNodes));

                
            }

            foreach(Tuple<string,string> tuple in top40SongList)
            {
                Write(String.Format("{0} - {1}", tuple.Item1, tuple.Item2));
            }
            
            Console.ReadLine();
        }

        private HtmlDocument GetHtml(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc;
            //check if a data file already exists from today, if it does then load it
            string fileDirectory = Directory.GetCurrentDirectory() + "\\Top40SongData";
            if (File.Exists(fileDirectory))
            {
                DateTime lastModified = File.GetLastWriteTime("Top40SongData");
                
                //check if data is from today
                if (DateTime.Compare(DateTime.Now.Date, lastModified.Date) == 0)
                {
                    doc = new HtmlDocument();
                    doc.Load(fileDirectory);
                    return doc;
                }
            }
            //if there is no data file from today, then download it
            Write("Downloading Song Data");
            doc = web.Load(url);
            doc.Save("Top40SongData");
            return doc;
        }

        private Tuple<string,string> GetSongTuple(HtmlNodeCollection nodes)
        {
            string songName = null;
            string songArtist = null;
            foreach(HtmlNode node in nodes)
                if (node.HasAttributes)
                {
                    if (node.Attributes[0].Value.Equals("cht-entry-artist"))
                    {
                        songArtist = node.InnerText.Trim();
                    }
                    else if (node.Attributes[0].Value.Equals("cht-entry-title"))
                    {
                        songName = node.InnerText;
                    }
                }

            return new Tuple<string, string>(songArtist, songName);
        }
        //so I dont have to type Console.WriteLine all the time
        private void Write(string text){
            Console.WriteLine(text);
        }
    }
}
