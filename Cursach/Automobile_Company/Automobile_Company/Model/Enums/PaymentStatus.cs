using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Статус оплаты
    public enum PaymentStatus
    {
        Pending,        // Ожидает
        Paid,           // Оплачено
        PartiallyPaid,  // Частично оплачено
        Overdue,        // Просрочено
        Cancelled       // Отменено
    }
}
