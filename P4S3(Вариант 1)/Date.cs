using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P4S3
{
            /*Создать класс Дата с полями: день (1-31),
        месяц (1-12), год (целое число).*/
    internal class Date : IComparable<Date>
    {
        private int day;
        private int month;
        private int year;
        public int Day {
            get {return day;}
            set {
                if (value <=0 || value >31) day = 1;
                else if (this.Month == 2) 
                {
                    if (this.Year % 4 ==0) 
                    {
                        if (value <=0 || value >29) day = 1;
                        else day = value;
                        return;
                    }
                    if (value <=0 || value >28) { day = 1; return; }
                }
                day = value;
            }
        }
        public int Month {
            get {return month;}
            set {
                if (value <=0 || value >12) month = 1;
                else month = value;
            }
        }
        public int Year {
            get {return year;}
            set {
                if (value <=0) year = 2000;
                else year = value;
            }
        }
        public Date(){
            Day = 1;
            Month = 1;
            Year = 2000;
        }
        public Date(int A, int B, int C){
            if (C <=0) year = 2000;
                else year = C;
            if (B <=0 || B >12) month = 1;
                else month = B;
            if (A <=0 || A >31) day = 1;
                else if (B == 2) 
                {
                    if (C % 4 ==0) 
                    {
                        if (A <=0 || A >29) day = 1;
                        else day = A;
                        return;
                    }
                    if (A <=0 || A >28) { day = 1; return; }
                }
                day = A;
        }
        public int CompareTo(Date other)
        {
            if (other == null) return 1;
            
            // Сначала сравниваем по году
            int yearComparison = Year.CompareTo(other.Year);
            if (yearComparison != 0) return yearComparison;
            
            // Если годы равны, сравниваем по месяцу
            int monthComparison = Month.CompareTo(other.Month);
            if (monthComparison != 0) return monthComparison;
            
            // Если месяцы равны, сравниваем по дню
            return Day.CompareTo(other.Day);
        }
        public override string ToString(){
            if (Day/10==0){
                if (Month/10==0){
                    return $"0{Day}.0{Month}.{Year}";
                }
                else return $"0{Day}.{Month}.{Year}";
            }
            else if (Month/10==0) return $"{Day}.0{Month}.{Year}";
            return $"{Day}.{Month}.{Year}";
        }
    }
}