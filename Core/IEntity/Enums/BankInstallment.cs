using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Enums
{
    public class BankInstallment
    {
        public enum CardType : byte
        {
            [EnumDescription("Thẻ thường")]
            CardBasic = 1,
            [EnumDescription("Thẻ Visa")]
            CardVisa = 2
        }
        public enum BankType : byte
        {
            [EnumDescription("Ngân hàng")]
            Bank = 1,
            [EnumDescription("Công ty tài chính")]
            FinancialCompany = 2
        }
    }
}
