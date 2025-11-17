using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace P5S3.ViewModel
{
    public class MainBaseView : BaseView
    {
        private IEnumerable<XElement> _date;
        private string _dateXml;
        private string _filePath;

        //для свзяки поиска 
        private string _Id = "";
        private string _Day = "";
        private string _Month = "";
        private string _Year = "";
        public string Id
        {
            get => _Id;
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Day
        {
            get => _Day;
            set
            {
                _Day = value;
                OnPropertyChanged(nameof(Day));
            }
        }
        public string Month
        {
            get => _Month;
            set
            {
                _Month = value;
                OnPropertyChanged(nameof(Month));
            }
        }
        public string Year
        {
            get => _Year;
            set
            {
                _Year = value;
                OnPropertyChanged(nameof(Year));
            }
        }
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        public string DateXml
        {
            get => _dateXml;
            set
            {
                _dateXml = value;
                OnPropertyChanged(nameof(DateXml));
            }
        }
        public IEnumerable<XElement> Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        public MainBaseView(string  filePath, XElement element)
        {
            FilePath = filePath;
            DateXml = element.ToString();
            Date = element.Elements("date");
        }
        public MainBaseView()
        {
        }
    }
}
