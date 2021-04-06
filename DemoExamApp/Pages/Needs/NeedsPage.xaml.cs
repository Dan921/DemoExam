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
    /// Логика взаимодействия для NeedsPage.xaml
    /// </summary>
    public partial class NeedsPage : Page
    {
        private int? clientId = null;
        private int? realtorId = null;

        private DemoExamDBEntities demoExamDBEntities = new DemoExamDBEntities();

        public NeedsPage()
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
            Manager.MainFrame.Navigate(new AddEditNeedPage(null));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var needsForRemoving = NeedsGrid.SelectedItems.Cast<Need>().ToList();

            if (MessageBox.Show($"Вы действительно хотите удалить {needsForRemoving.Count()} элементов?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    demoExamDBEntities.Needs.RemoveRange(needsForRemoving);
                    demoExamDBEntities.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    NeedsGrid.ItemsSource = demoExamDBEntities.Needs.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (NeedsGrid.SelectedItems.Count > 0)
            {
                Manager.MainFrame.Navigate(new AddEditNeedPage((Need)NeedsGrid.SelectedItem));
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            demoExamDBEntities.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            IQueryable<Need> needs = demoExamDBEntities.Needs;
            if (clientId != null)
            {
                needs = needs.Where(p => p.ClientId == clientId);
            }
            if (realtorId != null)
            {
                needs = needs.Where(p => p.RealtorId == realtorId);
            }
            NeedsGrid.ItemsSource = needs.ToList();
        }
    }
}
