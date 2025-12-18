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
        // Добавляем флаги для типа клиентов
        private ClientType _senderType = ClientType.Individual;
        private ClientType _receiverType = ClientType.Individual;
 
        // Поля для физического лица (отправитель)
        private string _senderFullName;
        private string _senderPassportSeries;
        private string _senderPassportNumber;
        private string _senderPhone;

        // Поля для юридического лица (отправитель)
        private string _senderCompanyName;
        private string _senderDirectorName;
        private string _senderLegalAddress;
        private string _senderInn;

        // Поля для физического лица (получатель)
        private string _receiverFullName;
        private string _receiverPassportSeries;
        private string _receiverPassportNumber;
        private string _receiverPhone;

        // Поля для юридического лица (получатель)
        private string _receiverCompanyName;
        private string _receiverDirectorName;
        private string _receiverLegalAddress;
        private string _receiverInn;

        // Остальные поля остаются
        private string _loadingAddress;
        private string _unloadingAddress;
        private double _routeLength;
        private decimal _orderCost;
        private PaymentMethod _paymentMethod;
        private string _notes;
        private ObservableCollection<CargoItem> _cargoItems;
        private CargoItem _selectedCargoItem;

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

        // Свойства для типа клиентов
        public ClientType SenderType
        {
            get => _senderType;
            set
            {
                _senderType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSenderIndividual));
                OnPropertyChanged(nameof(IsSenderLegal));
            }
        }

        public ClientType ReceiverType
        {
            get => _receiverType;
            set
            {
                _receiverType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsReceiverIndividual));
                OnPropertyChanged(nameof(IsReceiverLegal));
            }
        }

        // Изменяем на обычные get-only свойства (нельзя использовать для привязки IsChecked)
        public bool IsSenderIndividual => SenderType == ClientType.Individual;
        public bool IsSenderLegal => SenderType == ClientType.Legal;
        public bool IsReceiverIndividual => ReceiverType == ClientType.Individual;
        public bool IsReceiverLegal => ReceiverType == ClientType.Legal;

        // Свойства для физического лица (отправитель)
        public string SenderFullName
        {
            get => _senderFullName;
            set { _senderFullName = value; OnPropertyChanged(); }
        }

        public string SenderPassportSeries
        {
            get => _senderPassportSeries;
            set { _senderPassportSeries = value; OnPropertyChanged(); }
        }

        public string SenderPassportNumber
        {
            get => _senderPassportNumber;
            set { _senderPassportNumber = value; OnPropertyChanged(); }
        }

        public string SenderPhone
        {
            get => _senderPhone;
            set { _senderPhone = value; OnPropertyChanged(); }
        }

        // Свойства для юридического лица (отправитель)
        public string SenderCompanyName
        {
            get => _senderCompanyName;
            set { _senderCompanyName = value; OnPropertyChanged(); }
        }

        public string SenderDirectorName
        {
            get => _senderDirectorName;
            set { _senderDirectorName = value; OnPropertyChanged(); }
        }

        public string SenderLegalAddress
        {
            get => _senderLegalAddress;
            set { _senderLegalAddress = value; OnPropertyChanged(); }
        }

        public string SenderInn
        {
            get => _senderInn;
            set { _senderInn = value; OnPropertyChanged(); }
        }

        // Свойства для физического лица (получатель)
        public string ReceiverFullName
        {
            get => _receiverFullName;
            set { _receiverFullName = value; OnPropertyChanged(); }
        }

        public string ReceiverPassportSeries
        {
            get => _receiverPassportSeries;
            set { _receiverPassportSeries = value; OnPropertyChanged(); }
        }

        public string ReceiverPassportNumber
        {
            get => _receiverPassportNumber;
            set { _receiverPassportNumber = value; OnPropertyChanged(); }
        }

        public string ReceiverPhone
        {
            get => _receiverPhone;
            set { _receiverPhone = value; OnPropertyChanged(); }
        }

        // Свойства для юридического лица (получатель)
        public string ReceiverCompanyName
        {
            get => _receiverCompanyName;
            set { _receiverCompanyName = value; OnPropertyChanged(); }
        }

        public string ReceiverDirectorName
        {
            get => _receiverDirectorName;
            set { _receiverDirectorName = value; OnPropertyChanged(); }
        }

        public string ReceiverLegalAddress
        {
            get => _receiverLegalAddress;
            set { _receiverLegalAddress = value; OnPropertyChanged(); }
        }

        public string ReceiverInn
        {
            get => _receiverInn;
            set { _receiverInn = value; OnPropertyChanged(); }
        }

        // Остальные свойства остаются без изменений
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

        public PaymentMethod PaymentMethod
        {
            get => _paymentMethod;
            set { _paymentMethod = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public CargoItem SelectedCargoItem
        {
            get => _selectedCargoItem;
            set { _selectedCargoItem = value; OnPropertyChanged(); }
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

                // Создаем клиента-отправителя
                Client sender = CreateSenderClient();

                // Создаем клиента-получателя
                Client receiver = CreateReceiverClient();

                var order = new Order
                {
                    Sender = sender,
                    Receiver = receiver,
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

        private Client CreateSenderClient()
        {
            if (SenderType == ClientType.Individual)
            {
                return new IndividualClient
                {
                    FullName = SenderFullName,
                    PassportSeries = SenderPassportSeries,
                    PassportNumber = SenderPassportNumber,
                    Phone = SenderPhone
                };
            }
            else
            {
                return new LegalClient
                {
                    CompanyName = SenderCompanyName,
                    DirectorName = SenderDirectorName,
                    LegalAddress = SenderLegalAddress,
                    Inn = SenderInn,
                    Phone = SenderPhone
                };
            }
        }

        private Client CreateReceiverClient()
        {
            if (ReceiverType == ClientType.Individual)
            {
                return new IndividualClient
                {
                    FullName = ReceiverFullName,
                    PassportSeries = ReceiverPassportSeries,
                    PassportNumber = ReceiverPassportNumber,
                    Phone = ReceiverPhone
                };
            }
            else
            {
                return new LegalClient
                {
                    CompanyName = ReceiverCompanyName,
                    DirectorName = ReceiverDirectorName,
                    LegalAddress = ReceiverLegalAddress,
                    Inn = ReceiverInn,
                    Phone = ReceiverPhone
                };
            }
        }

        private void ValidateInput()
        {
            // Валидация отправителя
            if (SenderType == ClientType.Individual)
            {
                if (string.IsNullOrWhiteSpace(SenderFullName))
                    throw new ArgumentException("Не указано ФИО отправителя");
                if (string.IsNullOrWhiteSpace(SenderPhone))
                    throw new ArgumentException("Не указан телефон отправителя");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(SenderCompanyName))
                    throw new ArgumentException("Не указано название компании отправителя");
                if (string.IsNullOrWhiteSpace(SenderPhone))
                    throw new ArgumentException("Не указан телефон отправителя");
            }

            // Валидация получателя
            if (ReceiverType == ClientType.Individual)
            {
                if (string.IsNullOrWhiteSpace(ReceiverFullName))
                    throw new ArgumentException("Не указано ФИО получателя");
                if (string.IsNullOrWhiteSpace(ReceiverPhone))
                    throw new ArgumentException("Не указан телефон получателя");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ReceiverCompanyName))
                    throw new ArgumentException("Не указано название компании получателя");
                if (string.IsNullOrWhiteSpace(ReceiverPhone))
                    throw new ArgumentException("Не указан телефон получателя");
            }

            // Проверка, что отправитель и получатель не одинаковые
            if (AreClientsEqual())
                throw new ArgumentException("Отправитель и получатель не могут быть одинаковыми");

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

        private bool AreClientsEqual()
        {
            if (SenderType != ReceiverType)
                return false;

            if (SenderType == ClientType.Individual)
            {
                return SenderFullName == ReceiverFullName &&
                       SenderPhone == ReceiverPhone &&
                       SenderPassportSeries == ReceiverPassportSeries &&
                       SenderPassportNumber == ReceiverPassportNumber;
            }
            else
            {
                return SenderCompanyName == ReceiverCompanyName &&
                       SenderPhone == ReceiverPhone &&
                       SenderInn == ReceiverInn;
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