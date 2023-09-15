using System;

namespace TypesDemo.Application
{
    class TypeExercise
    {
        
        public double? BerechneFlaeche(double? laenge, double? breite)
        {
            if (laenge.HasValue && breite.HasValue)
            {
                return laenge * breite;
            }
            else
            {
                return null;
            }
        }

       
        public double BerechneFlaeche2(double? laenge, double? breite)
        {
            if (laenge.HasValue && breite.HasValue)
            {
                return laenge.Value * breite.Value;
            }
            else
            {
                return 0;
            }
        }

      
        public decimal BerechnePreis(decimal nettopreis, decimal? steuerProdukt, decimal? steuerKategorie)
        {
            decimal steuer = steuerProdukt ?? steuerKategorie ?? 1.2M;
            return nettopreis * steuer;
        }

       
        public double BerechneSchuelerProKlasse(int schuelerGesamt, int klassenGesamt)
        {
            if (klassenGesamt == 0)
            {
                throw new ArgumentException("klassenGesamt darf nicht null sein.");
            }

            return (double)schuelerGesamt / klassenGesamt;
        }

       
        public int BerechneAchtel(long wert)
        {
            return (int)(wert / 8);
        }

        
        public int BerechneLaenge(string? vorname, string? nachname)
        {
            int vornameLaenge = vorname?.Length ?? 0;
            int nachnameLaenge = nachname?.Length ?? 0;

            return vornameLaenge + nachnameLaenge;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TypeExercise typeExercise = new TypeExercise();

            Console.WriteLine("BerechneFlaeche(3,4):              " + typeExercise.BerechneFlaeche(3, 4));
            Console.WriteLine("BerechneFlaeche(3,null):           " + typeExercise.BerechneFlaeche(3, null));
            Console.WriteLine("BerechneFlaeche2(3,null):          " + typeExercise.BerechneFlaeche2(3, null));

            Console.WriteLine("BerechnePreis(100,1.2,null):       " + typeExercise.BerechnePreis(100, 1.2M, null));
            Console.WriteLine("BerechnePreis(100,1.2,1.1):        " + typeExercise.BerechnePreis(100, 1.2M, 1.1M));
            Console.WriteLine("BerechnePreis(100,null,1.1):       " + typeExercise.BerechnePreis(100, null, 1.1M));
            Console.WriteLine("BerechnePreis(100,null,null):      " + typeExercise.BerechnePreis(100, null, null));

            Console.WriteLine("BerechneSchuelerProKlasse(100, 6): " + typeExercise.BerechneSchuelerProKlasse(100, 6));
            Console.WriteLine("BerechneAchtel(120):               " + typeExercise.BerechneAchtel(120));
            Console.WriteLine("BerechneLaenge(null, nachname):    " + typeExercise.BerechneLaenge(null, "nachname"));
        }
    }
}
