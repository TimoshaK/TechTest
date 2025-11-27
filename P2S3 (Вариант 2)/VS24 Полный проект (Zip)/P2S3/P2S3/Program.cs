using System.Text;
using static System.Console;
namespace P2S3
{

    public class Programm
    {
        static int xmin = 34;
        static void Main()
        {
            Title = "MyTomogochi>.<";
            OutputEncoding = Encoding.Unicode;
            SetBufferSize(500, 500);
            Int32 x = 50, lastWidth = x;
            SetWindowSize(x, 30);
            int num = 1, lastnum = num;
            Interface(x, num);
            PetHouse house = new PetHouse();
            Cat cat = new Cat();
            Kitten kitten = new Kitten();
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
                                SetCursorPosition(0, 10);
                                Write("╟");
                                for (int i = 0; i < x - 2; i++)
                                {
                                    Write("─");
                                }
                                Write("╢\n");
                                string str = new string(' ', x);
                                action(num, house, x);
                                break;
                            case ConsoleKey.Escape:
                                return;
                            case ConsoleKey.DownArrow:
                                num = (num + 1 > 9) ? 1 : num + 1;
                                break;
                            case ConsoleKey.UpArrow:
                                num = (num - 1 < 1) ? 9 : num - 1;
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
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    return;
                }
            }
        }
        static void Interface(int x, int num)
        {
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.Red;
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
                " 1. Показать всех моих животных ",
                " 2. Взять нового питомца        ",
                " 3. Отдать питомца в добрые руки",
                " 4. Покормить питомца           ",
                " 5. Позвать питомца             ",
                " 6. Прогнать питомца            ",
                " 7. Попытаться погладить питомца",
                " 8. поиграть с питомцем         ",
                " 9. Получить осуждающий взгяд   "
            };
            Action<int, int> mark = (num, pos) =>
            {
                if (num == pos) { BackgroundColor = ConsoleColor.Black; ForegroundColor = ConsoleColor.Red; }
            };
            Action off = () =>
            {
                BackgroundColor = ConsoleColor.Red; ForegroundColor = ConsoleColor.Black;
            };
            string str = new string(' ', x - 34);

            int p = 1;
            foreach (var sentence in list)
            {
                Write("║"); mark(num, p); Write(sentence); Write($"{str}"); off(); WriteLine("║");//32 без рамок
                p++;
            }
            Write("╚");
            for (int i = 0; i < x - 2; i++)
            {
                Write("═");
            }
            Write("╝\n");

        }
        static void AddMenu(PetHouse house, int x)
        {
            string str = new string(' ', Console.WindowWidth - 1);
            string name = null;
            int act = 1, age = 1, breed = 1, m = 11;
            float weight = 1;
            while (true)
            {
                BackWall(x, m);
                LowWall(x, m + 1);
                Write("║Кошка или животное?(1 или 2): ");

                string input = ReadLine();
                if (int.TryParse(input, out act) && (act == 1 || act == 2))
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
            while (name == null || name == "")
            {
                BackWall(x, m + 1);
                Write(str);
                LowWall(x, m + 2);
                Write("║Какое имя у него? : ");
                name = ReadLine();
            }
            SetCursorPosition(0, m + 2);
            WriteLine(str);
            SetCursorPosition(0, m + 2);
            if (act == 1)
            {
                while (true)
                {
                    BackWall(x, m + 2);
                    LowWall(x, m + 3);
                    Write("║Какой у него возраст? : ");

                    string input = ReadLine();
                    if (int.TryParse(input, out age) && age >= 0 && age < 40)
                    {
                        SetCursorPosition(0, m + 3);
                        WriteLine(str);
                        SetCursorPosition(0, m + 3);
                        m++;
                        break;
                    }
                    else
                    {
                        SetCursorPosition(0, m + 2);
                        WriteLine(str);
                        SetCursorPosition(0, m + 2);
                        WriteLine("║Неверный ввод!");
                        Thread.Sleep(350);
                        SetCursorPosition(0, m + 2);
                        Write(str);
                    }
                }
            }

            while (true)
            {
                BackWall(x, m + 2);
                LowWall(x, m + 3);
                Write("║Какой вес? : ");
                string input = ReadLine();
                if (float.TryParse(input, out weight) && weight >= 0 && weight < 25)
                {
                    SetCursorPosition(0, m + 3);
                    WriteLine(str);
                    SetCursorPosition(0, m + 3);
                    break;
                }
                else
                {
                    SetCursorPosition(0, m + 2);
                    WriteLine(str);
                    SetCursorPosition(0, m + 2);
                    WriteLine("║Неверный ввод!");
                    Thread.Sleep(350);
                    SetCursorPosition(0, m + 2);
                    Write(str);
                }
            }
            while (true)
            {
                SetCursorPosition(0, m + 4);
                int i = 1;
                foreach (var Breed in Enum.GetValues(typeof(CatBreed)))
                {

                    BackWall(x, m + 3 + i); Write($"║{i}."); Write(Breed.ToString());
                    i++;
                }
                BackWall(x, m + 3);
                LowWall(x, m + 3 + i);
                SetCursorPosition(0, m + 3);
                Write("║Какая порода? (Выбрать номер) : ");

                string input = ReadLine();
                if (int.TryParse(input, out breed) && breed >= 1 && breed <= Enum.GetValues(typeof(CatBreed)).Length)
                {
                    break;
                }
                else
                {
                    SetCursorPosition(0, m + 3);
                    WriteLine(str);
                    SetCursorPosition(0, m + 3);
                    WriteLine("║Неверный ввод!");
                    Thread.Sleep(350);
                    SetCursorPosition(0, m + 3);
                    for (int j = 0; j < i + 2; j++)
                    {
                        WriteLine(str);
                    }
                }

            }
            breed -= 1;
            str += ' ';
            SetCursorPosition(0, 11);
            for (int i = 0; i < 20; i++)
            {
                WriteLine(str);
            }
            SetCursorPosition(0, 11);
            if (act == 2)
            {
                Animal animal = new Animal(name, weight, (CatBreed)breed);
                house.AddAnimal(animal);
            }
            else if (age < 2)
            {
                Kitten kit = new Kitten(name, weight, (CatBreed)breed, age);
                house.AddAnimal(kit);
            }
            else
            {
                Cat cat = new Cat(name, weight, (CatBreed)breed, age);
                house.AddAnimal(cat);
            }
            LowWall(x, 16);
            SetCursorPosition(0, 0);

        }
        static void action(int num, PetHouse house, int x)
        {
            switch (num)
            {
                case 1:
                    house.OutputList();
                    break;
                case 2:
                    AddMenu(house, x);
                    break;
                case 3:
                    string str = new string(' ', x);
                    if (house.animals.Count == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Животных нет!"); return; }
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║Какого питомца отдать?(↑/↓ )\n");
                    int select = SelectList(house, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == house.animals.Count() + 1)
                    {
                        break;
                    }
                    house.RemoveAnimal(select - 1);
                    LowWall(x, GetCursorPosition().Top);
                    break;
                case 4:
                    if (house.animals.Count == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Кормить некого!"); return; }
                    
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║Кого покормить?(↑/↓ )\n");
                    select = SelectList(house, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == house.animals.Count() + 1)
                    {
                        break;
                    }
                    if (Enum.GetValues(typeof(CatBreed)).Length == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Кормить нечем!"); return; }
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║Чем покормить?(↑/↓ )\n");
                    int selectEat = SelectEat(x);
                    if (selectEat == -1) { throw new ArgumentException("Выход из программы"); }
                    house.Feed(select - 1, (Eats)selectEat);
                    LowWall(x, GetCursorPosition().Top);
                    break;
                case 5:
                    if (house.animals.Count == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Звать некого!"); return; }
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║Кого позвать?(↑/↓ )\n");
                    select = SelectList(house, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == house.animals.Count() + 1)
                    {
                        break;
                    }
                    house.Ask(select - 1);
                    LowWall(x, GetCursorPosition().Top);
                    break;
                case 6:
                    if (house.animals.Count == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Прогонять некого!"); return; }
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║Кого прогнать?(↑/↓ )\n");
                    select = SelectList(house, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == house.animals.Count() + 1)
                    {
                        break;
                    }
                    house.Banish(select - 1);
                    LowWall(x, GetCursorPosition().Top);
                    break;
                case 7:
                    if (house.animals.Count == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Некого гладить!"); return; }
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║Кого погладить?(↑/↓ )\n");
                    select = SelectList(house, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == house.animals.Count() + 1)
                    {
                        break;
                    }
                    house.Stroke(select - 1);
                    LowWall(x, GetCursorPosition().Top);
                    break;
                case 8:
                    if (house.animals.Count == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Играть не с кем!"); return; }
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║С кем поиграть?(↑/↓ )\n");
                    select = SelectList(house, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == house.animals.Count() + 1)
                    {
                        break;
                    }
                    house.PlayWith(select - 1);
                    LowWall(x, GetCursorPosition().Top);
                    break;
                case 9:
                    if (house.animals.Count == 0) { BackWall(x, 11); LowWall(x, 12); Write("║Получать взгляд не от кого!"); return; }
                    SetCursorPosition(0, 11);
                    BackWall(x, 11); Write("║От кого получть взгляд?(↑/↓ )\n");
                    select = SelectList(house, x);
                    if (select == -1) { throw new ArgumentException("Выход из программы"); }
                    if (select == house.animals.Count() + 1)
                    {
                        break;
                    }
                    house.Get_view(select - 1);
                    LowWall(x, GetCursorPosition().Top);
                    break;
            }
        }
        public static void emoji(int actionNumber)
        {
            switch (actionNumber)
            {
                case 1: // OutputList - Кто у меня живет?
                    BCP(); Write("║  /\\_/\\"); WriteLine("  /\\_/\\");
                    BCP(); Write("║ ( o.o )"); WriteLine("( ^.^ )");
                    BCP(); Write("║  > ^ <"); WriteLine(" /     \\");
                    BCP(); WriteLine("║┌─────────────────┐");
                    BCP(); WriteLine("║│ Список питомцев │");
                    BCP(); WriteLine("║└─────────────────┘");
                    break;

                case 2: // AddAnimal - Взять еще одного питомца
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( ^.^ )");
                    BCP(); WriteLine("║ /     \\ Новый друг!");
                    BCP(); WriteLine("║▼━━━━━━━▼");
                    break;

                case 3: // RemoveAnimal - отдать в добрые руки
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( T.T )");
                    BCP(); WriteLine("║  ━━━   Прощай...");
                    BCP(); WriteLine("║ /     \\");
                    BCP(); WriteLine("║▼━━━━━━━▼");
                    break;

                case 4: // Feed - покормить кису
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( oᴗo )");
                    BCP(); WriteLine("║  🍖🥛   Ням-ням!");
                    BCP(); WriteLine("║ /     \\");
                    BCP(); WriteLine("║▼━━━━━━▼");
                    break;

                case 5: // Ask - позвать кису
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( •.• )");
                    BCP(); WriteLine("║  ? ?   Кис-кис-кис!");
                    BCP(); WriteLine("║ /   ↑ \\");
                    BCP(); WriteLine("║▼━━━━━━━▼");
                    break;

                case 6: // Banish - прогнать кису
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( #.# )");
                    BCP(); WriteLine("║  ← ←   Кшшш! Вон!");
                    BCP(); WriteLine("║ /     \\");
                    BCP(); WriteLine("║▼━━━━━━▼");
                    break;

                case 7: // Stroke - гладить или осуждающий взгляд
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( ಠ_ಠ )");
                    BCP(); WriteLine("║  ← →   Ты кто такой?");
                    BCP(); WriteLine("║ /     \\");
                    BCP(); WriteLine("║▼━━━━━━▼");
                    break;
                case 8:
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( ᵔ ᴗ ᵔ)");
                    BCP(); WriteLine("║  ↗ ↖   Мурррр...");
                    BCP(); WriteLine("║ /     \\");
                    BCP(); WriteLine("║▼━━━━━━▼");
                    break;
                case 9: // PlayWith - поиграть с кисой
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( ≧▽≦ )");
                    BCP(); WriteLine("║  🎾🧶   Играем!");
                    BCP(); WriteLine("║ /     \\");
                    BCP(); WriteLine("║▼━━━━━━▼");
                    break;

                case 10: // Get_view - получить взгляд
                    Random rand = new Random();
                    switch (rand.Next(4))
                    {
                        case 0:
                            {
                                BCP(); WriteLine("║  /\\_/\\");
                                BCP(); WriteLine("║ ( •.•)");
                                BCP(); WriteLine("║  o o   Привет!");
                                BCP(); WriteLine("║ /     \\");
                                BCP(); WriteLine("║▼━━━━━━▼");
                                break;
                            }
                        case 1:
                            {
                                BCP(); WriteLine("║  /\\_/\\");
                                BCP(); WriteLine("║ ( -.-)");
                                BCP(); WriteLine("║  z Z   Спать хочу...");
                                BCP(); WriteLine("║ /     \\");
                                BCP(); WriteLine("║▼━━━━━━▼");
                                break;
                            }
                        case 2:
                            {
                                BCP(); WriteLine("║  /\\_/\\");
                                BCP(); WriteLine("║ ( ôᴗô)");
                                BCP(); WriteLine("║  ^ ^   Любопытно!");
                                BCP(); WriteLine("║ /     \\");
                                BCP(); WriteLine("║▼━━━━━━▼");
                                break;
                            }
                        case 3:
                            {
                                BCP(); WriteLine("║  /\\_/\\");
                                BCP(); WriteLine("║ ( •̀ᴗ•́)");
                                BCP(); WriteLine("║  ▽ ▽   Охотничий взгляд");
                                BCP(); WriteLine("║ /     \\");
                                BCP(); WriteLine("║▼━━━━━━▼");
                                break;
                            }
                    }
                    break;
                default:
                    BCP(); WriteLine("║  /\\_/\\");
                    BCP(); WriteLine("║ ( ?.? )");
                    BCP(); WriteLine("║  o o   ");
                    BCP(); WriteLine("║ /     \\");
                    BCP(); WriteLine("║▼━━━━━━▼");
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
            BackWall(WindowWidth, GetCursorPosition().Top);
        }
        public static void BCFC(int mode)
        {
            if (mode == 1)
            {
                BackgroundColor = ConsoleColor.Black; ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                BackgroundColor = ConsoleColor.Red; ForegroundColor = ConsoleColor.Black;
            }
        }
        public static int SelectList(PetHouse house, int x)
        {
            string str   ;
            int select = house.animals.Count + 1;
            house.ListOfAnimal();
            BackWall(x, GetCursorPosition().Top); Write("║"); BCFC(1); WriteLine("Назад <-"); BCFC(0);
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
                            if (select <= house.animals.Count? !((house.animals[select-1] is Kitten)||(house.animals[select - 1] is Cat)||!(select==3)) : false)
                            {
                                str = new string(' ', x);
                                SetCursorPosition(0, 11);
                                for (int i = 0; i < 50; i++)
                                {
                                    WriteLine(str);
                                }
                                SetCursorPosition(0, 0);
                                BackWall(x, 11); LowWall(x, 12);
                                Write($"║С {house.animals[select - 1].Name} нельзя так взаимодействовать!");
                                Thread.Sleep(450);
                                select = house.animals.Count + 1;
                            }
                            if (select == house.animals.Count + 1)
                            {
                                str = new string(' ', x);
                                SetCursorPosition(0, 11);
                                for (int i = 0; i < 50; i++)
                                {
                                    WriteLine(str);
                                }
                                LowWall(x, 10);
                                SetCursorPosition(0, 0);
                                SetCursorPosition(0, 11);
                                return select;
                            }
                            
                            SetCursorPosition(0, 11);
                            str = new string(' ', x);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }


                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, 11);
                            return select;
                        case ConsoleKey.Escape:
                            return -1;
                        case ConsoleKey.DownArrow:
                            select = (select + 1 > house.animals.Count + 1) ? 1 : select + 1;
                            str = new string(' ', x - 1);
                            SetCursorPosition(0, 12);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, 12);
                            house.ListOfAnimal(select);
                            BackWall(x, GetCursorPosition().Top); Write("║"); if (select == house.animals.Count + 1) { BCFC(1); }
                            WriteLine("Назад <-"); BCFC(0);
                            LowWall(x, GetCursorPosition().Top);
                            break;
                        case ConsoleKey.UpArrow:
                            select = (select - 1 < 1) ? house.animals.Count + 1 : select - 1;
                            SetCursorPosition(0, 12);
                            str = new string(' ', x - 1);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, 12);
                            house.ListOfAnimal(select);
                            BackWall(x, GetCursorPosition().Top); Write("║"); if (select == house.animals.Count + 1) { BCFC(1); }
                            WriteLine("Назад <-"); BCFC(0);
                            LowWall(x, GetCursorPosition().Top);
                            break;
                    }
                }

            }
        }
        public static int SelectEat(int x)
        {
            string str;
            int select = 0;
            EatList(select);
            while (true)
            {
                if (KeyAvailable)
                {
                    var key = ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.Enter:
                            if (select == -1) return -1;
                            SetCursorPosition(0, 11);
                            str = new string(' ', x);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, 11);
                            return select;
                        case ConsoleKey.Escape:
                            return -1;
                        case ConsoleKey.DownArrow:
                            select = (select + 1 > Enum.GetValues(typeof(Eats)).Length) ? 0 : select + 1;
                            str = new string(' ', x - 1);
                            SetCursorPosition(0, 12);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, 12);
                            EatList(select);

                            break;
                        case ConsoleKey.UpArrow:
                            select = (select - 1 < 0) ? Enum.GetValues(typeof(Eats)).Length : select - 1;
                            SetCursorPosition(0, 12);
                            str = new string(' ', x - 1);
                            for (int i = 0; i < 50; i++)
                            {
                                WriteLine(str);
                            }
                            SetCursorPosition(0, 0);
                            SetCursorPosition(0, 12);
                            EatList(select);
                            break;
                    }
                }

            }
        }
        public static void EatList(int select = -1)
        {
            int ix = 1;
            SetCursorPosition(0, 12);
            foreach (var Eat in Enum.GetValues(typeof(Eats)))
            {

                BCP(); Write("║"); if (select == ix - 1) { BCFC(1); }
                Write($"{ix}."); WriteLine(Eat.ToString()); BCFC(0);
                ix++;
            }
            LowWall(WindowWidth, GetCursorPosition().Top);
        }
    }
}