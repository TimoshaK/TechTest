using Automobile_Company.Model.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
namespace Automobile_Company.Model
{
    [Serializable]
    [XmlRoot("Order")]
    public class Order : INotifyPropertyChanged
    {
        private Guid _id;
        private DateTime _orderDate;
        private Client _sender;
        private string _loadingAddress;
        private Client _receiver;
        private string _unloadingAddress;
        private double _routeLength;
        private decimal _orderCost;
        private ObservableCollection<CargoItem> _cargoItems;
        private OrderStatus _status;
        private PaymentStatus _paymentStatus;
        private PaymentMethod _paymentMethod;
        private decimal _paidAmount;
        private DateTime? _paymentDate;
        private string _notes;
        private double _progress;
        public Guid Id
        {
            get => _id;
            set
            {
                if (_id == Guid.Empty)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
                // Или если пытаемся установить то же значение (при десериализации)
                else
                {
                    // Ничего не делаем - уже правильное значение
                }
            }
        }
        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }
        [XmlIgnore]
        public Client Sender
        {
            get => _sender;
            set
            {
                _sender = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Sender));
            }
        }

        public string LoadingAddress
        {
            get => _loadingAddress;
            set
            {
                _loadingAddress = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(LoadingAddress));
            }
        }
        [XmlIgnore]
        public Client Receiver
        {
            get => _receiver;
            set
            {
                _receiver = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(Receiver));
            }
        }
        [XmlElement("SenderId")]
        public string SenderId { get; set; }

        [XmlElement("ReceiverId")]
        public string ReceiverId { get; set; }
        public string UnloadingAddress
        {
            get => _unloadingAddress;
            set
            {
                _unloadingAddress = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(UnloadingAddress));
            }
        }

        public double RouteLength
        {
            get => _routeLength;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Длина маршрута не может быть отрицательной");
                _routeLength = value;
                OnPropertyChanged(nameof(RouteLength));
            }
        }

        public decimal OrderCost
        {
            get => _orderCost;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Стоимость заказа не может быть отрицательной");
                _orderCost = value;
                OnPropertyChanged(nameof(OrderCost));
            }
        }
        [XmlIgnore]
        public ObservableCollection<CargoItem> CargoItems
        {
            get => _cargoItems ?? new ObservableCollection<CargoItem>();
            set
            {
                _cargoItems = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged(nameof(CargoItems));
                OnPropertyChanged(nameof(TotalWeight));
            }
        }
        [XmlArray("CargoItems")]
        [XmlArrayItem("CargoItemIds")]
        public List<Guid> CargoItemIds { get; set; }
        public OrderStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(Progress));
            }
        }

        public PaymentStatus PaymentStatus
        {
            get => _paymentStatus;
            set
            {
                _paymentStatus = value;
                OnPropertyChanged(nameof(PaymentStatus));
            }
        }

        public PaymentMethod PaymentMethod
        {
            get => _paymentMethod;
            set
            {
                _paymentMethod = value;
                OnPropertyChanged(nameof(PaymentMethod));
            }
        }

        public decimal PaidAmount
        {
            get => _paidAmount;
            set
            {
                if (value < 0 || value > OrderCost)
                    throw new ArgumentException("Оплаченная сумма некорректна");
                _paidAmount = value;
                OnPropertyChanged(nameof(PaidAmount));

                if (value == OrderCost && OrderCost > 0)
                    PaymentStatus = PaymentStatus.Paid;
                else if (value > 0)
                    PaymentStatus = PaymentStatus.PartiallyPaid;
            }
        }

        public DateTime? PaymentDate
        {
            get => _paymentDate;
            set
            {
                _paymentDate = value;
                OnPropertyChanged(nameof(PaymentDate));
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

        public double TotalWeight
        {
            get
            {
                if (CargoItems == null || CargoItems.Count == 0)
                    return 0;
                return CargoItems.Sum(item => item.TotalWeight);
            }
        }

        public double Progress
        {
            get
            {
                switch (Status)
                {
                    case OrderStatus.New:
                        return 0;
                    case OrderStatus.WaitingPayment:
                        return 0;
                    case OrderStatus.Paid:
                        return 5;
                    case OrderStatus.InProgress:
                        List<int> a = new List<int>();
                        foreach (var item in CargoItems)
                        {
                            if(item.CargoStatus == CargoItemStatus.Wait_trip)
                            {
                                a.Add(0);
                            }
                            else a.Add(1);
                        }
                        
                        return (50 * a.Sum() / a.Count);
                    case OrderStatus.Completed:
                        return 100;
                    case OrderStatus.Cancelled:
                        return 0;
                    default:
                        return 0;
                }
            }
            set { _progress = value; }// добавил для работы, была ошибка биндинга
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Order()
        {
            Id = Guid.NewGuid();
            OrderDate = DateTime.Now;
            Status = OrderStatus.New;
            PaymentStatus = PaymentStatus.Pending;
            CargoItems = new ObservableCollection<CargoItem>();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool CanEdit => Status == OrderStatus.New || Status == OrderStatus.WaitingPayment || Status == OrderStatus.Paid;
        public bool CanCancel => Status == OrderStatus.New || Status == OrderStatus.WaitingPayment|| Status == OrderStatus.Cancelled;
        public bool CanCreateTrip => PaymentStatus == PaymentStatus.Paid && Status == OrderStatus.Paid;
    }
}