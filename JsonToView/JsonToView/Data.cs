using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToView
{
    class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Algorithm { get; set; }
        public double WithdrawFee { get; set; }
        public double MinWithdraw { get; set; }
        public double MinBaseTrade { get; set; }
        public bool IsTipEnabled { get; set; }
        public double MinTip { get; set; }
        public int DepositConfirmations { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public string ListingStatus { get; set; }

    }
}
