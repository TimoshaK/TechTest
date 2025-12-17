using System;
using System.ComponentModel;
using Automobile_Company.Model.Enums;

namespace Automobile_Company.Model
{
    public class CargoItem : INotifyPropertyChanged
    {
        private Guid _id;
        private string _name;
        private string _unit;
        private double _quantity;
        private double _totalWeight;
        private decimal _insuranceValue;
        private CargoType _cargoType;
        private string _description;

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
                _name = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Unit));
            }
        }

        public double Quantity
        {
            get => _quantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Количество не может быть отрицательным");
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public double TotalWeight
        {
            get => _totalWeight;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Вес не может быть отрицательным");
                _totalWeight = value;
                OnPropertyChanged(nameof(TotalWeight));
            }
        }

        public decimal InsuranceValue
        {
            get => _insuranceValue;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Страховая стоимость не может быть отрицательной");
                _insuranceValue = value;
                OnPropertyChanged(nameof(InsuranceValue));
            }
        }

        public CargoType CargoType
        {
            get => _cargoType;
            set
            {
                _cargoType = value;
                OnPropertyChanged(nameof(CargoType));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CargoItem()
        {
            Id = Guid.NewGuid();
            CargoType = CargoType.General;
            Unit = "шт.";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}