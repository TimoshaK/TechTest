using System;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Automobile_Company.Commands;
using Automobile_Company.Model;
using Automobile_Company.Model.Enums;
using Automobile_Company.Services;

namespace Automobile_Company.ViewModels
{
    public class CreateVehicleViewModel : INotifyPropertyChanged
    {
        private string _licensePlate;
        private string _brand;
        private string _model;
        private double _loadCapacity;
        private string _purpose;
        private int _yearOfManufacture;
        private int _yearOfOverhaul;
        private double _mileageAtYearStart;
        private double _currentMileage;
        private VehicleBodyType _bodyType;
        private string _photoPath;
        private DateTime? _nextMaintenanceDate;
        private string _notes;
        private BitmapImage _photoImage;

        public string LicensePlate
        {
            get => _licensePlate;
            set { _licensePlate = value; OnPropertyChanged(); }
        }

        public string Brand
        {
            get => _brand;
            set { _brand = value; OnPropertyChanged(); }
        }

        public string Model
        {
            get => _model;
            set { _model = value; OnPropertyChanged(); }
        }

        public double LoadCapacity
        {
            get => _loadCapacity;
            set { _loadCapacity = value; OnPropertyChanged(); }
        }

        public string Purpose
        {
            get => _purpose;
            set { _purpose = value; OnPropertyChanged(); }
        }

        public int YearOfManufacture
        {
            get => _yearOfManufacture;
            set { _yearOfManufacture = value; OnPropertyChanged(); }
        }

        public int YearOfOverhaul
        {
            get => _yearOfOverhaul;
            set { _yearOfOverhaul = value; OnPropertyChanged(); }
        }

        public double MileageAtYearStart
        {
            get => _mileageAtYearStart;
            set { _mileageAtYearStart = value; OnPropertyChanged(); }
        }

        public double CurrentMileage
        {
            get => _currentMileage;
            set { _currentMileage = value; OnPropertyChanged(); }
        }

        public VehicleBodyType BodyType
        {
            get => _bodyType;
            set { _bodyType = value; OnPropertyChanged(); }
        }

        public string PhotoPath
        {
            get => _photoPath;
            set { _photoPath = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasPhoto)); }
        }

        public DateTime? NextMaintenanceDate
        {
            get => _nextMaintenanceDate;
            set { _nextMaintenanceDate = value; OnPropertyChanged(); }
        }

        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(); }
        }

        public BitmapImage PhotoImage
        {
            get => _photoImage;
            set { _photoImage = value; OnPropertyChanged(); }
        }

        public bool HasPhoto => !string.IsNullOrWhiteSpace(PhotoPath);

        public ICommand LoadPhotoCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Vehicle CreatedVehicle { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CreateVehicleViewModel()
        {
            // Устанавливаем значения по умолчанию
            BodyType = VehicleBodyType.Truck;
            YearOfManufacture = DateTime.Now.Year - 5; // 5 лет по умолчанию
            LoadCapacity = 10;
            CurrentMileage = 0;
            MileageAtYearStart = 0;

            // Инициализация команд
            LoadPhotoCommand = new RelayCommand(LoadPhoto);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void LoadPhoto(object parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Все файлы (*.*)|*.*",
                Title = "Выберите фото автомобиля"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    PhotoPath = openFileDialog.FileName;

                    // Загружаем изображение для предпросмотра
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(PhotoPath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    PhotoImage = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке фото: {ex.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Save(object parameter)
        {
            try
            {
                ValidateInput();

                var vehicle = new Vehicle
                {
                    LicensePlate = LicensePlate,
                    Brand = Brand,
                    Model = Model,
                    LoadCapacity = LoadCapacity,
                    Purpose = Purpose,
                    YearOfManufacture = YearOfManufacture,
                    YearOfOverhaul = YearOfOverhaul == 0 ? 0 : YearOfOverhaul,
                    MileageAtYearStart = MileageAtYearStart,
                    CurrentMileage = CurrentMileage,
                    BodyType = BodyType,
                    PhotoPath = PhotoPath,
                    NextMaintenanceDate = NextMaintenanceDate,
                    Notes = Notes
                };

                CreatedVehicle = vehicle;

                if (parameter is Window window)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании автомобиля: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(LicensePlate))
                throw new ArgumentException("Не указан государственный номер");

            if (string.IsNullOrWhiteSpace(Brand))
                throw new ArgumentException("Не указана марка автомобиля");

            if (string.IsNullOrWhiteSpace(Model))
                throw new ArgumentException("Не указана модель автомобиля");

            if (LoadCapacity <= 0)
                throw new ArgumentException("Грузоподъемность должна быть больше 0");

            if (YearOfManufacture < 1900 || YearOfManufacture > DateTime.Now.Year + 1)
                throw new ArgumentException("Некорректный год выпуска");

            if (YearOfOverhaul != 0 && (YearOfOverhaul < 1900 || YearOfOverhaul > DateTime.Now.Year + 1))
                throw new ArgumentException("Некорректный год капитального ремонта");

            if (MileageAtYearStart < 0)
                throw new ArgumentException("Пробег на начало года не может быть отрицательным");

            if (CurrentMileage < 0)
                throw new ArgumentException("Текущий пробег не может быть отрицательным");
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