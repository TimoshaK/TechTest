using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static P2S3.Programm;
namespace P2S3
{
    public class Animal
    {
        private CatWeight weight;
        public CatBreed Breed { get; }
        public CatWeight Weight 
        {
            get { return weight;  }
            set { weight = value; } 
        }
        public string Name { get; set; }
        public Animal()
        {
            Name = "Дворняга";
            Weight = new CatWeight(1);    
            Breed = (CatBreed)4;
        }
        public void SetWeight(float newWeight)
        {
            if(newWeight<=0.2)
            {
                BCP(); Console.WriteLine($"║{Name} Совсем истощал!!!");
                return;
            }
            weight = new CatWeight(newWeight); // Создаем новую структуру
        }
        public Animal(string name, float weight, CatBreed breed)
        {
            Name = name;
            if ((weight > 0) & (weight < 20))
            {
                Weight = new CatWeight(weight);
            }
            Breed = breed;
        }
        protected string GetBaseInfo()
        {
            return $"Кличка:{Name}, вес:{Weight.ToString():F2},порода: {Breed}";
        }
        public override string ToString()
        {
            return $"Кличка:{Name}, вес:{Weight.ToString():F2},порода: {Breed}";
        }
        public void Deconstruct(out CatWeight weight, out CatBreed breed)
        {
            weight = Weight;
            breed = Breed;
        }

        ~Animal() { }
    }
}
