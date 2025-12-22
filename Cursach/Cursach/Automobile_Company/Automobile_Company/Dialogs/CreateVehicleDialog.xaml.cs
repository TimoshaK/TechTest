using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Automobile_Company.ViewModels;

namespace Automobile_Company.Dialogs
{
    public partial class CreateVehicleDialog : Window
    {
        public CreateVehicleViewModel ViewModel => DataContext as CreateVehicleViewModel;

        public CreateVehicleDialog()
        {
            InitializeComponent();
        }

        public Model.Vehicle GetCreatedVehicle()
        {
            return ViewModel?.CreatedVehicle;
        }
    }
}
