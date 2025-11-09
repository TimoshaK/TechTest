using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static P2S3.Programm;
namespace P2S3 
{
    public class Cat : Animal
    {
        private float _age;
        public float Age 
        {
            get { return _age; }
            set 
            {
                if (value >= 0 || value <= 20)
                {
                    _age = value;
                }
                else
                {
                    throw new ArgumentException("Некорректный возраст");
                }
            }
        }
        private bool isHungry = true;
        private bool isPlayful = true;
        private bool inRoom = true;
        public bool IsHungry 
        {
            get {  return isHungry; }
            set { isHungry = value; }
        }
        public bool IsPlayful
        {
            get { return isPlayful; }
            set { isPlayful = value; }
        }
        public bool InRoom
        {
            get { return inRoom; }
        }
        
        public Cat() : base() { Age = 3; }
        public Cat(string Name, float Weight, CatBreed Breed, float age):base(Name, Weight, Breed)
        {
            if ((age >= 2) &(age < 20))
            {
                Age = age;
            }
            else

            {
                Age = 3;
            }
        }
        //поиграть, покормить, получить осуждающий взгляд от кошки(статический).
        public virtual void eat(Eats eats)
        {
            if (!inRoom)
            {
                BCP(); Console.WriteLine($"║{base.Name} не в комнате");
                return;
            }
            if (Weight.Weight >= 5)
            {
                Programm.emoji(44);
                BCP(); Console.WriteLine($"║{base.Name} и так толстый...");
                return;
            }
            if (isHungry|| Weight.Weight>2)
            {
                if ((eats == Eats.milk) || (eats == Eats.water) )
                {
                    BCP(); Console.WriteLine($"║{base.Name} пьет {eats}...");
                    Thread.Sleep(350);
                    BCP(); Console.WriteLine($"║Отходит. Она наелась?");
                    base.SetWeight(Weight.Weight + 0.05f);
                    isHungry = true;
                    return;
                }
                BCP(); Console.WriteLine($"║{base.Name} ест {eats}...");
                Thread.Sleep(350);
                BCP(); Console.WriteLine($"║Отходит и разваливается на полу.");
                base.SetWeight(Weight.Weight + 0.2f);
                isHungry = false;
                return;
            }
            Programm.emoji(44);
            BCP(); Console.WriteLine($"║{base.Name} Нюхает {eats}...");
            Thread.Sleep(150);
            BCP(); Console.WriteLine($"║Уходит.");
            inRoom=false;

        }
        public virtual void Play()
        {
            if (!inRoom)
            {
                BCP(); Console.WriteLine($"║{base.Name} не в комнате");
                return;
            }
            if (!isPlayful)
            {
                BCP(); Console.WriteLine($"║{base.Name} лениво оглядывается на игрушки и зевает...");
                return;
            }
            if (Weight.Weight < 1)
            {
                BCP(); Console.WriteLine($"║{base.Name} истощал!");
                return;
            }
            emoji(9); 
            BCP(); Console.WriteLine($"║{base.Name} гоняется за лазерной указкой...");
            base.SetWeight(Weight.Weight - 0.1f);
            isHungry = true;
            isPlayful = false; 
        }
        public static void get_view(string Name)
        {
            Thread.Sleep(350);
            BCP(); Console.WriteLine($"║{Name} смотрит с безразличным осуждением...");
        }
        public virtual void get_view2()
        {
            if (!inRoom)
            {
                BCP(); Console.WriteLine($"║{base.Name} не в комнате");
                return;
            }
            BCP(); Console.WriteLine("║Попытка погладить кису...");
            Thread.Sleep(350);
            
            if (!isHungry && !isPlayful)
            {
                Programm.emoji(8);
                BCP(); Console.WriteLine("║М-р-р М-р-р...");
                Thread.Sleep(350);
                BCP(); Console.WriteLine("║Ласкается");
                return;
            }
            BCP(); Console.WriteLine("║АЙ! Киса Кусается... ");
            Programm.emoji(7);
            Thread.Sleep(350);
            BCP(); Console.WriteLine("║Кошка смотрит с безразличным осуждением...");
            Thread.Sleep(350);
            BCP(); Console.WriteLine("║Убегает");
            inRoom = false;
            isPlayful = true;
        }
        // Перегрузка оператора сложения +
        public static Cat operator +(Cat cat1, Cat cat2)
        {
            return new Cat
            {
                Name = $"{cat1.Name}+{cat2.Name}",
                Age = (cat1.Age + cat2.Age) / 2
            };
        }
        // присваимвание 
        public static implicit operator Cat(string name)
        {
            return new Cat { Name = name, Age = 1 };
        }
        public virtual void call()
        {
            if (!inRoom)
            {
                BCP(); Console.WriteLine("║Кс-кс-кс...");
                BCP(); Console.WriteLine("║Топот лапок...");
                isHungry = true;
                isPlayful = true;
                inRoom = true;
                return;
            }
            BCP(); Console.WriteLine("║Подбегает и ласкается.. чего она хочет?");
            isHungry = true;
            isPlayful = true;
        }
        public virtual void uncall()
        {
            BCP(); Console.WriteLine("║Убегает...");
            inRoom = false;
        }
        public override string ToString()
        {
            return $"Кошка - {base.ToString()}, Возраст: {Age} годков";
        }

    }
}
