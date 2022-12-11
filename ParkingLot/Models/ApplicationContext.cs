using Microsoft.EntityFrameworkCore;

namespace ParkingLot.Models
{
    // Database context using EntityFramework ORM
    public class ApplicationContext : DbContext
    {
        // Таблица с журналом выездов с парковки
        public DbSet<History> History { get; set; } = null!;
        // Таблица со всеми ТС нахожящихся на парковке
        public DbSet<Vehicle> Vehicles { get; set; } = null!;

        // Создание БД и подключение к ней
        public ApplicationContext() => Database.EnsureCreated();

        // Конфигурационные настройки БД (сервер, название)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=ParkingLot;Trusted_Connection=True;TrustServerCertificate=true;");
        }

        // В этом методе в таблице Vehicles столбец PlateNumber(регистрационный номер) был сделан уникальным (неповторяющимся)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasIndex(s => s.PlateNumber).IsUnique();
        }
    }
}
