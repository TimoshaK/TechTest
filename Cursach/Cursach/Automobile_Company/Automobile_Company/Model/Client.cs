using System;
using System.Xml.Serialization;

namespace Automobile_Company.Model
{
    [Serializable]
    [XmlInclude(typeof(IndividualClient))]
    [XmlInclude(typeof(LegalClient))]
    [XmlRoot("Client")]
    public abstract class Client : IClient
    {
        private string _phone;

        public string Phone
        {
            get => _phone;
            set => _phone = value ?? throw new ArgumentNullException(nameof(value));
        }

        public abstract string GetClientInfo();
    }
    [Serializable]
    // Класс для физического лица
    public class IndividualClient : Client
    {
        private string _fullName;
        private string _passportSeries;
        private string _passportNumber;
        private DateTime _passportIssueDate;
        private string _passportIssuedBy;

        public string FullName
        {
            get => _fullName;
            set => _fullName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string PassportSeries
        {
            get => _passportSeries;
            set => _passportSeries = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string PassportNumber
        {
            get => _passportNumber;
            set => _passportNumber = value ?? throw new ArgumentNullException(nameof(value));
        }

        public DateTime PassportIssueDate
        {
            get => _passportIssueDate;
            set => _passportIssueDate = value;
        }

        public string PassportIssuedBy
        {
            get => _passportIssuedBy;
            set => _passportIssuedBy = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override string GetClientInfo()
        {
            return $"Физ. лицо: {FullName}, тел.: {Phone}";
        }
    }

    // Класс для юридического лица
    [Serializable]
    public class LegalClient : Client
    {
        private string _companyName;
        private string _directorName;
        private string _legalAddress;
        private string _bankName;
        private string _bankAccount;
        private string _inn;

        public string CompanyName
        {
            get => _companyName;
            set => _companyName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string DirectorName
        {
            get => _directorName;
            set => _directorName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string LegalAddress
        {
            get => _legalAddress;
            set => _legalAddress = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string BankName
        {
            get => _bankName;
            set => _bankName = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string BankAccount
        {
            get => _bankAccount;
            set => _bankAccount = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Inn
        {
            get => _inn;
            set => _inn = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override string GetClientInfo()
        {
            return $"Юр. лицо: {CompanyName}, тел.: {Phone}";
        }
    }
}