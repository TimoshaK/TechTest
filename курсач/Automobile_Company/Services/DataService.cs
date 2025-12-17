using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Automobile_Company.Services
{
    public class DataService
    {
        private static DataService _instance;
        public static DataService Instance => _instance ?? new DataService();

        public ObservableCollection<Order> Orders { get; private set; }
        public ObservableCollection<Trip> Trips { get; private set; }
        public ObservableCollection<Driver> Drivers { get; private set; }
        public ObservableCollection<Vehicle> Vehicles { get; private set; }
        public ObservableCollection<Client> Clients { get; private set; }

        private const string OrdersFile = "Data/orders.xml";
        private const string TripsFile = "Data/trips.xml";
        private const string DriversFile = "Data/drivers.xml";
        private const string VehiclesFile = "Data/vehicles.xml";
        private const string ClientsFile = "Data/clients.xml";

        private DataService()
        {
            LoadAllData();

            // Подписываемся на изменения коллекций
            Drivers.CollectionChanged += (s, e) => SaveDrivers();
            Vehicles.CollectionChanged += (s, e) => SaveVehicles();
            Clients.CollectionChanged += (s, e) => SaveClients();
            Orders.CollectionChanged += (s, e) => SaveOrders();
            Trips.CollectionChanged += (s, e) => SaveTrips();
        }

        private void LoadAllData()
        {
            // Создаем папку Data если ее нет
            Directory.CreateDirectory("Data");

            try
            {
                Orders = LoadData<ObservableCollection<Order>>(OrdersFile) ?? new ObservableCollection<Order>();
                Trips = LoadData<ObservableCollection<Trip>>(TripsFile) ?? new ObservableCollection<Trip>();
                Drivers = LoadData<ObservableCollection<Driver>>(DriversFile) ?? new ObservableCollection<Driver>();
                Vehicles = LoadData<ObservableCollection<Vehicle>>(VehiclesFile) ?? new ObservableCollection<Vehicle>();
                Clients = LoadData<ObservableCollection<Client>>(ClientsFile) ?? new ObservableCollection<Client>();
            }
            catch (Exception ex)
            {
                // В случае ошибки создаем пустые коллекции
                Orders = new ObservableCollection<Order>();
                Trips = new ObservableCollection<Trip>();
                Drivers = new ObservableCollection<Driver>();
                Vehicles = new ObservableCollection<Vehicle>();
                Clients = new ObservableCollection<Client>();
                Console.WriteLine($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private T LoadData<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath)) return null;

            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            catch
            {
                return null;
            }
        }

        private void SaveData<T>(string filePath, T data)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                var writer = new StreamWriter(filePath);
                serializer.Serialize(writer, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения данных: {ex.Message}");
            }
        }

        public void SaveOrders() => SaveData(OrdersFile, Orders);
        public void SaveTrips() => SaveData(TripsFile, Trips);
        public void SaveDrivers() => SaveData(DriversFile, Drivers);
        public void SaveVehicles() => SaveData(VehiclesFile, Vehicles);
        public void SaveClients() => SaveData(ClientsFile, Clients);

        // Методы для получения статистики
        public int GetCompletedOrdersCount() => Orders.Count(o => o.Status == OrderStatus.Completed);
        public int GetInProgressOrdersCount() => Orders.Count(o => o.Status == OrderStatus.InProgress);
        public int GetAvailableDriversCount() => Drivers.Count(d => d.IsAvailable);
        public int GetAvailableVehiclesCount() => Vehicles.Count(v => v.IsAvailable);

        // Методы для работы с данными
        public void AddOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            Orders.Add(order);

            // Если заказ оплачен, автоматически создаем рейс
            if (order.PaymentStatus == PaymentStatus.Paid)
            {
                var trip = new Trip
                {
                    Order = order,
                    Status = TripStatus.Created
                };
                Trips.Add(trip);
            }
        }
        public void AddTrip(Trip trip)
        {
            if (trip == null) throw new ArgumentNullException(nameof(trip));

            // Проверяем, что заказ оплачен
            if (trip.Order.PaymentStatus != PaymentStatus.Paid)
                throw new InvalidOperationException("Рейс можно создать только для оплаченного заказа");

            // Обновляем статусы транспортного средства и водителя
            if (trip.Vehicle != null)
            {
                trip.Vehicle.Status = VehicleStatus.OnTrip;
            }

            if (trip.Crew != null)
            {
                trip.Crew.Status = DriverStatus.OnTrip;
            }
            Trips.Add(trip);
        }
        public void AddDriver(Driver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));
            Drivers.Add(driver);
        }
        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
            Vehicles.Add(vehicle);
        }
        public void AddClient(Client client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            Clients.Add(client);
        }

        public void UpdateDriver(Driver driver)
        {
            var existingDriver = Drivers.FirstOrDefault(d => d.Id == driver.Id);
            if (existingDriver != null)
            {
                var index = Drivers.IndexOf(existingDriver);
                Drivers[index] = driver;
            }
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            var existingVehicle = Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
            if (existingVehicle != null)
            {
                var index = Vehicles.IndexOf(existingVehicle);
                Vehicles[index] = vehicle;
            }
        }

        public void DeleteDriver(Driver driver)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));

            if (!driver.CanDelete)
                throw new InvalidOperationException("Невозможно удалить водителя, который находится в работе");

            Drivers.Remove(driver);
            driver.AssignedVehicle = null;
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

            if (!vehicle.CanDelete)
                throw new InvalidOperationException("Невозможно удалить транспортное средство, которое находится в работе");

            Vehicles.Remove(vehicle);
        }

        public void DeleteClient(Client client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Проверяем, есть ли у клиента активные заказы
            bool hasActiveOrders = Orders.Any(o =>
                (o.Sender == client || o.Receiver == client) &&
                (o.Status == Model.Enums.OrderStatus.New ||
                 o.Status == Model.Enums.OrderStatus.WaitingPayment ||
                 o.Status == Model.Enums.OrderStatus.Paid ||
                 o.Status == Model.Enums.OrderStatus.InProgress));

            if (hasActiveOrders)
                throw new InvalidOperationException("Невозможно удалить клиента, у которого есть активные заказы");

            Clients.Remove(client);
        }

        public IEnumerable<Vehicle> GetAvailableVehiclesForCargo(CargoType cargoType, double weight)
        {
            return Vehicles.Where(v =>
                v.IsAvailable &&
                v.LoadCapacity >= weight &&
                IsVehicleSuitableForCargo(v, cargoType));
        }

        private bool IsVehicleSuitableForCargo(Vehicle vehicle, CargoType cargoType)
        {
            // Здесь можно реализовать логику сопоставления типа груза и типа кузова
            /*return cargoType switch
            {
                Model.Enums.CargoType.Refrigerated => vehicle.BodyType == Model.Enums.VehicleBodyType.Refrigerator,
                Model.Enums.CargoType.Liquid => vehicle.BodyType == Model.Enums.VehicleBodyType.Tanker,
                Model.Enums.CargoType.Bulk => vehicle.BodyType == Model.Enums.VehicleBodyType.Trailer,
                _ => true // Для остальных типов подходит любой кузов
            };*/
            return true;
        }

        public IEnumerable<Driver> GetAvailableDriversForVehicle(Vehicle vehicle)
        {
            // Если у машины есть закрепленный водитель, показываем его первым
            var drivers = Drivers.Where(d => d.IsAvailable);

            if (vehicle.AssignedDriver != null && vehicle.AssignedDriver.IsAvailable)
            {
                return drivers.OrderByDescending(d => d == vehicle.AssignedDriver);
            }

            return drivers;
        }
    }
}