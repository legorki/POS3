class PhoneNr : IEquatable<PhoneNr>, IComparable<PhoneNr>
{
    public long Vorwahl { get; }
    public long Telefonummer { get; }

    public PhoneNr(long vorwahl, long telefonnummer)
    {
        Vorwahl = vorwahl;
        Telefonummer = telefonnummer;
    }

    public override string ToString() => $"0{Vorwahl}/{Telefonummer}";

    public int CompareTo(PhoneNr other)
    {
        if (Vorwahl < other.Vorwahl)
            return -1;
        else if (Vorwahl > other.Vorwahl)
            return 1;
        else
        {
            if (Telefonummer < other.Telefonummer)
                return -1;
            else if (Telefonummer > other.Telefonummer)
                return 1;
            else
                return 0;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj is PhoneNr other)
        {
            return this.Vorwahl == other.Vorwahl && this.Telefonummer == other.Telefonummer;
        }
        return false;
    }

    public bool Equals(PhoneNr other)
    {
        if (other == null)
            return false;

        return this.Vorwahl == other.Vorwahl && this.Telefonummer == other.Telefonummer;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Vorwahl, Telefonummer);
    }
}

record PhoneRecord : IComparable<PhoneNr>
{
    public long Vorwahl { get; init; }
    public long Telefonummer { get; init; }

    public PhoneRecord(long vorwahl, long telefonnummer)
    {
        Vorwahl = vorwahl;
        Telefonummer = telefonnummer;
    }

    public int CompareTo(PhoneNr other)
    {
        PhoneNr phoneNr = new PhoneNr(Vorwahl, Telefonummer);
        return phoneNr.CompareTo(other);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // HTL Wien V
        PhoneNr nr1 = new PhoneNr(01, 54615);
        // BMBWF
        PhoneNr nr2 = new PhoneNr(01, 53120);
        // Handynummer
        PhoneNr nr3 = new PhoneNr(0699, 99999999);
        // HTL Wien V
        PhoneNr nr4 = new PhoneNr(01, 54615);

        Console.WriteLine($"nr1 ist ident mit nr2?:           {nr1.Equals(nr2)}");
        Console.WriteLine($"nr1 ist ident mit nr4?:           {nr1.Equals(nr4)}");
        Console.WriteLine($"nr1 ist ident mit (object) nr4?:  {nr1.Equals((object)nr4)}");
        Console.WriteLine($"nr3 ist ident mit null?:          {nr3.Equals(null)}");
        Console.WriteLine($"nr3 ist größer als nr4?:           {nr3.CompareTo(nr4) > 0}");
        Console.WriteLine($"Hash von nr1:           {nr1.GetHashCode()}");
        Console.WriteLine($"Hash von nr4:           {nr4.GetHashCode()}");

        List<PhoneNr> numbers = new List<PhoneNr>() { nr1, nr2, nr3, nr4 };
        numbers.Sort();
        foreach (PhoneNr n in numbers)
        {
            Console.WriteLine(n);
        }
    }
}