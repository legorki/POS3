using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using static TestHelpers.ProgramChecker;

namespace RegistrationSystem.Application
{

    public class RegistrationService
    {
        public IReadOnlyCollection<Applicant> Applicants => applicants;
        public Collection<Applicant> applicants = new Collection<Applicant>();  
        public Dictionary<string, IGradeChecker> gradeCheck = new Dictionary<string, IGradeChecker>();    

        public bool AddApplicant(Applicant applicant)
        {
            if (applicants.Any(x => x.Email == applicant.Email)){
                return false;
            }
            else
            {
                applicants.Add(applicant);
                return true;
            }
        }

        public bool AcceptApplicant(string email)
        {
            Applicant applicant = applicants.FirstOrDefault(x => x.Email == email);
            if(applicant == null){
                return false;
            }
            else if (applicant is AcceptedApplicant)
            {
                return false;
            }
            else
            {
                
             var gradeChecker = gradeCheck.FirstOrDefault(x => x.Key == applicant.Department).Value;

                if (gradeChecker != null)
                {
                    if(!gradeChecker.CanBeAccepted(applicant.Grades))
                    {
                        return false;
                    }
                }
                applicants[applicants.IndexOf(applicant)] = new AcceptedApplicant(applicant);
                return true;
            }
        }

        public int CountAccepted(string department)
        {
            return applicants.Count(x => x.Department == department && x is AcceptedApplicant);
        }

        public void RegisterGradeChecker(string department, IGradeChecker gradeChecker)
        {
            gradeCheck.Add(department, gradeChecker);
        }
    }

    public class Applicant
    {
        public string Firstname { get;  }
        public string Lastname { get;  }
        public string Email { get; }
        public string Department { get; }
        public IReadOnlyDictionary<string, int> Grades => grades; 

        public Dictionary<string, int> grades = new Dictionary<string, int>();  
        protected Applicant (Applicant other)
        {
            Firstname = other.Firstname; 
            Lastname = other.Lastname; 
            Email = other.Email;
            Department = other.Department;
            grades = other.grades;

        }

        public Applicant(string firstname, string lastname, string email, string department)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Department = department;
        }

        public bool AddGrade(string subject, int value)
        {
            if (!grades.ContainsKey(subject))
            {
                grades.Add(subject, value);
                return true;
            }
            else
            {
                return false;
            }
        }




    }

    public class AcceptedApplicant : Applicant
    {
        public AcceptedApplicant(Applicant a) : base(a)
        {
            DateAccepted = DateTime.UtcNow;
        }

        public DateTime DateAccepted { get; }
        
    }

    public interface IGradeChecker
    {
        public bool CanBeAccepted(IReadOnlyDictionary<string, int> grades);
    }

    public class HtlGradeChecker : IGradeChecker
    {
        public bool CanBeAccepted(IReadOnlyDictionary<string, int> grades)
        {
            if(grades.Any(x => x.Value >= 4)){
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class FsGradeChecker : IGradeChecker
    {
        public bool CanBeAccepted(IReadOnlyDictionary<string, int> grades)
        {
            if (grades.Any(x => x.Value >= 5))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Teste Klassenimplementierung.");
            CheckAndWrite(() => typeof(Applicant).GetConstructor(Type.EmptyTypes) is null, "Kein Defaultkonstruktor in Applicant.");
            CheckAndWrite(() => typeof(AcceptedApplicant).GetConstructor(Type.EmptyTypes) is null, "Kein Defaultkonstruktor in AcceptedApplicant.");

            CheckAndWrite(
                () => typeof(RegistrationService).GetProperties().Any() && typeof(RegistrationService).GetProperties().All(p => p.CanWrite == false),
                "Alle Properties in RegistrationService sind read only.");
            CheckAndWrite(
                () => typeof(Applicant).GetProperties().Any() && typeof(Applicant).GetProperties().All(p => p.CanWrite == false),
                "Alle Properties in Applicant sind read only.");
            CheckAndWrite(
                () => typeof(AcceptedApplicant).GetProperties().Any() && typeof(AcceptedApplicant).GetProperties().All(p => p.CanWrite == false),
                "Alle Properties in AcceptedApplicant sind read only.");
            CheckAndWrite(() => typeof(IGradeChecker).IsInterface, "IGradeChecker ist ein Interface.");
            CheckAndWrite(() => typeof(HtlGradeChecker).IsAssignableTo(typeof(IGradeChecker)), "HtlGradeChecker implementiert IGradeChecker.");
            CheckAndWrite(() => typeof(FsGradeChecker).IsAssignableTo(typeof(IGradeChecker)), "FsGradeChecker implementiert IGradeChecker.");

            CheckAndWrite(
                () => typeof(Applicant).GetProperty(nameof(Applicant.Grades))?.PropertyType == typeof(IReadOnlyDictionary<string, int>),
                "Applicant.Grades ist IReadOnlyDictionary<string, int>.");

            Console.WriteLine("Teste GradeCheckers");
            var htlGradeChecker = new HtlGradeChecker();
            CheckAndWrite(() =>
                htlGradeChecker.CanBeAccepted(new Dictionary<string, int> { { "D", 1 }, { "E", 2 }, { "M", 3 } })
                && !htlGradeChecker.CanBeAccepted(new Dictionary<string, int> { { "D", 1 }, { "E", 2 }, { "M", 4 } }), "HtlGradeChecker.CanBeAccepted rechnet richtig.", 2);
            var fsGradeChecker = new FsGradeChecker();
            CheckAndWrite(() =>
                fsGradeChecker.CanBeAccepted(new Dictionary<string, int> { { "D", 1 }, { "E", 2 }, { "M", 4 } })
                && !fsGradeChecker.CanBeAccepted(new Dictionary<string, int> { { "D", 1 }, { "E", 2 }, { "M", 5 } }), "FsGradeChecker.CanBeAccepted rechnet richtig.", 2);

            Console.WriteLine("Teste Applicants");
            var applicantHif = new Applicant(firstname: "A", lastname: "B", email: "C", department: "HIF");
            applicantHif.AddGrade("D", 1); applicantHif.AddGrade("E", 2); applicantHif.AddGrade("M", 3);
            CheckAndWrite(() => applicantHif.Grades.Count == 3 && applicantHif.Grades["D"] == 1, "AddGrades fügt die Noten hinzu.", 2);
            CheckAndWrite(() => !applicantHif.AddGrade("D", 2) && applicantHif.Grades.Count == 3 && applicantHif.Grades["D"] == 1, "AddGrades lehnt vorhandene Noten ab.", 2);
            var acceptedApplicant = new AcceptedApplicant(applicantHif);
            CheckAndWrite(() => acceptedApplicant.Email == applicantHif.Email && acceptedApplicant.DateAccepted > DateTime.UtcNow.AddMinutes(-1), "Konstruktor von AcceptedApplicant setzt die Werte.", 2);

            Console.WriteLine("Teste RegistrationService");
            var applicantHifToBad = new Applicant(firstname: "A", lastname: "B", email: "D", department: "HIF");
            applicantHifToBad.AddGrade("D", 1); applicantHifToBad.AddGrade("E", 2); applicantHifToBad.AddGrade("M", 4);

            var applicantduplicateMail = new Applicant(firstname: "A", lastname: "B", email: "C", department: "HIF");
            applicantduplicateMail.AddGrade("D", 1); applicantduplicateMail.AddGrade("E", 2); applicantduplicateMail.AddGrade("M", 3);

            var applicantFs = new Applicant(firstname: "A", lastname: "B", email: "E", department: "FIT");
            applicantFs.AddGrade("D", 1); applicantFs.AddGrade("E", 2); applicantFs.AddGrade("M", 4);

            var applicantFsToBad = new Applicant(firstname: "A", lastname: "B", email: "F", department: "FIT");
            applicantFsToBad.AddGrade("D", 1); applicantFsToBad.AddGrade("E", 2); applicantFsToBad.AddGrade("M", 5);

            var applicantNoGradeCheckDepartment = new Applicant(firstname: "A", lastname: "B", email: "G", department: "HBGM");
            applicantNoGradeCheckDepartment.AddGrade("D", 5); applicantNoGradeCheckDepartment.AddGrade("E", 5); applicantNoGradeCheckDepartment.AddGrade("M", 5);

            var registrationService = new RegistrationService();
            registrationService.AddApplicant(applicantHif);
            registrationService.AddApplicant(applicantHifToBad);
            registrationService.AddApplicant(applicantFs);
            registrationService.AddApplicant(applicantFsToBad);
            registrationService.AddApplicant(applicantNoGradeCheckDepartment);

            CheckAndWrite(() => registrationService.Applicants.Count == 5, "RegistrationService.Applicants zeigt die richtige Anzahl an Bewerbern.", 2);
            CheckAndWrite(() => !registrationService.AddApplicant(applicantduplicateMail), "RegistrationService.AddApplicant prüft Emails auf Eindeutigkeit.", 2);
            registrationService.RegisterGradeChecker("HIF", htlGradeChecker);
            registrationService.RegisterGradeChecker("FIT", fsGradeChecker);

            CheckAndWrite(() => registrationService.AcceptApplicant(applicantHif.Email), "RegistrationService.AcceptApplicant akzeptiert Bewerber mit ausreichenden Noten.", 2);
            CheckAndWrite(() => !registrationService.AcceptApplicant("X"), "RegistrationService.AcceptApplicant liefert false, wenn die Email nicht existiert.", 2);
            CheckAndWrite(() => !registrationService.AcceptApplicant(applicantHif.Email), "RegistrationService.AcceptApplicant liefert false, wenn der Bewerber schon akzeptiert wurde.", 2);
            CheckAndWrite(() => !registrationService.AcceptApplicant(applicantHifToBad.Email), "RegistrationService.AcceptApplicant liefert false, wenn die Noten für die HTL nicht passen.", 2);
            CheckAndWrite(() =>
                registrationService.AcceptApplicant(applicantFs.Email)
                && !registrationService.AcceptApplicant(applicantFsToBad.Email), "RegistrationService.AcceptApplicant liefert false, wenn die Noten für die FS nicht passen.", 2);
            CheckAndWrite(() =>
                registrationService.AcceptApplicant(applicantNoGradeCheckDepartment.Email),
                "RegistrationService.AcceptApplicant berücksichtigt keine Noten, wenn kein GradeChecker für die Abteilung definiert wurde.", 2);

            WriteSummary();
        }
    }
}