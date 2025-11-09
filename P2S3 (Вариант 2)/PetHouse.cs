using P2S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace P2S3
{
    public interface IPetHouse
    {
        void OutputList();//Кто у меня живет?
        void AddAnimal(Animal animal);//Взять еще одного питомца
        void RemoveAnimal(int index);//отдать в добрые руки
        void Feed(int index, Eats eats);//покормить кису
        void Ask(int index);//позвать кису
        void Banish(int index);//прогнать кису
        void Stroke(int index);//получить осждающий взгля или гладить
        void PlayWith(int index);//поиграть с кисой
        void Get_view(int index);
    }
    public class PetHouse : IPetHouse
    {
        public List<Animal> animals = new List<Animal> { };
        public void OutputList()
        {
            Programm.emoji(1);
            ListOfAnimal();
            Programm.LowWall(Console.WindowWidth, Console.GetCursorPosition().Top );
            Console.SetCursorPosition(Console.WindowWidth - 2, Console.GetCursorPosition().Top);
        }
        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
            Programm.emoji(2);
            Programm.BCP(); Console.WriteLine("║Добавление выполнено. выберите сл. метод");
            return;
        }
        public void RemoveAnimal(int index)
        {
            
            if (index >= 0 && index < animals.Count)
            {
                
                animals.RemoveAt(index);
                Programm.emoji(3);
                return;
            }
            else throw new ArgumentException(" Нет питомца с такии номером! ");
        }
        public void Feed(int index, Eats eats)
        {

            if (animals[index] is Cat cat) cat.eat(eats);
            else if (animals[index] is Kitten kitten) kitten.eat(eats);
            else throw new ArgumentException(" Можно кормить только котиков и котят! ");
            return;
        }
        public void Ask(int index)
        {
            
            if (animals[index] is Cat cat) cat.call();
            else if (animals[index] is Kitten kitten) kitten.call();
            else throw new ArgumentException(" Звать можно только котиков или котят! ");
            Programm.emoji(5);
            return;
        }
        public void Banish(int index)
        {

            if (animals[index] is Cat cat) cat.uncall();
            else if (animals[index] is Kitten kitten) kitten.uncall();
            else throw new ArgumentException(" Прогнать можно только котенка или кошку ");
            Programm.emoji(6);
            return;
        }
        public void Stroke(int index)//get_view2! 
        {

            if (animals[index] is Cat cat) cat.get_view2();
            else if (animals[index] is Kitten kitten) kitten.get_view2();
            else throw new ArgumentException("Погладить можно только котят и котиков!");
            return; 
        }
        public void PlayWith(int index)
        {

            if (animals[index] is Cat cat) cat.Play();
            else if (animals[index] is Kitten kitten) kitten.Play();
            else
            {
                throw new ArgumentException("Играть можно только с котиками и котятами!");
            }
            return;
        }
        public void Get_view(int index)
        {
            Programm.emoji(10);
            if (animals[index] is Cat cat) Cat.get_view(cat.Name);
            else if (animals[index] is Kitten kitten) Kitten.get_view(kitten.Name);  
            else throw new ArgumentException("получать взгяд можно только от котиков и котят!");
            
            return;
        }
        public void ListOfAnimal(int select = -1)
        {
            if (animals.Count == 0) { Programm.BCP(); Programm.LowWall(Console.WindowWidth, Console.GetCursorPosition().Top + 1); Console.WriteLine("║Животных нет!"); return; }
            int maxWidth = Console.WindowWidth - 1, i = 1;
            foreach (var animal in animals)
            {
                string text = $"{i}. {animal}";
                
                while (text.Length > 0)
                {
                    string prefix = text.StartsWith("║") ? "║ " : "║";
                    int availableWidth = maxWidth - prefix.Length;
                    int takeCount = Math.Min(availableWidth, text.Length);
                    string line = text.Substring(0, takeCount);
                    Programm.BCFC(0); Programm.BCP(); Console.Write(prefix); if (select == i){ Programm.BCFC(1); } Console.WriteLine(line);
                    text = text.Substring(takeCount);
                }
                Programm.BCFC(0);
                i++;
            }
        }
    }
}
