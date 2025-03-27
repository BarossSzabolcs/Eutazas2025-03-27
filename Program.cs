using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] sorok = File.ReadAllLines("utasadat.txt");

            List<Utasok> utasok = new List<Utasok>();

            foreach (string sor in sorok)
            {
                string[] adat = sor.Split(' ');
                int megalloszam = int.Parse(adat[0]);
                string datum = adat[1];
                string azonosito = adat[2];
                string tipus = adat[3];
                string ervenyesseg = adat[4];

                int jegyekDarab = 0;
                DateTime? datumervenyesseg = null;

                if (tipus == "JGY")
                {
                    jegyekDarab = int.Parse(ervenyesseg);
                }
                else 
                { 
                    datumervenyesseg = DateTime.ParseExact(ervenyesseg, "yyyyMMdd",null);
                }
                utasok.Add(new Utasok(megalloszam, tipus, datumervenyesseg, jegyekDarab));
            }
            Console.WriteLine($"2.feladat: {utasok.Count} utas akart felszállni.");

            int elutasitva = utasok.Count(x =>
                (x.Tipus == "JGY" && x.Jegyszam == 0) ||
                (x.Tipus != "JGY" && x.Ervenyesdatum < DateTime.Today)
            );
            Console.WriteLine($"3. feladat: {elutasitva} utas lett visszautasítva!");

            int legtobb = utasok.GroupBy(x => x.Megallo).OrderByDescending(g => g.Count()).First().Key;
            
            Console.WriteLine($"4. feladat: A legtöbb ember a {legtobb} számú megállóban akart felszállni!");

            //TAB,NYB,NYP,RVS,GYK

            string[] kedvezmenyes = { "TAB", "NYB" };
            string[] ingyenes = { "NYP", "RVS", "GYK" };

            int kedvezmenyesSzamolas = utasok.Count(x => kedvezmenyes.Contains(x.Tipus));
            int ingyenesSzamolas = utasok.Count(x => ingyenes.Contains(x.Tipus));

            Console.WriteLine($"5. feladat: {kedvezmenyesSzamolas} db kedvezményesen és {ingyenesSzamolas} db ingyenesen utazó volt!");
        }
        
    }
    class Utasok
    { 
        public int Megallo { get; set; }
        public string Tipus { get; set; }
        public DateTime? Ervenyesdatum { get; set;}

        public int Jegyszam { get; set; }

        public Utasok(int megalloszam, string tipus, DateTime? datumervenyesseg, int jegyekDarab)
        {
            Megallo = megalloszam;
            Tipus = tipus;
            Ervenyesdatum = datumervenyesseg;
            Jegyszam = jegyekDarab;
        }
    }
}
