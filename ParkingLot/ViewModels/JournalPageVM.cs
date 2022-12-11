using ParkingLot.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ParkingLot.ViewModels
{
    public class JournalPageVM : INotifyPropertyChanged
    {
        // Поле, необходимое для обращения к БД
        private readonly ApplicationContext _context = new();
        // Поле, содержащее список записей в таблице БД
        private ObservableCollection<History> _notes = new();

        // Свойство, обеспечивающее доступ к одноимённому полю и уведомлящее элементы UI об его изменении
        public ObservableCollection<History> Notes
        {
            get => _notes;
            set
            {
                _notes = value;

                // Уведомление UI элементов об изменении списка
                NotifyPropertyChanged("Notes");
            }
        }

        /// <summary>
        /// Обновление списка журнала
        /// </summary>
        public void UpdateHistory() => Notes = new ObservableCollection<History>(_context.History.ToList());

        /// <summary>
        /// Событие и метод для уведомления элементов UI об изменениях в журнале выбывших ТС
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
