using Microsoft.Win32;
using P5S3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
namespace BouncingButtons
{
    public partial class MainWindow : Window
    {
        
        public static XElement date;
        public static string path;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenXML(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string selectedFilePath = openFileDialog.FileName;
                    path = selectedFilePath;
                    date = XElement.Load(selectedFilePath);
                    var editWindow = new EditWindow(date, path);
                    editWindow.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void CreateXML(object sender, RoutedEventArgs e)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string XFilePath = Path.Combine(currentDirectory, "Dates.xml");
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XComment("список дат"),
                new XElement("dates",
                    new XElement("date",
                    new XAttribute("id", "1"),
                    new XAttribute("day", "19"),
                    new XAttribute("month", "10"),
                    new XAttribute("year", "2006")

                    ),
                    new XElement("date",
                    new XAttribute("id", "2"),
                    new XAttribute("day", "4"),
                    new XAttribute("month", "10"),
                    new XAttribute("year", "2010")
                    ),
                    new XElement("date",
                    new XAttribute("id", "3"),
                    new XAttribute("day", "5"),
                    new XAttribute("month", "10"),
                    new XAttribute("year", "2010")
                    )
                )
            );
            Console.WriteLine(XFilePath);
            path = XFilePath;
            date = doc.Root;
            doc.Save(XFilePath);
            MessageBox.Show($"Сохранено в: {XFilePath}", "Путь к xml",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
            var editWindow = new EditWindow(date, path);
            editWindow.Show();
            this.Close();
        }
    }
}