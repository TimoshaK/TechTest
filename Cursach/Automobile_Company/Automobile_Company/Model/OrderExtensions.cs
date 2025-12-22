using Automobile_Company.Model;
using System.Linq;

namespace Automobile_Company.Model
{
    public static class OrderExtensions
    {
        public static string DisplayInfo(this Order order)
        {
            return $"Заказ #{order.Id} - {order.Sender.GetClientInfo()} → {order.Receiver.GetClientInfo()}";
        }

        public static string RouteInfo(this Order order)
        {
            return $"{order.LoadingAddress} → {order.UnloadingAddress} ({order.RouteLength} км)";
        }

        public static string CargoInfo(this Order order)
        {
            return $"Груз: {order.TotalWeight:F2} кг, {order.CargoItems.Count} позиций";
        }
    }
}
