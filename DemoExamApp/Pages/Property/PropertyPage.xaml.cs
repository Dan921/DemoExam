using DemoExamApp.DataBase;
using DemoExamApp.Logic;
using DemoExamApp.Pages.Property;
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
    /// Логика взаимодействия для PropertyPage.xaml
    /// </summary>
    public partial class PropertyPage : Page
    {
        private LevenshteinHelper levenshteinHelper = new LevenshteinHelper();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public PropertyPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            demoExamDBEntities.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            RealEstatesGrid.ItemsSource = demoExamDBEntities.RealEstates.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                RealEstatesGrid.ItemsSource = demoExamDBEntities.RealEstates.ToList();
            }
            else
            {
                var realEstates = demoExamDBEntities.RealEstates.ToList();

                var parts = (sender as TextBox).Text.Split();
                foreach (var part in parts)
                {
                    if (part != "")
                    {
                        realEstates = realEstates.Where(p => levenshteinHelper.IsLevenshteinDistanceLess3(p.CityAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.StreetAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess1(p.ApartmentNumberAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess1(p.HouseAddress, part)).ToList();
                    }
                }
                RealEstatesGrid.ItemsSource = realEstates;
            }
        }

        private void CoordinateSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var realEstates = demoExamDBEntities.RealEstates.ToList();

            try
            {
                if (!string.IsNullOrWhiteSpace(FromLatitudeSearchTextBox.Text) && !(FromLatitudeSearchTextBox.Text == "-"))
                {
                    realEstates = realEstates.Where(p => p.CoordinateLatitude > int.Parse(FromLatitudeSearchTextBox.Text)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(ToLatitudeSearchTextBox.Text) && !(ToLatitudeSearchTextBox.Text == "-"))
                {
                    realEstates = realEstates.Where(p => p.CoordinateLatitude < int.Parse(ToLatitudeSearchTextBox.Text)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(FromLongitudeSearchTextBox.Text) && !(FromLongitudeSearchTextBox.Text == "-"))
                {
                    realEstates = realEstates.Where(p => p.CoordinateLongitude > int.Parse(FromLongitudeSearchTextBox.Text)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(ToLongitudeSearchTextBox.Text) && !(ToLongitudeSearchTextBox.Text == "-"))
                {
                    realEstates = realEstates.Where(p => p.CoordinateLongitude < int.Parse(ToLongitudeSearchTextBox.Text)).ToList();
                }
                RealEstatesGrid.ItemsSource = realEstates;
            }
            catch
            {
                MessageBox.Show("Неверный формат координат");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var realEstatesForRemoving = RealEstatesGrid.SelectedItems.Cast<RealEstate>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {realEstatesForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.RealEstates.RemoveRange(realEstatesForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    RealEstatesGrid.ItemsSource = demoExamDBEntities.RealEstates.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (RealEstatesGrid.SelectedItems.Count > 0)
            {
                var selectedRealEstate = (RealEstate)RealEstatesGrid.SelectedItem;
                switch (selectedRealEstate.Type)
                {
                    case "Apartment":
                        Manager.MainFrame.Navigate(new AddEditApartmentPage(selectedRealEstate));
                        break;
                    case "House":
                        Manager.MainFrame.Navigate(new AddEditHousePage(selectedRealEstate));
                        break;
                    case "Land":
                        Manager.MainFrame.Navigate(new AddEditLandPage(selectedRealEstate));
                        break;
                }
            }
        }

        private void AddLandButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditLandPage(null));
        }

        private void AddHouseButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditHousePage(null));
        }

        private void AddApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditApartmentPage(null));
        }
    }
}
