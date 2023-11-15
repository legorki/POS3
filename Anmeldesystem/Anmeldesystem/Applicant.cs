using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anmeldesystem
{
    internal class Applicant
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }

        public IReadOnlyDictionary<string, int> Grades { get; set; }

        public Applicant(Applicant other) { 

        }

        public bool AddGrade(string subject, int value)
        {

        }
    }
}
