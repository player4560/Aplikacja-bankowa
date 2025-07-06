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
    public partial class ExTransferWindow : Window
    {
        public bool TransferSuccessful { get; private set; }
        public string Recipient { get; private set; }
        public decimal Amount { get; private set; }

        public ExTransferWindow()
        {
            InitializeComponent();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            // Zbieranie danych z formularza
            Recipient = RecipientTextBox.Text?.Trim();
            string amountText = AmountTextBox.Text?.Trim();

            // Walidacja danych
            if (string.IsNullOrWhiteSpace(Recipient))
            {
                MessageBox.Show("Proszę podać nazwę odbiorcy.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(amountText))
            {
                MessageBox.Show("Proszę podać kwotę przelewu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź czy kwota jest poprawną liczbą
            if (!decimal.TryParse(amountText, out decimal amount))
            {
                MessageBox.Show("Proszę podać prawidłową kwotę (np. 100.50).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź czy kwota jest dodatnia
            if (amount <= 0)
            {
                MessageBox.Show("Kwota przelewu musi być większa od zera.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź czy kwota nie jest zbyt duża (zabezpieczenie)
            if (amount > 100000)
            {
                MessageBox.Show("Maksymalna kwota przelewu to 100,000 PLN.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Sprawdź czy nazwa odbiorcy nie jest zbyt krótka
            if (Recipient.Length < 3)
            {
                MessageBox.Show("Nazwa odbiorcy musi mieć co najmniej 3 znaki.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Potwierdź przelew
            var result = MessageBox.Show(
                $"Czy na pewno chcesz wykonać przelew?\n\nOdbiorca: {Recipient}\nKwota: {amount:N2} PLN",
                "Potwierdzenie przelewu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Amount = amount;
                TransferSuccessful = true;
                this.Close();
            }
        }
    }
}