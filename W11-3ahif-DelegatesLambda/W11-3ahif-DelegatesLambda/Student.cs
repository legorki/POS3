using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W11_3ahif_DelegatesLambda; 


public class Student
{
    public string FirstName { get; set; }
    public string LastName { get; set;  }
    public Gender Gender { set; get;  }

    public Student(string firstName, string lastName, Gender gender)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
    }

    public override string ToString()
    {
        return $"{FirstName}, {LastName}, {Gender}";
    }

}

