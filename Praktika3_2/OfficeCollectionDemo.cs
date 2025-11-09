using Praktika3_2;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace OfficeTechDemo
{
    public class OfficeCollectionDemo
    {
        // Наблюдаемая коллекция офисной техники
        private ObservableCollection<OfficeEquipment> equipments = new ObservableCollection<OfficeEquipment>();

        public OfficeCollectionDemo()
        {
            // Подписываем метод на событие CollectionChanged
            equipments.CollectionChanged += OnCollectionChanged;
        }

        // Метод-обработчик события
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("\nИзменение коллекции:");

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (OfficeEquipment item in e.NewItems)
                        Console.WriteLine($"Добавлен: {item}");
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (OfficeEquipment item in e.OldItems)
                        Console.WriteLine($"Удалён: {item}");
                    break;

                case NotifyCollectionChangedAction.Replace:
                    Console.WriteLine("Элемент заменён.");
                    break;
            }
        }

        // Демонстрация добавления и удаления
        public void Demo()
        {
            var printer = new OfficeEquipment("HP LaserJet", 8.5, 15000);
            var scanner = new OfficeEquipment("Canon ScanX", 4.2, 9000);
            var copier = new OfficeEquipment("Xerox CopyPro", 12.0, 20000);

            equipments.Add(printer);
            equipments.Add(scanner);
            equipments.Add(copier);

            equipments.Remove(scanner);
        }
    }
}
