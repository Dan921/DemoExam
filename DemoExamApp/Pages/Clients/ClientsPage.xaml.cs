using DemoExamApp.DataBase;
using DemoExamApp.Logic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoExamApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        private LevenshteinHelper levenshteinHelper = new LevenshteinHelper();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public ClientsPage()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditClientPage(null));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var clientsForRemoving = ClientsGrid.SelectedItems.Cast<Client>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {clientsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.Clients.RemoveRange(clientsForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    ClientsGrid.ItemsSource = demoExamDBEntities.Clients.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsGrid.SelectedItems.Count > 0)
            {
                Manager.MainFrame.Navigate(new AddEditClientPage((Client)ClientsGrid.SelectedItem));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            demoExamDBEntities.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            ClientsGrid.ItemsSource = demoExamDBEntities.Clients.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                ClientsGrid.ItemsSource = demoExamDBEntities.Clients.ToList();
            }
            else
            {
                var clients = demoExamDBEntities.Clients.ToList();
                var parts = (sender as TextBox).Text.Split();
                foreach(var part in parts)
                {
                    if (part != "")
                    {
                        clients = clients.Where(p => levenshteinHelper.IsLevenshteinDistanceLess3(p.FirstName, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.LastName, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.MiddleName, part)).ToList();
                    }
                }
                ClientsGrid.ItemsSource = clients;
            }
        }
    }
}
