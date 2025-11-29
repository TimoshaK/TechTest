using System;

namespace L2S3
{
    public sealed class Worker:Person{
        public int WorkDayTime 
        {
            get {return WorkDayTime; }
            set {
                if ((value > 0) && (value  < 16)) 
                {
                    WorkDayTime = value;
                }
                else
                {
                    WorkDayTime = 0;
                }
            }
        }
        public DateTime StartWorkDay
        {
            get { return StartWorkDay; }
            set
            {
                if (StartWorkDay.ToString("HH mm") == null)
                {
                    StartWorkDay = new DateTime(1,1,1,9,0,0);
                }
                else
                {
                    StartWorkDay = value;
                }
            }
        }

        public int Salary
        {
            get {return Salary; }
            set {
                if (value > 0) 
                {
                    Salary = value;
                }
                else Salary = 0;
            }
        }
        public Work Profession { get; set;}
        public Worker() : base()
        {
            WorkDayTime = 0;
            StartWorkDay = new DateTime();
            Profession = Work.unemployed;
        }
        public Worker(string Name, DateTime BirthDay, int Age, DateTime startWorkDay, int workDayTime, Work profession = Work.unemployed) : base(Name, Age, BirthDay)
        {
            WorkDayTime = workDayTime;
            StartWorkDay = startWorkDay;
            Profession = profession;
        }
        public override string ToString()
        {
            string str = Profession == Work.unemployed ? "Без работы." : StartWorkDay.ToString("HH mm");
            return $"{base.ToString()}\n" +
                $"Профессия: {Profession}, Рабочий день(часов): {WorkDayTime}\n"+
                $"Начало рабочего дня: {str}";
        }
    }
}