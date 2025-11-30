using System.Text;
using static System.Console;
using static P4S3.Interfaces;
namespace P4S3 
{ 
    class Program
    {
        static int IntData;
        static DateTime DateTimeData;
        static void Main()
        {
            Title = "TestProgramm";
            OutputEncoding = Encoding.Unicode;
            SetBufferSize(500, 500);
            Clear();
            Menu(Interfaces.list1);
            return;
        }

        
        internal static void Task1(int num, int x)
        {
            string str = new string(' ', x);
            string[] Mounths = {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "Augest",
                "September",
                "October",
                "November",
                "December"
            };
            IEnumerable<string> result1, result2;
            switch(num){
                case 1:
                {
                    while (true){
                        UserData("Введите число симвлов для сортировки по длинне строки : ", 0, x);
                        if (IntData>0) break;
                        else {
                            SetCursorPosition(0, 0);
                            WriteLine(str);
                            SetCursorPosition(0, 0);
                            WriteLine("Неверный ввод!");
                            Thread.Sleep(350);
                            SetCursorPosition(0, 0);
                            Write(str);
                        }
                    }
                    result1 = from p in Mounths where p.Length == IntData orderby p select p;
                    result2 = Mounths.Where(p => p.Length == IntData).OrderBy(p => p);
                    break;
                }
                case 2:
                {
                    string[] arr = {
                    "January",
                    "February",
                    "June",
                    "July",
                    "Augest",
                    "December"
                    };
                    result1 = from p in Mounths
                                    join r in arr on p equals r
                                    orderby p
                                    select p;
                    result2 = Mounths.Join(arr, p=>p,r=>r, (p,r)=> p);
                    //экв. var resultList4 = Mounths.Intersect(arr);
                    break;
                }
                case 3:
                {
                    result1 = from p in Mounths
                                    orderby p
                                    select p;
                    result2 = Mounths.OrderBy(p => p);
                    break;
                }
                case 4:
                {
                    result1 = from p in Mounths
                                    where (p.Contains('u') &&  p.Length >= 4)
                                    orderby p
                                    select p;
                    result2 = Mounths.Where(p => p.Contains('u') && p.Length >= 4);
                    break;
                }
                case 5: {
                    int i = 1;
                    BackWall() ;WriteLine("Все месяцы:");
                    foreach (var item in Mounths){ BackWall() ;WriteLine($"[{i}] {item}"); i++;}
                    SetCursorPosition(0,GetCursorPosition().Top-1);
                    LowWall();
                    return;
                }
                default: throw new ArgumentException("кнопка не обрабатывается");
            }
            BackWall() ;WriteLine("Результат для перовго метода");
            if (result1.Count()==0){
                BackWall() ; WriteLine("Нет дат!");
            }
            else foreach (var item in result1) {BackWall() ;WriteLine(item);}
            BackWall() ;WriteLine("Результат для второго метода");
            if (result2.Count()==0){
                BackWall() ; WriteLine("Нет дат!");
            }
            else foreach (var item in result2) { BackWall() ;WriteLine(item); }
            SetCursorPosition(0,GetCursorPosition().Top-1);
            LowWall();
            return;
        }
        internal static void Task2(int num, int x)
        {
            List<Date> date = new List<Date> {
                new Date(6, 1, 2025),
                new Date(12, 2, 2025),
                new Date(13, 3, 2024),
                new Date(18, 11, 2020),
                new Date(10, 12, 2020),
                new Date(19, 1, 2025),
                new Date(10, 2, 2025),
                new Date(10, 8, 2021),
                new Date(17, 7, 2024),
                new Date(11, 1, 2023),
                new Date(2, 12, 2023),
                new Date(10, 3, 2024),
                new Date(10, 1, 2020),
                new Date(4, 8, 2021),
                new Date(25, 4, 2025)
            };
            string str = new string (' ', 100);
            IEnumerable<Date> result1, result2;
            switch (num)
            {
                case 1:
                    {
                        while (true)
                        {
                            UserData("Введите год для перовго теста второго задания: ", 0, x);
                            if (IntData > 0) break;
                        }
                        result1 = from p in date
                                  where p.Year == IntData
                                  orderby p
                                  select p;
                        result2 = date.Where(p=>p.Year==IntData).OrderBy(p=>p);
                        break;
                    }
                case 2:
                    {
                        while (true)
                        {
                            UserData("Введите месяц для второго теста второго задания: ", 0, x);
                            if (IntData > 0 && IntData <= 12) break;
                        }
                        result1 = from p in date
                                  where p.Month == IntData
                                  orderby p
                                  select p;
                        result2 = date.Where(p=>p.Month == IntData).OrderBy(p=>p);
                        break;
                    }
                case 3:
                    {
                        Date date1, date2;
                        while (true)
                        {
                            UserData("Введите начальную дату(dd.mm.yyyy) :", 1, x);
                            date1 = new Date(DateTimeData.Day,DateTimeData.Month ,DateTimeData.Year);
                            UserData("Введите конечную дату(dd.mm.yyyy) :", 1, x);
                            date2 = new Date(DateTimeData.Day,DateTimeData.Month ,DateTimeData.Year);
                            if (date1.CompareTo(date2) == 1) { Date temp = new Date(date1.Day, date1.Month, date1.Year); date1 = date2; date2 = temp; }
                            break;
                        }
                        Func<Date,Date,Date, bool> Comparer = (p, date1, date2) =>
                        {
                            int comp1 = p.CompareTo(date1), comp2 = p.CompareTo(date2);

                            if (comp1==1 || comp1 == 0){
                                if (comp2 == -1 || comp2 == 0)
                                {
                                    return true;
                                }
                            }
                            return false;

                        };
                        result1 = from p in date
                                  where (Comparer(p, date1, date2))
                                  orderby p
                                  select p;
                        result2 = date.Where(p=> Comparer(p,date1,date2)==true).OrderBy(p=>p);
                        break;
                    }
                case 4:
                    {
                        result1 = from p in date
                                  where p == date.Max()
                                  select p;
                        result2 = date.Where(p=> p == date.Max()).OrderBy(p=>p);
                        break;
                    }
                case 5:
                    {
                        while (true)
                        {
                            UserData("Введите день: ", 0, x);
                            if (IntData > 0 && IntData <= 31) break;
                            SetCursorPosition(0, Count_of_select_element+3);
                            WriteLine(str);
                            SetCursorPosition(0, Count_of_select_element+3);
                            BackWall(); WriteLine("Неверный ввод!");
                            Thread.Sleep(350);
                            SetCursorPosition(0, Count_of_select_element+3);
                            Write(str);
                        }
                        result2 = result1  = date.OrderBy(p=>p);
                        result1 = from p in date
                                  where p.Day == IntData
                                  select p;
                        result1 = result1.Skip(0).Take(1);
                        result2 = date.Where(p=> p.Day==IntData);
                        result2 = result2.Skip(0).Take(1);
                        
                        break;
                    }
                case 6:
                    {
                        while (true)
                        {
                            UserData("По возрастанию или по убыванию?(0/1): ", 0, x);
                            if (IntData == 1 || IntData == 0) break;
                            SetCursorPosition(0, Count_of_select_element+3);
                            WriteLine(str);
                            SetCursorPosition(0, Count_of_select_element+3);
                            BackWall();WriteLine("Неверный ввод!");
                            Thread.Sleep(350);
                            SetCursorPosition(0, Count_of_select_element+3);
                            Write(str);
                        }
                        
                        if (IntData == 0){
                            result1 = from p in date orderby p select p;
                            result2 = date.OrderBy(p=>p);
                        }
                        else {
                            result1 = from p in date orderby p descending select p;
                            result2 = date.OrderByDescending(p=>p);
                        }
                        
                        
                        break;
                    }
                case 7: {
                    int i = 1;
                    BackWall() ;WriteLine("Все даты:");
                    foreach (var item in date){ BackWall() ;WriteLine($"[{i}] {item}"); i++;}
                    SetCursorPosition(0,GetCursorPosition().Top-1);
                    LowWall();
                    return;
                }
                default: throw new ArgumentException("кнопка не обрабатывается");
                

            }
            BackWall() ;WriteLine("Результат для перовго метода");
            if (result1.Count()==0){
                BackWall() ; WriteLine("Нет дат!");
            }
            else foreach (var item in result1) {BackWall() ;WriteLine(item);}
            BackWall() ;WriteLine("Результат для второго метода");
            if (result2.Count()==0){
                BackWall() ; WriteLine("Нет дат!");
            }
            else foreach (var item in result2) { BackWall() ;WriteLine(item); }
            SetCursorPosition(0,GetCursorPosition().Top-1);
            LowWall();  
            
        }
        static void UserData(string message, int mode, int x) {
            int num;
            DateTime date;
            string str = new string(' ', x);
            while (true)
            {
                SetCursorPosition(0, Count_of_select_element+2);
                BackWall(); Write(message);
                int curx = GetCursorPosition().Left, cury = GetCursorPosition().Top;
                LowWall();
                SetCursorPosition(curx,cury);
                string input = ReadLine();
                switch (mode){
                    case 0:
                    {
                        if (int.TryParse(input, out num))
                        {
                        SetCursorPosition(0, Count_of_select_element+2);
                        WriteLine($"{str}\n{str}");
                        IntData=num;
                        SetCursorPosition(0, Count_of_select_element+2);
                        return;
                        }
                        break; 
                    }
                    case 1:
                    {
                        if (DateTime.TryParse(input, out date))
                        {
                        SetCursorPosition(0, Count_of_select_element+2);
                        WriteLine($"{str}\n{str}");
                        DateTimeData=date;
                        SetCursorPosition(0, Count_of_select_element+2);
                        return;
                        }
                        break; 
                    }
                    default: throw new ArgumentException("Режим userDate некорректен");
                }
                SetCursorPosition(0, Count_of_select_element+2);
                WriteLine(str);
                SetCursorPosition(0, Count_of_select_element+2);
                BackWall() ; WriteLine("Неверный ввод!");
                Thread.Sleep(350);
                SetCursorPosition(0,Count_of_select_element+2);
                Write(str);
                
            }
        }
    }

}
