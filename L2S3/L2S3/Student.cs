using System;

namespace L2S3
{
    public class Student:Person{
        public int Course
        {
            get { return Course; }
            set
            {
                if (value > 0 && value < 5)
                {
                    Course = value;
                }
                else
                {
                    Course = 2;
                }
            }
        }
        public int Semester
        {
            get { return Semester; }
            set
            {
                if (value > 0 && value < 8 && (Course*2 == Semester || Course * 2 - 1 == Semester) )
                {

                    Semester = value;
                }
                else
                {
                    Semester = (Course * 2 - 1 >0)? Course * 2 - 1: 1;
                }
            }
        }
        public Specialization Way {get; set;}
        public Student() : base()
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
                $"Профессия: {Profession}, Рабочий день(часов): {WorkDayTime}\n" +
                $"Начало рабочего дня: {str}";
        }
    }
}