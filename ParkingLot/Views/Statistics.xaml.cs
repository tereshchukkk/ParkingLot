using ParkingLot.ViewModels;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParkingLot.Views
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Page
    {
        // ViewModel для вынесения логики в отдельный класс
        private static readonly StatisticsVM _vm = new();

        public Statistics()
        {
            InitializeComponent();

            DataContext = _vm;
        }

        // Производит обновление отображаемых на экране пользователя данных при каждой загрузке страницы
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Task.Run(() => _vm.UpdateValues());
        }
    }
}
