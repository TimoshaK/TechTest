using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L1S3
{

    public class WashingMachine : Appliance
    {
        private float _capacity;

        public float Capacity
        {
            get { return _capacity; }
            set
            {
                if (value > 0 && value <= 20)
                {
                    _capacity = value;
                }
                else _capacity = 1;
            }
        }

        public WashingMachine(string brand = "", string model = "", string color = "", float capacity = 1)
            : base(brand, model, color)
        {
            Capacity = capacity;
        }
        public override string ToString()
        {
            return $"Стиральная машина - {base.ToString()}, Объем: {Capacity} л";
        }
        public override void PrintInfo()
        {
            Console.WriteLine(ToString());
        }

    }
}
