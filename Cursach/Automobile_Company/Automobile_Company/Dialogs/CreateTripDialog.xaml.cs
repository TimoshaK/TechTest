using System.Windows;
using Automobile_Company.ViewModels;

namespace Automobile_Company.Dialogs
{
    public partial class CreateTripDialog : Window
    {
        public CreateTripViewModel ViewModel => DataContext as CreateTripViewModel;

        public CreateTripDialog()
        {
            InitializeComponent();
        }

        public Model.Trip GetCreatedTrip()
        {
            return ViewModel?.CreatedTrip;
        }
    }
}
