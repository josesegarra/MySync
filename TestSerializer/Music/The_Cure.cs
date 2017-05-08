using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music
{
    partial class Bands
    {
        internal static Band TheCure()
        {
            Band TheCure = new Band();
            TheCure.Name = "The Cure";
            TheCure.JpegPicture = Convert.FromBase64String(Pictures.cure);

            Member RobertS = new Member("Robert Smith", new DateTime(1959, 4, 21));
            Member SimonG=new Member("Simon Gallup", new DateTime(1960,6,1));
            Member RogerOD=new Member("Roger O'Donnell", new DateTime(1955,10,29));
            Member JasonC = new Member("Jason Cooper", new DateTime(1967, 6, 30));
            Member ReevesG=new Member("Reevs Gabriel", new DateTime(1967,1,31));
            Member MichaelD=new Member("Michael Dempsey", new DateTime(1958,11,29));
            Member LolT=new Member("Laurence Tolhurst", new DateTime(1959,2,3));
            Member MatthieuH = new Member("Matthieu Hartley", new DateTime(1960, 2, 4));
            Member PhilT= new Member("Phil Thornalley", new DateTime(1960, 1, 5));
            Member AndyA = new Member("Andy Anderson", new DateTime(1951,6,30));
            Member BorisW = new Member("Boris Williams", new DateTime(1957, 4, 24));
            Member PerryB = new Member("Perry Bamonte", new DateTime(1960, 9,3));
            Member PorlT = new Member("Porl Thompson", new DateTime(1957,11, 8));

            TheCure.Members.Add(new Period { Start = new DateTime(1978, 01, 01), End = new DateTime(1979,06, 30) }, new Member[] { RobertS, MichaelD, LolT });
            TheCure.Members.Add(new Period { Start = new DateTime(1979, 07, 01), End = new DateTime(1980, 06, 30) }, new Member[] { RobertS, LolT, SimonG,MatthieuH });
            TheCure.Members.Add(new Period { Start = new DateTime(1980, 07, 01), End = new DateTime(1982, 06, 30) }, new Member[] { RobertS, LolT, SimonG});
            TheCure.Members.Add(new Period { Start = new DateTime(1982, 07, 01), End = new DateTime(1983, 12, 31) }, new Member[] { RobertS, LolT, PhilT, AndyA});
            TheCure.Members.Add(new Period { Start = new DateTime(1984, 01, 01), End = new DateTime(1984, 12, 31) }, new Member[] { RobertS, LolT, AndyA, PorlT,PhilT });
            TheCure.Members.Add(new Period { Start = new DateTime(1985, 01, 01), End = new DateTime(1987, 06, 30) }, new Member[] { RobertS, LolT, PorlT, SimonG, BorisW });
            TheCure.Members.Add(new Period { Start = new DateTime(1987, 07, 01), End = new DateTime(1988, 12, 31) }, new Member[] { RobertS, LolT, PorlT, SimonG, BorisW , RogerOD});
            TheCure.Members.Add(new Period { Start = new DateTime(1989, 01, 01), End = new DateTime(1990, 06, 30) }, new Member[] { RobertS, PorlT, SimonG, BorisW, RogerOD });
            TheCure.Members.Add(new Period { Start = new DateTime(1990, 07, 01), End = new DateTime(1994, 12, 31) }, new Member[] { RobertS, PorlT, SimonG, BorisW, PerryB});
            TheCure.Members.Add(new Period { Start = new DateTime(1995, 01, 01), End = new DateTime(2005, 12, 31) }, new Member[] { RobertS, SimonG, RogerOD, PerryB, JasonC });
            TheCure.Members.Add(new Period { Start = new DateTime(2006, 01, 01), End = new DateTime(2010, 12, 31) }, new Member[] { RobertS, SimonG, PorlT,JasonC });
            TheCure.Members.Add(new Period { Start = new DateTime(2011, 01, 01), End = new DateTime(2012, 06, 30) }, new Member[] { RobertS, SimonG, RogerOD,JasonC });
            TheCure.Members.Add(new Period { Start = new DateTime(2012, 07, 01), End = new DateTime(2015, 12, 31) }, new Member[] { RobertS, SimonG, RogerOD, JasonC , ReevesG});

            Genres g0 = Genres.PostPunk | Genres.NewWave;
            TheCure.Records.Add(new Record("Three Imaginary Boys", TheCure, g0, new DateTime(1979, 5,8), "C. Parry"));
            TheCure.Records.Add(new Record("Seventeen Seconds", TheCure, g0 | Genres.Gothic, new DateTime(180, 4,22), "R. Smith|M. Hedges"));
            TheCure.Records.Add(new Record("Faith", TheCure, g0| Genres.Gothic, new DateTime(1981, 4,14), "R. Smith|M. Hedges"));
            TheCure.Records.Add(new Record("Pornography", TheCure, g0| Genres.Gothic, new DateTime(1982, 5,4), "R. Smith|P. Thornally"));
            TheCure.Records.Add(new Record("The Top", TheCure, g0| Genres.Gothic, new DateTime(1984,4,30), "R. Smith|D. Allen|C. Parry"));
            TheCure.Records.Add(new Record("The Head on the Door", TheCure, g0, new DateTime(1985,6,26), "R. Smith|D. Allen"));
            TheCure.Records.Add(new Record("Kiss Me, Kiss Me, Kiss Me", TheCure, g0, new DateTime(1987,5,7), "R. Smith|D. Allen"));
            TheCure.Records.Add(new Record("Disintegration", TheCure, g0, new DateTime(1989,5,2), "R. Smith|D. Allen"));
            TheCure.Records.Add(new Record("Wish", TheCure, g0, new DateTime(1992,4,21), "R. Smith|D. Allen"));
            TheCure.Records.Add(new Record("Wild Mood Swings", TheCure, g0, new DateTime(1996,5,7), "R. Smith|S. Lyon"));
            TheCure.Records.Add(new Record("Bloodflowers", TheCure, g0, new DateTime(2000,2,15), "R. Smith|P. Corkett"));
            TheCure.Records.Add(new Record("The Cure", TheCure, g0, new DateTime(2004,6,29), "R. Smith|R. Robinson"));
            TheCure.Records.Add(new Record("4:13 Dream", TheCure, g0, new DateTime(2008, 10, 27), "R. Smith|K. Uddin"));
            
            return TheCure;
        }
    }
}
