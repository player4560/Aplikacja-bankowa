using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static KoKaBank.ClientPanelWindow;

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
            // Pobieranie danych z pól tekstowych
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            string fullName = FullNameTextBox.Text;
            string PESEL = PeselTextBox.Text;
            string ID = IDTextBox.Text;
            string Nationality = NationalityTextBox.Text;




            // Walidacja danych wejściowych
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
            {
                MessageBox.Show("Wszystkie pola muszą być wypełnione.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdzenie, czy użytkownik już istnieje
            if (UserStorage.Users.Any(user => user.ClientId == username))
            {
                MessageBox.Show("Użytkownik o podanym numerze klienta już istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Tworzenie nowego użytkownika
            User newUser = new User
            {
                ClientId = username,
                Password = password,
                FullName = fullName,
                PESEL = PESEL,
                ID = ID,
                Nationality = Nationality,

                Balance = 0, // Bilans początkowy równy 0
                Transactions = new List<BankTransaction>() // Pusta lista transakcji
            };

            // Dodanie nowego użytkownika do lokalnej listy
            UserStorage.Users.Add(newUser);

            // Powiadomienie o sukcesie
            MessageBox.Show("Rejestracja zakończona sukcesem.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); // Zamknięcie okna po udanej rejestracji
        }
    }
}