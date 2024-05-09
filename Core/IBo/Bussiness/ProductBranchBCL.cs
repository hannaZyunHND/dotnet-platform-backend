using MI.Dal.IDbContext;
using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MI.Bo.Bussiness
{
    public class ProductBranchBCL : Base<ProductBranch>
    {
        public ProductBranchBCL()
        {

        }
        public int Create(ProductBranch department)
        {
            int result = 0;
            try
            {
                using (IDbContext db = new IDbContext())
                {

                    var kH = db.ProductBranch.FirstOrDefault(n => n.ID == department.ID);
                    if (kH == null)
                    {
                        db.Set<ProductBranch>().Add(department);
                        db.SaveChanges();
                        result = 1;
                    }
                    else
                    {
                        result = -1; // đã tồn tại
                    }
                }

            }
            catch (Exception ex)
            {
                return -99;
            }
            return result;
        }
    }
}
