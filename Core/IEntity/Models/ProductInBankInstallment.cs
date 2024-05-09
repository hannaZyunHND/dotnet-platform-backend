using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Models
{
    public partial class ProductInBankInstallment
    {
        public int Id { get; set; }
        public int ProductId { get; set; } = 0;
        public int BankInstallmentId { get; set; } = 0;
        public int Period { get; set; } = 0;
        public decimal PaymentFirst { get; set; } = 0;
        public decimal PaymentFirstPercent { get; set; } = 0;
        public decimal InterestPercent { get; set; } = 0;
        public decimal OthersFee { get; set; } = 0;
        public decimal OthersFeePercent { get; set; } = 0;
    }
}
