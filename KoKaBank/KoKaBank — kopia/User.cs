using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KoKaBank
{
    public class User
    {
     public string ClientId { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }

    public string Nationality { get; set; }
    public string PESEL { get; set; }

    public string ID { get; set; }

        public decimal Balance { get; set; } = 0m;
    public List<BankTransaction> Transactions { get; set; }
    }
}
