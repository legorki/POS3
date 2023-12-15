using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W11_3ahif_DelegatesLambda;

public delegate bool FilterHandler(Student s);

public class StudentList: List<Student>
{
    public override string ToString()
    {

        String ret = "\nStudentList:  \n";   
        foreach (Student item in this)
        {
            ret += $"{item}\n"; // item.ToString() + "\n";
        }
        return ret; 
    }

    public List<Student> FilterLastNameStartsWith(string LastNamePart)
    {
        List<Student> resultList = new();
        foreach (Student item in this)
        {
            if (item.LastName.StartsWith(LastNamePart))
                resultList.Add(item);
        }
        return resultList;
    }

    public List<Student> Filter(FilterHandler handler)
    {
        List<Student> resultList = new();
        foreach (Student item in this)
        {
            if (handler(item) )
                resultList.Add(item);
        }
        return resultList;
    }

    public StudentList Filter2(FilterHandler handler)
    {
        StudentList resultList = new();
        foreach (Student item in this)
        {
            if (handler(item))
                resultList.Add(item);
        }
        return resultList;
    }
}
