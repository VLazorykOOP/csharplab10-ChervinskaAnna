// See https://aka.ms/new-console-template for more information
//  За бажанням студента для задач можна створювати консольний проект або WinForm
// Бажано для задач лаб. робіт створити окремі класи
// Виконання  виконати в стилі багатозаданості :
//   Lab9T2  lab9task2 = new Lab9T2; lab9task2.Run();
// При бажанні можна створити багатозадачний режим виконання задач.
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;


namespace Lab10CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lab#9  or  Lab#10");
            Console.WriteLine("What task do you want?");
            Console.WriteLine("1. Task 1");
            Console.WriteLine("2. Task 2");
            Console.WriteLine("5. Exit");

            int choice;
            bool isValidChoice = false;

            do
            {
                Console.Write("Enter number of task ");
                isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice || choice < 1 || choice > 4)
                {
                    Console.WriteLine("This task not exist");
                    isValidChoice = false;
                }
            } while (!isValidChoice);
            switch (choice)
            {
                case 1:
                    task1();
                    break;
                case 2:
                    task2();
                    break;
                case 3:
                    break;
            }
        }

public class InvalidColorException : Exception
    {
        public InvalidColorException() { }
        public InvalidColorException(string message) : base(message) { }
        public InvalidColorException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidTriangleCastException : Exception
    {
        public InvalidTriangleCastException() { }
        public InvalidTriangleCastException(string message) : base(message) { }
        public InvalidTriangleCastException(string message, Exception inner) : base(message, inner) { }
    }

    public class ITriangle
    {
        protected int a, b;
        protected int c;
        public ITriangle(int Base, int side, int color)
        {
            a = Base;
            b = side;
            c = color;
        }

        public void Print()
        {
            Console.WriteLine($"Base: {a}");
            Console.WriteLine($"side: {b}");
        }
        public float P()
        {
            return a + b * 2;
        }

        public float S()
        {
            double p = P() / 2.0;
            float square = (float)Math.Sqrt(p * (p - a) * (p - a) * (p - b));
            if (float.IsNaN(square))
            {
                throw new InvalidTriangleCastException("Invalid triangle parameters: sides are not compatible for triangle construction.");
            }
            return square;
        }

        public bool EquilateralTriangle()
        {
            return a == b;
        }

        public int Side
        {
            get { return b; }
            set { b = value; }
        }
        public int Base
        {
            get { return a; }
            set { a = value; }
        }

        public int Color
        {
            get { return c; }
        }
    }


    public class DerivedTriangle : ITriangle
    {
        public DerivedTriangle(int Base, int side, int color) : base(Base, side, color)
        {
            try
            {
                object test = (object)this;
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Invalid Cast: {ex.Message}");
            }
        }
    }


    static void task1()
    {
        Console.WriteLine("Task 1");
        List<ITriangle> triangles = new List<ITriangle>();
        triangles.Add(new ITriangle(3, -5, 6));
        triangles.Add(new ITriangle(4, 4, 4));
        triangles.Add(new ITriangle(-2, 7, 9));
        triangles.Add(new ITriangle(8, 8, 1));
        triangles.Add(new ITriangle(1, 1, -1));
        foreach (var t in triangles)
        {
            try
            {
                Console.WriteLine();
                t.Print();
                Console.WriteLine($"P = {t.P()}");
                Console.WriteLine($"S = {t.S()}");
                Console.WriteLine($"Equilateral triangle? = {t.EquilateralTriangle()}");

                if (t.Color < 0 || t.Color > 255)
                {
                    throw new InvalidColorException("Invalid color value: it must be in the range of 0 to 255.");
                }
            }
            catch (InvalidTriangleCastException ex)
            {
                Console.WriteLine($"Invalid Triangle: {ex.Message}");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Invalid Cast: {ex.Message}");
            }
            catch (InvalidColorException ex)
            {
                Console.WriteLine($"Invalid Color: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }



    public delegate void EventHandle(object sender, ConveyorEventArgs e);

        public class Conveyor
        {
            string cityName;
            int processedItems;
            int consumedElectricity;
            int defectiveItems;
            int damagedItems;
            public event EventHandle ConveyorEvent;

            private Random rnd = new Random();
            private List<Employee> employees;

            public List<Employee> Employees { get { return employees; } }
            public Conveyor(string city, int workers)
            {
                cityName = city;
                employees = GenerateEmployees(workers);
            }

            private List<Employee> GenerateEmployees(int workerCount)
            {
                List<Employee> generatedEmployees = new List<Employee>();
                for (int i = 0; i < workerCount; i++)
                {
                    string name = "Employee " + (i + 1);
                    int age = rnd.Next(20, 60);
                    generatedEmployees.Add(new Employee(name, age));
                }
                return generatedEmployees;
            }

            protected virtual void OnConveyorEvent(ConveyorEventArgs e)
            {
                ConveyorEvent?.Invoke(this, e);
            }

            public void WorkOnConveyor(int days)
            {
                for (int day = 1; day <= days; day++)
                {
                    processedItems = 0;
                    consumedElectricity = 0;
                    defectiveItems = 0;
                    damagedItems = 0;

                    foreach (Employee employee in employees)
                    {
                        processedItems += rnd.Next(50, 101); 
                        consumedElectricity += rnd.Next(100, 501); 
                        defectiveItems += rnd.Next(0, 11); 
                        damagedItems += rnd.Next(0, 6);
                    }

                    OnConveyorEvent(new ConveyorEventArgs(cityName, processedItems, employees.Count, consumedElectricity, defectiveItems, damagedItems, day));
                }
            }
        }

        public class Employee
        {
            public string Name { get; }
            public int Age { get; }

            public Employee(string name, int age)
            {
                Name = name;
                Age = age;
            }
        }

        public class ConveyorEventArgs : EventArgs
        {
            public string CityName { get; }
            public int ProcessedItems { get; }
            public int WorkersCount { get; }
            public int ConsumedElectricity { get; }
            public int DefectiveItems { get; }
            public int DamagedItems { get; }
            public int Day { get; }

            public ConveyorEventArgs(string city, int processed, int workers, int electricity, int defective, int damaged, int currentDay)
            {
                CityName = city;
                ProcessedItems = processed;
                WorkersCount = workers;
                ConsumedElectricity = electricity;
                DefectiveItems = defective;
                DamagedItems = damaged;
                Day = currentDay;
            }
        }

        static void task2()
        {
            Conveyor conveyor = new Conveyor("Chernivtsi", 5);
            conveyor.ConveyorEvent += HandleConveyorEvent;

            conveyor.WorkOnConveyor(10); 
        }

        static void HandleConveyorEvent(object sender, ConveyorEventArgs e)
        {
            Console.WriteLine($"Day {e.Day}: In {e.CityName}, {e.WorkersCount} workers processed {e.ProcessedItems} items. Consumed electricity: {e.ConsumedElectricity} kWh. Produced {e.DefectiveItems} defective items. {e.DamagedItems} items were damaged.");

            foreach (Employee employee in ((Conveyor)sender).Employees)
            {
                Console.WriteLine($"Employee: {employee.Name}, Age: {employee.Age}");
            }
        }
    }
}