using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Praktika3_2
{
    class Program
    {
        static void Main()
        {
            ObservableCollection<OfficeEquipment> equipmentList = new ObservableCollection<OfficeEquipment>()
            {
                new OfficeEquipment("Принтер HP LaserJet", 8.5, 15000),
                new OfficeEquipment("Сканер Canon", 5.2, 10000),
                new OfficeEquipment("Монитор LG", 6.8, 22000),
                new OfficeEquipment("Ксерокс Xerox", 12.0, 35000)
            };

            // Подписка на событие изменений
            equipmentList.CollectionChanged += (sender, e) =>
            {
                Console.WriteLine($"[Событие] Коллекция изменена: {e.Action}");
            };

            while (true)
            {
                Console.WriteLine("\n--- МЕНЮ ---");
                Console.WriteLine("1. Показать коллекцию");
                Console.WriteLine("2. Удалить элементы");
                Console.WriteLine("3. Добавить элемент");
                Console.WriteLine("4. Поиск по названию");
                Console.WriteLine("5. Выйти");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowCollection(equipmentList);
                        break;

                    case "2":
                        RemoveElements(equipmentList);
                        break;

                    case "3":
                        AddElement(equipmentList);
                        break;

                    case "4":
                        SearchElement(equipmentList);
                        break;

                    case "5":
                        Console.WriteLine("Выход из программы...");
                        return;

                    default:
                        Console.WriteLine("Неверный выбор, попробуйте снова.");
                        break;
                }
            }
        }

        static void ShowCollection(ObservableCollection<OfficeEquipment> list)
        {
            Console.WriteLine("\nТекущая коллекция:");
            foreach (var item in list)
                Console.WriteLine(item);
        }

        static void RemoveElements(ObservableCollection<OfficeEquipment> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("Коллекция пуста.");
                return;
            }

            ShowCollection(list);
            Console.Write("\nСколько элементов хотите удалить? ");
            int count;
            if (!int.TryParse(Console.ReadLine(), out count) || count <= 0)
            {
                Console.WriteLine("Некорректное число.");
                return;
            }

            for (int i = 0; i < count && list.Count > 0; i++)
            {
                Console.Write("Введите название техники для удаления: ");
                string name = Console.ReadLine();

                var item = list.FirstOrDefault(x => x.Name != null && x.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
                if (item != null)
                {
                    list.Remove(item);
                    Console.WriteLine($"Удалено: {item.Name}");
                }
                else
                {
                    Console.WriteLine("Элемент не найден.");
                }
            }
        }

        static void AddElement(ObservableCollection<OfficeEquipment> list)
        {
            Console.Write("Введите название техники: ");
            string name = Console.ReadLine();

            Console.Write("Введите вес: ");
            double weight = double.Parse(Console.ReadLine());

            Console.Write("Введите цену: ");
            decimal price = decimal.Parse(Console.ReadLine());

            list.Add(new OfficeEquipment(name, weight, price));
            Console.WriteLine("Элемент добавлен успешно!");
        }

        static void SearchElement(ObservableCollection<OfficeEquipment> list)
        {
            Console.Write("Введите часть названия для поиска: ");
            string keyword = Console.ReadLine();

            // Используем предикат через IndexOf для нечувствительного к регистру поиска
            var results = list.Where(x => x.Name != null &&
                                          x.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (results.Count == 0)
                Console.WriteLine("Ничего не найдено.");
            else
            {
                Console.WriteLine("\nНайденные элементы:");
                foreach (var item in results)
                    Console.WriteLine(item);
            }
        }

    }
}
