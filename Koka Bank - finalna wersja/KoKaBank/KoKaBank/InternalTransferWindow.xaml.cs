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
    public partial class InternalTransferWindow : Window
    {
        public bool TransferSuccessful { get; private set; }
        public string Message { get; private set; }
        private string senderClientId;

        public InternalTransferWindow(string senderClientId)
        {
            InitializeComponent();
            this.senderClientId = senderClientId;
        }

        private void RecipientClientId_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Wyczyść informacje o odbiorcy przy zmianie numeru
            RecipientInfoText.Text = "";
        }

        private void CheckRecipient_Click(object sender, RoutedEventArgs e)
        {
            string recipientId = RecipientClientIdTextBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(recipientId))
            {
                RecipientInfoText.Text = "Proszę podać numer klienta odbiorcy.";
                RecipientInfoText.Foreground = Brushes.Red;
                return;
            }

            if (recipientId == senderClientId)
            {
                RecipientInfoText.Text = "Nie możesz wykonać przelewu do siebie.";
                RecipientInfoText.Foreground = Brushes.Red;
                return;
            }

            // Sprawdź czy użytkownik istnieje
            if (DatabaseHelper.UserExists(recipientId))
            {
                string recipientName = DatabaseHelper.GetUserName(recipientId);
                RecipientInfoText.Text = $"✓ Odbiorca: {recipientName} (Nr: {recipientId})";
                RecipientInfoText.Foreground = Brushes.Green;
            }
            else
            {
                RecipientInfoText.Text = "❌ Użytkownik o podanym numerze nie istnieje.";
                RecipientInfoText.Foreground = Brushes.Red;
            }
        }

        private void ExecuteTransfer_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź dane
            string recipientId = RecipientClientIdTextBox.Text?.Trim();
            string amountText = AmountTextBox.Text?.Trim();
            string description = DescriptionTextBox.Text?.Trim();

            // Walidacja podstawowa
            if (string.IsNullOrWhiteSpace(recipientId))
            {
                MessageBox.Show("Proszę podać numer klienta odbiorcy.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(amountText))
            {
                MessageBox.Show("Proszę podać kwotę przelewu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                description = "Przelew wewnętrzny";
            }

            // Sprawdź czy kwota jest poprawna
            if (!decimal.TryParse(amountText, out decimal amount))
            {
                MessageBox.Show("Proszę podać prawidłową kwotę (np. 100.50).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show("Kwota musi być większa od zera.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (amount > 50000)
            {
                MessageBox.Show("Maksymalna kwota przelewu wewnętrznego to 50,000 PLN.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź czy odbiorca istnieje
            if (!DatabaseHelper.UserExists(recipientId))
            {
                MessageBox.Show("Odbiorca o podanym numerze klienta nie istnieje.\nSprawdź numer i spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (recipientId == senderClientId)
            {
                MessageBox.Show("Nie możesz wykonać przelewu do siebie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pobierz nazwę odbiorcy do potwierdzenia
            string recipientName = DatabaseHelper.GetUserName(recipientId);

            // Potwierdź przelew
            var result = MessageBox.Show(
                $"Czy na pewno chcesz wykonać przelew?\n\n" +
                $"Odbiorca: {recipientName}\n" +
                $"Nr klienta: {recipientId}\n" +
                $"Kwota: {amount:N2} PLN\n" +
                $"Opis: {description}",
                "Potwierdzenie przelewu wewnętrznego",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Wykonaj przelew
                var transferResult = DatabaseHelper.ProcessInternalTransfer(senderClientId, recipientId, amount, description);

                if (transferResult.Success)
                {
                    TransferSuccessful = true;
                    Message = transferResult.Message;
                    MessageBox.Show(transferResult.Message, "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(transferResult.Message, "Błąd przelewu", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            TransferSuccessful = false;
            this.Close();
        }
    }
}