using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HtmlAgilityPack;

namespace ChartSongFinder
{
    public class Radio1ChartSongFinder
    {
        private string radio1Top40;
        private List<Tuple<string,string>> top40SongList;
        public List<Tuple<string,string>> Top40List { get { return top40SongList; }  }

        public Radio1ChartSongFinder()
        {
            radio1Top40 = "http://www.bbc.co.uk/radio1/chart/singles";
            HtmlDocument doc = GetHtml(radio1Top40);
            top40SongList = new List<Tuple<string, string>>();

            //narrow html nodes down to a collection of cht-entry-details divs
            //these each contain details about individual chart entries
            HtmlNodeCollection chartEntryNodes =  doc.DocumentNode.SelectNodes("//*[contains(@class,'cht-entry-details')]");

            
            foreach (HtmlNode entry in chartEntryNodes)
            {
                top40SongList.Add(GetSongTuple(entry));                
            }            
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
            doc = web.Load(url);
            doc.Save("Top40SongData");
            return doc;
        }

        private Tuple<string,string> GetSongTuple(HtmlNode entryNode)
        {
            string songName = null;
            string songArtist = null;
            HtmlNodeCollection nodes = entryNode.ChildNodes;
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
    }
}
