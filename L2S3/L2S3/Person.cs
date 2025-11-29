using System;

namespace L2S3
{
    public class Person{
        public string Name { get; set; }
        public int Age
        {
            get { return Age; }
            set
            {
                if (value > 0)
                {
                    Age = value;
                }
                else
                {
                    Age = 18;
                }
            }
        }
        public DateTime BirthDay{ get; set; }
        public Person()
        {
            Name = "Иван";
            Age = 20;
            BirthDay = new DateTime(2004, 11, 25);
        }
        public Person(string name, int age, DateTime birthDay)
        {
            Name = name;
            Age = age;
            birthDay = BirthDay;
        }
        public override bool Equals(Object? obj)
        {

            if (obj == null) return false;

            if (ReferenceEquals(this, obj)) return true;

            if (obj is not Person person) return false;

            return this.Name == person.Name && this.Age == person.Age;
        }
        public override int GetHashCode(){
            return this.GetHashCode();
        }
        public override string ToString() 
        {
            return $"{this.Name}, Возраст: {this.Age}, Дата рождения: {BirthDay.ToString("dd MMMM yyyy")}";
        }
    }
}