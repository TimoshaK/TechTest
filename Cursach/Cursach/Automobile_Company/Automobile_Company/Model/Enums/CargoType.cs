using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Тип груза
    public enum CargoType
    {
        General,        // Обычный
        Fragile,        // Хрупкий
        Perishable,     // Скоропортящийся
        Hazardous,      // Опасный
        Bulk,           // Насыпной
        Liquid,         // Жидкость
        Refrigerated    // Рефрижератор
    }
}
