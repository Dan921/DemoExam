using DemoExamApp.Pages.Needs;
using DemoExamApp.Pages.Offers;
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
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ClientsPage());
        }

        private void RealtorsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new RealtorsPage());
        }

        private void PropertyButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new PropertyPage());
        }

        private void OffersButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new OffersPage());
        }

        private void NeedsButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new NeedsPage());
        }
    }
}
