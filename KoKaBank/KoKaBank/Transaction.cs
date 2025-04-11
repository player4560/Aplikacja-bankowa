using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoKaBank
{
    public class Transaction
    {
        public DateTime Date { get; set; }  
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfter { get; set; }
    }
}
