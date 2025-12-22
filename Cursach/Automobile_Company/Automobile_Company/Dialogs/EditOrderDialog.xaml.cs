using Automobile_Company.Model;
using Automobile_Company.ViewModels;
using System.Windows;

namespace Automobile_Company.Dialogs
{
    public partial class EditOrderDialog : Window
    {
        public EditOrderViewModel ViewModel => DataContext as EditOrderViewModel;

        public EditOrderDialog(Order orderToEdit)
        {
            InitializeComponent();

            if (orderToEdit == null)
            {
                MessageBox.Show("Не выбран заказ для редактирования", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            // Устанавливаем ViewModel с передачей заказа
            DataContext = new EditOrderViewModel(orderToEdit);
        }

        public Order GetEditedOrder()
        {
            return ViewModel?.Order;
        }
    }
}