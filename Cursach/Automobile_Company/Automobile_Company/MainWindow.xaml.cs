using Automobile_Company.Model;
using Automobile_Company.Services;
using Automobile_Company.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Automobile_Company
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = DataContext as MainWindowViewModel;

            // Обработчик для нажатия правой кнопки мыши на карточке заказа
            this.PreviewMouseRightButtonDown += MainWindow_PreviewMouseRightButtonDown;
            this.Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Сохраняем все данные при закрытии приложения
                DataService.Instance.SaveAllData();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void MainWindow_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Обработка клика по карточке заказа для выделения
            if (e.OriginalSource is FrameworkElement element)
            {
                var order = element.DataContext as Model.Order;
                if (order != null && _viewModel != null)
                {
                    _viewModel.SelectedOrder = order;
                }
            }
        }

        private void OrderCard_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Проверяем количество кликов
            if (e.ClickCount == 2)
            {
                // Двойной клик
                if (sender is FrameworkElement element && element.DataContext is Model.Order order)
                {
                    if (_viewModel != null && _viewModel.EditOrderCommand.CanExecute(order))
                    {
                        _viewModel.EditOrderCommand.Execute(order);
                    }
                }
                e.Handled = true;
            }
            else if (e.ClickCount == 1)
            {
                // Одиночный клик
                if (sender is FrameworkElement element && element.DataContext is Model.Order order)
                {
                    if (_viewModel != null)
                    {
                        _viewModel.SelectedOrder = order;
                    }
                }
            }
        }
        private void DriverName_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is Vehicle vehicle)
            {
                // Сохраняем текущий автомобиль для использования в меню
                border.ContextMenu = (ContextMenu)this.FindResource("DriverContextMenu");
                border.ContextMenu.Tag = vehicle;
            }
        }
        // Обработчик для перетаскивания карточек (опционально)
        private void OrderCard_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is FrameworkElement element)
            {
                if (element.DataContext is Model.Order order)
                {

                }
            }
        }

        // Обработчик клавиш для быстрых действий
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (_viewModel == null) return;

            // Ctrl+N - создать новый заказ
            if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (_viewModel.CreateOrderCommand.CanExecute(null))
                {
                    _viewModel.CreateOrderCommand.Execute(null);
                    e.Handled = true;
                }
            }

            // Delete - удалить выбранный элемент
            else if (e.Key == Key.Delete)
            {
                var tabControl = this.FindName("MainTabControl") as TabControl;
                if (tabControl != null)
                {
                    switch (tabControl.SelectedIndex)
                    {
                        case 1: // Вкладка заказов
                            if (_viewModel.CancelOrderCommand.CanExecute(_viewModel.SelectedOrder))
                            {
                                _viewModel.CancelOrderCommand.Execute(_viewModel.SelectedOrder);
                                e.Handled = true;
                            }
                            break;
                        case 2: // Вкладка рейсов
                            if (_viewModel.CancelTripCommand.CanExecute(_viewModel.SelectedTrip))
                            {
                                _viewModel.CancelTripCommand.Execute(_viewModel.SelectedTrip);
                                e.Handled = true;
                            }
                            break;
                        case 3: // Вкладка водителей
                            if (_viewModel.DeleteDriverCommand.CanExecute(_viewModel.SelectedDriver))
                            {
                                _viewModel.DeleteDriverCommand.Execute(_viewModel.SelectedDriver);
                                e.Handled = true;
                            }
                            break;
                        case 4: // Вкладка автомобилей
                            if (_viewModel.DeleteVehicleCommand.CanExecute(_viewModel.SelectedVehicle))
                            {
                                _viewModel.DeleteVehicleCommand.Execute(_viewModel.SelectedVehicle);
                                e.Handled = true;
                            }
                            break;
                        case 5: // Вкладка клиентов
                            if (_viewModel.DeleteClientCommand.CanExecute(_viewModel.SelectedClient))
                            {
                                _viewModel.DeleteClientCommand.Execute(_viewModel.SelectedClient);
                                e.Handled = true;
                            }
                            break;
                    }
                }
            }
        }

        // Метод для обновления фильтра заказов при изменении текста поиска
        private void OrderSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Фильтрация происходит через ViewModel
            if (_viewModel != null)
            {
                // ViewModel должна автоматически обновлять FilteredOrders
            }
        }

        // Метод для переключения между видами (карточки/таблица)
        private void ToggleViewButton_Click(object sender, RoutedEventArgs e)
        {
            // Можно добавить кнопку для переключения между карточками и таблицей
            // Показывать/скрывать соответствующие элементы
        }

        // Метод для сортировки заказов
        private void SortOrdersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel != null && sender is ComboBox comboBox)
            {
                // Можно добавить логику сортировки в ViewModel
                // Например: по дате, по стоимости, по статусу
            }
        }
    }
}