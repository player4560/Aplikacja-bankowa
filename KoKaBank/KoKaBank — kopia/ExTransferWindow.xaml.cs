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
            Recipient = RecipientTextBox.Text;
            Amount = decimal.Parse(AmountTextBox.Text);

            if (Amount > 0 && !string.IsNullOrWhiteSpace(Recipient))
            {
                TransferSuccessful = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Proszę wprowadzić poprawne dane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

