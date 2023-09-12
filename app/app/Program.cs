/* 3AHIF S01 (2022-09-07) 
 * - VisualStudio Intro und Installation
 * - debug / break points
 * - Objekt-Referenzen
 * - Heap vs Stack: Objekt-Variablen (Zeiger auf Heap-Strukturen) 
 *   und lokale Primitives
 */

// See https://aka.ms/new-console-template for more information
using System.Linq.Expressions;

Console.WriteLine("Hello, World!");
Person p1 = new Person(); p1.alter = 19; p1.name = "JohnDoe";
Person p2;
Console.WriteLine("p1.alter=<" + p1.alter + ">");
Console.WriteLine("p1.name=<" + p1.name + ">");

for (int i = 0; i<= p1.alter; i++)
{
    Console.WriteLine("i=" + i); // + " p1.alter=" +p1.alter);
}
p1.printMe(); // p1 => JohnDoe, 19
p2 = p1;      // p2 => JohnDoe, 19 ("Referenz"-Variable)

p1.name = "MaxM";
p1.printMe();       // p1 => MaxM, 19
p2.printMe();       // p2 => MaxM, 19


int k = 2;  
int m = k;  // m = 2
k++;        // k = 3 !!!
Console.WriteLine("k=" + k + " m=" + m);

Person p3 = new Person();
p3.printMe();
public class Person
{
    public int alter;       // Anmerkung: prinzipiell IMMER private
    public string name;    // ?name: darf null sein ("Elvis" Operator)

    public void printMe()
    {
        Console.WriteLine("PRINT ME:");
                                    // this
        Console.WriteLine("alter=<" + this.alter + ">" +
                          " name=<"   + name     + ">");
        double meineVar; // "nichts" <=> eine lokale Variable
                         // musss initiert werden !!! 
        meineVar = 5;    // 5.0
        // Console.WriteLine("meineVar=" + meineVar); 
    }
}
