using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Статус водителя
    public enum DriverStatus
    {
        Available,      // Доступен
        OnTrip,         // В рейсе
        Vacation,       // В отпуске
        Sick,           // На больничном
        Training        // На обучении
    }
}
