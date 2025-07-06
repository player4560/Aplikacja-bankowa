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
            string nationality = NationalityTextBox.Text;
            string pesel = PeselTextBox.Text;
            string idNumber = IDTextBox.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Numer klienta, hasło i imię/nazwisko są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź czy numer klienta ma odpowiednią długość (np. 6 cyfr)
            if (username.Length < 6)
            {
                MessageBox.Show("Numer klienta musi mieć co najmniej 6 znaków.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Zapisz użytkownika w bazie danych
            if (DatabaseHelper.RegisterUser(username, password, fullName, nationality, pesel, idNumber))
            {
                MessageBox.Show($"Rejestracja zakończona sukcesem!\nTwój numer klienta: {username}\nMożesz się teraz zalogować.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Zamknięcie okna po udanej rejestracji
            }
            else
            {
                MessageBox.Show("Błąd rejestracji. Numer klienta może już istnieć w systemie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
