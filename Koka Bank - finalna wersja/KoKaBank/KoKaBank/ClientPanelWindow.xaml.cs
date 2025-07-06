using System;
using System.Collections.Generic;
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
    public partial class ClientPanelWindow : Window
    {
        private List<BankTransaction> transactions;
        private decimal currentBalance;
        private string clientId;

        public ClientPanelWindow(string clientId)
        {
            InitializeComponent();
            this.clientId = clientId;

            WelcomeText.Text = $"Witaj, Kliencie {clientId}!";

            // Pobierz aktualne dane z bazy danych
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            // Pobierz saldo z bazy danych
            currentBalance = DatabaseHelper.GetUserBalance(clientId);

            // Pobierz transakcje z bazy danych
            transactions = DatabaseHelper.GetUserTransactions(clientId);

            UpdateUI();
        }

        private void UpdateUI()
        {
            BalanceText.Text = string.Format("{0:N2} PLN", currentBalance);
            TransactionList.ItemsSource = null;
            TransactionList.ItemsSource = transactions;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź które przyciski zostały kliknięte
            Button clickedButton = sender as Button;
            string buttonContent = clickedButton.Content.ToString();

            if (buttonContent == "Przelew wewnętrzny")
            {
                // Nowy przelew wewnętrzny
                HandleInternalTransfer();
            }
            else if (buttonContent == "Przelew zewnętrzny")
            {
                // Stary przelew zewnętrzny
                HandleExternalTransfer();
            }
            else if (buttonContent == "Informacje o koncie")
            {
                // Pokazuje informacje o koncie
                ShowAccountInfo();
            }
        }

        private void HandleInternalTransfer()
        {
            // Sprawdź czy użytkownik ma wystarczające środki
            if (currentBalance <= 0)
            {
                MessageBox.Show("Nie masz wystarczających środków na koncie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Otwórz okno przelewu wewnętrznego
            InternalTransferWindow transferWindow = new InternalTransferWindow(clientId);
            transferWindow.Owner = this;
            transferWindow.ShowDialog();

            if (transferWindow.TransferSuccessful)
            {
                // Odśwież dane z bazy po udanym przelewie
                LoadDataFromDatabase();

                // Pokaż komunikat sukcesu (opcjonalnie, bo już jest w oknie przelewu)
                // MessageBox.Show(transferWindow.Message, "Przelew zrealizowany", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void HandleExternalTransfer()
        {
            // Sprawdź czy użytkownik ma wystarczające środki
            if (currentBalance <= 0)
            {
                MessageBox.Show("Nie masz wystarczających środków na koncie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ExTransferWindow transferWindow = new ExTransferWindow();
            transferWindow.Owner = this;
            transferWindow.ShowDialog();

            if (transferWindow.TransferSuccessful)
            {
                // Sprawdź czy użytkownik ma wystarczające środki na przelew
                if (transferWindow.Amount > currentBalance)
                {
                    MessageBox.Show("Nie masz wystarczających środków na ten przelew.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Oblicz nowe saldo
                decimal newBalance = currentBalance - transferWindow.Amount;

                // Zapisz transakcję w bazie danych jako przelew zewnętrzny
                DatabaseHelper.AddTransaction(
                    clientId,
                    "Przelew zewnętrzny do " + transferWindow.Recipient,
                    -transferWindow.Amount,
                    newBalance,
                    null, // brak odbiorcy w systemie
                    "external"
                );

                // Odśwież dane z bazy
                LoadDataFromDatabase();

                MessageBox.Show($"Przelew zewnętrzny w wysokości {transferWindow.Amount:N2} PLN do {transferWindow.Recipient} został zrealizowany.",
                              "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowAccountInfo()
        {
            var accountInfo = DatabaseHelper.GetAccountInfo(clientId);
            if (accountInfo != null)
            {
                string info = $"Informacje o koncie:\n\n" +
                             $"Numer klienta: {accountInfo.ClientId}\n" +
                             $"Imię i nazwisko: {accountInfo.FullName}\n" +
                             $"Obywatelstwo: {accountInfo.Nationality}\n" +
                             $"PESEL: {accountInfo.Pesel}\n" +
                             $"Nr dowodu: {accountInfo.IdNumber}\n" +
                             $"Saldo: {accountInfo.Balance:N2} PLN\n" +
                             $"Data założenia: {accountInfo.CreatedDate:d}";

                MessageBox.Show(info, "Informacje o koncie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Nie można pobrać informacji o koncie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class BankTransaction
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}