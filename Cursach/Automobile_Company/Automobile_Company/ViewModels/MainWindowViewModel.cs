using Automobile_Company.Commands;
using Automobile_Company.Dialogs;
using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using Automobile_Company.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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
        // Статистика - Главное окно
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
            // Инициализация команд для Binding
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
            InitializeSampleData();
            SubscribeToCollectionChanges();
        }
        //Опционально. Начальные данные
        private void InitializeSampleData()
        {
            InitializeSampleClients();
            InitializeSampleDrivers();
            InitializeSampleVehicles();
            InitializeSampleOrders();
        }
        private void InitializeSampleClients()
        {
            // 2 физических лица
            var individual1 = new IndividualClient
            {
                FullName = "Иванов Иван Иванович",
                Phone = "+7 (912) 345-67-89",
                PassportSeries = "4501",
                PassportNumber = "123456",
                PassportIssueDate = new DateTime(2015, 5, 15),
                PassportIssuedBy = "ОВД Центрального района г. Москвы"
            };

            var individual2 = new IndividualClient
            {
                FullName = "Петрова Анна Сергеевна",
                Phone = "+7 (923) 456-78-90",
                PassportSeries = "4502",
                PassportNumber = "654321",
                PassportIssueDate = new DateTime(2018, 8, 22),
                PassportIssuedBy = "ОВД Ленинского района г. Санкт-Петербурга"
            };

            // 2 юридических лица
            var legal1 = new LegalClient
            {
                CompanyName = "ООО 'СтройМонтаж'",
                Phone = "+7 (495) 123-45-67",
                DirectorName = "Сидоров Алексей Петрович",
                LegalAddress = "г. Москва, ул. Строителей, д. 15",
                BankName = "Сбербанк России",
                BankAccount = "40702810500000012345",
                Inn = "7712345678"
            };

            var legal2 = new LegalClient
            {
                CompanyName = "АО 'ПромТранс'",
                Phone = "+7 (812) 987-65-43",
                DirectorName = "Козлова Мария Ивановна",
                LegalAddress = "г. Санкт-Петербург, пр. Промышленный, д. 42",
                BankName = "ВТБ",
                BankAccount = "40702810600000054321",
                Inn = "7812345678"
            };

            Clients.Add(individual1);
            Clients.Add(individual2);
            Clients.Add(legal1);
            Clients.Add(legal2);
        }
        private void InitializeSampleDrivers()
        {
            // 2 водителя
            var driver1 = new Driver
            {
                FullName = "Смирнов Александр Васильевич",
                EmployeeId = "DRV-001",
                BirthYear = 1985,
                ExperienceYears = 12,
                Category = DriverCategory.C,
                SkillClass = DriverClass.I,
                PhoneNumber = "+7 (911) 111-22-33",
                Address = "г. Москва, ул. Водительская, д. 10, кв. 25",
                LicenseExpiryDate = new DateTime(2026, 12, 31),
                Status = DriverStatus.Available,
                Notes = "Ответственный водитель, без нарушений"
            };

            var driver2 = new Driver
            {
                FullName = "Волков Дмитрий Сергеевич",
                EmployeeId = "DRV-002",
                BirthYear = 1990,
                ExperienceYears = 8,
                Category = DriverCategory.C,
                SkillClass = DriverClass.II,
                PhoneNumber = "+7 (911) 222-33-44",
                Address = "г. Москва, ул. Транспортная, д. 5, кв. 12",
                LicenseExpiryDate = new DateTime(2025, 6, 30),
                Status = DriverStatus.Available,
                Notes = "Опыт междугородних перевозок"
            };

            Drivers.Add(driver1);
            Drivers.Add(driver2);


        }
        private void InitializeSampleVehicles()
        {
            // 2 транспортных средства
            var vehicle1 = new Vehicle
            {
                LicensePlate = "А001АА177",
                Brand = "Volvo",
                Model = "FH16",
                LoadCapacity = 20.0,
                Purpose = "Междугородние перевозки",
                YearOfManufacture = 2019,
                YearOfOverhaul = 2022,
                MileageAtYearStart = 250000,
                CurrentMileage = 280000,
                BodyType = VehicleBodyType.Van,
                Status = VehicleStatus.Available,
                NextMaintenanceDate = new DateTime(2024, 6, 15),
                Notes = "В отличном состоянии, недавно прошел ТО"
            };

            var vehicle2 = new Vehicle
            {
                LicensePlate = "В002ВВ177",
                Brand = "MAN",
                Model = "TGX",
                LoadCapacity = 18.0,
                Purpose = "Городские перевозки",
                YearOfManufacture = 2020,
                YearOfOverhaul = 0,
                MileageAtYearStart = 150000,
                CurrentMileage = 165000,
                BodyType = VehicleBodyType.Truck,
                Status = VehicleStatus.Available,
                NextMaintenanceDate = new DateTime(2024, 8, 20),
                Notes = "Экономный расход топлива"
            };

            // Прикрепляем водителей к транспорту
            if (Drivers.Count >= 2)
            {
                vehicle1.AssignedDriver = Drivers[0];
                Drivers[0].AssignedVehicle = vehicle1;

                vehicle2.AssignedDriver = Drivers[1];
                Drivers[1].AssignedVehicle = vehicle2;
            }

            Vehicles.Add(vehicle1);
            Vehicles.Add(vehicle2);


        }
        private void InitializeSampleOrders()
        {
            if (Clients.Count < 2) return;
 
            // 2 заказа
            var order1 = new Order
            {
                OrderDate = DateTime.Now.AddDays(-5),
                Sender = Clients[0], // Первый клиент (физическое лицо)
                Receiver = Clients[2], // Третий клиент (юридическое лицо)
                LoadingAddress = "г. Москва, ул. Промышленная, д. 10, склад №5",
                UnloadingAddress = "г. Санкт-Петербург, пр. Заводской, д. 25",
                RouteLength = 710,
                OrderCost = 150000,
                PaymentMethod = PaymentMethod.BankTransfer,
                PaymentStatus = PaymentStatus.Paid,
                PaidAmount = 150000,
                PaymentDate = DateTime.Now.AddDays(-3),
                Status = OrderStatus.Paid,
                Notes = "Хрупкий груз, требуется осторожная погрузка"
            };

            // Добавляем грузы к первому заказу
            var cargo1 = new CargoItem
            {
                Name = "Электронное оборудование",
                Unit = "шт.",
                Quantity = 1,
                TotalWeight = 1,
                InsuranceValue = 1,
                CargoType = CargoType.General,
                Description = "Компьютерные компоненты",
                AssignedOrder = order1
            };

            var cargo2 = new CargoItem
            {
                Name = "Упаковочные материалы",
                Unit = "кор.",
                Quantity = 1,
                TotalWeight = 2,
                InsuranceValue = 2,
                CargoType = CargoType.General,
                Description = "Картонные коробки",
                AssignedOrder = order1
            };

            order1.CargoItems.Add(cargo1);
            order1.CargoItems.Add(cargo2);
            order1.CargoItems.Add(cargo2); order1.CargoItems.Add(cargo2); order1.CargoItems.Add(cargo2);
            var order2 = new Order
            {
                OrderDate = DateTime.Now.AddDays(-2),
                Sender = Clients[3], // Четвертый клиент (юридическое лицо)
                Receiver = Clients[1], // Второй клиент (физическое лицо)
                LoadingAddress = "г. Санкт-Петербург, ул. Складская, д. 3",
                UnloadingAddress = "г. Москва, ул. Домовая, д. 15, кв. 42",
                RouteLength = 710,
                OrderCost = 85000,
                PaymentMethod = PaymentMethod.Cash,
                PaymentStatus = PaymentStatus.Paid,
                PaidAmount = 85000,
                PaymentDate = DateTime.Now.AddDays(-1),
                Status = OrderStatus.Paid,
                Notes = "Срочная доставка"
            };
            // Добавляем грузы ко второму заказу
            var cargo3 = new CargoItem
            {
                Name = "Мебель",
                Unit = "шт.",
                Quantity = 15,
                TotalWeight = 1200,
                InsuranceValue = 300000,
                CargoType = CargoType.General,
                Description = "Офисная мебель",
                AssignedOrder = order2
            };

            var cargo4 = new CargoItem
            {
                Name = "Оргтехника",
                Unit = "шт.",
                Quantity = 8,
                TotalWeight = 4,
                InsuranceValue = 200000,
                CargoType = CargoType.Fragile,
                Description = "Принтеры, сканеры",
                AssignedOrder = order2
            };

            order2.CargoItems.Add(cargo3);
            order2.CargoItems.Add(cargo4);

            Orders.Add(order1);
            Orders.Add(order2);


        }
        private void SubscribeToCollectionChanges()
        {
            Orders.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(CompletedOrdersCount));
                OnPropertyChanged(nameof(InProgressOrdersCount));
            };
            Drivers.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(AvailableDriversCount));
            };
            Vehicles.CollectionChanged += (s, e) => OnPropertyChanged(nameof(AvailableVehiclesCount));
        }
        // Методы команд
        
        private void CreateOrder(object parameter)
        {
            try
            {
                var dialog = new CreateOrderDialog();
                // Главное окно, расположение формы над главным окном, посередине.
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (dialog.ShowDialog() == true)
                {
                    var createdOrder = dialog.GetCreatedOrder();
                    if (createdOrder != null)
                    {
                        _dataService.AddOrder(createdOrder);
                        _dataService.AddClient(createdOrder.Receiver);
                        _dataService.AddClient(createdOrder.Sender);
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

        private bool CanEditOrder(object parameter) => SelectedOrder?.CanEdit ?? false;

        private void EditOrder(object parameter)
        {
            if (SelectedOrder == null) return;

            try
            {
                var dialog = new EditOrderDialog(SelectedOrder);
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (dialog.ShowDialog() == true)
                {
                    // Заказ уже обновлен в ViewModel, но можно обновить представление
                    MessageBox.Show("Заказ успешно обновлен!",
                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Обновляем свойства для уведомления UI
                    OnPropertyChanged(nameof(CompletedOrdersCount));
                    OnPropertyChanged(nameof(InProgressOrdersCount));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании заказа:\n{ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            }
        }

        private void CreateTrip(object parameter)
        {
            try
            {
                var dialog = new CreateTripDialog();
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (dialog.ShowDialog() == true)
                {
                    var createdTrip = dialog.GetCreatedTrip();
                    if (createdTrip != null)
                    {
                        _dataService.AddTrip(createdTrip);
                        MessageBox.Show($"Рейс #{createdTrip.Id} успешно создан!",
                                      "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        SelectedTrip = createdTrip;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании рейса( MAIN ):\n\n{ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                SelectedTrip.Status = TripStatus.Cancelled;

                // Освобождаем транспорт и водителя
                if (SelectedTrip.Vehicle != null)
                {
                    SelectedTrip.Vehicle.Status =VehicleStatus.Available;
                }
                foreach (var Crew in SelectedTrip.Crews)
                {
                    if (Crew != null)
                    {
                        Crew.Status =DriverStatus.Available;
                    }
                }
                // Освобождаем ордер
                foreach (var Item in SelectedTrip.Items)
                {
                    if (Item != null)
                    {
                        Item.CargoStatus = CargoItemStatus.Wait_trip;
                        Item.AssignedOrder.Status = OrderStatus.Paid;
                    }
                }
            }
        }
        private void AddDriver(object parameter)
        {
            try
            {
                var dialog = new CreateDriverDialog();
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (dialog.ShowDialog() == true)
                {
                    var createdDriver = dialog.GetCreatedDriver();
                    if (createdDriver != null)
                    {
                        _dataService.AddDriver(createdDriver);
                        MessageBox.Show($"Водитель {createdDriver.FullName} успешно добавлен!",
                                      "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        SelectedDriver = createdDriver;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении водителя:\n\n{ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            try
            {
                var dialog = new CreateVehicleDialog();
                dialog.Owner = Application.Current.MainWindow;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                if (dialog.ShowDialog() == true)
                {
                    var createdVehicle = dialog.GetCreatedVehicle();
                    if (createdVehicle != null)
                    {
                        _dataService.AddVehicle(createdVehicle);
                        MessageBox.Show($"Автомобиль {createdVehicle.FullName} успешно добавлен!",
                                      "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        SelectedVehicle = createdVehicle;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении автомобиля:\n\n{ex.Message}",
                               "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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