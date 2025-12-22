using Automobile_Company.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
namespace Automobile_Company.Model
{
    [Serializable]
    [XmlRoot("Trip")]
    public class Trip : INotifyPropertyChanged
    {
        private Guid _id;
        private List<CargoItem> _items;
        private Vehicle _vehicle;
        private List<Driver> _crews = new List<Driver> {new Driver(), new Driver(), new Driver() };
        private DateTime _arrivalToLoadingTime;
        private DateTime? _departureTime;
        private DateTime? _arrivalToUnloadingTime;
        private DateTime? _completionTime;
        private TripStatus _status;
        private double _actualRouteLength;
        private double _fuelConsumed;
        private string _notes;

        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        [XmlIgnore]
        public List<CargoItem> Items
        {
            get => _items;
            set
            {
                _items = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Items));
            }
        }
        [XmlArray("ItemIds")]
        [XmlArrayItem("ItemId")]
        public List<Guid> ItemIds { get; set; }
        
        [XmlIgnore]
        public Vehicle Vehicle
        {
            get => _vehicle;
            set
            {
                _vehicle = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Vehicle));
            }
        }
        [XmlElement("VehicleId")]
        public Guid? VehicleId { get; set; }
        [XmlIgnore]
        public List<Driver> Crews
        {
            get => _crews;
            set
            {
                _crews = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Crews));
            }
        }
        [XmlArray("CrewIds")]
        [XmlArrayItem("DriverId")]
        public List<Guid> CrewIds { get; set; }
        public DateTime ArrivalToLoadingTime
        {
            get => _arrivalToLoadingTime;
            set
            {
                _arrivalToLoadingTime = value;
                OnPropertyChanged(nameof(ArrivalToLoadingTime));
            }
        }

        public DateTime? DepartureTime
        {
            get => _departureTime;
            set
            {
                _departureTime = value;
                OnPropertyChanged(nameof(DepartureTime));
            }
        }

        public DateTime? ArrivalToUnloadingTime
        {
            get => _arrivalToUnloadingTime;
            set
            {
                _arrivalToUnloadingTime = value;
                OnPropertyChanged(nameof(ArrivalToUnloadingTime));
            }
        }

        public DateTime? CompletionTime
        {
            get => _completionTime;
            set
            {
                _completionTime = value;
                OnPropertyChanged(nameof(CompletionTime));
            }
        }

        public TripStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(Progress));
                OnPropertyChanged(nameof(CanCancel));
            }
        }

        public double ActualRouteLength
        {
            get => _actualRouteLength;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Фактическая длина маршрута не может быть отрицательной");
                _actualRouteLength = value;
                OnPropertyChanged(nameof(ActualRouteLength));
            }
        }

        public double FuelConsumed
        {
            get => _fuelConsumed;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Расход топлива не может быть отрицательным");
                _fuelConsumed = value;
                OnPropertyChanged(nameof(FuelConsumed));
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        private double _progress;
        public double Progress
        {
            get
            {
                switch (Status)
                {
                    case TripStatus.Created:
                        return 0;
                    case TripStatus.Loading:
                        return 25;
                    case TripStatus.OnRoute:
                        return 50;
                    case TripStatus.Unloading:
                        return 75;
                    case TripStatus.Completed:
                        return 100;
                    case TripStatus.Cancelled:
                        return 0;
                    default:
                        return 0;
                }
            }
            set { _progress = value; }
        }

        public bool CanCancel => Status == TripStatus.Created || Status == TripStatus.Loading;
        public bool CanEdit => Status == TripStatus.Created;

        public event PropertyChangedEventHandler PropertyChanged;

        public Trip()
        {
            Id = Guid.NewGuid();
            Status = TripStatus.Created;
            ArrivalToLoadingTime = DateTime.Now.AddDays(1);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}