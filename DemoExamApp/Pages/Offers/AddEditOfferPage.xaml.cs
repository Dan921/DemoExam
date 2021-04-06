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

namespace DemoExamApp.Pages.Offers
{
    /// <summary>
    /// Логика взаимодействия для AddEditOfferPage.xaml
    /// </summary>
    public partial class AddEditOfferPage : Page
    {
        private Offer _currentOffer = new Offer();

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public AddEditOfferPage(Offer selectedOffer)
        {
            if (selectedOffer != null)
                _currentOffer = demoExamDBEntities.Offers.Find(selectedOffer.Id);

            DataContext = _currentOffer;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (_currentOffer.Client == null)
                errors.AppendLine("Выберете клиента");
            if (_currentOffer.Realtor == null)
                errors.AppendLine("Выберете риэлтора");
            if (_currentOffer.RealEstate == null)
                errors.AppendLine("Выберете недвижимость");
            if (_currentOffer.Price == null)
                errors.AppendLine("Укажите цену");
            if (_currentOffer.Price < 0)
                errors.AppendLine("Цена должна быть положительным целым числом");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentOffer.Id == 0)
                demoExamDBEntities.Offers.Add(_currentOffer);
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
            RealEstatesComboBox.ItemsSource = demoExamDBEntities.RealEstates.ToList();
        }
    }
}
