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
        private string expectedClientId;
        private Action<string> onPasswordReset;

        public ResetPasswordWindow(string clientId, Action<string> onPasswordResetCallback)
        {
            InitializeComponent();
            expectedClientId = clientId;
            onPasswordReset = onPasswordResetCallback;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string clientId = ClientIdBox.Text;
            string newPassword = NewPasswordBox.Password;

            if (clientId != expectedClientId)
            {
                ResetMessage.Text = "Numer Klienta nie został odnaleziony";
                return;
            }

            onPasswordReset?.Invoke(newPassword);
            ResetMessage.Foreground = System.Windows.Media.Brushes.Green;
            ResetMessage.Text = "Hasło zostało zmienione.";
        }
    }
}

