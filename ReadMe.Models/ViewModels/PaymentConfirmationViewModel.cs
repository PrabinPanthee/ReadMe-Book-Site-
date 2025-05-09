using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadMe.Models.ViewModels
{
    public class PaymentConfirmationViewModel
    {
        public string Pidx { get; set; }
        public string TransactionId { get; set; }
        public string Tidx { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalAmountInRupees => TotalAmount / 100m;
        public string Mobile { get; set; }
        public string Status { get; set; }
        public string PurchaseOrderId { get; set; }
        public string PurchaseOrderName { get; set; }
    }
}
