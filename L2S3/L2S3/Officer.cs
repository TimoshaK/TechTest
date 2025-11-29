using System;

namespace L2S3
{
    public class Officer:Person{
        public Placement Place {  get; set; }
        public DateTime StartDate 
        {
            get { return StartDate; }
            set { StartDate = value; } 
        }
        public DateTime EndDate 
        {
            get { return EndDate; }
            set 
            {
                if (value < StartDate)
                {
                    EndDate = StartDate;
                    StartDate = value;
                }
                else
                {
                    EndDate = value;
                }
            }
        }
        public Officer(Person _person, DateTime startDate, DateTime endDate, Placement? place = null)
        {
            Name = _person.Name;
            BirthDay = _person.BirthDay;
            Age = _person.Age;
            StartDate = startDate;
            EndDate = endDate;
            if (place != null)
            {
                Place = place.Value;
            }
            else
            {
                Place = Placement.No_info;
            }
        }
        public override string ToString()
        {
            return $"Военослужащий.\nМесто службы: {Place}\n" +
                $"Период служыбы: c {StartDate.ToString("d")} " +
                $"по  {EndDate.ToString("d")}";
        }
    }
} 