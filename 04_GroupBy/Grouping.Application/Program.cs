using System;
using System.Collections.Generic;
using System.Linq;
using Grouping.Model;
using System.Text.Json;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Grouping
{
    class Program
    {
        private static JsonSerializerOptions serializerOptions
            = new JsonSerializerOptions { WriteIndented = true };
        static void Main(string[] args)
        {

            // *************************************************************************************
            // Schreibe in den nachfolgenden Übungen statt der Zeile
            // List<object> result = null!;
            // die korrekte LINQ Abfrage. Verwende den entsprechenden Datentyp statt object.
            // Du kannst eine "schöne" (also eingerückte) Ausgabe der JSON Daten erreichen, indem
            // du die Variable WriteIndented in Zeile 12 auf true setzt.
            //
            // !!HINWEIS!!A
            // Beende deine Abfrage immer mit ToList(), damit die Daten für die JSON Serialisierung
            // schon im Speicher sind. Ansonsten würde es auch einen Compilerfehler geben, da
            // WriteJson() eine Liste haben möchte.
            // *************************************************************************************

            ExamsDb db = ExamsDb.FromFiles("csv");

            {
                var resultA = db.Exams
                    .GroupBy(e => new { e.TeacherId, e.Teacher.Lastname });

                var resultB = resultA.Select(g => new
                {
                    g.Key.TeacherId,
                    g.Key.Lastname,
                    ExamsCount = g.Count()
                });
                var resultC = resultB.OrderBy(e => e.TeacherId)
                     .Take(3)
                     .ToList();
                Console.WriteLine("MUSTER: Anzahl der Prüfungen pro Lehrer (erste 3 Lehrer).");
                WriteJson(resultC);
            }

            // *************************************************************************************
            // ÜBUNG 1: Erstelle für jeden Lehrer eine Liste der Fächer, die er unterrichtet. Es
            // sind nur die ersten 10 Datensätze auszugeben. Das kann mit
            // .OrderBy(t=>t.TeacherId).Take(10)
            // am Ende der LINQ Anweisung gemacht werden. Hinweis: Verwende Distinct für die
            // liste der Unterrichtsgegenstände.
            // *************************************************************************************
            {
                /*
                var result = db.Lessons.ToList()
                    .GroupBy(l => new { l.TeacherId })
                    .Select(g => new
                    {
                        TeacherId = g.Key.TeacherId,
                        TeacherLastname = db.Teachers
                                         .Where(t => t.Id == g.Key.TeacherId).First().Lastname,
                        Subjects  = string.Join(", ", g.Select(l => l.Subject).Distinct())
                    })
                    .OrderBy(t => t.TeacherId).Take(5).ToList(); 
                */
                var result = db.Lessons.ToList()
                      .GroupBy(l => new { l.TeacherId })
                      .Select(g => new {
                          TeacherId = g.Key.TeacherId,
                          TeacherLastname = db.Teachers.ToList()
                                            .Where(t => t.Id == g.Key.TeacherId).First().Lastname,
                          Subjects = string.Join(", ", g.Select(l => l.Subject).Distinct())

                      })
                    .OrderBy(t => t.TeacherId).Take(10).ToList();
                Console.WriteLine("RESULT1");
                WriteJson(result);


            }

            // *************************************************************************************
            // ÜBUNG 2: Die 5AHIF möchte wissen, in welchem Monat sie welche Tests hat.
            //          Hinweis: Mit den Properties Month und Year kann auf das Monat bzw. Jahr
            //          eines DateTime Wertes zugegriffen werden. Die Ausgabe in DisplayMonth kann
            //          $"{mydate.Year:00}-{mydate.Month:00}" (mydate ist zu ersetzen)
            //          erzeugt werden
            // *************************************************************************************
            {
                //List<object> result = null!;
                var result2 = db.Exams.ToList()
                    .Where(e => e.SchoolclassId == "5AHIF").ToList()
                    .Select(e => new
                    {
                        YYMM = $"{e.Date.Year:00}-{e.Date.Month:00}",
                        ExamData = e.Subject + ":" + e.TeacherId
                    })
                    .GroupBy(exam1 => exam1.YYMM)
                    .Select(g => new {
                        YYMM = g.Key,
                        ExamListe = string.Join(", ", g.Select(exam => exam.ExamData))
                    }
                    ).OrderBy(exam2 => exam2.YYMM).ToList();

                Console.WriteLine("RESULT2");
                WriteJson(result2);
            }

            // *************************************************************************************
            // ÜBUNG 3: Jeder Schüler der 5AHIF soll eine Übersicht bekommen, welche Tests er pro Fach
            //          abgeschlossen hat.
            //          Es sind nur die ersten 2 Schüler mit OrderBy(p => p.Id).Take(2) am Ende des
            //          Statements auszugeben.
            //          Hinweis: Beachte die Datenstruktur in der Ausgabe.
            //   Pupil                           <-- Zuerst wird der Schüler projiziert (Select)
            //     |
            //     +-- Id
            //         Firstname
            //         Lastname
            //         Exams                     <-- Hier soll nach Subject gruppiert werden
            //           |
            //           +---- Subject           <-- Key der Gruppierung
            //           +---- SubjectExams      <-- Projektion der Gruppierung
            //                    |    
            //                    +------ Teacher
            //                    +------ Date
            //                    +------ Lesson
            // *************************************************************************************
            {
                // List<object> result = null!;

                var result3 = db.Pupils
                        .Where(p => p.SchoolclassId == "5AHIF").Take(2).ToList()
                        .Select(p => new
                        {
                            Id = p.Id,
                            FullName = p.Firstname + " " + p.Lastname,
                            Exams = db.Exams.Where(e => e.SchoolclassId == "5AHIF")
                                            .GroupBy(e => e.Subject)
                                            .Select(g => new {

                                                Subject = g.Key,
                                                SubjectExams = string.Join(", ",
                                                        g.Select(e => new {
                                                            YYMM = $"{e.Date.Year:00}-{e.Date.Month:00}-{e.Date.Day:00}",
                                                            Subject = e.Subject,
                                                            TeacherId = e.Teacher.Id,
                                                        })
                                                        )
                                            })
                        })


                    .ToList();

                Console.WriteLine("RESULT3");
                WriteJson(result3);


                var result = db.Exams
                        .Where(w => w.SchoolclassId == "5AHIF")
                        .GroupBy(g => new { g.Id, g.SchoolclassId, g.Subject })
                        .Select(s => new
                        {
                            Klassse = s.Key.SchoolclassId,
                            Test = s.Key.Id,
                            Fach = s.Key.Subject

                        }).ToList().OrderBy(p => p.Test).Take(2).ToList();

                Console.WriteLine("RESULT3B");

                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 4: Wie viele Klassen sind pro Tag und Stunde gleichzeitig im Haus?
            //          Hinweis: Gruppiere zuerst nach Tag und Stunde in Lesson. Für die Ermittlung
            //          der Klassenanzahl zähle die eindeutigen KlassenIDs, indem mit Distinct eine
            //          Liste dieser IDs (Id) erzeugt wird und dann mit Count() gezählt wird.
            //          Es sind mit OrderByDescending(g=>g.ClassCount).Take(5) nur die 5
            //          "stärksten" Stunden auszugeben.
            // *************************************************************************************
            {
                var result = db.Lessons.ToList().GroupBy(l => new { l.Day, l.PeriodNr })
                    .Select(g => new
                    {
                        PeriodNr = g.Key.PeriodNr,
                        Day = g.Key.Day,
                        ClassesCount = g.Select(l => new { l.SchoolclassId }).Distinct().Count()

                    }).ToList().OrderByDescending(x => x.ClassesCount).Take(5).ToList();

                Console.WriteLine("RESULT4");

                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 5: Wie viele Klassen gibt es pro Abteilung?
            // *************************************************************************************
            {
                var result = db.Schoolclasss.ToList()
                    .GroupBy(sc => new { sc.Department })
                    .Select(sc => new {
                        Department = sc.Key.Department,
                        ClassCount = sc.Count()
                    })
                    .ToList().OrderBy(x => x.ClassCount).ToList();

                Console.WriteLine("RESULT5");

                WriteJson(result);


            }

            // *************************************************************************************
            // ÜBUNG 6: Wie die vorige Übung, allerdings sind nur Abteilungen
            //          mit mehr als 10 Klassen auszugeben.
            //          Hinweis: Filtere mit Where nach dem Erstellen der Objekte mit Department
            //                   und Count
            // *************************************************************************************
            {
                var result = db.Schoolclasss.ToList()
                    .GroupBy(sc => sc.Department)
                    .Select(sc => new {
                        Department = sc.Key,
                        ClassCount = sc.Count()
                    })
                    .ToList().OrderByDescending(x => x.ClassCount)
                    .Where(x => x.ClassCount > 10)
                    .ToList();
                Console.WriteLine("RESULT6");
                WriteJson(result);
            }

            // *************************************************************************************
            // ÜBUNG 7: Wann ist der letzte Test (Max von Exam.Date) pro Lehrer und Fach der 5AHIF
            //          in der Tabelle Exams?
            {
                var result = db.Exams.ToList()
                    .Where(e => e.SchoolclassId == "5AHIF")
                    .GroupBy(e => new { e.TeacherId, e.Subject })
                        .Select(g => new
                        {
                            Teacher = g.Key.TeacherId,
                            Subject = g.Key.Subject,
                            LastTest = g.Max(e => e.Date)

                        }).ToList().Take(3).ToList();
                Console.WriteLine("RESULT7");
                WriteJson(result);
            }


        }

        public static void WriteJson<T>(List<T> result)
        {
            if (result is not null && typeof(T) == typeof(object))
            {
                Console.WriteLine("Warum erstellst du eine Liste von Elementen mit Typ object?");
                return;
            }
            Console.WriteLine(JsonSerializer.Serialize(result, serializerOptions));
        }
    }
}
