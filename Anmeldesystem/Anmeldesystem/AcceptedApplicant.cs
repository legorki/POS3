using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anmeldesystem
{
    internal class AcceptedApplicant : Applicant
    {
        public DateTime DateAccepted { get; private set; }

        public AcceptedApplicant(Applicant a)
        {

        }   
    }
}
