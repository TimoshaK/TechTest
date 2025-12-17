using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Статус транспортного средства
    public enum VehicleStatus
    {
        Available,      // Доступен
        OnTrip,         // В рейсе
        Maintenance,    // На обслуживании
        Repair,         // В ремонте
        Reserved        // Зарезервирован
    }
}
