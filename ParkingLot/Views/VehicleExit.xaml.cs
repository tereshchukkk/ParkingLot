using ParkingLot.Enums;
using ParkingLot.Models;
using ParkingLot.ViewModels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParkingLot.Views
{
    /// <summary>
    /// Interaction logic for VehicleExit.xaml
    /// </summary>
    public partial class VehicleExit : Page
    {
        // ViewModel для вынесения логики в отдельный класс
        private readonly VehicleExitVM _vm = new();

        public VehicleExit()
        {
            InitializeComponent();

            VehiclesList.DataContext = _vm;
        }

        // Позволяет прокручивать список, отображённый на экране, с помощью колёсика мыши
        private void VehiclesScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        // Отображает выбранное ТС в правой части экрана
        private void VehiclesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VehiclesList.SelectedIndex == -1)
            {
                return;
            }

            var vehicle = _vm.Vehicles[VehiclesList.SelectedIndex];

            string vehicleType = vehicle.Type switch
            {
                VehicleType.Car => "Автомобиль",
                VehicleType.Bus => "Автобус",
                VehicleType.Truck => "Грузовик",
                _ => string.Empty
            };

            DriverNameBox.Text = vehicle.DriverFullName;
            PlateNumberBox.Text = vehicle.PlateNumber;
            VehicleTypeBox.Text = vehicleType;
            ArrivalBox.Text = vehicle.ParkingTime.ToString();
            RentPriceBox.Text = _vm.GetRentPrice(vehicle).ToString() + " р.";
        }

        // Фильтрация списка ТС
        private void FilterCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e) => SortVehicles();

        // Фильтрация по убыванию/возрастанию
        private void FilterFlipper_IsFlippedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e) => SortVehicles();

        // Удаление ТС из списка
        private void ExitVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            var veh = VehiclesList.SelectedValue;

            _vm.DeleteVehicle((Vehicle)veh);

            DriverNameBox.Text = string.Empty;
            PlateNumberBox.Text = string.Empty;
            VehicleTypeBox.Text = string.Empty;
            ArrivalBox.Text = string.Empty;
            RentPriceBox.Text = string.Empty;
        }

        // Обращение к ViewModel для осуществления сортировки
        private void SortVehicles()
        {
            if (FilterCombobox.SelectedIndex == -1)
            {
                return;
            }

            _vm.IsSortingDescending = FilterFlipper.IsFlipped;

            switch (FilterCombobox.SelectedIndex)
            {
                case 0:
                    _vm.SortByFullName();
                    break;
                case 1:
                    _vm.SortByVehicleType();
                    break;
                case 2:
                    _vm.SortByPlateNumber();
                    break;
                case 3:
                    _vm.SortByArrivalDate();
                    break;
            }
        }

        // Обновление отображаемых данных при каждой загрузке страницы и сброс фильтрации
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => _vm.UpdateVehiclesList());

            FilterCombobox.SelectedIndex = -1;
        }
    }
}
