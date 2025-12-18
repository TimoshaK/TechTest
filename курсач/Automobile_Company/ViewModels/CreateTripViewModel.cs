using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Automobile_Company.Commands;
using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using Automobile_Company.Services;

namespace Automobile_Company.ViewModels
{
    public class CreateTripViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;

        private Order _selectedOrder;
        private Vehicle _selectedVehicle;
        private Driver _driverToAdd;
        private Driver _selectedDriverFromList;
        private DateTime? _arrivalToLoadingDate;
        private string _arrivalToLoadingTime;
        private string _notes;

        private ObservableCollection<Order> _availableOrders;
        private ObservableCollection<Vehicle> _availableVehicles;
        private ObservableCollection<Driver> _availableDrivers;
        private ObservableCollection<Driver> _selectedDrivers;

        public ObservableCollection<Order> AvailableOrders
        {
            get => _availableOrders;
            set { _availableOrders = value; OnPropertyChanged(); }
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

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsOrderSelected));
                LoadAvailableVehicles();
                //ClearVehicleSelection();
                //ClearDriverSelection();
            }
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

        public bool IsOrderSelected => SelectedOrder != null;
        public bool IsVehicleSelected => SelectedVehicle != null;
        public bool HasAssignedDriver => SelectedVehicle?.AssignedDriver != null;
        public bool CanCreateTrip => SelectedOrder != null &&
                                     SelectedVehicle != null &&
                                     SelectedDrivers.Count > 0 &&
                                     ArrivalToLoadingDate.HasValue &&
                                     !string.IsNullOrWhiteSpace(ArrivalToLoadingTime);

        public ICommand AddDriverCommand { get; }
        public ICommand RemoveDriverCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Trip CreatedTrip { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CreateTripViewModel()
        {
            _dataService = DataService.Instance;

            AvailableOrders = new ObservableCollection<Order>();
            AvailableVehicles = new ObservableCollection<Vehicle>();
            AvailableDrivers = new ObservableCollection<Driver>();
            SelectedDrivers = new ObservableCollection<Driver>();

            LoadAvailableOrders();

            // Устанавливаем значения по умолчанию
            ArrivalToLoadingDate = DateTime.Now.AddDays(1);
            ArrivalToLoadingTime = "09:00";

            // Инициализация команд
            AddDriverCommand = new RelayCommand(AddDriver, CanAddDriver);
            RemoveDriverCommand = new RelayCommand(RemoveDriver, CanRemoveDriver);
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadAvailableOrders()
        {
            AvailableOrders.Clear();

            // Загружаем только оплаченные заказы, для которых еще нет рейса
            var paidOrders = _dataService.Orders
                .Where(o => o.PaymentStatus == PaymentStatus.Paid &&
                           o.Status == OrderStatus.Paid)
                .ToList();

            // Проверяем, есть ли уже рейс для заказа
            foreach (var order in paidOrders)
            {
                bool hasTrip = _dataService.Trips.Any(t => t.Order?.Id == order.Id);
                if (!hasTrip)
                {
                    AvailableOrders.Add(order);
                }
            }
        }

        private void LoadAvailableVehicles()// пока без проверки на тип грузовика <-> тип груза 
        {
            AvailableVehicles.Clear();

            if (SelectedOrder == null) return;

            // Получаем общий вес груза в т
            double totalWeight = SelectedOrder.TotalWeight/1000;

            // Получаем типы груза из заказа
            var cargoTypes = SelectedOrder.CargoItems.Select(c => c.CargoType).Distinct();

            // Получаем доступные транспортные средства
            var availableVehicles = _dataService.Vehicles
                .Where(v => v.IsAvailable && v.LoadCapacity >= totalWeight)
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

        private void ClearVehicleSelection()
        {
            SelectedVehicle = null;
            AvailableVehicles.Clear();
            ClearDriverSelection();
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

        private bool CanRemoveDriver(object parameter)
        {
            return SelectedDriverFromList != null;
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

                var trip = new Trip
                {
                    Order = SelectedOrder,
                    Vehicle = SelectedVehicle,
                    Crew = SelectedDrivers.First(), // Основной водитель - первый в списке
                    ArrivalToLoadingTime = arrivalDateTime,
                    Status = TripStatus.Created,
                    Notes = Notes
                };

                CreatedTrip = trip;

                // Обновляем статусы
                SelectedOrder.Status = OrderStatus.InProgress;
                SelectedVehicle.Status = VehicleStatus.OnTrip;

                foreach (var driver in SelectedDrivers)
                {
                    driver.Status = DriverStatus.OnTrip;
                }

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
            if (SelectedOrder == null)
                throw new ArgumentException("Не выбран заказ");

            if (SelectedVehicle == null)
                throw new ArgumentException("Не выбрано транспортное средство");

            if (SelectedDrivers.Count == 0)
                throw new ArgumentException("Не добавлены водители");

            if (!ArrivalToLoadingDate.HasValue)
                throw new ArgumentException("Не указана дата прибытия на погрузку");

            if (string.IsNullOrWhiteSpace(ArrivalToLoadingTime))
                throw new ArgumentException("Не указано время прибытия на погрузку");

            // Проверяем, что вес груза не превышает грузоподъемность
            if (SelectedOrder.TotalWeight > SelectedVehicle.LoadCapacity)
                throw new ArgumentException($"Вес груза ({SelectedOrder.TotalWeight:F2} кг) превышает грузоподъемность транспорта ({SelectedVehicle.LoadCapacity} т)");
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