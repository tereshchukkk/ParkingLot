using System.Windows.Controls;

namespace ParkingLot.Views
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();

            LoadPages();
        }

        // Загрузка страниц приложения
        private void LoadPages()
        {
            MainMenuFrame.Navigate(new Statistics());
            EnterVehicleFrame.Navigate(new VehicleEnter());
            ExitVehicleFrame.Navigate(new VehicleExit());
            JournalFrame.Navigate(new JournalPage());
        }
    }
}
