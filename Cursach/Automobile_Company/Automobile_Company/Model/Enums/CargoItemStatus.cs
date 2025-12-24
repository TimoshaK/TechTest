using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Статус груза
    public enum CargoItemStatus
    {
        Wait_trip,        // Ждет рейс
        Trip_signed,     // Назначен
        OnRoute,          // В пути
        Unloading,        // Доставлен
    }
}

