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
            ApartmentsGrid.ItemsSource = demoExamDBEntities.Apartments.ToList();
            HousesGrid.ItemsSource = demoExamDBEntities.Houses.ToList();
            LandsGrid.ItemsSource = demoExamDBEntities.Lands.ToList();
        }

        private void AddApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditApartmentPage(null));
        }

        private void EditApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApartmentsGrid.SelectedItems.Count > 0)
            {
                Manager.MainFrame.Navigate(new AddEditApartmentPage((Apartment)ApartmentsGrid.SelectedItem));
            }
        }

        private void DeleteApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            var apartmentsForRemoving = ApartmentsGrid.SelectedItems.Cast<Apartment>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {apartmentsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.Apartments.RemoveRange(apartmentsForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    ApartmentsGrid.ItemsSource = demoExamDBEntities.Apartments.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void AddHouseButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditHousePage(null));
        }

        private void EditHouseButton_Click(object sender, RoutedEventArgs e)
        {
            if (HousesGrid.SelectedItems.Count > 0)
            {
                Manager.MainFrame.Navigate(new AddEditHousePage((House)HousesGrid.SelectedItem));
            }
        }

        private void DeleteHouseButton_Click(object sender, RoutedEventArgs e)
        {
            var housesForRemoving = HousesGrid.SelectedItems.Cast<House>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {housesForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.Houses.RemoveRange(housesForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    HousesGrid.ItemsSource = demoExamDBEntities.Houses.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void AddLandButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditLandPage(null));
        }

        private void EditLandButton_Click(object sender, RoutedEventArgs e)
        {
            if (LandsGrid.SelectedItems.Count > 0)
            {
                Manager.MainFrame.Navigate(new AddEditLandPage((Land)LandsGrid.SelectedItem));
            }
        }

        private void DeleteLandButton_Click(object sender, RoutedEventArgs e)
        {
            var landsForRemoving = LandsGrid.SelectedItems.Cast<Land>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {landsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.Lands.RemoveRange(landsForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    LandsGrid.ItemsSource = demoExamDBEntities.Lands.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                ApartmentsGrid.ItemsSource = demoExamDBEntities.Apartments.ToList();
                HousesGrid.ItemsSource = demoExamDBEntities.Houses.ToList();
                LandsGrid.ItemsSource = demoExamDBEntities.Lands.ToList();
            }
            else
            {
                var apartments = demoExamDBEntities.Apartments.ToList();
                var houses = demoExamDBEntities.Houses.ToList();
                var lands = demoExamDBEntities.Lands.ToList();

                var parts = (sender as TextBox).Text.Split();
                foreach (var part in parts)
                {
                    if (part != "")
                    {
                        apartments = apartments.Where(p => levenshteinHelper.IsLevenshteinDistanceLess3(p.CityAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.StreetAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess1(p.ApartmentNumberAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess1(p.HouseAddress, part)).ToList();

                        houses = houses.Where(p => levenshteinHelper.IsLevenshteinDistanceLess3(p.CityAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.StreetAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess1(p.HouseAddress, part)).ToList();

                        lands = lands.Where(p => levenshteinHelper.IsLevenshteinDistanceLess3(p.CityAddress, part) ||
                                levenshteinHelper.IsLevenshteinDistanceLess3(p.StreetAddress, part)).ToList();
                    }
                }
                ApartmentsGrid.ItemsSource = apartments;
                HousesGrid.ItemsSource = houses;
                LandsGrid.ItemsSource = lands;
            }
        }

        private void CoordinateSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var apartments = demoExamDBEntities.Apartments.ToList();
            var houses = demoExamDBEntities.Houses.ToList();
            var lands = demoExamDBEntities.Lands.ToList();

            try
            {
                if (!string.IsNullOrWhiteSpace(FromLatitudeSearchTextBox.Text) && !(FromLatitudeSearchTextBox.Text == "-"))
                {
                    apartments = apartments.Where(p => p.CoordinateLatitude > int.Parse(FromLatitudeSearchTextBox.Text)).ToList();
                    houses = houses.Where(p => p.CoordinateLatitude > int.Parse(FromLatitudeSearchTextBox.Text)).ToList();
                    lands = lands.Where(p => p.CoordinateLatitude > int.Parse(FromLatitudeSearchTextBox.Text)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(ToLatitudeSearchTextBox.Text) && !(ToLatitudeSearchTextBox.Text == "-"))
                {
                    apartments = apartments.Where(p => p.CoordinateLatitude < int.Parse(ToLatitudeSearchTextBox.Text)).ToList();
                    houses = houses.Where(p => p.CoordinateLatitude < int.Parse(ToLatitudeSearchTextBox.Text)).ToList();
                    lands = lands.Where(p => p.CoordinateLatitude < int.Parse(ToLatitudeSearchTextBox.Text)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(FromLongitudeSearchTextBox.Text) && !(FromLongitudeSearchTextBox.Text == "-"))
                {
                    apartments = apartments.Where(p => p.CoordinateLongitude > int.Parse(FromLongitudeSearchTextBox.Text)).ToList();
                    houses = houses.Where(p => p.CoordinateLongitude > int.Parse(FromLongitudeSearchTextBox.Text)).ToList();
                    lands = lands.Where(p => p.CoordinateLongitude > int.Parse(FromLongitudeSearchTextBox.Text)).ToList();
                }
                if (!string.IsNullOrWhiteSpace(ToLongitudeSearchTextBox.Text) && !(ToLongitudeSearchTextBox.Text == "-"))
                {
                    apartments = apartments.Where(p => p.CoordinateLongitude < int.Parse(ToLongitudeSearchTextBox.Text)).ToList();
                    houses = houses.Where(p => p.CoordinateLongitude < int.Parse(ToLongitudeSearchTextBox.Text)).ToList();
                    lands = lands.Where(p => p.CoordinateLongitude < int.Parse(ToLongitudeSearchTextBox.Text)).ToList();
                }
                ApartmentsGrid.ItemsSource = apartments;
                HousesGrid.ItemsSource = houses;
                LandsGrid.ItemsSource = lands;
            }
            catch
            {
                MessageBox.Show("Неверный формат координат");
            }
        }
    }
}
