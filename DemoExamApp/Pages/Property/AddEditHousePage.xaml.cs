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
    /// Логика взаимодействия для AddEditHousePage.xaml
    /// </summary>
    public partial class AddEditHousePage : Page
    {
        private RealEstate _currentRealEstate = new RealEstate();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditHousePage(RealEstate selectedRealEstate)
        {
            if (selectedRealEstate != null)
                _currentRealEstate = demoExamDBEntities.RealEstates.Find(selectedRealEstate.Id);

            DataContext = _currentRealEstate;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentRealEstate.CoordinateLatitude < -90 || _currentRealEstate.CoordinateLatitude > 90)
                errors.AppendLine("Широта может быть от -90 до 90");
            if (_currentRealEstate.CoordinateLongitude < -180 || _currentRealEstate.CoordinateLongitude > 180)
                errors.AppendLine("Долгота может быть от -180 до 180");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentRealEstate.Id == 0)
            {
                _currentRealEstate.Type = "House";
                demoExamDBEntities.RealEstates.Add(_currentRealEstate);
            }
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
