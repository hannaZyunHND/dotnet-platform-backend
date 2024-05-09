using MI.Dal.IDbContext;
using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MI.Bo.Bussiness
{
    public class ProductOldRenewalBCL: Base<ProductOldRenewal>
    {
        public ProductOldRenewalBCL()
        {

        }

        public List<CustomerRequestProductOldRenewal> GetAllProductOleRenewal()
        {
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    return _context.CustomerRequestProductOldRenewal.ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return new List<CustomerRequestProductOldRenewal>();
        }

        public List<CustomerRequestProductOldRenewal> Find(Utils.FilterQuery filter, out int total)
        {
            total = 0;
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    IQueryable<CustomerRequestProductOldRenewal> items = (from c in _context.CustomerRequestProductOldRenewal
                                                                          join p in _context.Product on c.ProductId equals p.Id
                                                                          select new CustomerRequestProductOldRenewal
                                                                          {
                                                                              Id = c.Id,
                                                                              ProductId = c.ProductId,
                                                                              CreateDate = c.CreateDate,
                                                                              DepartmentId = c.DepartmentId,
                                                                              Email = c.Email,
                                                                              FullName = c.FullName,
                                                                              IsSupported = c.IsSupported,
                                                                              LocationId = c.LocationId,
                                                                              PhoneNumber = c.PhoneNumber,
                                                                              ProductName = p.Name,
                                                                              Type = c.Type,
                                                                              Image = p.Avatar,
                                                                              DealPrice = c.DealPrice,
                                                                              ProductIdToExchange = c.ProductIdToExchange
                                                                            
                                                                          }) ;
                    

                    if (!String.IsNullOrEmpty(filter.keyword))
                        items = items.Where(x => x.ProductName.Contains(filter.keyword) || x.PhoneNumber.Contains(filter.keyword) || x.Email.Contains(filter.keyword) || x.FullName.Contains(filter.keyword));

                    switch (filter.type)
                    {
                        case (int)Entity.Enums.StatusProductOldRenewal.Old:
                            items = items.Where(x => x.Type == (int)Entity.Enums.StatusProductOldRenewal.Old);
                            break;
                        case (int)Entity.Enums.StatusProductOldRenewal.New:
                            items = items.Where(x => x.Type == (int)Entity.Enums.StatusProductOldRenewal.New);
                            break;
                    }
                    total = items.Count();
                    var result = items.OrderByDescending(x => x.CreateDate).Skip((filter.pageIndex - 1) * filter.pageSize).Take(filter.pageSize).ToList();

                    var productOldRenewals = _context.ProductOldRenewal.ToList();
                    if(result != null && result.Any())
                    {
                        foreach (var item in result)
                        {
                            var pricePrefer = productOldRenewals.Where(x => x.ProductId == item.ProductId && item.Type == 1).FirstOrDefault()?.PriceRefer;
                            item.PriceRefer = pricePrefer == null ? "0đ" : String.Format("{0:N0}", pricePrefer).Replace(",",".") + "đ";
                            item.DealPriceStr = item.DealPrice == null ? "0đ" : String.Format("{0:N0}", item.DealPrice).Replace(",", ".") + "đ";
                            item.NewProductExchange = _context.Product.Where(x => x.Id == (item.ProductIdToExchange ?? 0)).FirstOrDefault();
                            item.NewProductPriceSTR = "0đ";
                            item.SupportPriceSTR = "0đ";
                            if (item.NewProductExchange != null)
                            {
                                item.NewProductPriceSTR = item.NewProductExchange.Price == null ? "0đ" : String.Format("{0:N0}", item.NewProductExchange.Price).Replace(",", ".") + "đ";
                                var discount = item.NewProductExchange.Price / 100 * item.NewProductExchange.DiscountPercent;

                                item.SupportPriceSTR = String.Format("{0:N0}", (item.NewProductExchange.Price.GetValueOrDefault(0) - double.Parse(item.DealPrice.GetValueOrDefault(0).ToString()) - discount)) + "đ";
                            }
                        }
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {

            }
            return new List<CustomerRequestProductOldRenewal>();
        }

        public bool UpdateStatus(CustomerRequestProductOldRenewal obj)
        {
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    _context.Attach(obj);
                    var entry = _context.Entry(obj);
                    entry.Property(x => x.IsSupported).IsModified = true;
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
