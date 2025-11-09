using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace L1S3
{
    class Programm
    {
        static ConsoleColor color = ConsoleColor.DarkYellow;
        static ConsoleColor fcolor = ConsoleColor.Black;
        static int xmin = 34;
        public static int x;
        public static int X_end_of_Inteface = 5;
        static void Main()
        {
            Title = "<Test programm for Appliance>";
            OutputEncoding = Encoding.Unicode;
            SetBufferSize(500, 500);
            x = 50;
            int lastWidth = x;
            SetWindowSize(x, 30);
            int num = 1, lastnum = num;
            Interface(x, num);
            Realize obj  = new Realize();
            while (true)
            {
                try
                {
                    if (KeyAvailable)
                    {
                        var key = ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.Enter:
                                Interface(x, num);
                                SetCursorPosition(0, X_end_of_Inteface-1);
                                Write("╟");
                                for (int i = 0; i < x - 2; i++)
                                {
                                    Write("─");
                                }
                                Write("╢\n");
                                string str = new string(' ', x);
                                SetCursorPosition(x, X_end_of_Inteface);
                                action(num, obj, x, X_end_of_Inteface);
                                break;
                            case ConsoleKey.Escape:
                                return;
                            case ConsoleKey.DownArrow:
                                num = (num + 1 > 3) ? 1 : num + 1;
                                break;
                            case ConsoleKey.UpArrow:
                                num = (num - 1 < 1) ? 3 : num - 1;
                                break;
                        }
                    }
                    if (WindowWidth != lastWidth || (lastnum != num))
                    {
                        lastnum = num;
                        x = WindowWidth;
                        lastWidth = x;
                        x = WindowWidth > xmin ? WindowWidth : xmin;
                        Interface(x, num);
                    }
                }
                catch (ArgumentException ex)
                {
                    WriteLine($"{ex.Message}");
                    return;
                }
            }
        }
        static void Interface(int x, int num)
        {
            ForegroundColor = fcolor;
            BackgroundColor = color;
            Clear();
            SetCursorPosition(0, 0);
            Write("╔");//║
            int interval1, interval2;
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
            Write("Testing program |use (↑/↓/Enter)");
            for (int i = 0; i < interval2; i++)
            {
                Write("═");
            }
            WriteLine("╗");
            string[] list = new string[] {
                " 1. Показать всею технику       ",
                " 2. Добавить новую технику      ",
                " 3. удалить технику из списка   "
            };
            Action<int, int> mark = (num, pos) =>
            {
                if (num == pos) { BackgroundColor = fcolor; ForegroundColor = color; }
            };
            Action off = () =>
            {
                BackgroundColor = color; ForegroundColor = fcolor;
            };
            string str = new string(' ', (x - 34<0)?0:x-34);
            int p = 1;
            foreach (var sentence in list)
            {
                Write("║"); mark(num, p); Write(sentence); Write($"{str}"); off(); WriteLine("║");//32 без рамок
                p++;
            }
            LowWall(x, GetCursorPosition().Top);
            SetCursorPosition(0,GetCursorPosition().Top+2);
        }
        static void action(int num, Realize obj, int x, int y)
        {
            switch (num)
            {
                case 1:
                    obj.ListOfObject();
                    break;
                case 2:
                    AddMenu(obj, x);
                    break;
                case 3:
                    string str = new string(' ', x);
                    if (obj.appliances.Count == 0) { BackWall(x, X_end_of_Inteface); LowWall(x, X_end_of_Inteface+1); Write("║Техники нет!"); return; }
                    SetCursorPosition(0, X_end_of_Inteface);
                    BCP(); Write("║Какую вещь удалить?(↑/↓ )\n");
                    int select = SelectList(obj, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == obj.appliances.Count + 1)
                    {
                        break;
                    }
                    obj.RemoveObject(select - 1);
                    LowWall(x, X_end_of_Inteface-1);
                    break;
                
                
            }
        }
        public static void BackWall(int x, int a, int n = 0)
        {
            SetCursorPosition(x - 1, a); Write("║"); SetCursorPosition(n, a);
        }
        public static void LowWall(int x, int a)
        {
            SetCursorPosition(0, a);
            Write("╚");
            for (int i = 0; i < x - 2; i++)
            {
                Write("═");
            }
            Write("╝");
            SetCursorPosition(0, a - 1);
        }
        public static void BCP()
        {
            BackWall(x, GetCursorPosition().Top);
        }
        public static void BCFC(int mode)
        {
            if (mode == 1)
            {
                BackgroundColor = fcolor; ForegroundColor = color;
            }
            else
            {
                BackgroundColor = color; ForegroundColor = fcolor;
            }
        }
        public static int SelectList(Realize obj, int x)
        {
            string str= new string(' ', x); ;
            int select = obj.appliances.Count + 1;
            SetCursorPosition(x, X_end_of_Inteface);
            obj.ListOfObject();
            SetCursorPosition(0, GetCursorPosition().Top); Write(str); SetCursorPosition(0, GetCursorPosition().Top); BackWall(x, GetCursorPosition().Top); Write("║"); BCFC(1); WriteLine("Назад <-"); BCFC(0);
            LowWall(x, GetCursorPosition().Top);
            while (true)
            {
                if (KeyAvailable)
                {
                    var key = ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            if (select == -1) return -1;
                            if (select == obj.appliances.Count + 1)
                            {
                                str = new string(' ', x);
                                SetCursorPosition(0, X_end_of_Inteface);
                                for (int i = 0; i < 50; i++)
                                {
                                    WriteLine(str);
                                }
                                LowWall(x, X_end_of_Inteface-1);
                                SetCursorPosition(0, 0);
                                SetCursorPosition(0, X_end_of_Inteface );
                                return select;
                            }
                            SetCursorPosition(0, X_end_of_Inteface );
                            str = new string(' ', x);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, X_end_of_Inteface);
                            return select;
                        case ConsoleKey.Escape:
                            return -1;
                        case ConsoleKey.DownArrow:
                            select = (select + 1 > obj.appliances.Count + 1) ? 1 : select + 1;
                            str = new string(' ', x - 1);
                            SetCursorPosition(0, X_end_of_Inteface + 1);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, X_end_of_Inteface+1);
                            obj.ListOfObject(select);
                            SetCursorPosition(0, GetCursorPosition().Top); Write(str); SetCursorPosition(0, GetCursorPosition().Top); BackWall(x, GetCursorPosition().Top); Write("║"); if (select == obj.appliances.Count + 1) { BCFC(1); }
                            WriteLine("Назад <-"); BCFC(0);
                            LowWall(x, GetCursorPosition().Top);
                            break;
                        case ConsoleKey.UpArrow:
                            select = (select - 1 < 1) ? obj.appliances.Count + 1 : select - 1;
                            SetCursorPosition(0, X_end_of_Inteface + 1);
                            str = new string(' ', x - 1);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, X_end_of_Inteface+1);
                            obj.ListOfObject(select);
                            SetCursorPosition(0, GetCursorPosition().Top); Write(str); SetCursorPosition(0, GetCursorPosition().Top); BackWall(x, GetCursorPosition().Top); Write("║"); if (select == obj.appliances.Count + 1) { BCFC(1); }
                            WriteLine("Назад <-"); BCFC(0);
                            LowWall(x, GetCursorPosition().Top);
                            break;
                    }
                }

            }
        }
        static void AddMenu(Realize obj, int x)
        {
            string str = new string(' ', WindowWidth);
            string brand = null, model = null, color = null;
            int act, count_of_programm = 1, m = X_end_of_Inteface;
            float val = 1;
            while (true)
            {
                BackWall(x, m);
                LowWall(x, m + 1);
                Write("║Техника/Посудомойка/Стиральная машина?(1/2/3): ");
                string input = ReadLine();
                if (int.TryParse(input, out act) && (act == 1 || act == 2 || act == 3))
                {
                    break;
                }
                else
                {
                    SetCursorPosition(0, m);
                    WriteLine(str);
                    SetCursorPosition(0, m);
                    Write("║Неверный ввод!");
                    Thread.Sleep(350);
                    SetCursorPosition(0, m);
                    Write(str);
                }
            }
            while (brand == null || brand == "")
            {
                SetCursorPosition(0, m + 1);
                Write(str);
                SetCursorPosition(0, m + 1);
                BackWall(x, m + 1);
                LowWall(x, m + 2);
                Write("║Brand : ");
                brand = ReadLine();
                
            }
            SetCursorPosition(0, m + 2);
            WriteLine(str);
            while (model == null || model == "")
            {
                SetCursorPosition(0, m + 2);
                Write(str);
                SetCursorPosition(0, m + 2);
                BackWall(x, m + 2);
                LowWall(x, m + 3);
                Write("║Model : ");
                model = ReadLine();

            }
            SetCursorPosition(0, m + 3);
            WriteLine(str);
            while (color == null || color == "")
            {
                SetCursorPosition(0, m + 3);
                Write(str);
                SetCursorPosition(0, m + 3);
                BackWall(x, m + 3);
                LowWall(x, m + 4);
                Write("║Color : ");
                color = ReadLine();
            }
            SetCursorPosition(0, m + 4);
            WriteLine(str);
            if (act == 2)
            {
                while (true)
                {
                    BackWall(x, m + 4);
                    LowWall(x, m + 5);
                    Write("║Сколько программ у машинки? : ");
                    string input = ReadLine();
                    if (int.TryParse(input, out count_of_programm))
                    {
                        SetCursorPosition(0, m + 5);
                        WriteLine(str);
                        SetCursorPosition(0, m + 5);
                        break;
                    }
                    else
                    {
                        SetCursorPosition(0, m + 4);
                        WriteLine(str);
                        SetCursorPosition(0, m + 4);
                        BCP();
                        WriteLine("║Неверный ввод!");
                        Thread.Sleep(350);
                        SetCursorPosition(0, m + 4);
                        Write(str);
                    }
                }
            }
            if (act == 3)
            {
                while (true)
                {
                    BackWall(x, m + 4);
                    LowWall(x, m + 5);
                    Write("║Какой объем? : ");
                    string input = ReadLine();
                    if (float.TryParse(input, out val))
                    {
                        SetCursorPosition(0, m + 5);
                        WriteLine(str);
                        SetCursorPosition(0, m + 5);
                        break;
                    }
                    else
                    {
                        SetCursorPosition(0, m + 4);
                        WriteLine(str);
                        SetCursorPosition(0, m + 4);
                        BCP();
                        WriteLine("║Неверный ввод!");
                        Thread.Sleep(350);
                        SetCursorPosition(0, m + 4);
                        Write(str);
                    }
                }
            }
            SetCursorPosition(0, X_end_of_Inteface);
            for (int i = 0; i < 30; i++)
            {
                WriteLine(str);
            }
            SetCursorPosition(0, 0);
            SetCursorPosition(0, X_end_of_Inteface);
            if (act == 1)
            {
                Appliance appliance = new Appliance(brand,model,color);
                obj.AddObject(appliance);
            }
            else if (act == 3)
            {
                WashingMachine washingMachine = new WashingMachine(brand, model, color, val);
                obj.AddObject(washingMachine);
            }
            else
            {
                Dishwasher dishwasher = new Dishwasher(brand,model, color, count_of_programm);
                obj.AddObject(dishwasher);
            }
            BCP(); WriteLine("║Добавление выполнено. выберите сл. метод");
            LowWall(x, m+1);
            SetCursorPosition(0, 0);
        }
    }
}