using Microsoft.EntityFrameworkCore;
using ParkingLot.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingLot.Models
{
    [Table("Vehicles")]
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        // Полное имя водителя
        public string DriverFullName { get; set; }

        // Регистрационные номера машины
        public string PlateNumber { get; set; }

        // Тип ТС
        public VehicleType Type { get; set; }

        // Дата и время парковки
        public DateTime ParkingTime { get; set; }

        // Тариф для расчёта стоимости аренды парковочного места (зависит от типа ТС) в формате руб./час
        public int Tariff { get; set; }
    }
}
