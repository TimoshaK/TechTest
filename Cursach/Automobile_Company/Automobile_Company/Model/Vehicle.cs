using System;
using System.ComponentModel;
using Automobile_Company.Model.Enums;

namespace Automobile_Company.Model
{
    public class Vehicle : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name;
        private string _licensePlate;
        private string _brand;
        private string _model;
        private double _loadCapacity;
        private string _purpose;
        private int _yearOfManufacture;
        private int _yearOfOverhaul;
        private double _mileageAtYearStart;
        private string _photoPath;
        private VehicleBodyType _bodyType;
        private VehicleStatus _status;
        private Driver _assignedDriver;
        private double _currentMileage;
        private DateTime? _nextMaintenanceDate;
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
        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым");
                _name = value.ToUpper();
                OnPropertyChanged(nameof(Name));
            }
        }
        public string LicensePlate
        {
            get => _licensePlate;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Государственный номер не может быть пустым");
                _licensePlate = value.ToUpper();
                OnPropertyChanged(nameof(LicensePlate));
            }
        }

        public string Brand
        {
            get => _brand;
            set
            {
                _brand = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Brand));
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                _model = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Model));
            }
        }

        public double LoadCapacity
        {
            get => _loadCapacity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Грузоподъемность не может быть отрицательной");
                _loadCapacity = value;
                OnPropertyChanged(nameof(LoadCapacity));
            }
        }

        public string Purpose
        {
            get => _purpose;
            set
            {
                _purpose = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Purpose));
            }
        }

        public int YearOfManufacture
        {
            get => _yearOfManufacture;
            set
            {
                if (value < 1900 || value > DateTime.Now.Year + 1)
                    throw new ArgumentException("Некорректный год выпуска");
                _yearOfManufacture = value;
                OnPropertyChanged(nameof(YearOfManufacture));
            }
        }

        public int YearOfOverhaul
        {
            get => _yearOfOverhaul;
            set
            {
                if (value != 0 && (value < 1900 || value > DateTime.Now.Year + 1))
                    throw new ArgumentException("Некорректный год капитального ремонта");
                _yearOfOverhaul = value;
                OnPropertyChanged(nameof(YearOfOverhaul));
            }
        }

        public double MileageAtYearStart
        {
            get => _mileageAtYearStart;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Пробег не может быть отрицательным");
                _mileageAtYearStart = value;
                OnPropertyChanged(nameof(MileageAtYearStart));
            }
        }

        public string PhotoPath
        {
            get => _photoPath;
            set
            {
                _photoPath = value;
                OnPropertyChanged(nameof(PhotoPath));
            }
        }

        public VehicleBodyType BodyType
        {
            get => _bodyType;
            set
            {
                _bodyType = value;
                OnPropertyChanged(nameof(BodyType));
            }
        }

        public VehicleStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public Driver AssignedDriver
        {
            get => _assignedDriver;
            set
            {
                _assignedDriver = value;
                OnPropertyChanged(nameof(AssignedDriver));
            }
        }

        public double CurrentMileage
        {
            get => _currentMileage;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Текущий пробег не может быть отрицательным");
                _currentMileage = value;
                OnPropertyChanged(nameof(CurrentMileage));
            }
        }

        public DateTime? NextMaintenanceDate
        {
            get => _nextMaintenanceDate;
            set
            {
                _nextMaintenanceDate = value;
                OnPropertyChanged(nameof(NextMaintenanceDate));
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

        public bool IsAvailable => Status == VehicleStatus.Available;
        public string FullName => $"{Brand} {Model} ({LicensePlate})";
        public int Age => DateTime.Now.Year - YearOfManufacture;

        public event PropertyChangedEventHandler PropertyChanged;

        public Vehicle()
        {
            Id = Guid.NewGuid();
            Status = VehicleStatus.Available;
            CurrentMileage = 0;
        }
        public string GetVehicleInfo()
        {
            return $"{Name} {Model}, {LicensePlate}";
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanDelete => Status == VehicleStatus.Available;
    }
}