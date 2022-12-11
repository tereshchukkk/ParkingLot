using ParkingLot.Models;
using ParkingLot.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ParkingLot.Views
{
    /// <summary>
    /// Interaction logic for VehicleEnter.xaml
    /// </summary>
    public partial class VehicleEnter : Page
    {
        // ViewModel для вынесения логики в отдельный класс
        private readonly VehicleEnterVM _vm = new();

        public VehicleEnter()
        {
            InitializeComponent();

            VehiclesList.DataContext = _vm;
        }

        // Добавление нового ТС
        private void AddVehicleButton_Click(object sender, RoutedEventArgs e)
        {
            VehicleAdd vehicleAdd = new();

            if (vehicleAdd.ShowDialog() == true)
            {
                _vm.AddVehicle(vehicleAdd.DriverName, vehicleAdd.PlateNumber, vehicleAdd.Type);
            }
        }

        // Изменение параметров добавленных ТС
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // TODO В UI необходимо сначала выбрать поле, затем нажимать на кнопку (что неправильно)
                var vehicle = (Vehicle)VehiclesList.Items.GetItemAt(VehiclesList.SelectedIndex);

                EditVehicleInfo update = new(vehicle!.DriverFullName,
                                             vehicle!.Type,
                                             vehicle!.PlateNumber);

                // Данный участок кода срабатывает, при срабатывании в модульном окне команды DialogResult = true;
                if (update.ShowDialog() == true)
                {
                    _vm.UpdateVehicleInfo(update.OutVehicle.Id, update.OutVehicle);
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Вам необходимо выбрать ТС\n\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Фильтрация ТС
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DriverNameBox.Text) && string.IsNullOrEmpty(PlateNumberBox.Text) && VehicleTypeComboBox.SelectedIndex == -1)
            {
                return;
            }

            _vm.SearchVehicles(DriverNameBox.Text, PlateNumberBox.Text, VehicleTypeComboBox.SelectedIndex);
        }

        // Сбрасывает условия фильтрации и возвращает список ТС к первоначальному виду
        private void ResetSearchButton_Click(object sender, RoutedEventArgs e)
        {
            DriverNameBox.Text = string.Empty;
            PlateNumberBox.Text = string.Empty;
            VehicleTypeComboBox.SelectedIndex = -1;

            _vm.ResetSearch();
        }

        // Позволяет прокручивать список ТС колёсиком мыши
        private void VehiclesScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        // Обновляет список транспорта при каждой загрузке данной страницы
        private void Page_Loaded(object sender, RoutedEventArgs e) => Task.Run(() => _vm.ResetSearch());
    }
}
