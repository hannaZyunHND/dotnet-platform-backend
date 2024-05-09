using MI.Dal.IDbContext;
using MI.Entity.Models;
using PlatformWEB.Services.BankInstallment.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace PlatformWEB.Services.BankInstallment.Repository
{
    public interface IBankInstallmentRepository
    {
        List<BankInstallmentMinify> GetBankInstallmentInProduct(int productId, int bankType);
        List<int> GetBankPeriod(int productId, int bankId);
        List<ProductInBankInstallment> GetBankPeriodDetail(int productId, int bankId, int period);
    }
    public class BankInstallmentRepository : IBankInstallmentRepository
    {
        public BankInstallmentRepository()
        {

        }
        public List<BankInstallmentMinify> GetBankInstallmentInProduct(int productId, int bankType)
        {
            using (IDbContext _context = new IDbContext())
            {
                if (productId > 0)
                {
                    var productInBank = _context.ProductInBankInstallment.Where(r => r.ProductId == productId).AsQueryable();
                    var listBank = from p in productInBank
                                   group p by p.BankInstallmentId into grouped
                                   select grouped.Key;
                    var listBankMinified = _context.BankInstallment.Where(r => listBank.Contains(r.Id)).Where(r => r.Type == bankType)
                                                           .Select(r => new BankInstallmentMinify { Id = r.Id, Name = r.Name, Avatar = r.Avatar, Type = r.Type }).ToList();
                    return listBankMinified;
                }
            }
            return null;
        }

        public List<int> GetBankPeriod(int productId, int bankId)
        {
            using (IDbContext _context = new IDbContext())
            {
                if (productId > 0 && bankId > 0)
                {
                    var query = _context.ProductInBankInstallment.Where(r => r.ProductId == productId && r.BankInstallmentId == bankId).AsQueryable();
                    var result = from q in query
                                 group q by q.Period into grouped
                                 select grouped.Key;
                    return result.ToList();

                }
            }
            return null;
        }

        public List<ProductInBankInstallment> GetBankPeriodDetail(int productId, int bankId, int period)
        {
            using (IDbContext _context = new IDbContext())
            {
                if (productId > 0 && bankId > 0 && period > 0)
                {
                    var query = _context.ProductInBankInstallment.Where(r => r.ProductId == productId && r.BankInstallmentId == bankId && r.Period == period).AsQueryable();

                    return query.ToList();
                }
            }
            return null;
        }
    }
}
