using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KoKaBank
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            string fullName = FullNameTextBox.Text;
            // nie wiem czy string powinno zostac stworzone dla pesel obywatelstwo itp

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Przykładowa logika rejestracji (np. zapisanie w bazie danych lub pamięci)
            // Tutaj możesz rozważyć zapis do pliku, w bazie danych lub w inny sposób.

            MessageBox.Show("Rejestracja zakończona sukcesem.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); // Zamknięcie okna po udanej rejestracji
        }
    }
}
