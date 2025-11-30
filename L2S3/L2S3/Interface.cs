using System;
using System.Drawing;
using System.Xml.Linq;
using static System.Console;
namespace L2S3
{
    internal class Interfaces{
        private static int Count_of_select_element = 0, xmin = 30, lasttop = 0, IntData = -1;
        private static DateTime DateTimeData;
        private static string StringData;
        private static Specialization SpecializationData;
        private static Placement PlacementData;
        private static Work WorkData;
        private static TimeSpan TimeSpanData;
        public static ConsoleColor Fcolor = ConsoleColor.Black, Bcolor = ConsoleColor.Blue;
        public static List<Person> peoples = new List<Person>{
            new Person("Рамзан", 22,  new DateTime(2004, 11, 25)),
            new Officer("Ахмед", 26 ,new DateTime(2001, 11, 25),new DateTime(2004, 11, 25),new DateTime(2004, 11, 25), Placement.Russia, "A2"),
            new Student("Егор", 18 ,new DateTime(2001, 11, 25),2 ,4, Specialization.law, "A1"),
            new Worker("Давид", 19 ,new DateTime(2001, 11, 25),new TimeSpan(10,0, 0),8, Work.Programmer, "A1"),
            };
        private static string[] list1 = new string[] {
                " \"Enter\"-Развернуть",
                " \"N\"-Добавить ",
                " \"D\"-Удалить ",
                " \"Q\"-Выход "
            };
        private static string[] list2 = new string[] {
                " \"V\" Показать пароль ",
                " \"Enter\" Изменить поле ",
                " \"C\" Изменить пароль ",
                " \"Q\" назад "
            };
        public static void Menu1()
        {
            Int32 x = 60, lastWidth = x;
            SetWindowSize(x, 35);
            int num = 1, lastnum = num;
            Update_interface(x, num, peoples, list1);
            while (true)
            {
                try
                {
                    SetCursorPosition(0, 0);
                    Count_of_select_element = peoples.Count();
                    if (KeyAvailable)
                    {
                        var key = ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.Enter:
                                if(num - 1 >= 0 && peoples.Count() > num - 1&&
                                    PasswordMenu(x, peoples[num - 1]))
                                {
                                    Menu2(x, peoples[num]);
                                }
                                else
                                {
                                    
                                }
                                    break;
                            case ConsoleKey.Q:
                                return;
                            case ConsoleKey.DownArrow:
                                num = (num + 1 > Count_of_select_element) ? 1 : num + 1;
                                break;
                            case ConsoleKey.UpArrow:
                                num = (num - 1 < 1) ? Count_of_select_element : num - 1;
                                break;
                            case ConsoleKey.N:
                                peoples.Add((Person)AddMenu(x));
                                Update_interface(x, num, peoples, list1);
                                break;
                            case ConsoleKey.D:
                                if (num - 1 >= 0 && peoples.Count()>num-1) { peoples.RemoveAt(num - 1);
                                Update_interface(x, num, peoples, list1); }
                                break;
                        }
                    }
                    if (WindowWidth != lastWidth || lastnum != num)
                    {
                        lastnum = num;
                        lastWidth = x;
                        x = WindowWidth > xmin+2 ? WindowWidth : xmin+2;
                        Update_interface(x, num, peoples, list1);
                    }
                    Thread.Sleep(30);
                }
                catch (ArgumentException ex)
                {
                    WriteLine($"Ошибка: {ex.Message}");
                    return;
                }
            }
        }
        private static void Menu2(int x, Person person)
        {
            int lastWidth = x; 
            Update_interface2(x, person, list2);
            while (true)
            {
                try
                {
                    SetCursorPosition(0, 0);
                    Count_of_select_element = peoples.Count();
                    if (KeyAvailable)
                    {
                        var key = ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.Enter:
                                break;
                            case ConsoleKey.Q:
                                return;
                            case ConsoleKey.N:
                                break;
                            case ConsoleKey.D:
                                break;
                        }
                    }
                    if (WindowWidth != lastWidth)
                    {
                        lastWidth = x;
                        x = WindowWidth > xmin + 2 ? WindowWidth : xmin + 2;
                        Update_interface2(x, person, list2);
                    }
                    Thread.Sleep(30);
                }
                catch (ArgumentException ex)
                {
                    WriteLine($"Ошибка: {ex.Message}");
                    return;
                }
            }

        }
        static void Update_interface(int x, int num, List<Person> peoples, string[] list)
        {
            string title = "use (↑/↓/Keyboard)";
            int interval1, interval2, Num_of_sentence = 1;
            xmin = title.Length+2;
            ForegroundColor = Fcolor;
            BackgroundColor = Bcolor;
            Clear();
            SetCursorPosition(0, 0);
            Write("╔");//║
            if ((x - xmin) % 2 == 0)
            {
                interval1 = interval2 = (x - xmin) / 2;
            }
            else
            {
                interval1 = (x - xmin) / 2;
                interval2 = interval1 + 1;
            }
            for (int i = 0; i < interval1; i++)
            {
                Write("═");
            }
            Write(title);
            for (int i = 0; i < interval2; i++)
            {
                Write("═");
            }
            WriteLine("╗");
            int t = 0;
            string line = "";
            while (t < list.Count() && x >= 30)
            {
                if (t < list.Count() && x - 2 - line.Length >= list[t].Length)
                {
                    line += list[t];
                    t++;
                }
                else
                {
                    BackWall(); WriteLine(line);
                    line = "";
                }

            }
            BackWall(); WriteLine(line);
            SeparateWall();
            foreach (var sentence in peoples)
            {
                Separator(sentence.ToString(),x,Num_of_sentence==num);
                Num_of_sentence++;
            }
            Write("╚");
            for (int i = 0; i < x - 2; i++)
            {
                Write("═");
            }
            Write("╝\n");
            lasttop = GetCursorPosition().Top - 1;
            SetCursorPosition(0, 0);
        }
        static void Update_interface2(int x, Person people, string[] list)
        {
            string title = "use (↑/↓/Keyboard)";
            int interval1, interval2;
            xmin = title.Length + 2;
            ForegroundColor = Fcolor;
            BackgroundColor = Bcolor;
            Clear();
            SetCursorPosition(0, 0);
            Write("╔");//║
            if ((x - xmin) % 2 == 0)
            {
                interval1 = interval2 = (x - xmin) / 2;
            }
            else
            {
                interval1 = (x - xmin) / 2;
                interval2 = interval1 + 1;
            }
            for (int i = 0; i < interval1; i++)
            {
                Write("═");
            }
            Write(title);
            for (int i = 0; i < interval2; i++)
            {
                Write("═");
            }
            WriteLine("╗");
            int t = 1;
            string line = new string(list[0]);
            while (t < list.Count() && x>= 30)
            {
                if (t  < list.Count() && x - 2 - line.Length >= list[t].Length)
                {
                    line += list[t];
                    t++;
                }
                else
                {
                    BackWall(); WriteLine(line);
                    line = "";
                }
                
            }
            BackWall(); WriteLine(line);
            SeparateWall();
            Write("╚");
            for (int i = 0; i < x - 2; i++)
            {
                Write("═");
            }
            Write("╝\n");
            lasttop = GetCursorPosition().Top - 1;
            SetCursorPosition(0, 0);
        }
        static bool PasswordMenu (int x, Person people)
        {
            SetCursorPosition(0, lasttop);
            SeparateWall();
            string str = new string(' ', x);
            while (true)
            {
                if (KeyAvailable)
                {
                    var key = ReadKey(true).Key;
                    if (key == ConsoleKey.Q) return false;
                }
                UserData(" Введите пароль: ", 2, x, lasttop+1);
                    if (people.ComparePasswords(new Password(StringData)))
                    {
                        return true;
                    }
                    else
                    {
                        SetCursorPosition(0, lasttop+1);
                        WriteLine(str);
                        SetCursorPosition(0, lasttop+1);
                        BackWall();
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("Неверный пароль");
                        ForegroundColor = Fcolor;
                        Thread.Sleep(350);
                        SetCursorPosition(0, lasttop + 1);
                        Write(str);
                    }
            }
            
        }
        static object AddMenu(int x)
        {
            SetCursorPosition(0, lasttop);
            SeparateWall();
            string str = new string(' ', WindowWidth);
            int selPerson = 1, lastselPerson = selPerson;
            Action<int, int> mark = (num, pos) =>
            {
                if (num == pos) { BackgroundColor = Fcolor; ForegroundColor = Bcolor; }
            };
            Action off = () =>
            {
                BackgroundColor = Bcolor; ForegroundColor = Fcolor;
            };
            string[] items = new string[] {
                    "Student", "Officer", "Worker", "None"
                };
            Action<int> sellist = (num) =>
            {
                int number=1;
                SetCursorPosition(0, lasttop+1);
                WriteLine(str);
                SetCursorPosition(0, lasttop + 1);
                BackWall();
                
                foreach (string item in items)
                {
                    if (num == number) { BackgroundColor = Fcolor; ForegroundColor = Bcolor; }
                    Write(item); 
                    off();
                    Write(' ');
                    number++;
                }
                LowWall();
            };
            Count_of_select_element = items.Count(); bool braked = false;
            sellist(selPerson);
            while (true)
            {
                if (KeyAvailable)
                {
                    var key = ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            SetCursorPosition(0, lasttop + 2);
                            SeparateWall();
                            SetCursorPosition(0, lasttop+3);
                            braked = true;
                            break;
                        case ConsoleKey.RightArrow:
                            selPerson = (selPerson + 1 > Count_of_select_element) ? 1 : selPerson + 1;
                            break;
                        case ConsoleKey.LeftArrow:
                            selPerson = (selPerson - 1 < 1) ? Count_of_select_element : selPerson - 1;
                            break;
                    }
                    if (lastselPerson != selPerson)
                    {
                        lastselPerson = selPerson;
                        sellist(selPerson);
                    }
                    if (braked)
                    {
                        break;
                    }
                }
            }
            int Course=0, Semester=0, Salary=0;
            TimeSpan StartTime=new TimeSpan();
            DateTime BirthDay = DateTimeData,StartDate = new DateTime(), EndDate = new DateTime();
            Placement placement = 0;
            Specialization specialization = 0;
            Work Profession = 0;
            UserData(" Имя: ", 2, x, lasttop + 3); 
            string Name = StringData;
            UserData(" Возраст: ", 0, x, lasttop + 5, 3);
            int Age = IntData;
            UserData(" Дата Рождения ", 1, x, lasttop + 7);
            
            
            switch (selPerson)
            {
                case 1:
                    {
                        UserData(" Курс: ", 0, x, lasttop + 9, 0);
                        Course = IntData;
                        UserData(" Семестр: ", 0, x, lasttop + 11, 1 , Course);
                        Semester = IntData;
                        UserData(" Специализация: ", 3, x, lasttop + 13);
                        specialization = SpecializationData;
                        break;
                    }
                case 2:
                    {
                        UserData(" Дата начала службы: ", 1, x, lasttop + 9);
                        StartDate = DateTimeData;
                        UserData(" Дата окончания службы: ", 1, x, lasttop + 11);
                        EndDate = DateTimeData;
                        UserData(" Место службы: ", 4, x, lasttop + 13);
                        placement = PlacementData;
                        break;
                    }
                case 3:
                    {
                        UserData(" Зарплата: ", 0, x, lasttop + 9,2);
                        Salary = IntData;
                        UserData(" Начало рабочего дня: ",6, x, lasttop + 11);
                        StartTime = TimeSpanData;
                        UserData(" Профессия: ", 5, x, lasttop + 13);
                        Profession = WorkData;
                        break;
                    }
                default: break;
                
            }
            if (selPerson == 4) { lasttop -= 6; }
            string pasw1,pasw2;
            Password password1 = new Password(), password2;
            while (true)
            {
                UserData(" Придумайте пароль: ", 2, x, lasttop + 15);
                password1 = new Password(StringData);
                SetCursorPosition(0, lasttop + 16);
                Write(str);
                SetCursorPosition(0, lasttop + 16);BackWall();
                if (password1.IsStrong())
                {
                    BackgroundColor = ConsoleColor.Black;
                    ForegroundColor = ConsoleColor.Green; WriteLine(" Надежный пароль "); ForegroundColor = Fcolor;
                    BackgroundColor = ConsoleColor.Blue;
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red; WriteLine(" Ненадежный пароль! "); ForegroundColor = Fcolor;
                }
                UserData(" Повторите пароль: ", 2, x, lasttop + 17);
                password2 = new Password(StringData);
                SetCursorPosition(0, lasttop + 18);
                Write(str);
                SetCursorPosition(0, lasttop + 18); BackWall();
                if (password1.Equals(password2))
                {
                    BackgroundColor = ConsoleColor.Black;
                    ForegroundColor = ConsoleColor.Green;
                    Write("Человек успешно добавлен!");
                    ForegroundColor = Fcolor;
                    BackgroundColor = ConsoleColor.Blue;
                    LowWall();
                    Thread.Sleep(350);
                    break ;
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Пароли не совпадают! Повторите попытку");
                    ForegroundColor = Fcolor;
                    LowWall();
                    Thread.Sleep(350);
                    SetCursorPosition(0, lasttop + 15);
                    for(int i =0; i < 15; i++)
                    {
                        SetCursorPosition(0, lasttop + 15 + i);
                        Write(str);
                    }
                    SetCursorPosition(0, lasttop + 15);
                    continue;
                }
            }
            switch (selPerson)
            {
                case 1:
                    {
                        return new Student(Name,Age,BirthDay,Course,Semester,specialization, password1.Value);
                    }
                case 2:
                    {
                        return new Officer(Name, Age, BirthDay, StartDate, EndDate, placement, password1.Value);
                    }
                case 3:
                    {
                        return new Worker(Name, Age, BirthDay,StartTime,Salary, Profession, password1.Value);
                    }
                case 4:
                    {
                        lasttop += 6;
                        return new Person(Name, Age, BirthDay,password1.Value);
                    }
                default: return new Person();
            }

        }
        static void UserData(string message, int mode, int x, int y, int CompareMode=0, int course = 1)
        {
            int num;
            DateTime date;
            Specialization spec;
            Placement placement;
            Work work;
            TimeSpan timespan;
            string str = new string(' ', x);
            Func<int, int, int, bool> config = (x, mode, course) =>
            {
                switch (mode)
                {
                    case 0:
                        return x > 0 && x <= 5;
                    case 1:
                        return course * 2 == x || course * 2 - 1 == x;
                    case 2:
                        return x >= 0;
                    case 3:
                        return x > 0 && x < 160;
                    default: return true;
                }
            };
            while (true)
            {
                SetCursorPosition(0, y);
                BackWall(); Write(message);
                int curx = GetCursorPosition().Left, cury = GetCursorPosition().Top;
                LowWall();
                SetCursorPosition(curx, cury);
                string input = ReadLine();
                switch (mode)
                {
                    case 0:
                        {
                            if (int.TryParse(input, out num) && config(num,CompareMode,course))
                            {
                                IntData = num;
                                SetCursorPosition(0, y + 1);
                                SeparateWall();
                                return;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (DateTime.TryParse(input, out date))
                            {
                                DateTimeData = date;
                                SetCursorPosition(0, y+1);
                                SeparateWall();
                                return;
                            }
                            break;
                        }
                    case 2:
                        {
                            if (input != null && input != "")
                            {
                                StringData = input;
                                SetCursorPosition(0, y + 1);
                                SeparateWall();
                                return;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (Specialization.TryParse(input, out spec))
                            {
                                SpecializationData = spec;
                                SetCursorPosition(0, y + 1);
                                SeparateWall();
                                return;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (Placement.TryParse(input, out placement))
                            {
                                PlacementData = placement;
                                SetCursorPosition(0, y + 1);
                                SeparateWall();
                                return;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (Work.TryParse(input, out work))
                            {
                                WorkData = work;
                                SetCursorPosition(0, y + 1);
                                SeparateWall();
                                return;
                            }
                            break;
                        }
                    case 6:
                        {
                            if (TimeSpan.TryParse(input, out timespan))
                            {
                                TimeSpanData = timespan;
                                SetCursorPosition(0, y + 1);
                                SeparateWall();
                                return;
                            }
                            break;
                        }
                    default: throw new ArgumentException("Режим userDate некорректен");
                }
                SetCursorPosition(0, y);
                WriteLine(str);
                SetCursorPosition(0, y);
                BackWall();
                ForegroundColor = ConsoleColor.Red;
                WriteLine("Неверный ввод!");
                ForegroundColor = Fcolor; 
                Thread.Sleep(350);
                SetCursorPosition(0, y);
                Write(str);

            }
        }
        public static void LowWall()
        {
            int x = WindowWidth;
            Write("\n╚");
            for (int i = 0; i < x - 2; i++)
            {
                Write("═");
            }
            Write("╝\n");

        }
        public static void SeparateWall()
        {
            int x = WindowWidth;
            Write("╟");
            for (int i = 0; i < x - 2; i++)
            {
                Write("─");
            }
            Write("╢\n");
        }
        public static void BackWall()
        {
            Write("║"); SetCursorPosition(WindowWidth-1, GetCursorPosition().Top); Write("║"); SetCursorPosition(1,GetCursorPosition().Top);
        }
        public static void Separator(string sentence,int x, bool q)
        {
            BackWall(); if (q) { BackgroundColor = Fcolor; ForegroundColor = Bcolor; }
            int m = 0, j = 0, Length = sentence.ToString().Length;
            while (Length != 0)
            {
                if (m == x - 3)
                {
                    BackgroundColor = Bcolor; ForegroundColor = Fcolor;
                    WriteLine();
                    BackWall();
                    if (q) { BackgroundColor = Fcolor; ForegroundColor = Bcolor; }
                    m = 0;
                }
                Write(sentence.ToString()[j]);
                j++; m++; Length--;
            }
            string str = new string(' ', x - 2 - m);
            WriteLine($"{str}");
            BackgroundColor = Bcolor; ForegroundColor = Fcolor;
        }

    }
}