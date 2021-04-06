using DemoExamApp.DataBase;
using DemoExamApp.Logic;
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
    /// Логика взаимодействия для RealtorsPage.xaml
    /// </summary>
    public partial class RealtorsPage : Page
    {
        private LevenshteinHelper levenshteinHelper = new LevenshteinHelper();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public RealtorsPage()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditRealtorPage(null));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var realtorsForRemoving = RealtorsGrid.SelectedItems.Cast<Realtor>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {realtorsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.Realtors.RemoveRange(realtorsForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    RealtorsGrid.ItemsSource = demoExamDBEntities.Realtors.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(RealtorsGrid.SelectedItems.Count > 0)
            {
                Manager.MainFrame.Navigate(new AddEditRealtorPage((Realtor)RealtorsGrid.SelectedItem));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            demoExamDBEntities.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            RealtorsGrid.ItemsSource = demoExamDBEntities.Realtors.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                RealtorsGrid.ItemsSource = demoExamDBEntities.Realtors.ToList();
            }
            else
            {
                var realtors = demoExamDBEntities.Realtors.ToList();
                var parts = (sender as TextBox).Text.Split();
                foreach (var part in parts)
                {
                    if (part != "")
                    {
                        realtors = realtors.Where(p => levenshteinHelper.IsLevenshteinDistanceLess3(p.FirstName, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.LastName, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.MiddleName, part)).ToList();
                    }
                }
                RealtorsGrid.ItemsSource = realtors;
            }
        }

        private void OffersButton_Click(object sender, RoutedEventArgs e)
        {
            if (RealtorsGrid.SelectedItems.Count > 0)
            {
                var offerPage = new OffersPage();
                offerPage.SetData(null, ((Realtor)RealtorsGrid.SelectedItem).Id);
                Manager.MainFrame.Navigate(offerPage);
            }
        }

        private void NeedsButton_Click(object sender, RoutedEventArgs e)
        {
            if (RealtorsGrid.SelectedItems.Count > 0)
            {
                var needsPage = new NeedsPage();
                needsPage.SetData(null, ((Realtor)RealtorsGrid.SelectedItem).Id);
                Manager.MainFrame.Navigate(needsPage);
            }
        }
    }
}
