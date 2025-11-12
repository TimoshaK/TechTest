using Microsoft.Win32;
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
        private static XElement date;
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
                    date = XElement.Load(selectedFilePath);
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
            string XFilePath = Path.Combine(currentDirectory, "books.xml");
            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XComment("список дат"),
                new XElement("dates",
                    new XElement("date",
                    new XAttribute("id", "1"),
                        new XElement("day", "19"),
                        new XElement("month", "10"),
                        new XElement("year", "2006")
                    ),
                    new XElement("date",
                    new XAttribute("id", "2"),
                        new XElement("day", "04"),
                        new XElement("month", "10"),
                        new XElement("year", "2010")
                    )
                    
                )
            );
            Console.WriteLine(XFilePath);
            date = doc.Root;
            doc.Save(XFilePath);
            MessageBox.Show($"{XFilePath}", "СЮДА!",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}