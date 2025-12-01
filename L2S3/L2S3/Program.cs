
using System;
using System.Text;
using static System.Console;
using static L2S3.Interfaces;
namespace L2S3
{
    class Program
    {
        private static List<Password> passwords = new List<Password>();
        static void Main()
        {
            Title = "TestProgramm";
            OutputEncoding = Encoding.Unicode;
            SetBufferSize(500, 500);
            Clear();
            Menu1();
            return;
        }
        // методы для тестирования парролей, но не используются в интерфейсе.
        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("🎯 ГЛАВНОЕ МЕНЮ");
                Console.WriteLine("1. 📥 Добавить пароль");
                Console.WriteLine("2. 📋 Показать все пароли");
                Console.WriteLine("3. 🗑️  Удалить пароль");
                Console.WriteLine("4. 🔧 Операции с паролями");
                Console.WriteLine("5. 🚪 Выход");
                Console.Write("\nВыберите действие: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AddPassword();
                        break;
                    case "2":
                        ShowAllPasswords();
                        break;
                    case "3":
                        DeletePassword();
                        break;
                    case "4":
                        OperationsMenu();
                        break;
                    case "5":
                        Console.WriteLine("\n👋 До свидания!");
                        return;
                    default:
                        Console.WriteLine("❌ Неверный выбор!");
                        WaitForKey();
                        break;
                }
            }
        }

        static void AddPassword()
        {
            Console.Clear();
            Console.WriteLine("📥 ДОБАВЛЕНИЕ ПАРОЛЯ");
            Console.Write("Введите пароль: ");
            string input = Console.ReadLine() ?? string.Empty;

            if (!string.IsNullOrEmpty(input))
            {
                passwords.Add(new Password(input));
                Console.WriteLine("✅ Пароль успешно добавлен!");
            }
            else
            {
                Console.WriteLine("❌ Пароль не может быть пустым!");
            }
            
            WaitForKey();
        }

        static void ShowAllPasswords()
        {
            Console.Clear();
            Console.WriteLine("📋 СПИСОК ПАРОЛЕЙ");

            if (passwords.Count == 0)
            {
                Console.WriteLine("🚫 Нет сохраненных паролей");
            }
            else
            {
                for (int i = 0; i < passwords.Count; i++)
                {
                    Console.WriteLine($"\n#{i + 1}");
                    passwords[i].DisplayInfo();
                }
            }
            
            WaitForKey();
        }

        static void DeletePassword()
        {
            Console.Clear();
            Console.WriteLine("🗑️  УДАЛЕНИЕ ПАРОЛЯ");

            if (passwords.Count == 0)
            {
                Console.WriteLine("🚫 Нет паролей для удаления");
                WaitForKey();
                return;
            }

            ShowPasswordList();
            
            Console.Write("Выберите номер пароля для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= passwords.Count)
            {
                var deletedPassword = passwords[index - 1];
                passwords.RemoveAt(index - 1);
                Console.WriteLine($"✅ Пароль '{deletedPassword}' успешно удален!");
            }
            else
            {
                Console.WriteLine("❌ Неверный номер пароля!");
            }
            
            WaitForKey();
        }

        static void OperationsMenu()
        {
            if (passwords.Count == 0)
            {
                Console.WriteLine("❌ Нет паролей для операций. Сначала добавьте пароли.");
                WaitForKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("🔧 ОПЕРАЦИИ С ПАРОЛЯМИ");
                Console.WriteLine("1. ✏️  Замена последнего символа");
                Console.WriteLine("2. 📏 Сравнение длин паролей");
                Console.WriteLine("3. ⚖️  Проверка на равенство");
                Console.WriteLine("4. 🔄 Сброс пароля на новое значение");
                Console.WriteLine("5. 📝 Средний символ пароля");
                Console.WriteLine("6. 🔙 Назад в главное меню");
                Console.Write("\nВыберите операцию: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ReplaceLastCharOperation();
                        break;
                    case "2":
                        CompareLengthsOperation();
                        break;
                    case "3":
                        CheckEqualityOperation();
                        break;
                    case "4":
                        ResetPasswordOperation();
                        break;
                    case "5":
                        ShowMiddleCharOperation();
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("❌ Неверный выбор!");
                        WaitForKey();
                        break;
                }
            }
        }

        static void ReplaceLastCharOperation()
        {
            Console.Clear();
            Console.WriteLine("✏️  ЗАМЕНА ПОСЛЕДНЕГО СИМВОЛА");
            ShowPasswordList();
            
            Console.Write("Выберите номер пароля: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= passwords.Count)
            {
                Console.Write("Введите новый символ: ");
                char newChar = Console.ReadLine()?.FirstOrDefault() ?? ' ';
                
                var oldPassword = passwords[index - 1];
                var newPassword = oldPassword.ReplaceLastCharacter(newChar);
                
                passwords[index - 1] = newPassword;
                
                Console.WriteLine($"\n✅ Пароль изменен:");
                Console.WriteLine($"Старый: {oldPassword}");
                Console.WriteLine($"Новый: {newPassword}");
            }
            else
            {
                Console.WriteLine("❌ Неверный номер пароля!");
            }
            
            WaitForKey();
        }

        static void CompareLengthsOperation()
        {
            Console.Clear();
            Console.WriteLine("📏 СРАВНЕНИЕ ДЛИН ПАРОЛЕЙ");
            ShowPasswordList();
            
            Console.Write("Выберите номер первого пароля: ");
            if (int.TryParse(Console.ReadLine(), out int index1) && index1 > 0 && index1 <= passwords.Count)
            {
                Console.Write("Выберите номер второго пароля: ");
                if (int.TryParse(Console.ReadLine(), out int index2) && index2 > 0 && index2 <= passwords.Count)
                {
                    var p1 = passwords[index1 - 1];
                    var p2 = passwords[index2 - 1];
                    
                    Console.WriteLine($"\n📊 Результаты сравнения:");
                    Console.WriteLine($"Пароль 1: {p1} (длина: {p1.Length})");
                    Console.WriteLine($"Пароль 2: {p2} (длина: {p2.Length})");
                    Console.WriteLine($"Пароль 1 длиннее Пароля 2: {(p1.IsLongerThan(p2) ? "✅ Да" : "❌ Нет")}");
                    Console.WriteLine($"Пароль 1 короче Пароля 2: {(p1.IsShorterThan(p2) ? "✅ Да" : "❌ Нет")}");
                }
                else
                {
                    Console.WriteLine("❌ Неверный номер второго пароля!");
                }
            }
            else
            {
                Console.WriteLine("❌ Неверный номер первого пароля!");
            }
            
            WaitForKey();
        }

        static void CheckEqualityOperation()
        {
            Console.Clear();
            Console.WriteLine("⚖️  ПРОВЕРКА НА РАВЕНСТВО");
            ShowPasswordList();
            
            Console.Write("Выберите номер первого пароля: ");
            if (int.TryParse(Console.ReadLine(), out int index1) && index1 > 0 && index1 <= passwords.Count)
            {
                Console.Write("Выберите номер второго пароля: ");
                if (int.TryParse(Console.ReadLine(), out int index2) && index2 > 0 && index2 <= passwords.Count)
                {
                    var p1 = passwords[index1 - 1];
                    var p2 = passwords[index2 - 1];
                    
                    Console.WriteLine($"\n⚖️  Результаты проверки:");
                    Console.WriteLine($"Пароль 1: {p1}");
                    Console.WriteLine($"Пароль 2: {p2}");
                    Console.WriteLine($"Пароли равны: {(p1.Equals(p2) ? "✅ Да" : "❌ Нет")}");
                    Console.WriteLine($"Пароли не равны: {(p1.NotEquals(p2) ? "✅ Да" : "❌ Нет")}");
                }
                else
                {
                    Console.WriteLine("❌ Неверный номер второго пароля!");
                }
            }
            else
            {
                Console.WriteLine("❌ Неверный номер первого пароля!");
            }
            
            WaitForKey();
        }

        static void ResetPasswordOperation()
        {
            Console.Clear();
            Console.WriteLine("🔄 СБРОС ПАРОЛЯ НА НОВОЕ ЗНАЧЕНИЕ");
            ShowPasswordList();
            
            Console.Write("Выберите номер пароля для сброса: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= passwords.Count)
            {
                Console.Write("Введите новый пароль: ");
                string newPasswordValue = Console.ReadLine() ?? string.Empty;
                
                if (!string.IsNullOrEmpty(newPasswordValue))
                {
                    var oldPassword = passwords[index - 1];
                    var newPassword = oldPassword.Reset(newPasswordValue);
                    
                    passwords[index - 1] = newPassword;
                    
                    Console.WriteLine($"\n✅ Пароль сброшен:");
                    Console.WriteLine($"Старый: {oldPassword}");
                    Console.WriteLine($"Новый: {newPassword}");
                }
                else
                {
                    Console.WriteLine("❌ Новый пароль не может быть пустым!");
                }
            }
            else
            {
                Console.WriteLine("❌ Неверный номер пароля!");
            }
            
            WaitForKey();
        }

        static void ShowMiddleCharOperation()
        {
            Console.Clear();
            Console.WriteLine("📝 СРЕДНИЙ СИМВОЛ ПАРОЛЯ");
            ShowPasswordList();
            
            Console.Write("Выберите номер пароля: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= passwords.Count)
            {
                var password = passwords[index - 1];
                var middleChar = password.Value.GetMiddleCharacter();
                
                Console.WriteLine($"\n📝 Средний символ пароля '{password}': '{middleChar}'");
            }
            else
            {
                Console.WriteLine("❌ Неверный номер пароля!");
            }
            
            WaitForKey();
        }

        static void ShowPasswordList()
        {
            Console.WriteLine("\n📋 Доступные пароли:");
            for (int i = 0; i < passwords.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {passwords[i]}");
            }
            Console.WriteLine();
        }

        static void WaitForKey()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }

    }
}