using BouncingButtons;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
using System.Xml.Linq;
using P5S3.ViewModel;

namespace P5S3
{
    public partial class EditWindow : Window
    {
        private bool ChangeMode = false;
        private static MainBaseView p;        
        public EditWindow()
        {
            InitializeComponent();
        }
        public EditWindow(XElement date, string path)
        {
            InitializeComponent();
            p = new MainBaseView(path,date);
            mylist.ItemsSource = p.Date;
            this.DataContext = p;
            p.PropertyChanged += UpdateSearchSourse;
            foreach (var item in p.Date)
            {
                int id = (int)item.Attribute("id");
                int day = (int)item.Attribute("day");
                int month = (int)item.Attribute("month");
                int year = (int)item.Attribute("year");
                try
                {
                    var testDate = new DateTime(year, month, day);
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show($"Некорректная дата в элементе с id={id}: {day:00}.{month:00}.{year}.\n" +
                        $"Для корректной работы программы удалите её.",
                     "Ошибка в данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                    continue; 
                }
            }
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();

        }
        private void SortAscending_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IEnumerable<XElement> result = p.Date
                    .OrderBy(item => new DateTime(
                        (int)item.Attribute("year"),
                        (int)item.Attribute("month"),
                        (int)item.Attribute("day")))
                    .ToList();
                mylist.ItemsSource = result;
            }
            catch
            {
                MessageBox.Show("Критическая ошибка !", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void SortDescending_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = from item in p.Date
                         let day = (int)item.Attribute("day")
                         let month = (int)item.Attribute("month")
                         let year = (int)item.Attribute("year")
                         let date = new DateTime(year, month, day)
                         orderby date descending
                         select item;
            mylist.ItemsSource = result;
            }
            catch
            {
                MessageBox.Show("Критическая ошибка !", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FindFor(object sender, RoutedEventArgs e)
        {
            SearchGrid.Visibility = Visibility.Visible;
            
            List<int> res = new List<int> { };
            foreach (var item in p.Date)
            {
                res.Add((int)item.Attribute("id"));
            }

        }
        private void UpdateSearchSourse(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(p.Id) || e.PropertyName == nameof(p.Day) || e.PropertyName == nameof(p.Month) || e.PropertyName == nameof(p.Year))
            {

                var result = from item in p.Date
                             let id = int.TryParse(p.Id, out int idval) ? idval : -1
                             let day = int.TryParse(p.Day, out int dayval) ? dayval : -1
                             let month = int.TryParse(p.Month, out int monthval) ? monthval : -1
                             let year = int.TryParse(p.Year, out int yearval) ? yearval : -1
                             where (
                                   (id == -1 || (int)item.Attribute("id") == id) &&
                                   (day == -1 || (int)item.Attribute("day") == day) &&
                                   (month == -1 || (int)item.Attribute("month") == month) &&
                                   (year == -1 || (int)item.Attribute("year") == year)
                             )
                             select item;
                mylist.ItemsSource = result;
            }
            else
            {
                return;
            }
        } 
        private void CloseFindWindow_Click(object sender, RoutedEventArgs e)
        {
            SearchGrid.Visibility = Visibility.Collapsed;
            p.Day = "";
            p.Id = "";
            p.Year = "";
            p.Month = "";
            mylist.ItemsSource = p.Date;
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            if (mylist.SelectedItem is XElement selectedElement)
            {
                // Конвертируем в List, удаляем элемент и обновляем свойство
                var list = p.Date.ToList();
                list.Remove(selectedElement);
                p.Date = list;
                mylist.ItemsSource = p.Date;
                SearchGrid.Visibility = Visibility.Collapsed;//закрываем окно поиска если оно открыто
                var newXml = new XDocument(
                new XElement("dates",
                    p.Date
                    )
                );
                p.DateXml = newXml.ToString();
            }
            else
            {
                MessageBox.Show("Выберите элемент из списка!", "info",
                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void Change(object sender, RoutedEventArgs e)
        {
            if (mylist.SelectedItem is XElement selectedElement)
            {
                ChangeMode = true;
                SearchGrid.Visibility = Visibility.Collapsed;
                AddData(this, null);
            }
            else
            {
                MessageBox.Show("Выберите элемент из списка!", "info",
                        MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void AddData(object sender, RoutedEventArgs e)
        {
            InputGrid.Visibility = Visibility.Visible;// Показываем панель ввода
            SearchGrid.Visibility = Visibility.Collapsed;//закрываем окно поиска если оно открыто
            if (ChangeMode)
            {
                IdTextBox.Text = (string)(((XElement)mylist.SelectedItem).Attribute("id").Value);
                DayTextBox.Text = (string)(((XElement)mylist.SelectedItem).Attribute("day").Value); ;
                MonthTextBox.Text = (string)(((XElement)mylist.SelectedItem).Attribute("month").Value) ;
                YearTextBox.Text = (string)(((XElement)mylist.SelectedItem).Attribute("year").Value);
            }
            else
            {
                var today = DateTime.Today;
                List<int> res = new List<int> { };
                foreach (var item in p.Date)
                {
                    res.Add((int)item.Attribute("id"));
                }
                int id = res.Max() + 1;
                IdTextBox.Text = id.ToString();
                DayTextBox.Text = today.Day.ToString();
                MonthTextBox.Text = today.Month.ToString();
                YearTextBox.Text = today.Year.ToString();
            }
            ErrorTextBlock.Text = "";
            DayTextBox.Focus();
            DayTextBox.SelectAll();
        }
        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    // Создаем новый элемент
                    int id = int.Parse(IdTextBox.Text);
                    int day = int.Parse(DayTextBox.Text);
                    int month = int.Parse(MonthTextBox.Text);
                    int year = int.Parse(YearTextBox.Text);
                    
                    if (ChangeMode)
                    {
                       
                        var list1 = p.Date.ToList();
                        list1.Remove((XElement)mylist.SelectedItem);
                        p.Date=list1;
                    }
                    var newElement = new XElement("date",
                        new XAttribute("id", id),
                        new XAttribute("day", day),
                        new XAttribute("month", month),
                        new XAttribute("year", year)
                    );
                    var list = p.Date?.ToList() ?? new List<XElement>();
                    list.Add(newElement);
                    p.Date = list;
                    var newXml = new XDocument(
                        new XElement("dates",
                            p.Date
                        )
                    );
                    p.DateXml = newXml.ToString();
                    mylist.ItemsSource = p.Date;
                    InputGrid.Visibility = Visibility.Collapsed;
                    ChangeMode = false;
                    MessageBox.Show("Данные обновлены!", "info",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    ErrorTextBlock.Text = $"Ошибка: {ex.Message}";
                    ChangeMode = false;
                }
            }
        }
        private void CancelAddButton_Click(object sender, RoutedEventArgs e)
        {
            InputGrid.Visibility = Visibility.Collapsed;
            ChangeMode = false;
        }
        private bool ValidateInput()
        {
            ErrorTextBlock.Text = "";
            // Проверка дня
            if ((!int.TryParse(IdTextBox.Text, out int Id) || Id < 0 ) )
            {
                ErrorTextBlock.Text = "Id должен быть числом больше 0 и не занятым!";
                return false;
            }

            // Проверка дня
            if (!int.TryParse(DayTextBox.Text, out int day) || day < 1 || day > 31)
            {
                ErrorTextBlock.Text = "День должен быть числом от 1 до 31";
                return false;
            }

            // Проверка месяца
            if (!int.TryParse(MonthTextBox.Text, out int month) || month < 1 || month > 12)
            {
                ErrorTextBlock.Text = "Месяц должен быть числом от 1 до 12";
                return false;
            }

            // Проверка года
            if (!int.TryParse(YearTextBox.Text, out int year) || year < 1 || year > 9999)
            {
                ErrorTextBlock.Text = "Год должен быть корректным числом";
                return false;
            }

            // Проверка корректности даты
            try
            {
                var testDate = new DateTime(year, month, day);
            }
            catch (ArgumentOutOfRangeException)
            {
                ErrorTextBlock.Text = "Некорректная дата (например, 31 февраля)";
                return false;
            }
            // Проверка на дубликат
            if (CheckForDuplicate(Id, day, month, year))
            {
                ErrorTextBlock.Text = "Такая дата или такой ID уже существует";
                return false;
            }

            return true;
        }
        private bool CheckForDuplicate(int id, int day, int month, int year)
        {
            if (ChangeMode)
            {
                int curID = (int)((XElement)mylist.SelectedItem).Attribute("id");
                return p.Date?.Any(date =>
                ((int)date.Attribute("id") == id) && ((int)date.Attribute("id") != curID)) == true;
            }
            return p.Date?.Any(date =>
                (int)date.Attribute("id") == id ||
                ((int)date.Attribute("day") == day &&
                (int)date.Attribute("month") == month &&
                (int)date.Attribute("year") == year)) == true;
        }

        private void Save(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(p.DateXml))
                {
                    MessageBox.Show("Нет данных для сохранения.", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                MessageBoxResult result = MessageBox.Show(
            $"Вы уверены, что хотите сохранить файл?\n\nПуть: {p.FilePath}",
            "Подтверждение сохранения",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

                // Если пользователь подтвердил - сохраняем
                if (result == MessageBoxResult.Yes)
                {
                    // Сохраняем XML в файл
                    File.WriteAllText(p.FilePath, p.DateXml);

                    MessageBox.Show($"Файл успешно сохранен!\n{p.FilePath}", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Нет прав для сохранения файла.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Директория для сохранения не найдена.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка ввода-вывода: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
