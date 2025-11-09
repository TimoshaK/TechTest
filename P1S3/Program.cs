using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Authentication;


class Files
{
    public string name { get; set; }
    public long size { get; set; }
    public string editData { get; set; }
    public string editTime { get; set; }
    public bool catalog { get; set; }
    public string lastname { get; set; }

    public Files(string Name, long Size, string EditData, string EditTime, bool Catalog = false)
    {
        if (string.IsNullOrEmpty(Name)) name = "No Name";
        else name = Name;
        if (Name.Contains('.'))
        {
            string namePart = Path.GetFileNameWithoutExtension(Name);
            name= string.IsNullOrEmpty(namePart) ? "No Name" + Path.GetExtension(Name) : Name;
        }
        size = Size < 0 ? 0 : Size;
        editData = string.IsNullOrEmpty(EditData) ? "01.01.2025" : EditData;
        editTime = string.IsNullOrEmpty(EditTime) ? "00:00" : EditTime;
        catalog = Catalog;
    }
}
class Program
{
    static void BC(int x){ Console.BackgroundColor = (ConsoleColor)x;}
    static void FC(int x) { Console.ForegroundColor = (ConsoleColor)x; }
    static void CW(string x, int y = -1) {
        if (y>=0) {
            char ch = x[0];
            string str = new string(ch, y);
            Console.Write(str);
            return;
        }
        Console.Write(x); 
    }
    static void Main()
    {
        List<Files> files = new List<Files>
        {
            new Files("", 0, "15.03.24", "14:30", true),
            new Files("VeryLongDirectory", 0, "16.03.24", "10:15", true),
            new Files("Pics", 0, "17.03.24", "09:45", true),
            new Files("Music", 0, "18.03.24", "16:20", true),
            new Files("Videos", 0, "19.03.24", "11:10", true),
            new Files("Projects", 0, "20.03.24", "13:25", true),
            new Files("Backup", 0, "21.03.24", "15:40", true),
            new Files("Temp", 0, "22.03.24", "08:55", true),
            new Files("Downloads", 0, "23.03.24", "17:30", true),
            new Files("Desktop", 0, "24.03.24", "12:05", true),
            new Files("Config", 5222, "25.03.24", "14:50", true),
            new Files("Logs", 0, "26.03.24", "10:25", true),
            new Files("System", 0, "27.03.24", "09:10", true),
            new Files("Users", 42233, "28.03.24", "16:45", true),
            new Files("Shared", 0, "29.03.24", "11:20", true),
            new Files("Archive", -666, "30.03.24", "13:35", true),
            new Files("Source", 0, "01.04.24", "15:00", true),
            new Files("Destination", 0, "02.04.24", "08:15", true),
            new Files("Data", 0, "03.04.24", "17:50", true),
            new Files("Work", 0, "04.04.24", "12:25", true),
            new Files("A.exe", 24424242424576, "15.03.24", "14:25"),
            new Files("AB.txt", 1024242424224, "16.03.24", "10:10"),
            new Files("ABC.jpg", 34524267, "17.03.24", "09:40"),
            new Files("ABCD.mp3", 45678, "18.03.24", "16:15"),
            new Files("ABCDE.pdf", 12342456, "19.03.24", "11:05"),
            new Files("ABCDEF.doc", 204857, "20.03.24", "13:20"),
            new Files(".xls", 9874242424565, "21.03.24", "15:35"),
            new Files("ABC123.zip", 556789, "22.03.24", "08:50"),
            new Files("File1.txt", 8192, "23.03.24", "17:25"),
            new Files("Img.png", 30720, "24.03.24", "12:00"),
            new Files("Document.pdf", 2457600, "25.03.24", "14:45"),
            new Files("Picture.jpg", 2345678, "26.03.24", "10:20"),
            new Files("MusicFile.mp3", 7890123, "27.03.24", "09:05"),
            new Files("VideoFile.avi", 23456789, "28.03.24", "16:40"),
            new Files("Program.edxe", 4096000, "29.03.24", "11:15"),
            new Files("DataFile.daft", 1123445242424256, "30.03.24", "13:30"),
            new Files("Archive1.zidp", 16384, "01.04.24", "14:55"),
            new Files("Config.insi", 5120, "02.04.24", "08:10"),
            new Files("ReadMe.txt", 32768, "03.04.24", "17:45"),
            new Files("Backup1.bak", 1234567, "04.04.24", "12:20"),
            new Files("VeryLongFileNameExample.txt", 102404554, "05.03.24", "14:05"),
            new Files("AnotherExtremelyLongFileName.doc", 307200, "06.03.24", "09:50"),
            new Files("SuperLongFileNameForTesting.xls", 61442222200, "07.03.24", "16:25"),
            new Files("ExtremelyLongFileNameForExample.adcs", 78901, "08.03.24", "11:00"),
            new Files("TheLongestFileNameEverCreated.xml", 78901, "09.03.24", "13:15"),
            new Files("AnotherVeryLongFileName.ava", 45622278, "10.03.24", "15:30"),
            new Files("ThisIsVeryLongFileName.ipy", 45678, "11.03.24", "08:45"),
            new Files("IncrediblyLongFileName.son", 45678, "12.03.24", "17:10"),
            new Files("ExceptionallyLongName.tml", 12288, "13.03.24", "12:35"),
            new Files("RemarkablyLongFileName.css", 18432, "14.03.24", "14:50"),
            new Files("Makefile", 5120, "15.04.24", "10:05"),
            new Files("README", 8192, "16.04.24", "09:30"),
            new Files("LICENSE", 16384, "17.04.24", "16:55"),
            new Files("CHANGELOG", 12288, "18.04.24", "11:40"),
            new Files("Dockerfile", 6144, "19.04.24", "13:55"),
            new Files("report.docf", 456789, "20.04.24", "15:20"),
            new Files("presentation.ptx", 789012, "21.04.24", "08:35"),
            new Files("spreadsheet.lsx", 345678, "22.04.24", "17:00"),
            new Files("database.Adb", 223344, "23.04.24", "12:45"),
            new Files("script.ejs", 24576, "24.04.24", "14:10"),
            new Files("style.css", 18432, "25.04.24", "10:25"),
            new Files("index.tmff", 12288, "26.04.24", "09:00"),
            new Files("package.tar", 445566, "27.04.24", "16:35"),
            new Files("library.dldl", 102400, "28.04.24", "11:50"),
            new Files("font.ttf", 78643, "29.04.24", "13:05")
        };
        files = Sort_and_split(files);
        Int32 x = 80, lastWidth = x;
        Int32 y = 25, lastHeight = y;
        int num = 0,lastnum = 0,top = 1,lasttop = 1;
        Console.SetBufferSize(500, 500);
        Console.SetWindowSize(x, y);
        if (files.Count < 65) return;
        Outputs(x, y, files,num,top);
        while(true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.DownArrow:
                        if (top == 1) num = num +1> y -9 ? 0 : num + 1;
                        if (top == 0) num = num>(3*(y - 9)+1)? 0: num + 1;
                        break;
                    case ConsoleKey.UpArrow:
                        num = num - 1 < 0 ? (top == 0)? (3 * (y - 9) +2): y - 9 : num - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        if (top +1 >1) top = 0;
                        else top = 1;
                        num = 0;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (top - 1 < 0) top = 1;
                        else  top = 0;
                        num = 0;
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
            Console.Title = "Norton Commander";
            if (Console.WindowWidth != lastWidth || Console.WindowHeight != lastHeight|| num!=lastnum || top != lasttop)
            {
                if (num != lastnum) 
                {
                    lastnum = num;
                    UpdateDisplay(x, y, files, num, top ); continue; 
                }
                if (top != lasttop)
                {
                    lasttop=top;
                    UpdateDisplay(x, y, files, num, top); continue;
                }
                x = Console.WindowWidth;
                y = Console.WindowHeight;
                lastWidth = x;
                lastHeight = y;
                
                x = Console.WindowWidth > 62 ? Console.WindowWidth : 62;
                y = Console.WindowHeight > 9 ? Console.WindowHeight : 9;
                UpdateDisplay(x, y, files,num, top);
                
            }
            //Thread.Sleep(50);
        }
    }
    //{  Black = 0, Blue = 9, White = 15, DarkBlue = 1, Cyan = 11, DarkCyan = 3, DarkGreen = 2, Yelow = 14 , Greeen = 10}
    static void UpdateDisplay(int x, int y, List<Files> files,int num,int top)
    {
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Outputs(x, y, files,num,top);
    }
    static List<Files> Sort_and_split(List<Files> files)
    {
        files.Sort((f1, f2) => f1.name.CompareTo(f2.name));
        //разделим имя
        char[] charSeparators = new char[] { '.' };
        foreach (var file in files)
        {
            string[] result;
            if (file.name.Contains('.'))
            {
                result = file.name.Split(charSeparators);
                file.name = result[0];
                file.lastname = result[1];
            }
        }
        return files;
    }
    static string Magic(Files file, int interval, char ch = 'n')
    {
        string result = null;
        
        switch (ch)
        {
            case ('n'):
                string name = new string(file.name);
                string lastname = new string(file.lastname);
                if (interval - lastname.Length < name.Length + 1)
                {
                    name = name.Substring(0, interval - lastname.Length - 1);
                    name = name + '~' + lastname;
                }
                else
                {
                    while (name.Length != interval - lastname.Length) name += ' ';
                    name += lastname;
                }
                result = name;
                break;
            case ('s'):
                string size = file.size.ToString();
                if (file.catalog) size = "<Dir>";
                if (size.Length>interval) size = size.Substring(0, interval- 1) + '~';
                while (size.Length != interval) size = ' ' + size;
                result = size;
                break;
            case ('d'):
                string data = new string(file.editData);
                while (data.Length != interval) data = ' ' + data;
                result = data;
                break;
            case ('t'):
                string time= new string(file.editTime);
                while (time.Length != interval) time = ' ' + time;
                result = time;
                break;
        }
        return result;
    }
    static void Outputs(int x, int y, List<Files> files, int num, int top)
    {
        List<string> commandsHigh = new List<string>() { "Левая", "Файл", "Диск", "Команды", "Правая" };
        int interval = 1;
        for (; 6 + 4 * interval + 15 < x / 2; interval++) ;
        int data = interval;
        while (6 + data + 3 * interval + 15 > x / 2) data--;
        BC(2);
        Action<string> printWord = word =>
        {
            if (word.Length > 0) FC(14); CW($"{word[0]}"); FC(0); if (word.Length > 1) CW($"{word.Substring(1)}");
        };
        CW(" ");
        printWord(commandsHigh[0]); CW(" ", data);
        printWord(commandsHigh[1]); CW(" ", interval);
        printWord(commandsHigh[2]); CW(" ", interval);
        printWord(commandsHigh[3]); CW(" ", interval);
        printWord(commandsHigh[4]); CW(" ", x / 2 - 11);
        BC(11);
        DateTime now = DateTime.Now;
        Console.WriteLine($"{now:hh:mm}");
        BC(1); FC(11); 
        // Левая
        CW("╔");
        int interval1 = 9, interval2 = 9, interval3 = 9;
        while (interval1 + interval2 + interval3 < x / 2 - 4)
        {
            interval1++;
            if (interval1 + interval2 + interval3 == x / 2 - 4) break;
            interval2++;
            if (interval1 + interval2 + interval3 == x / 2 - 4) break;
            interval3++;
        }
        CW("═", interval1); CW("╤");
        interval2 -= 5; CW("═", interval2 / 2);
        if ( top == 0 ) { BC(11); FC(1); }
        CW(" C:/ ");
        BC(1); FC(11);
        CW("═", (interval2 % 2 == 0) ? interval2 / 2 : interval2 / 2 + 1); interval2 +=5; CW("╤");
        CW("═", interval3);CW("╗╔");
        // Правая   (min7    |    min6   |  min8  | min5) min 31 ! 
        int interval4 = 7, interval5 = 6, interval6 = 8, interval7 = 5;
        while (interval4 + interval5 + interval6 + interval7 < x / 2 - 5)
        {
            interval4++;
            if (interval4 + interval5 + interval6 + interval7 == x/ 2 -5) break;
            interval5++;
            if (interval4 + interval5 + interval6 + interval7 == x/ 2 -5) break;
            interval6++;
            if (interval4 + interval5 + interval6 + interval7 == x/ 2 -5) break;
            interval7++;
        }
        CW("═", interval4); CW("╤");
        interval5 -= 5;CW("═", interval5 / 2);
        if (top == 1) { BC(11); FC(1); }
        CW(" C:/ ");
        FC(11); BC(1);
        CW("═", (interval5 % 2 == 0) ? interval5 / 2 : interval5 / 2 + 1);interval5 += 5;CW("╤");
        CW("═", interval6);CW("╤");
        CW("═", interval7);CW("╗"); CW("\n");
        // 3 строка
        //↓ - не работает(
        CW("║"); FC(15); CW("C:|"); FC(11); CW(" ", (interval1-9==0)? 0: (interval1 - 9)/2); FC(15); CW("Имя"); FC(11); CW(" ", ((interval1 - 9) % 2 == 0) ? (interval1 - 9) / 2 +3: (interval1 - 9) / 2 + 4);CW("│");
        CW(" ", (interval2 - 3)/ 2); FC(15); CW("Имя"); FC(11); CW(" ", ((interval2 - 3) % 2 == 0) ? (interval2 - 3) / 2 : (interval2 - 3) / 2 + 1);CW("│");CW(" ", (interval3 - 3) / 2); FC(15); CW("Имя"); FC(11);
        CW(" ", ((interval3 - 3) % 2 == 0) ? (interval3 - 3) / 2 : (interval3 - 3) / 2 + 1);
        CW("║║"); FC(15); CW("C:| "); FC(11); CW(" ", (interval4 - 7 == 0) ? 0 : (interval4 - 7) / 2); FC(15); CW("Имя"); FC(11); CW(" ", ((interval4 - 7) % 2 == 0) ? (interval4 - 7) / 2: (interval4 - 7) / 2 + 1);CW("│");
        CW(" ", (interval5 - 6) / 2); FC(15); CW("Размер"); FC(11); CW(" ", ((interval5 - 6) % 2 == 0) ? (interval5 - 6) / 2 : (interval5 - 6) / 2 + 1);CW("│");
        CW(" ", (interval6 - 4) / 2); FC(15); CW("Дата"); FC(11); CW(" ", ((interval6 - 4) % 2 == 0) ? (interval6 - 4) / 2 : (interval6 - 4) / 2 + 1);CW("│");
        CW(" ", (interval7 - 5) / 2); FC(15); CW("Время"); FC(11); CW(" ", ((interval7 - 5) % 2 == 0) ? (interval7 - 5) / 2 : (interval7 - 5) / 2 + 1);CW("║"); CW("\n");
        string str;
        for(int i = 0; i < y-8;i++)
        {
            
            CW("║");
            if (i < files.Count)if (files[i].lastname == null) { BC(9); FC(0); }
            if ((top == 0)&& (i == num)) { BC(11); FC(1); }
            CW((i>files.Count())? str = new string(' ', interval1): Magic( files[i], interval1));
            BC(1); FC(11);CW("│");
            if (i + y - 8 < files.Count) if (files[i + y - 8].lastname == null) { BC(9); FC(0); }
            if ((top == 0)&& (i + y - 8 == num)) { BC(11); FC(1); }
            CW((i + y-8 >= files.Count()) ? str = new string(' ', interval2) : Magic(files[i + y - 8], interval2));  
            BC(1); FC(11);CW("│");
            if (i + 2 * y - 16 < files.Count) if (files[i + 2 * y - 16].lastname == null) { BC(9); FC(0); }
            if ((top == 0)&& (i + 2 * y - 16 == num)) { BC(11); FC(1); }
            CW((i + 2 * y - 16 >= files.Count()) ? str = new string(' ', interval3) : Magic(files[i + 2*y -16], interval3)); 
            BC(1); FC(11);CW("║║");
            if (i < files.Count) if (files[i].lastname == null) { BC(9); FC(0); }
            if ((top == 1) && (i == num)) { BC(11); FC(1);}
            
            CW((i > files.Count()) ? str = new string(' ', interval4) : Magic(files[i], interval4)); if ((files[i].lastname == null)&&(!((top == 1) && (i == num)))) { BC(1); FC(11); }
            CW("│");
            CW((i > files.Count()) ? str = new string(' ', interval5) : Magic(files[i], interval5, 's')); CW("│");
            CW((i > files.Count()) ? str = new string(' ', interval6) : Magic(files[i], interval6,'d')); CW("│");
            CW((i > files.Count()) ? str = new string(' ', interval7) : Magic(files[i], interval7,'t'));
            BC(1); FC(11);
            CW("║"); CW("\n");
        }
        // y - 4 строка левая
        CW("╟");CW("─", interval1);CW("┴");CW("─", interval2);CW("┴");CW("─", interval3);CW("╢╟");
        // y - 4 правая
        CW("─", interval4);CW("┴");CW("─", interval5);CW("┴");CW("─", interval6);CW("┴");CW("─", interval7);CW("╢"); CW("\n");
        for (int i = 0; i < 2; i++){
            CW("║ . . ");CW(" ", (x / 2 ==31 )? 0 : (x/2-31)/2 );CW("КАТАЛОГ");CW(" ", (x / 2 - 31) %2==0? (x / 2 - 31) / 2 : (x / 2 - 31) / 2+1);CW($"{now:dd:MM:yyyy}  {now:hh:mm}║");
        }
        CW("\n");
        for (int i = 0; i < 2; i++)
        {
            CW("╚");CW("═", x / 2 - 2);CW("╝");
        }
        CW("\n");
        BC(0);Console.SetCursorPosition(0, y-1);
        List<string> commands = new List<string>() { "Помощь", "Вызов", "Чтение", "Правка", "Копия", "НовИмя", "НовКат", "Удал-е", "Меню", "Выход" };
        int p = 1;
        FC(15);
        foreach (string command in commands)
        {
            CW(" "+ p);BC(10);FC(0);
            CW(command);BC(0);FC(15);
            p++;
        }
        Console.SetCursorPosition(0, y-2);
        CW("C:\\NC>");
    }
}
