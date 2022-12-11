using ParkingLot.Enums;
using System;
using System.Windows;

namespace ParkingLot.Views
{
    /// <summary>
    /// Interaction logic for VehicleAdd.xaml
    /// </summary>
    public partial class VehicleAdd : Window
    {
        // Свойтва, возвращающее введённые пользователем регистрационные номера, ФИО водителя, тип ТС соответственно
        public string PlateNumber { get => PlateNumberTextBlock.Text.ToUpper(); }
        public string DriverName { get => DriverNameTextBlock.Text; }
        public VehicleType Type 
        { 
            get
            {
                return VehicleTypeComboBox.SelectedIndex switch
                {
                    0 => VehicleType.Car,
                    1 => VehicleType.Truck,
                    2 => VehicleType.Bus,
                    _ => throw new ArgumentException()
                };
            }
        }

        public VehicleAdd()
        {
            InitializeComponent();
        }

        // Добавление нового ТС в БД
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(VehicleTypeComboBox.Text))
            {
                MessageBox.Show("Необходимо выбрать тип ТС", 
                                "Ошибка ввода",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            if (string.IsNullOrEmpty(PlateNumber) || PlateNumber.Length < 8 || !PlateNumber.Contains('-'))
            {
                MessageBox.Show("Необходимо указать регистрационный номер",
                                "Ошибка ввода",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            if (string.IsNullOrEmpty(DriverName))
            {
                MessageBox.Show("Необходимо указать ФИО водителя",
                                "Ошибка ввода",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            DialogResult = true;
        }
    }
}
