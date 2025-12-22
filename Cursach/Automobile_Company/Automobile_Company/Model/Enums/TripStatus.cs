using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Статус рейса
    public enum TripStatus
    {
        Created,        // Создан
        Loading,        // Погрузка
        OnRoute,        // В пути
        Unloading,      // Разгрузка
        Completed,      // Завершен
        Cancelled       // Отменен
    }
}
