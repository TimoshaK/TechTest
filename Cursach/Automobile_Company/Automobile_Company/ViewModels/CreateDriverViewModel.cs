using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Automobile_Company.Commands;
using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using Automobile_Company.Services;

namespace Automobile_Company.ViewModels
{
    public class CreateDriverViewModel : INotifyPropertyChanged
    {
        private string _fullName;
        private string _employeeId;
        private int _birthYear;
        private int _experienceYears;
        private DriverCategory _category;
        private DriverClass _skillClass;
        private string _phoneNumber;
        private string _address;
        private DateTime? _licenseExpiryDate;
        private string _notes;

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        public string EmployeeId
        {
            get => _employeeId;
            set { _employeeId = value; OnPropertyChanged(); }
        }

        public int BirthYear
        {
            get => _birthYear;
            set { _birthYear = value; OnPropertyChanged(); }
        }

        public int ExperienceYears
        {
            get => _experienceYears;
            set { _experienceYears = value; OnPropertyChanged(); }
        }

        public DriverCategory Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        public DriverClass SkillClass
        {
            get => _skillClass;
            set { _skillClass = value; OnPropertyChanged(); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public DateTime? LicenseExpiryDate
        {
            get => _licenseExpiryDate;
            set { _licenseExpiryDate = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Driver CreatedDriver { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CreateDriverViewModel()
        {
            // Устанавливаем значения по умолчанию
            Category = DriverCategory.C;
            SkillClass = DriverClass.III;
            BirthYear = DateTime.Now.Year - 30; // 30 лет по умолчанию
            ExperienceYears = 5;

            // Инициализация команд
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save(object parameter)
        {
            try
            {
                ValidateInput();

                var driver = new Driver
                {
                    FullName = FullName,
                    EmployeeId = EmployeeId,
                    BirthYear = BirthYear,
                    ExperienceYears = ExperienceYears,
                    Category = Category,
                    SkillClass = SkillClass,
                    PhoneNumber = PhoneNumber,
                    Address = Address,
                    LicenseExpiryDate = LicenseExpiryDate,
                    Notes = Notes
                };

                CreatedDriver = driver;

                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании водителя: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(FullName))
                throw new ArgumentException("Не указано ФИО водителя");

            if (string.IsNullOrWhiteSpace(EmployeeId))
                throw new ArgumentException("Не указан табельный номер");

            if (BirthYear < 1900 || BirthYear > DateTime.Now.Year - 18)
                throw new ArgumentException("Некорректный год рождения");

            if (ExperienceYears < 0 || ExperienceYears > (DateTime.Now.Year - BirthYear - 18))
                throw new ArgumentException("Некорректный стаж работы");
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