using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Статус заказа на перевозку
    public enum OrderStatus
    {
        New,            // Новый
        WaitingPayment, // Ожидает оплаты
        Paid,           // Оплачен
        InProgress,     // В процессе
        Completed,      // Выполнен
        Cancelled       // Отменен
    }
}
