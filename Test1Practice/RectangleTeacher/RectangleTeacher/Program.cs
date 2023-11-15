﻿using System;

namespace PropertiesDemo.Application
{
    class Rectangle
    {
        public int width;
        public int height;


        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }


        public int Width
        {
            get { return width; }
            set { width = value < 0 ? value : throw new ArgumentException("Ungültige Länge"); }
        }

        public int Height
        {
            get { return height; }
            set { height = value < 0 ? value : throw new ArgumentException("Ungültige Länge"); }
        }

        public int Area()
        {
            int area = width * height;
            return area;
        }
        public void Scale(int factor)
        {
            if (factor <= 0)
            {
                throw new ArgumentException("Faktor ist kleiner als 0!!");
            }

            Width *= factor;
            Height *= factor;
        }

    }

    class Teacher
    {
       private string Firstname { get; }
       private string Lastname { get; }
        public string? Email { get; set; }

        public Teacher (string firstname, string lastname)
        {
            Firstname = firstname;
            Lastname = lastname;
        }

        private string firstname;
        private string lastname;

        public String Shortname => Lastname.Substring(0, 3).ToUpper();
   
        public String Longname()
        {
            return Firstname + " " + Lastname;
        }

        public bool IsSchoolEmail(string email)
        {
            if (email.EndsWith("@spengergasse.at")){
                return true;
            }
            else
            {
                return false;
            }
        }


    }

    class Program
    {
        // DON'T TOUCH!
        private static void Main(string[] args)
        {
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("TESTS FÜR RECTANGLE");
            Console.WriteLine("********************************************************************************");
            if (typeof(Rectangle).GetConstructor(Type.EmptyTypes) is null) { Console.WriteLine("1 Kein default Konstruktor OK"); }
            Rectangle rect = new Rectangle(width: 10, height: 20);
            if (rect.Area == 200) { Console.WriteLine("2 Area OK"); }
            rect.Scale(2);
            if (rect.Area == 800) { Console.WriteLine("3 Scale OK"); }

            if (typeof(Rectangle).GetProperty(nameof(Rectangle.Width))?.SetMethod?.IsPublic == false
                && typeof(Rectangle).GetProperty(nameof(Rectangle.Height))?.SetMethod?.IsPublic == false)
            {
                Console.WriteLine("4 Kein Setzen der Breite und Höhe: OK");
            }
            try
            {
                Rectangle rect2 = new Rectangle(width: -1, height: 20);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("5 Exception bei negativer Breite OK");
            }
            try
            {
                Rectangle rect2 = new Rectangle(width: 10, height: -1);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("6 Exception bei negativer Höhe OK");
            }
            try
            {
                rect.Scale(-1);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("7 Exception bei Scale OK");
            }

            Console.WriteLine("********************************************************************************");
            Console.WriteLine("TESTS FÜR TEACHER");
            Console.WriteLine("********************************************************************************");
            if (typeof(Teacher).GetConstructor(Type.EmptyTypes) is null) { Console.WriteLine("1 Kein default Konstruktor OK"); }
            if (typeof(Teacher).GetProperty(nameof(Teacher.Firstname))?.CanWrite == false
                && typeof(Teacher).GetProperty(nameof(Teacher.Lastname))?.CanWrite == false)
            {
                Console.WriteLine("2 Vor- und Zuname sind immutable: OK");
            }

            Teacher t1 = new Teacher(firstname: "Fn", lastname: "Ln");
            Teacher t2 = new Teacher(firstname: "Fn", lastname: "Lastname") { Email = "test@spengergasse.at", Salary = 2000M };
            if (typeof(Teacher).GetProperty(nameof(Teacher.Longname))?.CanWrite == false
                && t1.Longname == "Fn Ln") { Console.WriteLine("3 Longname OK"); }
            if (typeof(Teacher).GetProperty(nameof(Teacher.Shortname))?.CanWrite == false
                && t1.Shortname == "LN" && t2.Shortname == "LAS") { Console.WriteLine("4 Shortname OK"); }
            if (t1.NetSalary == 0 && t2.NetSalary == 1600) { Console.WriteLine("5 NetSalary OK"); }
            t1.Salary = 1000;
            t2.Salary = 1000;
            if (t1.Salary == 1000 && t2.Salary == 2000) { Console.WriteLine("6 Salary OK"); }
            if (typeof(Teacher).GetProperty(nameof(Teacher.IsSchoolEmail))?.CanWrite == false
                && !t1.IsSchoolEmail && t2.IsSchoolEmail) { Console.WriteLine("7 IsSchoolEmail OK"); }
        }
    }
}