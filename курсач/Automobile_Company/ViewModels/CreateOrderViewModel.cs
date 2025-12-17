using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Automobile_Company.Commands;
using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using Automobile_Company.Services;

namespace Automobile_Company.ViewModels
{
    public class CreateOrderViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;
        
        private Client _selectedSender;
        private Client _selectedReceiver;
        private string _loadingAddress;
        private string _unloadingAddress;
        private double _routeLength;
        private decimal _orderCost;
        private PaymentMethod _paymentMethod;
        private string _notes;
        private ObservableCollection<CargoItem> _cargoItems;
        private CargoItem _selectedCargoItem;

        public ObservableCollection<Client> Clients { get; }
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

        public Client SelectedSender
        {
            get => _selectedSender;
            set
            {
                _selectedSender = value;
                OnPropertyChanged();
            }
        }

        public Client SelectedReceiver
        {
            get => _selectedReceiver;
            set
            {
                _selectedReceiver = value;
                OnPropertyChanged();
            }
        }

        public string LoadingAddress
        {
            get => _loadingAddress;
            set
            {
                _loadingAddress = value;
                OnPropertyChanged();
            }
        }

        public string UnloadingAddress
        {
            get => _unloadingAddress;
            set
            {
                _unloadingAddress = value;
                OnPropertyChanged();
            }
        }

        public double RouteLength
        {
            get => _routeLength;
            set
            {
                _routeLength = value;
                OnPropertyChanged();
            }
        }

        public decimal OrderCost
        {
            get => _orderCost;
            set
            {
                _orderCost = value;
                OnPropertyChanged();
            }
        }

        public PaymentMethod PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                _paymentMethod = value;
                OnPropertyChanged();
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }

        public CargoItem SelectedCargoItem
        {
            get => _selectedCargoItem;
            set
            {
                _selectedCargoItem = value;
                OnPropertyChanged();
            }
        }

        public double TotalWeight => CargoItems?.Sum(item => item.TotalWeight) ?? 0;
        public decimal TotalInsuranceValue => CargoItems?.Sum(item => item.InsuranceValue) ?? 0;

        public ICommand AddCargoItemCommand { get; }
        public ICommand RemoveCargoItemCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Order CreatedOrder { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CreateOrderViewModel()
        {
            _dataService = DataService.Instance;
            Clients = _dataService.Clients;
            CargoItems = new ObservableCollection<CargoItem>();
            PaymentMethod = PaymentMethod.Cash;

            // Инициализация команд
            AddCargoItemCommand = new RelayCommand(AddCargoItem);
            RemoveCargoItemCommand = new RelayCommand(RemoveCargoItem, CanRemoveCargoItem);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
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
                CargoType = CargoType.General
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
                
                var order = new Order
                {
                    Sender = SelectedSender,
                    Receiver = SelectedReceiver,
                    LoadingAddress = LoadingAddress,
                    UnloadingAddress = UnloadingAddress,
                    RouteLength = RouteLength,
                    OrderCost = OrderCost,
                    PaymentMethod = PaymentMethod,
                    Notes = Notes
                };

                // Добавляем грузы в заказ
                foreach (var cargoItem in CargoItems)
                {
                    order.CargoItems.Add(cargoItem);
                }

                CreatedOrder = order;
                
                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании заказа: {ex.Message}", 
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateInput()
        {
            if (SelectedSender == null)
                throw new ArgumentException("Не выбран отправитель");
            
            if (SelectedReceiver == null)
                throw new ArgumentException("Не выбран получатель");
            
            if (string.IsNullOrWhiteSpace(LoadingAddress))
                throw new ArgumentException("Не указан адрес погрузки");
            
            if (string.IsNullOrWhiteSpace(UnloadingAddress))
                throw new ArgumentException("Не указан адрес разгрузки");
            
            if (RouteLength <= 0)
                throw new ArgumentException("Длина маршрута должна быть больше 0");
            
            if (OrderCost <= 0)
                throw new ArgumentException("Стоимость заказа должна быть больше 0");
            
            if (CargoItems.Count == 0)
                throw new ArgumentException("Добавьте хотя бы один груз");
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