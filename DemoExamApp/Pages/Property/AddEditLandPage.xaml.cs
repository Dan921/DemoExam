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
    /// Логика взаимодействия для AddEditLandPage.xaml
    /// </summary>
    public partial class AddEditLandPage : Page
    {
        private Land _currentLand = new Land();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditLandPage(Land selectedLand)
        {
            if (selectedLand != null)
                _currentLand = demoExamDBEntities.Lands.Find(selectedLand.Id);

            DataContext = _currentLand;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentLand.CoordinateLatitude < -90 || _currentLand.CoordinateLatitude > 90)
                errors.AppendLine("Широта может быть от -90 до 90");
            if (_currentLand.CoordinateLongitude < -180 || _currentLand.CoordinateLongitude > 180)
                errors.AppendLine("Долгота может быть от -180 до 180");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentLand.Id == 0)
                demoExamDBEntities.Lands.Add(_currentLand);
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
