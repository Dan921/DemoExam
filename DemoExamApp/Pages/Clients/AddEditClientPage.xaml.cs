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
    /// Логика взаимодействия для AddEditClientPage.xaml
    /// </summary>
    public partial class AddEditClientPage : Page
    {
        private Client _currentClient = new Client();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditClientPage(Client selectedClient)
        {
            if (selectedClient != null)
                _currentClient = demoExamDBEntities.Clients.Find(selectedClient.Id);

            DataContext = _currentClient;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentClient.PhoneNumber) && string.IsNullOrWhiteSpace(_currentClient.Email))
                errors.AppendLine("Необходим номер телефона или адрес алектроннной почты");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentClient.Id == 0)
                demoExamDBEntities.Clients.Add(_currentClient);
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
