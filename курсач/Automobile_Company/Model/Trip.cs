using System;
using System.ComponentModel;
using Automobile_Company.Model.Enums;

namespace Automobile_Company.Model
{
    public class Trip : INotifyPropertyChanged
    {
        private Guid _id;
        private Order _order;
        private Vehicle _vehicle;
        private Driver _crew;
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
            private set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public Order Order
        {
            get => _order;
            set
            {
                _order = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Order));
            }
        }

        public Vehicle Vehicle
        {
            get => _vehicle;
            set
            {
                _vehicle = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Vehicle));
            }
        }

        public Driver Crew
        {
            get => _crew;
            set
            {
                _crew = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Crew));
            }
        }

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
            set { Progress = value; }
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