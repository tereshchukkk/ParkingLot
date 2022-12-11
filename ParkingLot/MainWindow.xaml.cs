using System.Windows;
using System.Windows.Input;

namespace ParkingLot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Loads main menu page
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Navigate(new Views.Menu());
        }

        // Позволяет закрыть приложение по нажатию кнопки
        private void ShutDownButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Позволяет свернуть окно с приложением
        private void MinimizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        // Позволяет передвигать окно приложения
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton is MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
