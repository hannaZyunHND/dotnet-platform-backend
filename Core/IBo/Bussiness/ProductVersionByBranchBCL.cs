using MI.Dal.IDbContext;
using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MI.Bo.Bussiness
{
    public class ProductVersionByBranchBCL:Base<ProductVersionByBranch>
    {
        public ProductVersionByBranchBCL()
        {

        }
        public bool Add(int idBranch, List<ProductVersionByBranch> entitys)
        {
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    _context.ProductVersionByBranch.RemoveRange(_context.ProductVersionByBranch.Where(x => x.ProductBranchID == idBranch));
                    _context.ProductVersionByBranch.AddRange(entitys);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
