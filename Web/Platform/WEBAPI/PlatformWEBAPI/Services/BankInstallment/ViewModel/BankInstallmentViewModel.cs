using static MI.Entity.Enums.BankInstallment;

namespace PlatformWEBAPI.Services.BankInstallment.ViewModel
{
    public class BankInstallmentMinify
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Type { get; set; } = (int)BankType.Bank;
    }


}
