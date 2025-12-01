using System;

namespace L2S3
{
    public class Student:Person{
        private int _course;
        private int _semester;
        public int Course
        {
            get { return _course; }
            set
            {
                if (value > 0 && value <= 5)
                {
                    _course = value;
                }
                else
                {
                    _course = 2;
                }
            }
        }
        public int Semester
        {
            get { return _semester; }
            set
            {
                if (value > 0 && value < 8 && (Course*2 == _semester || Course * 2 - 1 == _semester) )
                {

                    _semester = value;
                }
                else
                {
                    _semester = (Course * 2 - 1 >0)? Course * 2 - 1: 1;
                }
            }
        }
        public Specialization Way {get; set;}
        public Student() : base()
        {
            Course = 1;
            Semester = 1;
            Way = Specialization.engineering;
        }
        public Student(string Name, int Age, DateTime BirthDay,  int course, int semester, 
            Specialization way = Specialization.engineering, string pas = "1234") : base(Name, Age, BirthDay, pas)
        {
            Course = course;
            Semester = semester;
            Way = way;
        }
        public string BaseInfo()
        {
            return $"{base.ToString()}" +
                $"Курс: {Course}, Семестр: {Semester}" +
                $"Направление: {Way}";
        }
        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
}