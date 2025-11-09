using System;

namespace Praktika3_2
{
    public class OfficeEquipment : ICloneable, IComparable<OfficeEquipment>
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; set; }

        public OfficeEquipment(string name, double weight, decimal price)
        {
            Name = name;
            Weight = weight;
            Price = price;
        }

        public int CompareTo(OfficeEquipment other)
        {
            if (other == null) return 1;
            return Price.CompareTo(other.Price);
        }

        public object Clone()
        {
            return new OfficeEquipment(Name, Weight, Price);
        }

        public override string ToString()
        {
            return $"{Name} | Вес: {Weight} кг | Цена: {Price} руб.";
        }
    }
}
