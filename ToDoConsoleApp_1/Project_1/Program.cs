using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project_1
{
    class Program
    {

        static void Main(string[] args)
        {
            List<TaskModel> tasks = new List<TaskModel>();
            string command = "";
            Console.WriteLine("Cześć, dzisiaj jest: {0}, co chciałbyś więc zrobić ?", DateTime.UtcNow.ToLocalTime());
            Console.WriteLine();
            do
            {
                Console.WriteLine("Wpisz komendę lub słowo -help- w celu wyświetlenia podpowiedzi");
                command = Console.ReadLine().ToLower();
                if (command == "add")
                {
                    tasks.Add(AddTask());
                }
                else if (command == "show")
                {
                    ShowTask(tasks);
                }
                else if (command == "save")
                {
                    SaveTask(tasks);
                }
                else if (command == "load")
                {
                    LoadTask();
                }
                else if (command == "remove")
                {
                    RemoveTask(tasks);
                }
                else if (command == "help")
                {
                    Help();
                }

            } while (command != "exit");

            Console.ReadKey();
        }

        private static void Help()
        {
            Console.WriteLine();
            Console.WriteLine("add    - dodaj nowe zadanie.");
            Console.WriteLine("show   - wyświetl moje zadania.");
            Console.WriteLine("save   - zapisz moje zadania.");
            Console.WriteLine("load   - wczytaj moje zadania.");
            Console.WriteLine("remove - usuń zadanie.");
            Console.WriteLine("help   - wyświetl pomoc.");
            Console.WriteLine("exit   - zamknij program.");
            Console.WriteLine();
        }

        private static void RemoveTask(List<TaskModel> tasks)
        {
            ConsoleEx.WriteLine("Które zadanie chcesz usunąć?", ConsoleColor.Red);

            for (int i = 0; i < tasks.Count; i++)
            {
                ConsoleEx.WriteLine("".PadLeft(20, '-'), ConsoleColor.DarkCyan);
                Console.WriteLine($"{i + 1} : {tasks.ElementAt(i).Task}");
                ConsoleEx.WriteLine("".PadLeft(20, '-'), ConsoleColor.DarkCyan);
            }

            int number = 0;
            do
            {
                ConsoleEx.Write("Podaj numer zadania: ", ConsoleColor.Red);
                number = Convert.ToInt32(Console.ReadLine());
                if (number < 0 && number >= tasks.Count)
                {
                    ConsoleEx.WriteLine("Wprowadzony numer zadania jest poza zakresem", ConsoleColor.Red);
                    break;
                }
                else break;
            } while (true);

            ConsoleEx.Write("Czy napewno chcesz usunąć to zadanie? [T/N]", ConsoleColor.Red);

            do
            {
                string answer = Console.ReadLine().ToLower();
                if (answer == "t")
                {
                    tasks.RemoveAt(number - 1);
                    Console.WriteLine($"Skasowano zadanie numer: {number}");
                    break;
                }
                else if (answer == "n")
                {
                    break;
                }
                else
                {
                    ConsoleEx.Write("Zła odpowiedz. Wpisz [T/N]", ConsoleColor.Red);
                }
            } while (true);
        }

        private static void LoadTask()
        {
            List<TaskModel> tasks = new List<TaskModel>();
            string[] text = File.ReadAllLines("Data.csv");
            for (int i = 0; i < text.Length; i++)
            {
                string[] example = text[i].Split(',');
                if (example.Length == 5 && example[2] != " ")
                {
                    TaskModel task = new TaskModel(example[0], Convert.ToDateTime(example[1]), Convert.ToDateTime(example[2]), example[3]);
                    tasks.Add(task);
                }
                else
                {
                    TaskModel task = new TaskModel(example[0], Convert.ToDateTime(example[1]), example[3]);
                    tasks.Add(task);
                }
            }
            ShowTask(tasks);
        }

        private static void SaveTask(List<TaskModel> tasks)
        {
            string[] tab = new string[tasks.Count];
            for (int i = 0; i < tasks.Count; i++)
            {
                string text;
                text = $"{tasks.ElementAt(i).Task}, {tasks.ElementAt(i).DateStart.ToString()}, {tasks.ElementAt(i).DateEnd.ToString()}, {tasks.ElementAt(i).Priority},{tasks.ElementAt(i).TaskDuration}";

                tab[i] += text;
            }
            File.WriteAllLines("Data.csv", tab, Encoding.Unicode);
            ConsoleEx.WriteLine("Plik został zapisany", ConsoleColor.Green);
        }

        private static void ShowTask(List<TaskModel> tasks)
        {
            Print1("Opis zadania", "Data Rozpoczęcia", "Data Zakończenia", "Ważność", "Czas Trwania");

            foreach (TaskModel task in tasks)
            {
                if (!task.DateEnd.HasValue)
                {
                    Print(task.Task, task.DateStart.ToString(), task.DateEnd.ToString(), task.Priority, task.TaskDuration);
                }
                else
                {
                    Print1(task.Task, task.DateStart.ToString(), task.DateEnd.ToString(), task.Priority, task.TaskDuration);
                }
            }

            void Print(string task, string dateStart, string dateEnd, string priority, string taskDuration)
            {
                Console.Write(task.PadLeft(30));
                ConsoleEx.Write("|", ConsoleColor.DarkRed);
                Console.Write(dateStart.PadLeft(20));
                ConsoleEx.Write("|", ConsoleColor.DarkRed);
                Console.Write(dateEnd.PadLeft(20));
                ConsoleEx.Write("|", ConsoleColor.DarkRed);
                Console.Write(priority.PadLeft(10));
                ConsoleEx.Write("|", ConsoleColor.DarkRed);
                Console.Write(taskDuration.PadLeft(20));
                ConsoleEx.WriteLine("|", ConsoleColor.DarkRed);
                ConsoleEx.WriteLine("".PadLeft(120, '-'), ConsoleColor.DarkRed);
            }
            void Print1(string task, string dateStart, string dateEnd, string priority, string taskDuration)
            {
                Console.Write(task.PadLeft(30));
                ConsoleEx.Write("|", ConsoleColor.Green);
                Console.Write(dateStart.PadLeft(20));
                ConsoleEx.Write("|", ConsoleColor.Green);
                Console.Write(dateEnd.PadLeft(20));
                ConsoleEx.Write("|", ConsoleColor.Green);
                Console.Write(priority.PadLeft(10));
                ConsoleEx.Write("|", ConsoleColor.Green);
                Console.Write(taskDuration.PadLeft(20));
                ConsoleEx.WriteLine("|", ConsoleColor.Green);
                ConsoleEx.WriteLine("".PadLeft(120, '-'), ConsoleColor.Green);
            }
        }

        private static TaskModel AddTask()
        {
            TaskModel task = null;
            ConsoleEx.Write("Dodaj zadanie do swojego kalendarza: ", ConsoleColor.DarkBlue);
            string a = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(a))
            {
                bool check = false;
                do
                {
                    ConsoleEx.WriteLine("Podaj datę rozpoczęcia zadania w formacie: [DD/MM/RRRR]", ConsoleColor.DarkBlue);
                    string dateStart = Console.ReadLine();

                    if (DateTime.TryParse(dateStart, out DateTime Temp) == true)
                    {
                        DateTime parsedDateS = DateTime.Parse(dateStart);
                        ConsoleEx.WriteLine("Podaj przewidywaną date zakończenia zadania w formacie: [DD/MM/RRRR]", ConsoleColor.DarkBlue);
                        string dateEnd = Console.ReadLine();
                        if (DateTime.TryParse(dateEnd, out DateTime Tempp) == true)
                        {
                            DateTime parsedDateE = DateTime.Parse(dateEnd);
                            ConsoleEx.WriteLine("Podaj priorytet zadania: [Important]/[Not Important]",ConsoleColor.DarkBlue);
                            string priority = Console.ReadLine().ToUpper();
                            task = new TaskModel(a, parsedDateS, parsedDateE, priority);
                            check = true;
                        }
                        else
                        {
                            ConsoleEx.WriteLine("Podaj priorytet zadania: [Important]/[Not Important]",ConsoleColor.DarkBlue);
                            string priority = Console.ReadLine().ToUpper();
                            task = new TaskModel(a, parsedDateS, priority);
                            check = true;
                        }
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Wpisz poprawną datę w formacie: [DD/MM/RRR]",ConsoleColor.Red);
                    }
                } while (check != true);
            }

            if (task == null)
            {
                Console.WriteLine("Nie dodałeś żadnego zadania");
                return task;
            }
            return task;
        }
    }
}




