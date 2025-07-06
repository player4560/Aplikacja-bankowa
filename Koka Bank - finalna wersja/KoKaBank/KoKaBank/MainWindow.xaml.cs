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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KoKaBank
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string loginInput = loginBox.Text;
            string passwordInput = ShowPasswordCheckBox.IsChecked == true
                ? PasswordTextBox.Text
                : PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(loginInput) || string.IsNullOrWhiteSpace(passwordInput))
            {
                OutputText.Text = "Proszę uzupełnić numer klienta i hasło.";
                OutputText.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // Sprawdź dane logowania w bazie danych
            if (DatabaseHelper.ValidateUser(loginInput, passwordInput))
            {
                OutputText.Foreground = System.Windows.Media.Brushes.Green;
                OutputText.Text = $"Witaj, Kliencie {loginInput}!";

                // Pokaż alert bezpieczeństwa
                var alert = new AlertWindow();
                alert.ShowDialog(); // blokujące okno

                // Otwiera panel klienta
                var clientPanel = new ClientPanelWindow(loginInput);
                clientPanel.Show();

                // Ukryj okno logowania
                this.Hide();
            }
            else
            {
                OutputText.Foreground = System.Windows.Media.Brushes.Red;
                OutputText.Text = "Nieprawidłowy numer klienta lub hasło.";
            }
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = PasswordBox.Password;
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordBox.Password = PasswordTextBox.Text;
            PasswordBox.Visibility = Visibility.Visible;
            PasswordTextBox.Visibility = Visibility.Collapsed;
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            var resetWindow = new ResetPasswordWindow("", newPassword =>
            {
                // Ta funkcja zostanie wywołana po zmianie hasła
                MessageBox.Show("Hasło zostało zaktualizowane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            resetWindow.ShowDialog();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog(); // Otwarcie okna rejestracji
        }
    }
}
