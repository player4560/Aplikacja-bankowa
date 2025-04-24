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
    public partial class ClientPanelWindow : Window
    {
        private List<BankTransaction> transactions;
        private decimal currentBalance;
        private User currentUser;  //dla nowych uzytkownikow
       
        public ClientPanelWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
        


            WelcomeText.Text = $"Witaj, Kliencie {user.ClientId}!";

            if (user.ClientId == "123456") // demo user
            {
                transactions = new List<BankTransaction>
                {
                    new BankTransaction { Date = new DateTime(2025, 4, 1), Description = "Wpłata własna", Amount = 5000m, BalanceAfter = 5000m },
                    new BankTransaction { Date = new DateTime(2025, 4, 3), Description = "Przelew do Jan Kowalski", Amount = -120m, BalanceAfter = 4880m },
                    new BankTransaction { Date = new DateTime(2025, 4, 5), Description = "Zakupy Allegro", Amount = -350.5m, BalanceAfter = 4529.5m },
                    new BankTransaction { Date = new DateTime(2025, 4, 7), Description = "Przelew od Anna Nowak", Amount = 250m, BalanceAfter = 4779.5m }
                };
                currentBalance = transactions[transactions.Count - 1].BalanceAfter;
            }
            else
            {
                transactions = new List<BankTransaction>();
                currentBalance = 0m;
            }

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
            ExTransferWindow transferWindow = new ExTransferWindow();
            transferWindow.Owner = this;
            transferWindow.ShowDialog();

            if (transferWindow.TransferSuccessful)
            {
                currentBalance -= transferWindow.Amount;

                transactions.Insert(0, new BankTransaction
                {
                    Date = DateTime.Now,
                    Description = "Przelew do " + transferWindow.Recipient,
                    Amount = -transferWindow.Amount,
                    BalanceAfter = currentBalance
                });

                UpdateUI();
            }
        }



        private void ShowAccountInfo_Click(object sender, RoutedEventArgs e)
        {
            string info = $"Imię i nazwisko: {currentUser.FullName}\n" +
                          $"PESEL: {currentUser.PESEL}\n" +
                          $"Obywatelstwo: {currentUser.Nationality}\n" +
                          $"Seria i numer dowodu osobistego: {currentUser.ID}\n" +
                          $"ID klienta: {currentUser.ClientId}";

            MessageBox.Show(info, "Dane konta", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public class BankTransaction
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public decimal BalanceAfter { get; set; }
        }
    }
}
