using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace Automobile_Company.Services
{
    public class DataService
    {
        private static DataService _instance;
        public static DataService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataService();
                }
                return _instance;
            }
        }
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
        }
        private void LoadAllData()
        {
            // Создаем папку Data если ее нет
            Directory.CreateDirectory("Data");

            try
            {
                // Загружаем каждую коллекцию отдельно
                Orders = LoadCollection<Order>(OrdersFile) ?? new ObservableCollection<Order>();
                Trips = LoadCollection<Trip>(TripsFile) ?? new ObservableCollection<Trip>();
                Drivers = LoadCollection<Driver>(DriversFile) ?? new ObservableCollection<Driver>();
                Vehicles = LoadCollection<Vehicle>(VehiclesFile) ?? new ObservableCollection<Vehicle>();
                Clients = LoadCollection<Client>(ClientsFile) ?? new ObservableCollection<Client>();

                // После загрузки восстанавливаем связи между объектами
                RestoreObjectReferences();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // В случае ошибки создаем пустые коллекции
                CreateEmptyCollections();
            }
        }
        private void CreateEmptyCollections()
        {
            Orders = new ObservableCollection<Order>();
            Trips = new ObservableCollection<Trip>();
            Drivers = new ObservableCollection<Driver>();
            Vehicles = new ObservableCollection<Vehicle>();
            Clients = new ObservableCollection<Client>();
        }
        private void RestoreObjectReferences()
        {
            // Восстанавливаем связи Driver-Vehicle
            RestoreDriverVehicleReferences();
            // Восстанавливаем связи Order-Client и Order-Cargo
            RestoreOrderReferences();
            // Восстанавливаем связи Trip
            RestoreTripReferences();
        }

        private void RestoreDriverVehicleReferences()
        {
            // Создаем словари для быстрого поиска по Id
            var driversDict = Drivers.ToDictionary(d => d.Id);
            var vehiclesDict = Vehicles.ToDictionary(v => v.Id);

            // Восстанавливаем связи водителя с транспортом
            foreach (var driver in Drivers)
            {
                if (driver.AssignedVehicleId.HasValue && driver.AssignedVehicleId != Guid.Empty)
                {
                    driver.AssignedVehicle = vehiclesDict[driver.AssignedVehicleId.Value];
                }
            }

            // Восстанавливаем связи транспорта с водителем
            foreach (var vehicle in Vehicles)
            {
                if (vehicle.AssignedDriverId.HasValue && vehicle.AssignedDriverId != Guid.Empty)
                {
                    vehicle.AssignedDriver = driversDict[vehicle.AssignedDriverId.Value];
                }
            }
        }

        private void RestoreOrderReferences()
        {
            // Создаем словарь клиентов для быстрого поиска
            // Так как у Client нет Id, создаем словарь на основе уникальных данных
            var clientsDict = new Dictionary<string, Client>();

            foreach (var client in Clients)
            {
                var key = GetClientKey(client);
                if (!clientsDict.ContainsKey(key))
                {
                    clientsDict[key] = client;
                }
            }

            // Восстанавливаем связи заказа
            foreach (var order in Orders)
            {
                // Восстанавливаем отправителя
                if (!string.IsNullOrEmpty(order.SenderId.ToString()))
                {
                    order.Sender = clientsDict[order.SenderId];
                }

                // Восстанавливаем получателя
                if (!string.IsNullOrEmpty(order.ReceiverId.ToString()))
                {
                    order.Receiver = clientsDict[order.ReceiverId];
                }

                // Восстанавливаем связи грузов с заказом
                foreach (var cargo in order.CargoItems)
                {
                    cargo.AssignedOrder = order;
                    cargo.AssignedOrderId = order.Id;
                }
            }
        }

        private string GetClientKey(Client client)
        {
            if (client is IndividualClient individual)
            {
                return $"IND_{individual.FullName}_{individual.Phone}_{individual.PassportSeries}_{individual.PassportNumber}";
            }
            else if (client is LegalClient legal)
            {
                return $"LEG_{legal.CompanyName}_{legal.Phone}_{legal.Inn}";
            }
            return null;
        }

        private void RestoreTripReferences()
        {
            // Создаем словари для быстрого поиска
            var vehiclesDict = Vehicles.ToDictionary(v => v.Id);
            var driversDict = Drivers.ToDictionary(d => d.Id);
            var cargoDict = GetAllCargoItems().ToDictionary(c => c.Id);

            foreach (var trip in Trips)
            {
                // Восстанавливаем транспорт
                if (trip.VehicleId.HasValue && trip.VehicleId != Guid.Empty)
                {
                    trip.Vehicle = vehiclesDict[trip.VehicleId.Value];
                }

                // Восстанавливаем экипаж
                var restoredCrews = new List<Driver>();
                foreach (var driverId in trip.CrewIds ?? new List<Guid>())
                {
                    if (driverId != Guid.Empty)
                    {
                        var driver = driversDict[driverId];
                        if (driver != null)
                            restoredCrews.Add(driver);
                    }
                }
                trip.Crews = restoredCrews;

                // Восстанавливаем грузы
                var restoredItems = new List<CargoItem>();
                foreach (var cargoId in trip.ItemIds ?? new List<Guid>())
                {
                    if (cargoId != Guid.Empty)
                    {
                        var cargo = cargoDict[cargoId];
                        if (cargo != null)
                            restoredItems.Add(cargo);
                    }
                }
                trip.Items = restoredItems;
            }
        }

        private List<CargoItem> GetAllCargoItems()
        {
            var allCargoItems = new List<CargoItem>();

            foreach (var order in Orders)
            {
                allCargoItems.AddRange(order.CargoItems);
            }

            return allCargoItems;
        }

        public IEnumerable<Driver> GetAvailableDriversForVehicle(Vehicle vehicle)
        {
            return Drivers.Where(d =>
                d.IsAvailable &&
                d.AssignedVehicle == null &&
                // Проверяем, что водитель имеет нужную категорию для этого транспорта
                IsDriverSuitableForVehicle(d, vehicle))
                .OrderBy(d => d.FullName);
        }

        private bool IsDriverSuitableForVehicle(Driver driver, Vehicle vehicle)
        {
            // Базовая проверка - водитель должен иметь категорию CE или C для грузовых
            // Это упрощенная логика, можно расширить
            return driver.Category == DriverCategory.C || driver.Category == DriverCategory.CE;
        }

        private ObservableCollection<T> LoadCollection<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath)) return null;
            
            try
            {
                var serializer = new XmlSerializer(typeof(List<T>));
                using (var reader = new StreamReader(filePath))
                {
                    var items = (List<T>)serializer.Deserialize(reader);
                    return items != null ? new ObservableCollection<T>(items) : null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки {filePath}: {ex.Message}");
                return null;
            }
        }

        public void SaveAllData()
        {
            try
            {
                // Перед сохранением обновляем Id полей связей
                UpdateReferenceIds();
                // Сохраняем каждую коллекцию в отдельный файл
                SaveCollection(OrdersFile, Orders.ToList());
                SaveCollection(TripsFile, Trips.ToList());
                SaveCollection(DriversFile, Drivers.ToList());
                SaveCollection(VehiclesFile, Vehicles.ToList());
                SaveCollection(ClientsFile, Clients.ToList());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void UpdateReferenceIds()
        {
            // Обновляем Id связей для водителей и транспорта
            foreach (var driver in Drivers)
            {
                driver.AssignedVehicleId = driver.AssignedVehicle?.Id;
            }

            foreach (var vehicle in Vehicles)
            {
                vehicle.AssignedDriverId = vehicle.AssignedDriver?.Id;
            }
            // Обновляем ключи клиентов для заказов
            foreach (var order in Orders)
            {
                order.SenderId = GetClientKey(order.Sender);
                order.ReceiverId = GetClientKey(order.Receiver);
            }
            // Обновляем Id связей для рейсов
            foreach (var trip in Trips)
            {
                trip.VehicleId = trip.Vehicle?.Id;
                trip.CrewIds = trip.Crews?.Select(d => d.Id).ToList() ?? new List<Guid>();
                trip.ItemIds = trip.Items?.Select(c => c.Id).ToList() ?? new List<Guid>();
            }
        }
        private void SaveCollection<T>(string filePath, List<T> items)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<T>));
                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, items);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения в {filePath}: {ex.Message}");
                throw;
            }
        }
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

        }
        public void AddTrip(Trip trip)
        {
            if (trip == null) throw new ArgumentNullException(nameof(trip));
            // Обновляем статусы транспортного средства и водителя
            if (trip.Vehicle != null)
            {
                trip.Vehicle.Status = VehicleStatus.OnTrip;
            }
            foreach (var Crew in trip.Crews)
            {
                if (Crew != null)
                {
                    Crew.Status = DriverStatus.OnTrip;
                }
            }
            foreach (var cargoitem in trip.Items)
            {
                if (cargoitem != null)
                {
                    cargoitem.CargoStatus = CargoItemStatus.Trip_signed;
                    cargoitem.AssignedOrder.Status = OrderStatus.InProgress;
                    if (cargoitem.AssignedOrder.PaymentStatus != PaymentStatus.Paid)
                        throw new InvalidOperationException("Ордер не оплачен!");
                }
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

            // Проверяем, нет ли уже такого клиента
            bool clientExists = false;

            if (client is IndividualClient individual)
            {
                clientExists = Clients.OfType<IndividualClient>().Any(c =>
                    c.FullName == individual.FullName &&
                    c.Phone == individual.Phone &&
                    c.PassportSeries == individual.PassportSeries &&
                    c.PassportNumber == individual.PassportNumber);
            }
            else if (client is LegalClient legal)
            {
                clientExists = Clients.OfType<LegalClient>().Any(c =>
                    c.CompanyName == legal.CompanyName &&
                    c.Phone == legal.Phone &&
                    c.Inn == legal.Inn);
            }

            if (!clientExists)
            {
                Clients.Add(client);
            }
            // Если клиент уже существует, можно не добавлять или показать сообщение
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
    }
}