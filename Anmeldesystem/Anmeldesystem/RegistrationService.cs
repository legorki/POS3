using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnmeldeSystem;

internal class RegistrationService
{
    public IReadOnlyCollection<Applicant> Applicants => _applicants;
    private Collection<Applicant> _applicants = new Collection<Applicant>();
    private Dictionary<string, IGradeChecker> _gradeChecker = new Dictionary<string, IGradeChecker>();


    public bool AddApplicant(Applicant applicant) {

        if (Applicants.Any(x => x.Email == applicant.Email))
        {
            return false;
        }
        else {
            _applicants.Add(applicant);
            return true;
        }
    }


    public bool AcceptApplicant(string email)
    {
        var applicant = _applicants.FirstOrDefault(x => x.Email == email);

        if (applicant == null ) { 
            return false;
        }else if(applicant is AcceptedApplicant) {
            return false;
        }
        else
        {
            var gradeChecker = _gradeChecker.FirstOrDefault(x => x.Key == applicant.Department).Value;
            if (gradeChecker != null)
            {

                if (!gradeChecker.CanBeAccepted(applicant.Grades))
                {

                    return false;
                }
            }

            _applicants[_applicants.IndexOf(applicant)] = new AcceptedApplicant(applicant);

            //_applicants.Remove(applicant);
            //_applicants.Add(new AcceptedApplicant(applicant));
            
            return true;
            
        }

    }

    public int CountAccepted(string department) {
       return _applicants.Count(x => x.Department == department && x is AcceptedApplicant);

    }

    public void RegisterGradeChecker(string department, IGradeChecker gradeChecker) {
       
        _gradeChecker.Add(department, gradeChecker);
    }
}
