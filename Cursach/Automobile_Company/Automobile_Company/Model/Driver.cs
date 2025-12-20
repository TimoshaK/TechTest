using System;
using System.ComponentModel;
using Automobile_Company.Model.Enums;

namespace Automobile_Company.Model
{
    public class Driver : INotifyPropertyChanged
    {
        private Guid _id;
        private string _fullName;
        private string _employeeId;
        private int _birthYear;
        private int _experienceYears;
        private DriverCategory _category;
        private DriverClass _skillClass;
        private DriverStatus _status;
        private Vehicle _assignedVehicle;
        private string _phoneNumber;
        private string _address;
        private DateTime? _licenseExpiryDate;
        private string _medicalCheckInfo;
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

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string EmployeeId
        {
            get => _employeeId;
            set
            {
                _employeeId = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(EmployeeId));
            }
        }

        public int BirthYear
        {
            get => _birthYear;
            set
            {
                if (value < 1900 || value > DateTime.Now.Year - 18)
                    throw new ArgumentException("Некорректный год рождения");
                _birthYear = value;
                OnPropertyChanged(nameof(BirthYear));
                OnPropertyChanged(nameof(Age));
            }
        }

        public int ExperienceYears
        {
            get => _experienceYears;
            set
            {
                if (value < 0 || value > Age - 18)
                    throw new ArgumentException("Некорректный стаж работы");
                _experienceYears = value;
                OnPropertyChanged(nameof(ExperienceYears));
            }
        }

        public DriverCategory Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
            }
        }

        public DriverClass SkillClass
        {
            get => _skillClass;
            set
            {
                _skillClass = value;
                OnPropertyChanged(nameof(SkillClass));
            }
        }

        public DriverStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(IsAvailable));
            }
        }

        public Vehicle AssignedVehicle
        {
            get => _assignedVehicle;
            set
            {
                _assignedVehicle = value;
                OnPropertyChanged(nameof(AssignedVehicle));
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public DateTime? LicenseExpiryDate
        {
            get => _licenseExpiryDate;
            set
            {
                _licenseExpiryDate = value;
                OnPropertyChanged(nameof(LicenseExpiryDate));
            }
        }

        public string MedicalCheckInfo
        {
            get => _medicalCheckInfo;
            set
            {
                _medicalCheckInfo = value;
                OnPropertyChanged(nameof(MedicalCheckInfo));
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

        public int Age => DateTime.Now.Year - BirthYear;
        public bool IsAvailable => Status == DriverStatus.Available;
        public bool HasAssignedVehicle => AssignedVehicle != null;

        public event PropertyChangedEventHandler PropertyChanged;

        public Driver()
        {
            Id = Guid.NewGuid();
            Status = DriverStatus.Available;
            Category = DriverCategory.C;
            SkillClass = DriverClass.III;
            FullName = "";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanDelete => Status == DriverStatus.Available;
    }
}