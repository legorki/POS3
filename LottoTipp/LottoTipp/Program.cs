using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine($"Prüfe die Tipps auf Duplikate...");
        var lottoTipp = new LottoTipp();
        lottoTipp.AddQuicktipps(1000);
        for (int i = 0; i < 1000; i++)
        {
            var tipp = lottoTipp.GetTipp(i);
            if (tipp.Distinct().Count() != 6)
            {
                Console.Error.WriteLine($"FEHLER! Der Tipp {string.Join(",", tipp)} hat Duplikate!");
                return;
            }
            if (tipp.Max() > 45)
            {
                Console.Error.WriteLine($"FEHLER! Der Tipp {string.Join(",", tipp)} hat Zahlen über 45.");
                return;
            }
            if (tipp.Min() < 1)
            {
                Console.Error.WriteLine($"FEHLER! Der Tipp {string.Join(",", tipp)} hat Zahlen unter 1.");
                return;
            }
        }

        Console.WriteLine($"Generiere 5 Tipps...");
        for (int i = 0; i < 5; i++)
        {
            var tipp = lottoTipp.GetTipp(i);
            Console.WriteLine($"Tipp {i}: {string.Join(" ", tipp)}");
        }

        Console.WriteLine($"Generiere 1 000 000 Tipps und zähle die 6er, 5er, ...");
        var usedMemory = GC.GetTotalMemory(forceFullCollection: true);
        lottoTipp.AddQuicktipps(1_000_000);
        var drawnNumbers = new int[] { 4, 2, 1, 8, 32, 16 };
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < 1_000_000; i++)
        {
            var rightNumbers = lottoTipp.CheckTipp(i, drawnNumbers);
            if (rightNumbers >= 5)
            {
                var tipp = lottoTipp.GetTipp(i);
                Console.WriteLine($"Tipp {i:000 000} hat {rightNumbers} Richtige: {string.Join(" ", tipp)}");
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Berechnung nach {stopwatch.ElapsedMilliseconds} ms beendet.");
        Console.WriteLine($"{(GC.GetTotalMemory(forceFullCollection: true) - usedMemory) / 1048576M:0.00} MBytes belegt.");
    }
}

class LottoTipp
{
    private readonly Random _random = new Random(906); // Fixed Seed, erzeugt immer die selbe Sequenz an Werten.
    private List<int[]> tipps = new List<int[]>();

    public int TippCount => tipps.Count;

    public int[] GetTipp(int number)
    {
        if (number < 0 || number >= TippCount)
            throw new IndexOutOfRangeException("Invalid Tipp number");

        return tipps[number];
    }

    private int[] GetNumbers()
    {
        var numbers = new List<int>();
        while (numbers.Count < 6)
        {
            int number = _random.Next(1, 46);
            if (!numbers.Contains(number))
                numbers.Add(number);
        }
        return numbers.ToArray();
    }

    public void AddQuicktipps(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var tipp = GetNumbers();
            tipps.Add(tipp);
        }
    }

    public int CheckTipp(int tippNr, int[] drawnNumbers)
    {
        if (tippNr < 0 || tippNr >= TippCount)
            throw new IndexOutOfRangeException("Invalid Tipp number");

        int[] tipp = tipps[tippNr];

        int rightNumbers = tipp.Intersect(drawnNumbers).Count();
        return rightNumbers;
    }
}
