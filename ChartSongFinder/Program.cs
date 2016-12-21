using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChartSongFinder;

    class Program
    {
        static void Main(string[] args)
        {
            Radio1ChartSongFinder sf = new Radio1ChartSongFinder();
            int count = 1;
            foreach(Tuple<string,string> tuple in sf.Top40List)
            {
                Console.WriteLine("{0}: {1} - {2}", count, tuple.Item1, tuple.Item2);
                count++;
            }
            Console.ReadLine();
        }
    }

