using Automobile_Company.Commands;
using Automobile_Company.Dialogs;
using Automobile_Company.Model;
using Automobile_Company.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Automobile_Company.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService;

        private Order _selectedOrder;
        private Trip _selectedTrip;
        private Driver _selectedDriver;
        private Vehicle _selectedVehicle;
        private Client _selectedClient;
        public ObservableCollection<Order> Orders { get; private set; }
        public ObservableCollection<Trip> Trips { get; private set; }
        public ObservableCollection<Driver> Drivers { get; private set; }
        public ObservableCollection<Vehicle> Vehicles { get; private set; }
        public ObservableCollection<Client> Clients { get; private set; }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public Trip SelectedTrip
        {
            get => _selectedTrip;
            set
            {
                _selectedTrip = value;
                OnPropertyChanged();
            }
        }

        public Driver SelectedDriver
        {
            get => _selectedDriver;
            set
            {
                _selectedDriver = value;
                OnPropertyChanged();
            }
        }

        public Vehicle SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                _selectedVehicle = value;
                OnPropertyChanged();
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        // Статистика
        public int CompletedOrdersCount => _dataService.GetCompletedOrdersCount();
        public int InProgressOrdersCount => _dataService.GetInProgressOrdersCount();
        public int AvailableDriversCount => _dataService.GetAvailableDriversCount();
        public int AvailableVehiclesCount => _dataService.GetAvailableVehiclesCount();

        // Команды
        public ICommand CreateOrderCommand { get; }
        public ICommand EditOrderCommand { get; }
        public ICommand CancelOrderCommand { get; }
        public ICommand CreateTripCommand { get; }
        public ICommand CancelTripCommand { get; }
        public ICommand AddDriverCommand { get; }
        public ICommand EditDriverCommand { get; }
        public ICommand DeleteDriverCommand { get; }
        public ICommand AddVehicleCommand { get; }
        public ICommand EditVehicleCommand { get; }
        public ICommand DeleteVehicleCommand { get; }
        public ICommand DeleteClientCommand { get; }

        public MainWindowViewModel()
        {
            _dataService = DataService.Instance;

            Orders = _dataService.Orders;
            Trips = _dataService.Trips;
            Drivers = _dataService.Drivers;
            Vehicles = _dataService.Vehicles;
            Clients = _dataService.Clients;

            // Инициализация команд
            CreateOrderCommand = new RelayCommand(CreateOrder);
            EditOrderCommand = new RelayCommand(EditOrder, CanEditOrder);
            CancelOrderCommand = new RelayCommand(CancelOrder, CanCancelOrder);
            CreateTripCommand = new RelayCommand(CreateTrip);
            CancelTripCommand = new RelayCommand(CancelTrip, CanCancelTrip);
            AddDriverCommand = new RelayCommand(AddDriver);
            EditDriverCommand = new RelayCommand(EditDriver, CanEditDriver);
            DeleteDriverCommand = new RelayCommand(DeleteDriver, CanDeleteDriver);
            AddVehicleCommand = new RelayCommand(AddVehicle);
            EditVehicleCommand = new RelayCommand(EditVehicle, CanEditVehicle);
            DeleteVehicleCommand = new RelayCommand(DeleteVehicle, CanDeleteVehicle);
            DeleteClientCommand = new RelayCommand(DeleteClient, CanDeleteClient);

            // Подписки на изменения
            SubscribeToCollectionChanges();
        }

        private void SubscribeToCollectionChanges()
        {
            Orders.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(CompletedOrdersCount));
                OnPropertyChanged(nameof(InProgressOrdersCount));
            };

            Drivers.CollectionChanged += (s, e) => OnPropertyChanged(nameof(AvailableDriversCount));
            Vehicles.CollectionChanged += (s, e) => OnPropertyChanged(nameof(AvailableVehiclesCount));
        }

        // Методы команд
        private void CreateOrder(object parameter)
        {
            try
            {
                var dialog = new CreateOrderDialog();

                // Устанавливаем владельца окна
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (dialog.ShowDialog() == true)
                {
                    var createdOrder = dialog.GetCreatedOrder();
                    if (createdOrder != null)
                    {
                        _dataService.AddOrder(createdOrder);

                        MessageBox.Show($"Заказ #{createdOrder.Id} успешно создан!",
                                      "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Автоматически выделяем созданный заказ
                        SelectedOrder = createdOrder;
                    }
                }
            }
            catch (Exception ex)
            {
                // Подробное сообщение об ошибке
                string errorMessage = $"Ошибка при создании заказа:\n\n{ex.Message}";

                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nВнутренняя ошибка: {ex.InnerException.Message}";
                }

                errorMessage += $"\n\nStackTrace:\n{ex.StackTrace}";

                MessageBox.Show(errorMessage, "Критическая ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для показа уведомления (можно реализовать как всплывающее окно)
        private void ShowNotification(string title, string message)
        {
            // Простой вариант через MessageBox
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

            // Или можно создать красивую панель уведомлений
            // CreateNotificationPanel(title, message);
        }

        private bool CanEditOrder(object parameter) => SelectedOrder?.CanEdit ?? false;

        private void EditOrder(object parameter)
        {
            if (SelectedOrder == null) return;

            // Для простоты создаем новый диалог редактирования
            MessageBox.Show("Редактирование заказа будет реализовано в следующей версии",
                "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool CanCancelOrder(object parameter) => SelectedOrder?.CanCancel ?? false;

        private void CancelOrder(object parameter)
        {
            if (SelectedOrder == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите отменить заказ #{SelectedOrder.Id}?",
                "Подтверждение отмены",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SelectedOrder.Status = Model.Enums.OrderStatus.Cancelled;
                // Сохраняем изменения
                _dataService.SaveOrders();
            }
        }

        private void CreateTrip(object parameter)
        {
            /*var dialog = new CreateTripDialog();
            if (dialog.ShowDialog() == true)
            {
                // Логика создания рейса
            }*/
        }

        private bool CanCancelTrip(object parameter) => SelectedTrip?.CanCancel ?? false;

        private void CancelTrip(object parameter)
        {
            if (SelectedTrip == null) return;

            var result = MessageBox.Show(
                $"Вы уверены, что хотите отменить рейс #{SelectedTrip.Id}?",
                "Подтверждение отмены",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SelectedTrip.Status = Model.Enums.TripStatus.Cancelled;

                // Освобождаем транспорт и водителя
                if (SelectedTrip.Vehicle != null)
                {
                    SelectedTrip.Vehicle.Status = Model.Enums.VehicleStatus.Available;
                }

                if (SelectedTrip.Crew != null)
                {
                    SelectedTrip.Crew.Status = Model.Enums.DriverStatus.Available;
                }

                // Сохраняем изменения
                _dataService.SaveTrips();
                _dataService.SaveVehicles();
                _dataService.SaveDrivers();
            }
        }

        private void AddDriver(object parameter)
        {
            /*var dialog = new EditDriverDialog();
            if (dialog.ShowDialog() == true)
            {
                // Логика добавления водителя
            }*/
        }

        private bool CanEditDriver(object parameter) => SelectedDriver != null;

        private void EditDriver(object parameter)
        {
            if (SelectedDriver == null) return;

            /*var dialog = new EditDriverDialog(SelectedDriver);
            if (dialog.ShowDialog() == true)
            {
                // Логика редактирования водителя
            }*/
        }

        private bool CanDeleteDriver(object parameter) => SelectedDriver?.CanDelete ?? false;

        private void DeleteDriver(object parameter)
        {
            if (SelectedDriver == null) return;

            try
            {
                _dataService.DeleteDriver(SelectedDriver);
                MessageBox.Show("Водитель успешно удален", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddVehicle(object parameter)
        {
            /*var dialog = new EditVehicleDialog();
            if (dialog.ShowDialog() == true)
            {
                // Логика добавления транспортного средства
            }*/
        }

        private bool CanEditVehicle(object parameter) => SelectedVehicle != null;

        private void EditVehicle(object parameter)
        {
            if (SelectedVehicle == null) return;

            //var dialog = new EditVehicleDialog(SelectedVehicle);
            //if (dialog.ShowDialog() == true)
            //{
            //   // Логика редактирования транспортного средства
            //}
        }

        private bool CanDeleteVehicle(object parameter) => SelectedVehicle?.CanDelete ?? false;

        private void DeleteVehicle(object parameter)
        {
            if (SelectedVehicle == null) return;

            try
            {
                _dataService.DeleteVehicle(SelectedVehicle);
                MessageBox.Show("Транспортное средство успешно удалено", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDeleteClient(object parameter) => SelectedClient != null;

        private void DeleteClient(object parameter)
        {
            if (SelectedClient == null) return;

            try
            {
                _dataService.DeleteClient(SelectedClient);
                MessageBox.Show("Клиент успешно удален", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}