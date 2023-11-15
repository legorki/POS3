namespace W11_3ahif_DelegatesLambda; 

internal class Program
{
    public delegate int Transformer(int x);

    static int Square(int x) { return x * x; }
    static int Cube(int x) => x * x * x;

    static void Main(string[] args)
    {
        Transformer t = Cube;
        int result = t(3);  // 27
        Console.WriteLine(result);
        // Create delegate instance
        // Invoke

        int[] values = { 1, 2, 3 };
        // foreach (int i in values)   Console.Write(Square(i)+ " "); 

        Transform(values, Square); // Hook in the Square/Cube method; Funktionsübergabe als Parameter
        foreach (int i in values) Console.Write(i + " ");
        Console.WriteLine("***********");
        Transform(values, Square);      // Funktion als Variable gespeichert und übergeben
        foreach (int i in values) Console.Write(i + " "); //
        Console.WriteLine("***********");

        int[] values2 = { 1, 2, 3 };
        Transform(values2, Test1.SquareStatisch);      // Funktion als Variable gespeichert und übergeben
        foreach (int i in values2) Console.Write(i + " ");
        Console.WriteLine("*********** Test1.SquareStatisch");
        Test1 t1 = new Test1();
        Transform(values2, t1.SquareAlsMemberMethode);      // Funktion als Variable gespeichert und übergeben
        foreach (int i in values2) Console.Write(i + " ");
        Console.WriteLine("*********** (t1) Test1.SquareAlsMemberMethode");

        Student s1 = new Student(lastName: "LastName01", firstName: "FirstName01", gender: 0);
        Student s2 = new Student(lastName: "LastName02", firstName: "FirstName02", gender: 0);

        Console.WriteLine(s1);
        // StudentList studentList = new();
        // studentList.Add(s1);
        //studentList.Add(s2);
        //Console.WriteLine(studentList);

        StudentList studentList = new StudentList()
        {
            new Student(lastName: "LastName01", firstName: "FirstName01", gender: Gender.UNKNOWN),
            new Student(lastName: "LastName02", firstName: "FirstName02", gender: Gender.FEMALE),
            new Student(lastName: "ALastName03", firstName: "FirstName03", gender:Gender.MALE),
            new Student(lastName: "LastName04", firstName: "FirstName04",  gender:Gender.UNKNOWN),
            new Student(lastName: "ALastName10", firstName: "FirstName10", gender:Gender.FEMALE),
            new Student(lastName: "LastName11", firstName: "FirstName11",  gender:Gender.MALE),
        };
        Console.WriteLine(studentList);

        List<Student> filterLastNameStartsWith = studentList.FilterLastNameStartsWith("A");
        String ret = "\n filterLastNameStartsWith :  \n";
        foreach (Student item in filterLastNameStartsWith)  ret += $"{item}\n"; ;
        Console.WriteLine(ret);

        List<Student> filterMitFilter = studentList.Filter(FilterGenderIsNotUnknown);
        ret = "\n filter: FilterGenderIsNotUnknown  :  \n";
        foreach (Student item in filterMitFilter) ret += $"{item}\n"; ;
        Console.WriteLine(ret);


        StudentList filterMitFilter2 = studentList.Filter2(FilterGenderIsNotUnknown);
        ret = "\n filter2: FilterGenderIsNotUnknown  :  \n";
        Console.WriteLine(ret + filterMitFilter2.ToString());

        StudentList filterMitFilter2a =
                               // public delegate bool FilterHandler(Student s);
            studentList.Filter2(s => s.Gender != Gender.UNKNOWN)
                       .Filter2(s => s.LastName.StartsWith("A"));
        ret = "\n filter2: Gender is not UNKNOWN => LastName starts with A :  \n";
        Console.WriteLine(ret + filterMitFilter2a.ToString());
        
    }

    static public bool FilterLastNameStartsWith2(Student s)
    {
        return s.LastName.StartsWith("A"); 
    }

    static public bool FilterGenderIsNotUnknown(Student s)
    {
        return s.Gender != Gender.UNKNOWN;
    }

    static void Transform(int[] values, Transformer t)
    {
        for (int i = 0; i < values.Length; i++)
            values[i] = t(values[i]);
    }
}