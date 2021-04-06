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
    /// Логика взаимодействия для OffersPage.xaml
    /// </summary>
    public partial class OffersPage : Page
    {
        private int? clientId = null;
        private int? realtorId = null;

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public OffersPage()
        {
            InitializeComponent();
        }

        public void SetData(int? clientId, int? realtorId)
        {
            this.clientId = clientId;
            this.realtorId = realtorId;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditOfferPage(null));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var offersForRemoving = OffersGrid.SelectedItems.Cast<Offer>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {offersForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.Offers.RemoveRange(offersForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    OffersGrid.ItemsSource = demoExamDBEntities.Offers.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (OffersGrid.SelectedItems.Count > 0)
            {
                Manager.MainFrame.Navigate(new AddEditOfferPage((Offer)OffersGrid.SelectedItem));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            demoExamDBEntities.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            IQueryable<Offer> offers = demoExamDBEntities.Offers;
            if (clientId != null)
            {
                offers = offers.Where(p => p.ClientId == clientId);
            }
            if (realtorId != null)
            {
                offers = offers.Where(p => p.RealtorId == realtorId);
            }
            OffersGrid.ItemsSource = offers.ToList();
        }
    }
}
