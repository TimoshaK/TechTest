using System.Windows;
using Automobile_Company.ViewModels;

namespace Automobile_Company.Dialogs
{
    public partial class CreateOrderDialog : Window
    {
        public CreateOrderViewModel ViewModel => DataContext as CreateOrderViewModel;

        public CreateOrderDialog()
        {
            InitializeComponent();
        }

        public Model.Order GetCreatedOrder()
        {
            return ViewModel?.CreatedOrder;
        }
    }
}