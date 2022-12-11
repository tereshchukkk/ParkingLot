using ParkingLot.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ParkingLot.ViewModels
{
    public class VehicleExitVM : INotifyPropertyChanged
    {
        // Поле для работы с БД
        private readonly ApplicationContext _context = new();
        // Поле, содержащее список транспорта
        private ObservableCollection<Vehicle> _vehicles = new();

        // Свойство, содержащее в себе список транспорта для привязки к элементам UI и получения доступа к полю данного класса
        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set
            {
                _vehicles = value;

                // Уведомление элементов UI об изменении списка 
                NotifyPropertyChanged("Vehicles");
            }
        }

        // Свойство, необходимое для уведомления методов о типе сортировки (убывающая/возрастающая)
        public bool IsSortingDescending { get; set; }

        // Заполняет список транспорта при создании объекта класса
        public VehicleExitVM()
        {
            Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.ToList());
        }

        /// <summary>
        /// Удаляет транспорт из базы данных и создаёт запись в журнале БД
        /// </summary>
        /// <param name="vehicle"> ТС, которое должно быть удалено </param>
        public async void DeleteVehicle(Vehicle vehicle)
        {
            try
            {
                var dbVehicle = _context.Vehicles.FirstOrDefault(v => v == vehicle);

                if (dbVehicle is null)
                {
                    throw new ArgumentException("No such car was found in database. (DeleteVehicle method)");
                }

                _context.Vehicles.Remove(dbVehicle);
                Vehicles.Remove(vehicle);

                var history = new History()
                {
                    DriverFullName = vehicle.DriverFullName,
                    DepartureTime = DateTime.Now,
                    PlateNumber = vehicle.PlateNumber,
                    RentPrice = GetRentPrice(vehicle)
                };

                _context.History.Add(history);

                await _context.SaveChangesAsync();

                MessageBox.Show("ТС было успешно выписано из списка", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\nDelete method", 
                                "Ошибка", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }
        }
        
        /// <summary>
        /// Обновление списка ТС
        /// </summary>
        public void UpdateVehiclesList()
        {
            Vehicles = new ObservableCollection<Vehicle>(_context.Vehicles.ToList());
        }

        /// <summary>
        /// Рассчитывает стоимость аренды парковочного места для ТС
        /// </summary>
        /// <param name="vehicle"> ТС для которого проводится расчёт</param>
        /// <returns> Стоимость аренды </returns>
        public decimal GetRentPrice(Vehicle vehicle)
        {
            if (vehicle is null)
            {
                throw new ArgumentNullException(nameof(vehicle), "Vehicle variable provided for GetRentPrice() method was null.");
            }

            decimal rentPrice = -1;

            try
            {
                double hoursSpent = (DateTime.Now - vehicle.ParkingTime).TotalHours;
                rentPrice = vehicle.Tariff * (int)hoursSpent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, 
                                "Ошибка", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
            }

            return rentPrice;
        }

        #region Sortings region
        /*
         * В данном фрагменте реализована сортировка списка транспорта по разным параметрам.
         * 
         * В каждом методе производится сортировка, основываясь на значении Свойства IsAscending, которое содержит информацию о том, является
         * ли сортировка возрастающей, либо убывающей.
         * 
         * После осуществлеия сортировки все элементы пользовательского интерфейса, которые привязаны к свойству Vehicles, уведомляются об
         * изменении содержения данного списка и отображают уже отфильтрованный список.
         */

        // Сортировка по ФИО водителя
        public void SortByFullName()
        {
            // Проверка на то, является ли сортировка убывающей
            if (IsSortingDescending)
            {
                // Сортировка списка по убыванию
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderByDescending(v => v.DriverFullName));
            }
            else
            {
                // Сортировка списка по возрастанию
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderBy(v => v.DriverFullName));
            }
        }
        
        // Сортировка по типу транспорта (по алфавиту)
        public void SortByVehicleType()
        {
            if (IsSortingDescending)
            {
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderByDescending(v => v.Type.ToString()));
            }
            else
            {
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderBy(v => v.Type.ToString()));
            }
        }
        
        // Сортировка по регистрационным номерам транспорта
        public void SortByPlateNumber()
        {
            if (IsSortingDescending)
            {
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderByDescending(v => v.PlateNumber));
            }
            else
            {
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderBy(v => v.PlateNumber));
            }
        }

        // Сортировка по дате стоянки
        public void SortByArrivalDate()
        {
            if (IsSortingDescending)
            {
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderByDescending(v => v.ParkingTime));
            }
            else
            {
                Vehicles = new ObservableCollection<Vehicle>(Vehicles.OrderBy(v => v.ParkingTime));
            }
        }
        #endregion

        /// <summary>
        /// Событие и метод для уведомления элементов UI об изменении значения списка всех ТС
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                    new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
