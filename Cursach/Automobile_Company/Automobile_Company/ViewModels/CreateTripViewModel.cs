using Automobile_Company.Commands;
using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using Automobile_Company.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Automobile_Company.ViewModels
{
    public class CreateTripViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        private CargoItem _selectedItemfromList;
        private Vehicle _selectedVehicle;
        private CargoItem _itemToAdd;
        private Driver _driverToAdd;
        private Driver _selectedDriverFromList;
        private DateTime? _arrivalToLoadingDate;
        private string _arrivalToLoadingTime;
        private string _notes;

        private ObservableCollection<CargoItem> _availableItems;
        private ObservableCollection<CargoItem> _selectedItems;
        private ObservableCollection<Vehicle> _availableVehicles;
        private ObservableCollection<Driver> _availableDrivers;
        private ObservableCollection<Driver> _selectedDrivers;

        public CargoItem ItemToAdd
        {
            get => _itemToAdd;
            set { _itemToAdd = value; OnPropertyChanged(); }
        }
        public CargoItem SelectedItemfromList
        {
            get => _selectedItemfromList;
            set { _selectedItemfromList = value; OnPropertyChanged(); }
        }
        public ObservableCollection<CargoItem> AvailableItems
        {
            get => _availableItems;
            set { _availableItems = value; OnPropertyChanged(); }
        }
        public ObservableCollection<CargoItem> SelectedItems
        {
            get => _selectedItems;
            set 
            { 
                _selectedItems = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Vehicle> AvailableVehicles
        {
            get => _availableVehicles;
            set { _availableVehicles = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Driver> AvailableDrivers
        {
            get => _availableDrivers;
            set { _availableDrivers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Driver> SelectedDrivers
        {
            get => _selectedDrivers;
            set { _selectedDrivers = value; OnPropertyChanged(); }
        }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsVehicleSelected));
                OnPropertyChanged(nameof(HasAssignedDriver));
                
                LoadAvailableDrivers();
                LoadAvailableItems();
                AddAssignedDriverIfExists();
            }
        }

        public Driver DriverToAdd
        {
            get => _driverToAdd;
            set { _driverToAdd = value; OnPropertyChanged(); }
        }

        public Driver SelectedDriverFromList
        {
            get => _selectedDriverFromList;
            set { _selectedDriverFromList = value; OnPropertyChanged(); }
        }

        public DateTime? ArrivalToLoadingDate
        {
            get => _arrivalToLoadingDate;
            set { _arrivalToLoadingDate = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCreateTrip)); }
        }

        public string ArrivalToLoadingTime
        {
            get => _arrivalToLoadingTime;
            set { _arrivalToLoadingTime = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanCreateTrip)); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public bool IsItemSelected => SelectedItemfromList!= null;
        public bool IsVehicleSelected => SelectedVehicle != null;
        public bool HasAssignedDriver => SelectedVehicle?.AssignedDriver != null;
        public bool CanCreateTrip =>
                                     SelectedVehicle != null &&
                                     SelectedItems!=null &&
                                     SelectedDrivers.Count > 0 &&
                                     ArrivalToLoadingDate.HasValue &&
                                     !string.IsNullOrWhiteSpace(ArrivalToLoadingTime);
        public ICommand AddDriverCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand RemoveDriverCommand { get; }
        public ICommand RemoveItemCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Trip CreatedTrip { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CreateTripViewModel()
        {
            _dataService = DataService.Instance;

            AvailableItems = new ObservableCollection<CargoItem>();
            AvailableVehicles = new ObservableCollection<Vehicle>();
            AvailableDrivers = new ObservableCollection<Driver>();
            SelectedDrivers = new ObservableCollection<Driver>();
            SelectedItems = new ObservableCollection<CargoItem>();
            LoadAvailableVehicles();
            
            // Устанавливаем значения по умолчанию
            ArrivalToLoadingDate = DateTime.Now.AddDays(1);
            ArrivalToLoadingTime = "09:00";

            // Инициализация команд
            AddDriverCommand = new RelayCommand(AddDriver, CanAddDriver);
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
            RemoveDriverCommand = new RelayCommand(RemoveDriver, CanRemoveDriver);
            RemoveItemCommand = new RelayCommand(RemoveItem, CanRemoveItem);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadAvailableItems()
        {
            try
            {
                AvailableItems.Clear();

                if (SelectedVehicle == null)
                {
                    return;
                }
                ObservableCollection<CargoItem> newList = new ObservableCollection<CargoItem>();
                if (_dataService?.Orders != null)
                {
                    foreach (var item in _dataService.Orders)
                    {
                        // Проверяем item и CargoItems на null
                        if (item?.CargoItems != null)
                        {
                            foreach (var cargo in item.CargoItems)
                            {
                                if (cargo != null)
                                    newList.Add(cargo);
                            }
                        }
                    }
                }
                else
                {
                    return;
                }

                var paidItems = newList
                    .Where(o => o != null &&
                               o.AssignedOrder != null &&
                               o.AssignedOrder.PaymentStatus == PaymentStatus.Paid &&
                               o.AssignedOrder.Status == OrderStatus.Paid)
                    .ToList();

                var compatibleItems = paidItems
                    .Where(item => IsVehicleSuitableForCargo(SelectedVehicle, item.CargoType))
                    .ToList();

                var selectedItemsIds = new HashSet<Guid>();
                if (SelectedItems != null)
                {
                    foreach (var item in SelectedItems)
                    {
                        if (item != null)
                            selectedItemsIds.Add(item.Id);
                    }
                }
                var filteredItems = compatibleItems
                    .Where(item => !selectedItemsIds.Contains(item.Id))
                    .ToList();

                foreach (var item in filteredItems)
                {
                    AvailableItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Подробное логирование ошибки
                string errorMsg = $"Ошибка в LoadAvailableItems:\n" +
                                 $"Message: {ex.Message}\n" +
                                 $"Inner: {ex.InnerException?.Message}\n" +
                                 $"StackTrace: {ex.StackTrace}";

                Console.WriteLine(errorMsg);
                MessageBox.Show(errorMsg, "Критическая ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool IsVehicleSuitableForCargo(Vehicle vehicle, CargoType cargoType)
        {
            // Здесь можно реализовать логику сопоставления типа груза и типа кузова
            switch (cargoType)
            {
                case (CargoType.Bulk):
                    if (vehicle.BodyType == VehicleBodyType.Trailer || vehicle.BodyType == VehicleBodyType.Truck) return true;
                    return false;
                case (CargoType.Hazardous):
                    if (vehicle.BodyType == VehicleBodyType.Van) return true;
                    return false;
                case (CargoType.Fragile):
                    if (vehicle.BodyType == VehicleBodyType.Van) return true;
                    return false;
                case (CargoType.Perishable):
                    if (vehicle.BodyType == VehicleBodyType.Refrigerator) return true;
                    return false;
                case (CargoType.General):
                    if (vehicle.BodyType == VehicleBodyType.Van) return true;
                    return false;
                case (CargoType.Liquid):
                    if (vehicle.BodyType == VehicleBodyType.Tanker) return true;
                    return false;
                case (CargoType.Refrigerated):
                    if (vehicle.BodyType == VehicleBodyType.Refrigerator) return true;
                    return false;
                default:
                    break;
            }
            return false;
        }
        private void LoadAvailableVehicles()
        {
            // Получаем доступные транспортные средства
            var availableVehicles = _dataService.Vehicles
                .Where(v => v.IsAvailable && (v.AssignedDriver==null || v.AssignedDriver.IsAvailable))
                .ToList();
            foreach (var vehicle in availableVehicles)
            {
                AvailableVehicles.Add(vehicle);
            }
        }

        private void LoadAvailableDrivers()
        {
            AvailableDrivers.Clear();

            if (SelectedVehicle == null) return;

            // Получаем доступных водителей для выбранного транспорта
            var availableDrivers = _dataService.Drivers
                .Where(d => d.IsAvailable)
                .ToList();

            // Исключаем уже выбранных водителей
            var selectedDriverIds = SelectedDrivers.Select(d => d.Id).ToList();
            availableDrivers = availableDrivers
                .Where(d => !selectedDriverIds.Contains(d.Id))
                .ToList();

            foreach (var driver in availableDrivers)
            {
                AvailableDrivers.Add(driver);
            }
        }

        private void AddAssignedDriverIfExists()
        {
            if (SelectedVehicle?.AssignedDriver != null &&
                SelectedVehicle.AssignedDriver.IsAvailable)
            {
                // Проверяем, не добавлен ли уже этот водитель
                bool alreadyAdded = SelectedDrivers.Any(d => d.Id == SelectedVehicle.AssignedDriver.Id);
                if (!alreadyAdded)
                {
                    SelectedDrivers.Add(SelectedVehicle.AssignedDriver);
                    OnPropertyChanged(nameof(CanCreateTrip));

                    // Обновляем список доступных водителей
                    LoadAvailableDrivers();
                }
            }
        }

        private void ClearDriverSelection()
        {
            SelectedDrivers.Clear();
            AvailableDrivers.Clear();
            DriverToAdd = null;
            SelectedDriverFromList = null;
        }

        private bool CanAddDriver(object parameter)
        {
            return DriverToAdd != null &&
                   SelectedDrivers.Count < 3 && // Максимум 3 водителя на рейс
                   !SelectedDrivers.Contains(DriverToAdd);
        }
        private bool CanAddItem(object parameter)
        {
            return ItemToAdd != null &&
                  SelectedItems.Sum(o => o.TotalWeight)/1000 <= SelectedVehicle.LoadCapacity && 
                  !SelectedItems.Contains(ItemToAdd);
        }
        private void AddDriver(object parameter)
        {
            if (DriverToAdd != null && !SelectedDrivers.Contains(DriverToAdd))
            {
                SelectedDrivers.Add(DriverToAdd);
                DriverToAdd = null;
                OnPropertyChanged(nameof(CanCreateTrip));

                // Обновляем список доступных водителей
                LoadAvailableDrivers();
            }
        }
        private void AddItem(object parameter)
        {
            if (ItemToAdd != null && !SelectedItems.Contains(ItemToAdd))
            {
                SelectedItems.Add(ItemToAdd);
                DriverToAdd = null;
                OnPropertyChanged(nameof(CanCreateTrip));
                // Обновляем список доступных водителей
                LoadAvailableItems();
            }
        }
        private bool CanRemoveDriver(object parameter)
        {
            return SelectedDriverFromList != null;
        }
        private bool CanRemoveItem(object parameter)
        {
            return SelectedItemfromList != null;
        }
        private void RemoveDriver(object parameter)
        {
            if (SelectedDriverFromList != null)
            {
                SelectedDrivers.Remove(SelectedDriverFromList);
                SelectedDriverFromList = null;
                OnPropertyChanged(nameof(CanCreateTrip));

                // Обновляем список доступных водителей
                LoadAvailableDrivers();
            }
        }
        private void RemoveItem(object parameter)
        {
            if (SelectedItemfromList != null)
            {
                SelectedItems.Remove(SelectedItemfromList);
                SelectedItemfromList = null;
                OnPropertyChanged(nameof(CanCreateTrip));

                // Обновляем список доступных водителей
                LoadAvailableItems();
            }
        }
        private bool CanSave(object parameter)
        {
            return CanCreateTrip;
        }

        private void Save(object parameter)
        {
            try
            {
                ValidateInput();
                // Создаем объект DateTime для прибытия на погрузку
                DateTime arrivalDateTime;
                if (!DateTime.TryParse($"{ArrivalToLoadingDate.Value:yyyy-MM-dd} {ArrivalToLoadingTime}",
                    out arrivalDateTime))
                {
                    throw new ArgumentException("Некорректный формат времени прибытия");
                }
                if (SelectedDrivers.Count == 2) { SelectedDrivers.Add(new Driver()); }
                if (SelectedDrivers.Count == 1) { SelectedDrivers.Add(new Driver()); SelectedDrivers.Add(new Driver()); }
                var trip = new Trip
                {
                    Items = SelectedItems.ToList(),
                    Vehicle = SelectedVehicle,
                    Crews = SelectedDrivers.ToList(),
                    ArrivalToLoadingTime = arrivalDateTime,
                    Status = TripStatus.Created,
                    Notes = Notes
                };
                // Обновляем статусы
                SelectedVehicle.Status = VehicleStatus.OnTrip;
                foreach (var item in SelectedItems)
                {
                    item.CargoStatus = CargoItemStatus.Trip_signed;
                    
                }
                foreach (var driver in SelectedDrivers)
                {
                    driver.Status = DriverStatus.OnTrip;
                }
                CreatedTrip = trip;
                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании рейса: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateInput()
        {
            if (SelectedItems == null)
                throw new ArgumentException("Не выбран груз");

            if (SelectedVehicle == null)
                throw new ArgumentException("Не выбрано транспортное средство");

            if (SelectedDrivers.Count == 0)
                throw new ArgumentException("Не добавлены водители");

            if (!ArrivalToLoadingDate.HasValue)
                throw new ArgumentException("Не указана дата прибытия на погрузку");

            if (string.IsNullOrWhiteSpace(ArrivalToLoadingTime))
                throw new ArgumentException("Не указано время прибытия на погрузку");

            // Проверяем, что вес груза не превышает грузоподъемность
            if (SelectedItems.Sum(o=>o.TotalWeight) > SelectedVehicle.LoadCapacity*1000)
                throw new ArgumentException($"Вес груза ({SelectedItems.Sum(o => o.TotalWeight):F2} кг) превышает грузоподъемность транспорта ({SelectedVehicle.LoadCapacity} т)");
        }

        private void Cancel(object parameter)
        {
            if (parameter is Window window)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}