using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    partial class Bands
    {
        internal static Band U2()
        {
            Band u2 = new Band();
            u2.Name = "U2";
            u2.JpegPicture = Convert.FromBase64String(Pictures.u2);
            // Alternatively you can fetch pictures from resources:
            // see foreach (var s in System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames()) Console.WriteLine(s.GetType() + ":" + s);

            // U2 members have always been the same
            u2.Members.Add(
                new Period { 
                    Start = new DateTime(1977, 01, 01), 
                    End = new DateTime(2015, 12, 31) 
                },
                new Member[] { 
                    new Member("Bono", new DateTime(1960, 05, 10)),
                    new Member("Larry Mullen Jr.", new DateTime(1961, 10, 13)),
                    new Member("The Edge", new DateTime(1961, 08, 08)),
                    new Member("Adam Clayton", new DateTime(1960, 03, 13))
                }
            );
            Genres g0=Genres.Alternative| Genres.PostPunk | Genres.Rock;
            u2.Records.Add(new Record("Boy", u2, g0, new DateTime(1980, 10,20), "S. Lillywhite"));
            u2.Records.Add(new Record("October", u2, g0, new DateTime(1981, 10, 12), "S. Lillywhite"));
            u2.Records.Add(new Record("War", u2, g0, new DateTime(1983, 02,28), "S. Lillywhite"));
            u2.Records.Add(new Record("The Unforgettable Fire", u2, g0, new DateTime(1984,10,1),"B. Eno, D. Lanois"));
            u2.Records.Add(new Record("The Joshua Tree", u2, g0, new DateTime(1987, 3, 9), "B. Eno|D. Lanois"));
            u2.Records.Add(new Record("Rattle and Hum", u2, g0 | Genres.Roots, new DateTime(1988,10,10),"J. Iovine"));
            u2.Records.Add(new Record("Achtung Baby", u2, g0, new DateTime(1991,11,18),"D. Lanois|B. Eno"));
            u2.Records.Add(new Record("Zooropa", u2, g0 , new DateTime(1993,7,5),"Flood|B. Eno|The Edge"));
            u2.Records.Add(new Record("Pop", u2, g0 | Genres.Dance, new DateTime(1997,3,4),"Flood|Howie B|S. Osborne"));
            u2.Records.Add(new Record("All That You Can't Leave Behind", u2, g0, new DateTime(2000, 10, 30), "D, Lanois|B. Eno"));
            u2.Records.Add(new Record("How to Dismantle an Atomic Bomb", u2, Genres.Rock, new DateTime(2004,11,22),"S. Lillywhite|C. Thomas*|J. Lee*|N. Hooper*|Flood*|D. Lanois*|B. Eno*|C. Glanville*"));
            u2.Records.Add(new Record("No Line on the Horizon", u2, Genres.Rock, new DateTime(2009,02,27),"B. Eno|D. Lanois|S. Lillywhite"));
            u2.Records.Add(new Record("Songs of Innocence", u2, g0, new DateTime(2014, 9, 9), "Danger Mouse|P. Epworth*|R. Tedder*|D. Gaffney*|Flood*"));
            return u2;
        }

        
    }
}
