using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace L1S3
{
    public interface For_interface
    {
        public void AddObject(Appliance obj);
        public void RemoveObject(int index);
        public void ListOfObject(int select = -1);
    }
    public class Realize : For_interface
    {
        public List<Appliance> appliances = new List<Appliance>();
        public void AddObject(Appliance obj)
        {
            appliances.Add(obj);
            return;
        }
        public void RemoveObject(int index)
        {
            if (index >= 0 && index < appliances.Count)
            {
                appliances.RemoveAt(index);
                return;
            }
            else throw new ArgumentException(" Нет техники с таким номером ");
        }
        public void ListOfObject(int select = -1 )
        {
            if (appliances.Count == 0) { Programm.BackWall(Programm.x, Programm.X_end_of_Inteface); Programm.LowWall(Programm.x, Programm.X_end_of_Inteface + 1); Console.WriteLine("║Техники нет!"); return; }
            int maxWidth = Programm.x - 1, i = 1, y = Programm.X_end_of_Inteface ;
            foreach (var obj in appliances)
            {
                string text = $"{i}. {obj}";

                while (text.Length > 0)
                {
                    string prefix = text.StartsWith("║") ? "║ " : "║";
                    int availableWidth = maxWidth - prefix.Length;
                    int takeCount = Math.Min(availableWidth, text.Length);
                    string line = text.Substring(0, takeCount);
                    Programm.BCFC(0); Programm.BackWall(Programm.x, y); Console.Write(prefix); if (select == i) { Programm.BCFC(1); }
                    Console.WriteLine(line);
                    text = text.Substring(takeCount);
                    y++;
                }
                Programm.BCFC(0);
                i++;
            }
            Programm.LowWall(Programm.x, y);
            Console.SetCursorPosition(Programm.x - 2, y);
        }
    }

}