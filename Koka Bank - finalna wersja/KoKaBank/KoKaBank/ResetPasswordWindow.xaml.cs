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
    public partial class ResetPasswordWindow : Window
    {
        private Action<string> onPasswordReset;

        public ResetPasswordWindow(string clientId, Action<string> onPasswordResetCallback)
        {
            InitializeComponent();
            onPasswordReset = onPasswordResetCallback;
            
            // Jeśli podano clientId, ustaw go w polu
            if (!string.IsNullOrEmpty(clientId))
            {
                ClientIdBox.Text = clientId;
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string clientId = ClientIdBox.Text;
            string newPassword = NewPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(clientId))
            {
                ResetMessage.Text = "Proszę podać numer klienta.";
                ResetMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                ResetMessage.Text = "Proszę podać nowe hasło.";
                ResetMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            if (newPassword.Length < 4)
            {
                ResetMessage.Text = "Hasło musi mieć co najmniej 4 znaki.";
                ResetMessage.Foreground = System.Windows.Media.Brushes.Red;
                return;
            }

            // Sprawdź czy użytkownik istnieje i zmień hasło
            if (DatabaseHelper.UserExists(clientId))
            {
                if (DatabaseHelper.ChangePassword(clientId, newPassword))
                {
                    ResetMessage.Foreground = System.Windows.Media.Brushes.Green;
                    ResetMessage.Text = "Hasło zostało zmienione pomyślnie.";
                    
                    // Wywołaj callback
                    onPasswordReset?.Invoke(newPassword);
                    
                    // Zamknij okno po 2 sekundach
                    var timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(2);
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        this.Close();
                    };
                    timer.Start();
                }
                else
                {
                    ResetMessage.Text = "Wystąpił błąd podczas zmiany hasła.";
                    ResetMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }
            else
            {
                ResetMessage.Text = "Numer klienta nie został odnaleziony.";
                ResetMessage.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
    }
}