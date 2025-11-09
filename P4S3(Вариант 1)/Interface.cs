using System;
using static System.Console;
namespace P4S3
{
    internal class Interfaces{
        public static int  Count_of_select_element = 2;
        
        public static string[] list1 = new string[] {
                " task 1 ",
                " task 2 "
            };
        public static string[] list2 = new string[] {
                "последовательность месяцев с длиной строки n      ",
                "выбрать только летние и зимние месяцы             ",
                "вывести названия месяцев в алфавитном порядке     ",
                "с «u» в названии и длиной имени не менее 4(кол-во)",
                "Показать массив выборки                           ",
                "назад<-                                           "
            };
        public static string[] list3 = new string[] {
                "вывести список дат для заданного года       ",
                "вывести список дат в заданном месяце        ",
                "вывести кол-во дат в определѐнном диапазоне ",
                "вывести максимальную дату                   ",
                "вывести первую дату для заданного дня       ",
                "вывести список дат (по возр-ию или по уб-ию)",
                "Показать массив выборки                     ",
                "назад<-                                     "
            };
        public static void Menu(string[] list)
        {
            Int32 x = 60, lastWidth = x;
            SetWindowSize(x, 35);
            int num = 1, lastnum = num;
            Update_interface(list, x, num);
            while (true)
            {
                try
                {
                
                    SetCursorPosition(0, 0);
                    Count_of_select_element = list.Count();
                    if (KeyAvailable)
                    {
                        var key = ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.Enter:
                                Update_interface(list, x, num);
                                SetCursorPosition(0, 0);
                                if (list[num-1].Contains("назад<-"))//назад
                                {
                                    
                                    return;
                                }
                                else if (list == list2){//действие
                                    SetCursorPosition(0, Count_of_select_element+1);
                                    Write("╟");
                                    for (int i = 0; i < x - 2; i++) Write("─");
                                    Write("╢\n");
                                    Program.Task1(num, x);
                                }
                                else if (list == list3 ){
                                    SetCursorPosition(0, Count_of_select_element+1);
                                    Write("╟");
                                    for (int i = 0; i < x - 2; i++) Write("─");
                                    Write("╢\n");
                                    Program.Task2(num, x);
                                }
                                else if(list == list1 && num == 1){//списки действий
                                    Menu(list2);
                                    num = 2;
                                }
                                else if (list == list1 && num == 2){
                                    Menu(list3);
                                    num = 1;
                                }
                                break;
                            case ConsoleKey.Escape:
                                return;
                            case ConsoleKey.DownArrow:
                                num = (num + 1 > Count_of_select_element) ? 1 : num + 1;
                                break;
                            case ConsoleKey.UpArrow:
                                num = (num - 1 < 1) ? Count_of_select_element : num - 1;
                                break;
                        }
                    }
                    if (WindowWidth != lastWidth || lastnum != num)
                    {
                        lastnum = num;
                        lastWidth = x;
                        x = WindowWidth > list[0].Length+2 ? WindowWidth : list[0].Length+2;
                        Update_interface(list, x, num);
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    return;
                }
            }
        }
        static void Update_interface(string[] list, int x, int num)
        {

            string title = "use (↑/↓/Enter)";
            int xmin, interval1, interval2, Num_of_sentence = 1;
            ConsoleColor Fcolor = ConsoleColor.Black,Bcolor = ConsoleColor.White ; 
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
            Action<int, int> mark = (num, pos) =>
            {
                if (num == pos) { BackgroundColor = Fcolor; ForegroundColor = Bcolor; }
            };
            Action off = () =>
            {
                BackgroundColor = Bcolor; ForegroundColor = Fcolor;
            };
            string str = new string(' ', x - list[0].Length-2);
            foreach (var sentence in list)
            {
                Write("║"); mark(num, Num_of_sentence); Write(sentence); Write($"{str}"); off(); WriteLine("║");
                Num_of_sentence++;
            }
            Write("╚");
            for (int i = 0; i < x - 2; i++)
            {
                Write("═");
            }
            Write("╝\n");
            SetCursorPosition(0, 0);
        }
        public static void LowWall()
        {
            int x = WindowWidth;
            Write("\n╚");
            for (int i = 0; i < x - 2; i++)
            {
                Write("═");
            }
            Write("╝");

        }
        public static void BackWall()
        {
            Write("║"); SetCursorPosition(WindowWidth-1, GetCursorPosition().Top); Write("║"); SetCursorPosition(1,GetCursorPosition().Top);
        }
    }
}