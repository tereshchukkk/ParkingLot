using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingLot.Models
{
    [Table("History")]
    public class History
    {
        [Key]
        public int Id { get; set; }

        // ФИО водителя
        public string DriverFullName { get; set; }

        // Регистрационные номера
        public string PlateNumber { get; set; }

        // Стоимость аренды
        public decimal RentPrice { get; set; }

        // Дата и время отъезда
        public DateTime DepartureTime { get; set; }
    }
}
