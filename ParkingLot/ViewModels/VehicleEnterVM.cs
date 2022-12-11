using ParkingLot.Enums;
using ParkingLot.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ParkingLot.ViewModels
{
    public class VehicleEnterVM : INotifyPropertyChanged
    {
        // Поле для работы с БД
        private readonly ApplicationContext _context = new();
        // Поле, содержащее список всех ТС
        private ObservableCollection<Vehicle> _vehicles = new();

        // Свойство для получения и установки значения списка всех ТС
        public ObservableCollection<Vehicle> Vehicles 
        { 
            get => _vehicles; 
            set
            {
                _vehicles = value;
                NotifyPropertyChanged("Vehicles");
            }
        }

        // Конструктор класса
        public VehicleEnterVM()
        {
            // Список ТС заполняется по умолчанию при объявлении объекта данного класса в контексте данных визуального отображения
            Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.ToList());
        }

        /// <summary>
        /// Добавление транспортного средства в БД
        /// </summary>
        /// <param name="driverName"> ФИО водителя </param>
        /// <param name="plateNumber"> Регистрационные номера </param>
        /// <param name="type"> Тип ТС </param>
        public void AddVehicle(string driverName, string plateNumber, VehicleType type)
        {
            // Проверка на оставшиеся места на парковке
            if (_context.Vehicles.ToList().Count == 50)
            {
                MessageBox.Show("Не хватает места на парковке",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                return;
            }

            // Исключение, срабатывающее в случае передачи пустых значений в качестве параметров метода
            if (string.IsNullOrEmpty(driverName) || string.IsNullOrEmpty(plateNumber))
            {
                throw new ArgumentNullException(nameof(driverName), "One of a few fields were null while accessing AddVehicle Method.");
            }

            try
            {
                // Определение тарифа для расчёта конечной стоимости аренды парковочного места, исходя из типа ТС
                // Автомобиль - 5д.е./час, Автобус - 8д.е./час, Грузовик - 10д.е./час
                int tariff = type switch
                {
                    VehicleType.Car => 5,
                    VehicleType.Bus => 8,
                    VehicleType.Truck => 10,
                    _ => throw new ArgumentException("Vehicle type was in incorrect format while accessing AddVehicle method.", nameof(type))
                };

                // Создание нового экземпляра класса Vehicle
                var vehicle = new Vehicle
                {
                    DriverFullName = driverName,
                    PlateNumber = plateNumber,
                    Type = type,
                    Tariff = tariff,
                    ParkingTime = DateTime.Now
                };

                // Добавление нового ТС в список, к которому привязаны UI элементы
                Vehicles.Add(vehicle);

                // Добавление нового ТС в базу данных с последующим сохранением
                _context.Add(vehicle);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // В случае срабатывания любого из исключений, на экран будет выведено соответствующее окно
                // с текстом ошибки и работа приложения не будет прекращена
                MessageBox.Show(ex.Message, 
                                "Ошибка", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Производит поиск по списку
        /// </summary>
        /// <param name="driverName"> ФИО водителя </param>
        /// <param name="plateNumber"> Регистрационные номера </param>
        /// <param name="typeIndex"> Индекс типа ТС </param>
        public void SearchVehicles(string? driverName, string? plateNumber, int typeIndex)
        {
            // Очистка текущего списка всех ТС
            Vehicles.Clear();

            // Обработка случая, где все параметры указаны
            if (!string.IsNullOrEmpty(driverName) && !string.IsNullOrEmpty(plateNumber) && typeIndex >= 0 && typeIndex <= 2)
            {
                // Производится поиск по списку автомобилей с целью найти схожие(*не обязательно идентичные) ТС
                // по переданным параметрам с использованием LINQ запросов
                Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.Where(v => v.DriverFullName.Contains(driverName)
                                                                                       && v.PlateNumber.Contains(plateNumber)
                                                                                       && v.Type == (VehicleType)typeIndex).ToList());

                // После передачи в список ТС результатов, производится досрочный выход из метода, с целью сократить
                // время работы метода
                return;
            }

            // Обработка случая, где не указано ФИО водителя
            if (string.IsNullOrEmpty(driverName) && !string.IsNullOrEmpty(plateNumber) && typeIndex >= 0 && typeIndex <= 2)
            {
                Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.Where(v => v.PlateNumber.Contains(plateNumber)
                                                                                       && v.Type == (VehicleType)typeIndex).ToList());

                return;
            }

            // Обработка случая, где не указан регистрационный номер
            if (!string.IsNullOrEmpty(driverName) && string.IsNullOrEmpty(plateNumber) && typeIndex >= 0 && typeIndex <= 2)
            {
                Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.Where(v => v.DriverFullName.Contains(driverName)
                                                                                       && v.Type == (VehicleType)typeIndex).ToList());

                return;
            }

            // Обработка случая, где не указан тип ТС
            if (!string.IsNullOrEmpty(driverName) && !string.IsNullOrEmpty(plateNumber) && typeIndex == -1)
            {
                Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.Where(v => v.DriverFullName.Contains(driverName)
                                                                                       && v.PlateNumber.Contains(plateNumber)).ToList());

                return;
            }

            // Обработка случая, где указано лишь ФИО водителя
            if (!string.IsNullOrEmpty(driverName) && string.IsNullOrEmpty(plateNumber) && typeIndex == -1)
            {
                Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.Where(v => v.DriverFullName.Contains(driverName)).ToList());

                return;
            }

            // Обработка случая, где указан лишь регистрационный номер
            if (string.IsNullOrEmpty(driverName) && !string.IsNullOrEmpty(plateNumber) && typeIndex == -1)
            {
                Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.Where(v => v.PlateNumber.Contains(plateNumber)).ToList());

                return;
            }

            // Обработка случая, где указан лишь тип ТС
            if (string.IsNullOrEmpty(driverName) && string.IsNullOrEmpty(plateNumber) && typeIndex >= 0 && typeIndex <= 2)
            {
                Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.Where(v => v.Type == (VehicleType)typeIndex).ToList());

                return;
            }
        }

        /// <summary>
        /// Сбрасывает настройки поиска
        /// </summary>
        public void ResetSearch()
        {
            // Устанавливает в список ТС все записи в БД
            Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.ToList());
        }

        /// <summary>
        /// Редактирует данные автомобиля в БД
        /// </summary>
        /// <param name="vehicleId"> Id редактируемого автомобиля в БД </param>
        /// <param name="vehicle"> Изменённые значения </param>
        public void UpdateVehicleInfo(int vehicleId, Vehicle vehicle)
        {
            if (vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle parameter was null while accessing Update vehicle method.");
            }

            try
            {
                // Получение записи из БД
                var dbVehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicleId);

                // Проверка на наличие записи в БД
                if (dbVehicle is null)
                {
                    throw new ArgumentOutOfRangeException(nameof(vehicleId), "No such vehicle with provided Id was found while accessing update vehicle method.");
                }

                // Запись, полученная с использованием EntityFramework, отслеживается, и все изменения внесённые в неё
                // будут перенесены в БД автоматически при сохранении изменений

                // Изменение записи
                dbVehicle.DriverFullName = vehicle.DriverFullName;
                dbVehicle.PlateNumber = vehicle.PlateNumber;
                dbVehicle.Type = vehicle.Type;
                dbVehicle.Tariff = vehicle.Type switch
                {
                    VehicleType.Car => 5,
                    VehicleType.Truck => 10,
                    VehicleType.Bus => 8,
                    _ => -1
                };
                 
                // Сохранение изменений
                _context.SaveChanges();

                // Обновление списка ТС
                ResetSearch();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие и метод для уведомления элементов UI об изменении значения списка всех ТС
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
