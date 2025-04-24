using System;
using System.Linq;
using System.Windows;

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

            // Dodanie użytkownika demo przy starcie aplikacji, jeśli nie ma go na liście
            if (UserStorage.Users.Count == 0)
            {
                var demoUser = new User
                {
                    ClientId = "123456",
                    Password = "admin",
                    FullName = "Demo User",
                    Balance = 1000m
                };
                UserStorage.Users.Add(demoUser);
            }
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

            // Sprawdzenie, czy użytkownik istnieje w liście
            var user = UserStorage.Users.FirstOrDefault(u => u.ClientId == loginInput && u.Password == passwordInput);
            if (user != null)
            {
                OutputText.Foreground = System.Windows.Media.Brushes.Green;
                OutputText.Text = $"Witaj, Kliencie {loginInput}!";

                // Pokaż alert bezpieczeństwa
                var alert = new AlertWindow();
                alert.ShowDialog(); // blokujące okno

                // Otwórz panel klienta
                var clientPanel = new ClientPanelWindow(user);
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
            var resetWindow = new ResetPasswordWindow("123456", newPassword =>
            {
                // Zmieniamy hasło demo
                var demoUser = UserStorage.Users.FirstOrDefault(u => u.ClientId == "123456");
                if (demoUser != null)
                {
                    demoUser.Password = newPassword;
                    MessageBox.Show("Hasło zostało zaktualizowane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
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