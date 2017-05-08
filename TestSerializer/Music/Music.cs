using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    [Flags]
    enum Genres { 
        Alternative = 1, PostPunk = 1 << 1, Rock = 1 << 2, Industrial = 1<< 3, Dance = 1<<4 , Roots = 1 << 5, 
        RythmBlues = 1 << 6, Psychedelic = 1 << 7, Country = 1 << 8, NewWave = 1<< 9 , Gothic = 1<<10
    };
    enum Alive { Yes, No};    
    class Member
    {
        internal string Name { get; set; }
        internal DateTime DOB;
        internal Alive Alive = Alive.Yes;
        internal Member(string n,DateTime d)
        {
            Name = n;
            DOB = d;
        }

        public override string ToString()
        {
            return Name + " (" + DOB.Year + ")"+(Alive==Alive.No?" decesased":"");
        }
    }

    class Period
    {
        internal DateTime Start;
        internal DateTime End;
    }
    
    class Band 
    {
        public string Name="";
        public Dictionary<Period, Member[]> Members = new Dictionary<Period, Member[]>();
        public List<Record> Records = new List<Record>();
        public byte[] JpegPicture=null;
        public void Print()
        {
            Console.WriteLine("Band:        "+Name);
            Console.WriteLine("Discography: ");
            foreach(Record r in Records)
            {
                Console.Write("             "+r.Released.ToString("yyyy-MM-dd") + "   ");
                Console.Write(r.Title.PadRight(35).Substring(0, 35) + "  ");
                Console.Write(r.Genres.ToString().PadRight(35).Substring(0, 35) + "  ");
                Console.Write(r.Genres.ToString().PadRight(35).Substring(0, 35) + "  ");
                Console.Write(String.Join(", ", r.Produced).PadRight(50).Substring(0,50));
            }
            Console.WriteLine("Members: ");
            foreach (KeyValuePair<Period, Member[]> k in Members)
            {
                Console.Write("             " + k.Key.Start.ToString("yyyy-MM") + "..." + k.Key.End.ToString("yyyy-MM")+"     ");
                Console.Write(String.Join<Member>(", ", k.Value));
                Console.WriteLine("");
            }
            Console.WriteLine(" ");
        }
    }
    
    class Record
    {
        public Record(string t,Band b,Genres g,DateTime r,string p="")
        {
            Title = t;
            Band = b;
            Genres = g;
            Released = r;
            Produced = p.Split('|');
        }

        public string Title;
        public Band Band;
        public Genres Genres;
        public DateTime Released;
        public string[] Produced { get; set; }
    }

  
}
