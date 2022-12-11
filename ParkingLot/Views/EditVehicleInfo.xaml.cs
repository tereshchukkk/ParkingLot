using ParkingLot.Enums;
using ParkingLot.Models;
using System;
using System.Linq;
using System.Windows;

namespace ParkingLot.Views
{
    /// <summary>
    /// Interaction logic for EditVehicleInfo.xaml
    /// </summary>
    public partial class EditVehicleInfo : Window
    {
        // Поле для взаимодействия с базой данных
        private readonly ApplicationContext _context = new();
        // ID выбранного ТС для корректного обновления данных в БД
        public int VehicleId { get; set; }
        // Свойство, взаимодействующее с текстовым полем, содержащим ФИО водителя
        public string DriverName { get => DriverNameTextBlock.Text; set => DriverNameTextBlock.Text = value; }
        // Свойство, взаимодействующее с текстовым полем, содержащим регистрационный номер
        public string PlateNumber { get => PlateNumberTextBlock.Text; set => PlateNumberTextBlock.Text = value; }
        // Свойство, возвращающее тип ТС на основе выбора в диалоговом окне
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
        // ТС после внесения изменений, которое возвращается в основную форму, после закрытия диалогового окна
        public Vehicle OutVehicle
        {
            get
            {
                return new Vehicle()
                {
                    Id = VehicleId,
                    DriverFullName = DriverName,
                    PlateNumber = PlateNumber,
                    Type = Type
                };
            }
        }

        public EditVehicleInfo(string driverName, VehicleType type, string plateNumber)
        {
            InitializeComponent();

            UploadDataInListBlocks(driverName, type, plateNumber);
        }

        /// <summary>
        /// Загружает данные о выбранном ТС в текстовые поля на странице
        /// </summary>
        /// <param name="driverName"> ФИО водителя </param>
        /// <param name="type"> Тип ТС </param>
        /// <param name="plateNumber"> Регистрационные номера </param>
        private void UploadDataInListBlocks(string driverName, VehicleType type, string plateNumber)
        {
            var incomeVehicle = _context.Vehicles.FirstOrDefault(v => v.DriverFullName == driverName 
                                                                   && v.PlateNumber == plateNumber 
                                                                   && v.Type == type)!;

            DriverNameTextBlock.Text = incomeVehicle.DriverFullName;
            PlateNumberTextBlock.Text = incomeVehicle.PlateNumber;
            VehicleTypeComboBox.SelectedIndex = (int)incomeVehicle.Type;
            VehicleId = incomeVehicle.Id;
        }

        /// <summary>
        /// Проверка введённых значений и возвращение из диалогового окна
        /// </summary>
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
