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


namespace Lab10CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lab#8 ");
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
                case 5:
                    break;
            }
        }

    

        static void task1()
        {

        }

        public delegate void FireEventHandler(object sender, FireEventArgs e);

        public class NewTown
        {
            //властивостi
            string townname; //назва мiста
            int buildings; //число будинкiв у мiстi
            int days; //число днiв спостереження
                      //мiськi служби
            Police policeman;
            Ambulance ambulanceman;
            FireDetect fireman;
            //подiї в мiстi
            public event FireEventHandler Fire;
            string[] resultservice; //результати дiй служб 
                                    //моделювання випадкових подiй
            private Random rnd = new Random();
            //iмовiрнiсть пожежi в будинку в поточний день
            double fireprobability;
            /// <summary>
            /// Конструктор мiста
            /// Створює служби й включає спостереження
            /// за подiями
            /// </summary>
            /// <param name="name">назва мiста</param>
            /// <param name="buildings">число будинкiв</param>
            /// <param name="days">число днiв спостереження</param>
            public NewTown(string name, int buildings, int days)
            {
                townname = name;
                this.buildings = buildings;
                this.days = days;
                fireprobability = 1e-3;
                //Створення служб
                policeman = new Police(this);
                ambulanceman = new Ambulance(this);
                fireman = new FireDetect(this);
                //Пiдключення до спостереження за подiями
                policeman.On();
                ambulanceman.On();
                fireman.On();
            }
            /// <summary>
            /// Запалюється подiя.
            /// По черзi викликаються оброблювачi подiї 
            /// </summary>
            /// <param name="e">
            /// вхiднi й вихiднi аргументи подiї
            /// </param>
            protected virtual void OnFire(FireEventArgs e)
            {
                const string MESSAGE_FIRE =
                "У мiстi {0} пожежа! Будинок {1}. День {2}-й";
                Console.WriteLine(string.Format(MESSAGE_FIRE, townname,
                e.Building, e.Day));
                if (Fire != null)
                {
                    Delegate[] eventhandlers =
                    Fire.GetInvocationList();
                    resultservice = new string[eventhandlers.Length];
                    int k = 0;
                    foreach (FireEventHandler evhandler in
                    eventhandlers)
                    {
                        evhandler(this, e);
                        resultservice[k++] = e.Result;
                    }
                }
            }

            public void LifeOurTown()
            {
                const string OK =
                "У мiстi {0} усi спокiйно! Пожеж не було.";
                bool wasfire = false;
                for (int day = 1; day <= days; day++)
                    for (int building = 1; building <= buildings; building++)
                    {
                        if (rnd.NextDouble() < fireprobability)
                        {
                            FireEventArgs e = new FireEventArgs(building, day);
                            OnFire(e);
                            wasfire = true;
                            for (int i = 0; i < resultservice.Length; i++)
                                Console.WriteLine(resultservice[i]);
                        }
                    }
                if (!wasfire)
                    Console.WriteLine(string.Format(OK, townname));
            }
        }
        public abstract class Receiver
        {
            protected NewTown town;
            protected Random rnd = new Random();
            public Receiver(NewTown town)
            { this.town = town; }
            public void On()
            {
                town.Fire += new FireEventHandler(It_is_Fire);
            }
            public void Off()
            {
                town.Fire -= new FireEventHandler(It_is_Fire);
            }
            public abstract void It_is_Fire(object sender, FireEventArgs e);
        }//class Receiver
        public class Police : Receiver
        {
            public Police(NewTown town) : base(town) { }
            public override void It_is_Fire(object sender, FireEventArgs e)
            {
                const string OK =
                "Мiлiцiя знайшла винних!";
                const string NOK =
                "Мiлiцiя не знайшла винних! Наслiдок триває.";
                if (rnd.Next(0, 10) > 6)
                    e.Result = OK;
                else e.Result = NOK;
            }
        }// class Police
        public class FireDetect : Receiver
        {
            public FireDetect(NewTown town) : base(town) { }
            public override void It_is_Fire(object sender, FireEventArgs e)
            {
                const string OK =
                "Пожежнi згасили пожежу!";
                const string NOK =
                "Пожежа триває! Потрiбна допомога.";
                if (rnd.Next(0, 10) > 4)
                    e.Result = OK;
                else e.Result = NOK;
            }
        }// class Firedetect
        public class Ambulance : Receiver
        {
            public Ambulance(NewTown town) : base(town) { }
            public override void It_is_Fire(object sender, FireEventArgs e)
            {
                const string OK =
                "Швидка надала допомогу!";
                const string NOK =
                " Є постраждалi! Потрiбнi лiки.";
                if (rnd.Next(0, 10) > 2)
                    e.Result = OK;
                else e.Result = NOK;
            }
        }// class Ambulance
        /// <summary>
        /// Клас, що задає вхiднi й вихiднi аргументи подiї
        /// </summary>
        public class FireEventArgs : EventArgs
        {
            int building;
            int day;
            string result;
            //Доступ до вхiдних i вихiдних аргументiв
            public int Building
            { get { return building; } }
            public int Day
            { get { return day; } }
            public string Result
            {
                get { return result; }
                set { result = value; }
            }
            public FireEventArgs(int building, int day)
            {
                this.building = building; this.day = day;
            }
        }//class Fireeventargs

        
            static void task2()
            {
                Console.WriteLine("Lab#9  or  Lab#10");
                Console.WriteLine("Hello World!");
                NewTown sometown = new NewTown("Канск", 20, 100);
                sometown.LifeOurTown();
            }
        
    }

}