using System;

namespace L2S3
{
    
    public abstract class Person{
        private int _age;
        private Password _password = new Password("1234");
        public string Name { get; set; }
        public int Age
        {
            get { return _age; }
            set
            {
                if (value > 0)
                {
                    _age = value;
                }
                else
                {
                    _age = 18;
                }
            }
        }
        public DateTime BirthDay{ get; set; }
        private Password password
        {
            set
            {
                _password = value;
            }
        }
        public Person()
        {
            Name = "Иван";
            Age = 20;
            BirthDay = new DateTime(2004, 11, 25);
        }
        public Person(string name, int age, DateTime birthDay, string pas ="1234")
        {
            Name = name;
            Age = age;
            BirthDay = birthDay;
            password = new Password(pas);
        }
        internal bool ComparePasswords(Password password)
        {
            return (this._password).Equals(password);
        }
        public override bool Equals(Object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not Person person) return false;
            return this.Name == person.Name && this.Age == person.Age;
        }
        public void Update_Pasword(string StringData)
        {
            password = new Password(StringData);
        }
        internal string ShowPassword()
        {
            return _password.Value;
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