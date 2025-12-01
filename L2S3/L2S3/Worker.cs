using System;

namespace L2S3
{
    public sealed class Worker:Person{
        private int _workDayTime;
        private int _salary;
        public TimeSpan StartWorkDay { get; set; }
        public int Salary
        {
            get { return _salary; }
            set {
                if (value > 0) 
                {
                    _salary = value;
                }
                else _salary = 0;
            }
        }
        public Work Profession { get; set;}
        public Worker() : base()
        {
            Salary = 20000;
            StartWorkDay = new TimeSpan(10, 0,0);
            Profession = Work.unemployed;
        }
        public Worker(string Name, int Age,DateTime BirthDay, TimeSpan startWorkDay, 
            int salary, Work profession = Work.unemployed, string pas = "1234") : base(Name, Age, BirthDay, pas)
        {
            Salary = salary;
            StartWorkDay = startWorkDay;
            Profession = profession;
        }
        public string BaseInfo()
        {
            string str = Profession == Work.unemployed ? "Без работы." : (StartWorkDay.ToString(@"hh\.mm")).ToString();
            return $"{base.ToString()}" +
                $" Профессия: {Profession}, Зарплата: {Salary}" +
                $" Начало рабочего дня: {str}";
        }
        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}