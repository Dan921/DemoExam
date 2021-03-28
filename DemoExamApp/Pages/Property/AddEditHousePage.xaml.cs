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
        private House _currentHouse = new House();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditHousePage(House selectedHouse)
        {
            if (selectedHouse != null)
                _currentHouse = demoExamDBEntities.Houses.Find(selectedHouse.Id);

            DataContext = _currentHouse;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentHouse.CoordinateLatitude < -90 || _currentHouse.CoordinateLatitude > 90)
                errors.AppendLine("Широта может быть от -90 до 90");
            if (_currentHouse.CoordinateLongitude < -180 || _currentHouse.CoordinateLongitude > 180)
                errors.AppendLine("Долгота может быть от -180 до 180");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentHouse.Id == 0)
                demoExamDBEntities.Houses.Add(_currentHouse);
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
