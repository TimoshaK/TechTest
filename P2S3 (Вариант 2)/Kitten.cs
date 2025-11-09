using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static P2S3.Programm;
namespace P2S3
{
    public class Kitten:Cat
    {
        public Kitten() : base()
        {
            Age = 1;
        }
        public Kitten(string name, float weight, CatBreed breed, float age) 
        : base(name, weight, breed, age) 
        {
            if ((age >= 0) & (age < 2)) Age = age;
            else Age = 1;//котенок до 2х лет, возрат 0 допускаю(новорожденный)
            if (weight <= 0) weight = 1;
        }
        public override void Play()
        {

            if (base.IsPlayful)
            {
                if (Weight.Weight < 0.3)
                {
                    BCP(); Console.WriteLine($"║{base.Name} истощал!");
                    return;
                }
                BCP(); Console.WriteLine($"║{base.Name} Гоняется за бантиком");
                base.SetWeight(Weight.Weight - 0.1f);
                base.IsPlayful = false;
                Programm.emoji(9);
                return;
            }
            base.IsHungry = true;
            BCP(); Console.WriteLine($"║{base.Name} не реагирует на веревочку...");
            BCP(); Console.WriteLine($"║{base.Name} сейчас он не хочет играть!");
        }
        public override void eat(Eats eats)
        {
            if (Weight.Weight >= 2)
            {
                Programm.emoji(44);
                BCP(); Console.WriteLine($"║{base.Name} не нужно столько есть!");
                return;
            }
            if (base.IsHungry)
            {
                if (eats == Eats.milk)
                {
                    BCP(); Console.WriteLine($"║{base.Name} пьет {eats}...");
                    Thread.Sleep(350);
                    BCP(); Console.WriteLine($"║Котенок отходит.");
                    base.SetWeight(Weight.Weight + 0.05f);
                    base.IsHungry = false;
                    base.IsPlayful = true;
                    Programm.emoji(4);
                    return;
                }
                BCP(); Console.WriteLine($"║{base.Name} Нюхает {eats}...");
                Thread.Sleep(350);
                BCP(); Console.WriteLine($"║Котенок не хочет есть это...");
                Programm.emoji(44);
                return;
            }
            Programm.emoji(44);
            BCP(); Console.WriteLine($"║{base.Name} даже не смотрит в миску");
        }
        public override void get_view2()
        {
            BCP(); Console.WriteLine("║Попытка погладить котенка...");
            Thread.Sleep(350);
            if (!IsPlayful & !IsHungry)
            {
                Programm.emoji(8);
                BCP(); Console.WriteLine("║мило мурчит...");
                return;
            }
            base.IsHungry = true;
            Programm.emoji(7);
        }
        public override void call()
        {
            if (InRoom)
            {
                BCP(); Console.WriteLine($"║{base.Name} бежит на руки и спотыкается");
                base.IsHungry = true;
                return;
            }
            BCP(); Console.WriteLine($"║{base.Name} не в комнате");
        }
        public override void uncall()
        {
            BCP(); Console.WriteLine($"║{base.Name} растерялся...");
            base.IsHungry = true;
            return;
        }
        public override string ToString()
        {
            return $"котеночек - {GetBaseInfo()}, Возраст: {Age} годков";
        }
    }
}
