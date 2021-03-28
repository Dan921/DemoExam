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

namespace DemoExamApp.Pages.Property
{
    /// <summary>
    /// Логика взаимодействия для AddEditApartmentPage.xaml
    /// </summary>
    public partial class AddEditApartmentPage : Page
    {
        private Apartment _currentApartment = new Apartment();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditApartmentPage(Apartment selectedApartment)
        {
            if (selectedApartment != null)
                _currentApartment = demoExamDBEntities.Apartments.Find(selectedApartment.Id);

            DataContext = _currentApartment;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentApartment.CoordinateLatitude < -90 || _currentApartment.CoordinateLatitude > 90)
                errors.AppendLine("Широта может быть от -90 до 90");
            if (_currentApartment.CoordinateLongitude < -180 || _currentApartment.CoordinateLongitude > 180)
                errors.AppendLine("Долгота может быть от -180 до 180");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentApartment.Id == 0)
                demoExamDBEntities.Apartments.Add(_currentApartment);
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
    }
}
