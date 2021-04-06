using DemoExamApp.DataBase;
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

namespace DemoExamApp.Pages.Needs
{
    /// <summary>
    /// Логика взаимодействия для AddEditNeedPage.xaml
    /// </summary>
    public partial class AddEditNeedPage : Page
    {
        private Need _currentNeed = new Need();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditNeedPage(Need selectedNeed)
        {
            if (selectedNeed != null)
                _currentNeed = demoExamDBEntities.Needs.Find(selectedNeed.Id);

            DataContext = _currentNeed;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentNeed.Client == null)
                errors.AppendLine("Выберете клиента");
            if (_currentNeed.Realtor == null)
                errors.AppendLine("Выберете риэлтора");
            if (_currentNeed.TypeOfRealEstate == null)
                errors.AppendLine("Выберете тип недвижимости");
            if (_currentNeed.MinPrice < 0)
                errors.AppendLine("Цена должна быть положительным целым числом");
            if (_currentNeed.MaxPrice < 0)
                errors.AppendLine("Цена должна быть положительным целым числом");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentNeed.TypeOfRealEstate == "Apartment")
            {
                _currentNeed.MinTotalFloors = null;
                _currentNeed.MaxTotalFloors = null;
            }
            if (_currentNeed.TypeOfRealEstate == "House")
            {
                _currentNeed.MinFloor = null;
                _currentNeed.MaxFloor = null;
            }
            if (_currentNeed.TypeOfRealEstate == "Land")
            {
                _currentNeed.MinTotalFloors = null;
                _currentNeed.MaxTotalFloors = null;
                _currentNeed.MinFloor = null;
                _currentNeed.MaxFloor = null;
                _currentNeed.MinRooms = null;
                _currentNeed.MaxRooms = null;
            }

            if (_currentNeed.Id == 0)
                demoExamDBEntities.Needs.Add(_currentNeed);
            try
            {
                demoExamDBEntities.SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClientsComboBox.ItemsSource = demoExamDBEntities.Clients.ToList();
            RealorsComboBox.ItemsSource = demoExamDBEntities.Realtors.ToList();
            RealEstatesComboBox.ItemsSource = new List<string>(){ "Apartment", "House", "Land" };
        }

        private void RealEstatesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(((ComboBox)sender).SelectedItem.ToString() == "Apartment")
            {
                MinRoomsTextBox.IsEnabled = true;
                MaxRoomsTextBox.IsEnabled = true;
                MinFloorTextBox.IsEnabled = true;
                MaxFloorTextBox.IsEnabled = true;
                MinTotalFloorsTextBox.IsEnabled = false;
                MaxTotalFloorsTextBox.IsEnabled = false;
            }
            if (((ComboBox)sender).SelectedItem.ToString() == "House")
            {
                MinRoomsTextBox.IsEnabled = true;
                MaxRoomsTextBox.IsEnabled = true;
                MinTotalFloorsTextBox.IsEnabled = true;
                MaxTotalFloorsTextBox.IsEnabled = true;
                MinFloorTextBox.IsEnabled = false;
                MaxFloorTextBox.IsEnabled = false;
            }
            if (((ComboBox)sender).SelectedItem.ToString() == "Land")
            {
                MinRoomsTextBox.IsEnabled = false;
                MaxRoomsTextBox.IsEnabled = false;
                MinFloorTextBox.IsEnabled = false;
                MaxFloorTextBox.IsEnabled = false;
                MinTotalFloorsTextBox.IsEnabled = false;
                MaxTotalFloorsTextBox.IsEnabled = false;
            }
        }
    }
}
