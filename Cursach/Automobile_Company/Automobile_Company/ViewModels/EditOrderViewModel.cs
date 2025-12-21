using Automobile_Company.Commands;
using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Automobile_Company.ViewModels
{
    public class EditOrderViewModel : INotifyPropertyChanged
    {
        private readonly Order _order;
        private OrderStatus _status;
        private PaymentMethod _paymentMethod;
        private PaymentStatus _paymentStatus;
        private decimal _paidAmount;
        private DateTime? _paymentDate;
        private string _arrivalTime;
        private string _loadingAddress;
        private string _unloadingAddress;
        private double _routeLength;
        private decimal _orderCost;
        private string _notes;
        private ObservableCollection<CargoItem> _cargoItems;
        private CargoItem _selectedCargoItem;
        private DateTime? _arrivalToLoadingDate;
        private string _arrivalToLoadingTime;

        public Order Order => _order;

        public OrderStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public PaymentMethod PaymentMethod
        {
            get => _paymentMethod;
            set { _paymentMethod = value; OnPropertyChanged(); }
        }

        public PaymentStatus PaymentStatus
        {
            get => _paymentStatus;
            set { _paymentStatus = value; OnPropertyChanged(); }
        }

        public decimal PaidAmount
        {
            get => _paidAmount;
            set { _paidAmount = value; OnPropertyChanged(); }
        }

        public DateTime? PaymentDate
        {
            get => _paymentDate;
            set { _paymentDate = value; OnPropertyChanged(); }
        }

        public string ArrivalTime
        {
            get => _arrivalTime;
            set { _arrivalTime = value; OnPropertyChanged(); }
        }

        public string LoadingAddress
        {
            get => _loadingAddress;
            set { _loadingAddress = value; OnPropertyChanged(); }
        }

        public string UnloadingAddress
        {
            get => _unloadingAddress;
            set { _unloadingAddress = value; OnPropertyChanged(); }
        }

        public double RouteLength
        {
            get => _routeLength;
            set { _routeLength = value; OnPropertyChanged(); }
        }

        public decimal OrderCost
        {
            get => _orderCost;
            set { _orderCost = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CargoItem> CargoItems
        {
            get => _cargoItems;
            set
            {
                _cargoItems = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalWeight));
                OnPropertyChanged(nameof(TotalInsuranceValue));
            }
        }

        public CargoItem SelectedCargoItem
        {
            get => _selectedCargoItem;
            set { _selectedCargoItem = value; OnPropertyChanged(); }
        }

        public DateTime? ArrivalToLoadingDate
        {
            get => _arrivalToLoadingDate;
            set { _arrivalToLoadingDate = value; OnPropertyChanged(); }
        }

        public string ArrivalToLoadingTime
        {
            get => _arrivalToLoadingTime;
            set { _arrivalToLoadingTime = value; OnPropertyChanged(); }
        }

        public double TotalWeight => CargoItems?.Sum(item => item.TotalWeight) ?? 0;
        public decimal TotalInsuranceValue => CargoItems?.Sum(item => item.InsuranceValue) ?? 0;

        // Проверяем, есть ли связанный рейс
        public bool HasTrip => Order.Status == OrderStatus.InProgress || Order.Status == OrderStatus.Paid;

        public ICommand AddCargoItemCommand { get; }
        public ICommand RemoveCargoItemCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public EditOrderViewModel(Order order)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));

            // Инициализация свойств из заказа
            InitializeFromOrder();

            // Инициализация команд
            AddCargoItemCommand = new RelayCommand(AddCargoItem);
            RemoveCargoItemCommand = new RelayCommand(RemoveCargoItem, CanRemoveCargoItem);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            // Загружаем информацию о рейсе, если он есть
            LoadTripInfo();
        }
        public EditOrderViewModel()
        {
            // Инициализация свойств из заказа
            InitializeFromOrder();

            // Инициализация команд
            AddCargoItemCommand = new RelayCommand(AddCargoItem);
            RemoveCargoItemCommand = new RelayCommand(RemoveCargoItem, CanRemoveCargoItem);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            // Загружаем информацию о рейсе, если он есть
            LoadTripInfo();
        }
        private void InitializeFromOrder()
        {
            Status = _order.Status;
            PaymentMethod = _order.PaymentMethod;
            PaymentStatus = _order.PaymentStatus;
            PaidAmount = _order.PaidAmount;
            PaymentDate = _order.PaymentDate;
            LoadingAddress = _order.LoadingAddress;
            UnloadingAddress = _order.UnloadingAddress;
            RouteLength = _order.RouteLength;
            OrderCost = _order.OrderCost;
            Notes = _order.Notes;

            // Копируем грузы в новую коллекцию для редактирования
            CargoItems = new ObservableCollection<CargoItem>();
            foreach (var cargoItem in _order.CargoItems)
            {
                // Создаем копию груза для редактирования
                var copy = new CargoItem
                {
                    Id = cargoItem.Id,
                    Name = cargoItem.Name,
                    Unit = cargoItem.Unit,
                    Quantity = cargoItem.Quantity,
                    TotalWeight = cargoItem.TotalWeight,
                    InsuranceValue = cargoItem.InsuranceValue,
                    CargoType = cargoItem.CargoType,
                    Description = cargoItem.Description,
                    AssignedOrder = _order
                };
                CargoItems.Add(copy);
            }

            // Форматируем время прибытия из PaymentDate
            if (PaymentDate.HasValue)
            {
                ArrivalTime = PaymentDate.Value.ToString("HH:mm");
            }
        }

        private void LoadTripInfo()
        {
            // Ищем связанный рейс
            var dataService = Services.DataService.Instance;
            var trip = dataService.Trips.FirstOrDefault(t =>
                t.Items != null &&
                t.Items.Any(item =>
                    item.AssignedOrder != null &&
                    item.AssignedOrder.Id == _order.Id));

            if (trip != null)
            {
                ArrivalToLoadingDate = trip.ArrivalToLoadingTime.Date;
                ArrivalToLoadingTime = trip.ArrivalToLoadingTime.ToString("HH:mm");
            }
        }

        private void AddCargoItem(object parameter)
        {
            var cargoItem = new CargoItem
            {
                Name = "Новый груз",
                Unit = "шт.",
                Quantity = 1,
                TotalWeight = 0,
                InsuranceValue = 0,
                CargoType = CargoType.General,
                AssignedOrder = _order
            };
            CargoItems.Add(cargoItem);
            SelectedCargoItem = cargoItem;
        }

        private bool CanRemoveCargoItem(object parameter)
        {
            return SelectedCargoItem != null;
        }

        private void RemoveCargoItem(object parameter)
        {
            if (SelectedCargoItem != null)
            {
                CargoItems.Remove(SelectedCargoItem);
                SelectedCargoItem = null;
            }
        }

        private void Save(object parameter)
        {
            try
            {
                ValidateInput();

                // Обновляем свойства заказа
                _order.Status = Status;
                _order.PaymentMethod = PaymentMethod;
                _order.PaymentStatus = PaymentStatus;
                _order.PaidAmount = PaidAmount;

                // Обновляем PaymentDate с учетом времени прибытия
                if (PaymentDate.HasValue && !string.IsNullOrWhiteSpace(ArrivalTime))
                {
                    if (DateTime.TryParse($"{PaymentDate.Value:yyyy-MM-dd} {ArrivalTime}",
                        out DateTime paymentDateTime))
                    {
                        _order.PaymentDate = paymentDateTime;
                    }
                }

                _order.LoadingAddress = LoadingAddress;
                _order.UnloadingAddress = UnloadingAddress;
                _order.RouteLength = RouteLength;
                _order.OrderCost = OrderCost;
                _order.Notes = Notes;

                // Обновляем грузы
                _order.CargoItems.Clear();
                foreach (var cargoItem in CargoItems)
                {
                    // Обновляем ссылку на заказ
                    cargoItem.AssignedOrder = _order;
                    _order.CargoItems.Add(cargoItem);
                }

                // Обновляем информацию о рейсе, если есть
                UpdateTripInfo();

                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTripInfo()
        {
            var dataService = Services.DataService.Instance;
            var trip = dataService.Trips.FirstOrDefault(t =>
                t.Items != null &&
                t.Items.Any(item =>
                    item.AssignedOrder != null &&
                    item.AssignedOrder.Id == _order.Id));

            if (trip != null && ArrivalToLoadingDate.HasValue && !string.IsNullOrWhiteSpace(ArrivalToLoadingTime))
            {
                if (DateTime.TryParse($"{ArrivalToLoadingDate.Value:yyyy-MM-dd} {ArrivalToLoadingTime}",
                    out DateTime arrivalDateTime))
                {
                    trip.ArrivalToLoadingTime = arrivalDateTime;
                }
            }
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(LoadingAddress))
                throw new ArgumentException("Не указан адрес погрузки");

            if (string.IsNullOrWhiteSpace(UnloadingAddress))
                throw new ArgumentException("Не указан адрес разгрузки");

            if (RouteLength <= 0)
                throw new ArgumentException("Длина маршрута должна быть больше 0");

            if (OrderCost <= 0)
                throw new ArgumentException("Стоимость заказа должна быть больше 0");

            if (PaidAmount < 0 || PaidAmount > OrderCost)
                throw new ArgumentException("Оплаченная сумма некорректна");

            if (CargoItems.Count == 0)
                throw new ArgumentException("Добавьте хотя бы один груз");

            // Проверка времени прибытия
            if (!string.IsNullOrWhiteSpace(ArrivalTime))
            {
                if (!DateTime.TryParse($"2000-01-01 {ArrivalTime}", out _))
                    throw new ArgumentException("Некорректный формат времени прибытия (используйте HH:mm)");
            }

            // Проверка времени прибытия на погрузку, если указано
            if (!string.IsNullOrWhiteSpace(ArrivalToLoadingTime))
            {
                if (!DateTime.TryParse($"2000-01-01 {ArrivalToLoadingTime}", out _))
                    throw new ArgumentException("Некорректный формат времени прибытия на погрузку (используйте HH:mm)");
            }
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