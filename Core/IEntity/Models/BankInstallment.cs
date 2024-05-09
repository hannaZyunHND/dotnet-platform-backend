﻿using System;
using System.Collections.Generic;
using static MI.Entity.Enums.BankInstallment;

namespace MI.Entity.Models
{
    public partial class BankInstallment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Url { get; set; }
        public string Period { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int Type { get; set; } = (int)BankType.Bank;

    }
}
