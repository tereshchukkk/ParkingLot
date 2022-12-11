using ParkingLot.ViewModels;
using System.Windows.Controls;

namespace ParkingLot.Views
{
    /// <summary>
    /// Interaction logic for JournalPage.xaml
    /// </summary>
    public partial class JournalPage : Page
    {
        // ViewModel для вынесения логики в отдельный класс
        private readonly JournalPageVM _vm = new();

        public JournalPage()
        {
            InitializeComponent();

            DataContext = _vm;
        }

        // Производит обновление элементов, отображающихся на экране пользователя при каждой загрузке данной страницы
        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _vm.UpdateHistory();
        }
    }
}
