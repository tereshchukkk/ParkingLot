using ParkingLot.Models;
using System.ComponentModel;
using System.Linq;

namespace ParkingLot.ViewModels
{
    public class StatisticsVM : INotifyPropertyChanged
    {
        // Поле, необходимое для работы с БД
        private readonly ApplicationContext _context = new();
        // Поле, содержащее количество оставшихся мест на парковке
        private int _placesLeft;
        // Поле, содержащее количество занятых мест на парковке
        private int _placesTaken;

        // Свойство, обеспечивающее доступ к чтению и записи поля _placesLeft
        public int PlacesLeft
        {
            get => _placesLeft;
            set
            {
                _placesLeft = value;
                NotifyPropertyChanged("PlacesLeft");
            }
        }

        // Свойство, обеспечивающее доступ к чтению и записи поля _placesTaken
        public int PlacesTaken
        {
            get => _placesTaken;
            set
            {
                _placesTaken = value;
                NotifyPropertyChanged("PlacesTaken");

                PlacesLeft = 50 - _placesTaken;
            }
        }

        // При объявлении объекта данного класса проводятся расчёты количества оставшихся и занятых мест на парковке
        public StatisticsVM()
        {
            UpdateValues();
        }

        /// <summary>
        /// Обновление данных
        /// </summary>
        public void UpdateValues()
        {
            PlacesTaken = _context.Vehicles.ToList().Count;
        }

        /// <summary>
        /// Событие и метод для уведомления элементов UI об изменении количества занятых и свободных стояночных мест
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
