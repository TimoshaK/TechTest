using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L1S3
{

    public class Dishwasher : Appliance
    {
        private int _numberOfPrograms;

        public int NumberOfPrograms
        {
            get { return _numberOfPrograms; }
            set
            {
                if (value > 0 && value <= 15)
                {
                    _numberOfPrograms = value;
                }
                else _numberOfPrograms = 1;
            }
        }

        public Dishwasher(string brand = "", string model = "", string color = "", int numberOfPrograms = 0)
            : base(brand, model, color)
        {
            NumberOfPrograms = numberOfPrograms;
        }
        public override string ToString()
        {
            return $"Посудомойка - {base.ToString()}, Количсетво программ: {NumberOfPrograms}";
        }
        public override void PrintInfo()
        {
            Console.WriteLine(ToString());
        }

    }
}
