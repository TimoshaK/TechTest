using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobile_Company.Model.Enums
{
    // Способ оплаты
    public enum PaymentMethod
    {
        Cash,           // Наличные
        BankTransfer,   // Банковский перевод
        Card,           // Карта
        Online          // Онлайн
    }
}
