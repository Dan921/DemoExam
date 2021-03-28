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

namespace DemoExamApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditRealtorPage.xaml
    /// </summary>
    public partial class AddEditRealtorPage : Page
    {
        private Realtor _currentRealtor = new Realtor();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditRealtorPage(Realtor selectedRealtor)
        {
            if (selectedRealtor != null)
                _currentRealtor = demoExamDBEntities.Realtors.Find(selectedRealtor.Id);

            DataContext = _currentRealtor;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentRealtor.FirstName))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrWhiteSpace(_currentRealtor.LastName))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrWhiteSpace(_currentRealtor.MiddleName))
                errors.AppendLine("Укажите отчество");
            if (_currentRealtor.ShareOfTheCommission > 100 || _currentRealtor.ShareOfTheCommission < 0)
                errors.AppendLine("Доля от комиссии должна быть числом от 0 до 100");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentRealtor.Id == 0)
                demoExamDBEntities.Realtors.Add(_currentRealtor);
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
