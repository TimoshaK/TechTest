using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace L1S3
{
    public class Appliance
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        public Appliance(string brand = "", string model = "", string color = "")
        {
            Brand = brand;
            Model = model;
            Color = color;
        }
        public override string ToString()
        {
            return $"Производитель: {Brand}, Модель: {Model},Цвет: {Color}";
        }
        public virtual void PrintInfo()
        {
            Console.Write(ToString());
        }

    }
}
