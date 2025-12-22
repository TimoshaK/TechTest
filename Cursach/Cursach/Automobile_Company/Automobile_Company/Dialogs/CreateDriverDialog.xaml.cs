using Automobile_Company.ViewModels;
using System.Windows;

namespace Automobile_Company.Dialogs
{
    public partial class CreateDriverDialog : Window
    {
        public CreateDriverViewModel ViewModel => DataContext as CreateDriverViewModel;
        public CreateDriverDialog()
        {
            InitializeComponent();
        }
        public Model.Driver GetCreatedDriver()
        {
            return ViewModel?.CreatedDriver;
        }
    }
}
