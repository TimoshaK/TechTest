using System;

namespace L2S3
{
    public class Officer:Person{
        private DateTime _endDate;
        public Placement Place {  get; set; }
        public DateTime StartDate {  get; set; }
        public DateTime EndDate 
        {
            get { return _endDate; }
            set 
            {
                if (value < StartDate)
                {
                    _endDate = StartDate;
                    StartDate = value;
                }
                else
                {
                    _endDate = value;
                }
            }
        }
        public Officer(string Name,int Age, DateTime BirthDay,  DateTime startDate, 
            DateTime endDate, Placement? place = null, string pas = "1234") : base(Name, Age, BirthDay, pas)
        {
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
        public string BaseInfo()
        {
            return $"{base.ToString()}, Военнослужащий.\nМесто службы: {Place}" +
                $"Период службы: c {StartDate.ToString("d")} " +
                $"по  {EndDate.ToString("d")}";
        }
        public override string ToString()
        {
            return $"{base.ToString()}";
        }
    }
} 