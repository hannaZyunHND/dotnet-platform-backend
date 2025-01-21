using MI.Dal.IDbContext;
//using MI.Dapper.Data.Models;
using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MI.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
//using MI.Dapper.Data.Models;

namespace MI.Bo.Bussiness
{
    public partial class ProductBCL : Base<Product>
    {
        public ProductBCL()
        {

        }
        public IEnumerable<Product> GetListProductParent()
        {
            using (IDbContext _context = new IDbContext())
            {
                var Product = _context.Product.AsQueryable();
                Product = Product.Where(r => r.ParentId == 0);
                Product = from p in Product
                          select new Product()
                          {
                              Id = p.Id,
                              Name = p.Name
                          };
                return Product.AsEnumerable();
            }
        }
        public List<Product> Get(int pageIndex, int pageSize, string sortBy, string sortDir, out int total)
        {
            using (IDbContext _context = new IDbContext())
            {
                var Product = _context.Product.AsQueryable();
                if (sortDir == "asc")
                {
                    Product = Product.OrderBy(x => x.GetType().GetProperty(sortBy).GetValue(x, null));
                }
                else
                {
                    Product = Product.OrderByDescending(x => x.GetType().GetProperty(sortBy).GetValue(x, null));
                }

                total = Product.Count();

                return Product.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
        public KeyValuePair<bool, string> UpdateVoucher(Dictionary<int, string> dicVoucher)
        {
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    foreach (var item in dicVoucher)
                    {
                        var entity = new Product { Id = item.Key, Voucher = item.Value };
                        var entry = _context.Entry(entity);
                        entry.Property(p => p.Voucher).IsModified = true;
                    }
                    _context.SaveChanges();
                    return new KeyValuePair<bool, string>(true, "Thành công");
                }
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, "Thất bại");
            }


        }

        public Dictionary<int, string> GetVoucherById(List<int> lstId)
        {
            using (IDbContext _context = new IDbContext())
            {
                return _context.Product.Where(x => x.Status != 3 && lstId.Contains(x.Id)).Select(x => new { x.Id, x.Voucher }).ToList().ToDictionary(d => d.Id, d => d.Voucher);
            }
        }

        public Dictionary<int, string> GetAllName()
        {
            using (IDbContext _context = new IDbContext())
            {
                return _context.Product.Where(x => x.Status != 3).Select(x => new { x.Id, x.Name }).ToList().ToDictionary(d => d.Id, d => d.Name);
            }
        }
        public void Test()
        {
            Console.WriteLine(1);
        }
        public List<Product> Search(string productName, int pageIndex, int pageSize, string sortBy, string sortDir, out int total)
        {
            using (IDbContext _context = new IDbContext())
            {
                var Product = _context.Product.AsQueryable();

                if (sortDir == "asc")
                {
                    Product = Product.OrderBy(x => x.GetType().GetProperty(sortBy).GetValue(x, null));
                }
                else
                {
                    Product = Product.OrderByDescending(x => x.GetType().GetProperty(sortBy).GetValue(x, null));
                }

                total = Product.Count();

                return Product.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public int CreateProduct(Product product)
        {
            int result = 0;
            try
            {
                using (IDbContext db = new IDbContext())
                {

                    var kH = db.Product.FirstOrDefault(n => n.Code.Trim() == product.Code.Trim());
                    if (kH == null)
                    {
                        db.Set<Product>().Add(product);

                        db.SaveChanges();
                        result = product.Id;
                    }
                    else
                        result = -1; // Sản phẩm đã tồn tại
                }

            }
            catch (Exception ex)
            {
                return -99;
            }
            return result;
        }

        public Dictionary<int, Product> GetByOrderDetail()
        {
            using (IDbContext _context = new IDbContext())
            {
                var query = _context.OrderDetail.
                    Join(_context.Product,
                    order => order.ProductId,
                    product => product.Id,
                    (od, pt) => new { Oder = od, Product = pt }).ToList();
                return new Dictionary<int, Product>();
            }

        }


        public int Add(Product product, List<ProductInZone> lstObj)
        {
            int result = 0;
            try
            {
                using (IDbContext db = new IDbContext())
                {

                    var kH = db.Product.FirstOrDefault(n => n.Code.Trim() == product.Code.Trim());
                    if (kH == null)
                    {
                        db.ProductInZone.RemoveRange(db.ProductInZone.Where(x => x.ProductId == product.Id));
                        db.ProductInZone.AddRange(lstObj);
                        db.Product.Add(product);
                        db.SaveChanges();
                        result = product.Id;
                    }
                    else
                        result = -1; // Sản phẩm đã tồn tại
                }

            }
            catch (Exception ex)
            {
                return -99;
            }
            return result;
        }
        public int Update(Entity.Models.Product product, List<ProductInZone> lstObj)
        {
            int result = 0;
            try
            {
                using (IDbContext db = new IDbContext())
                {

                    var kH = db.Product.Any(n => n.Code.Trim() == product.Code.Trim() && n.Id != product.Id);
                    if (!kH)
                    {
                        db.ProductInZone.RemoveRange(db.ProductInZone.Where(x => x.ProductId == product.Id));
                        db.ProductInZone.AddRange(lstObj);
                        db.Product.Update(product);
                        db.SaveChanges();
                        result = product.Id;
                    }
                    else
                        result = -1; // Sản phẩm đã tồn tại
                }

            }
            catch (Exception ex)
            {
                return -99;
            }
            return result;
        }

        public List<ProductPriceInZoneListByDate> GetAllProductPriceInZoneListByDate(string zoneList, int productId)
        {
            using (IDbContext context = new IDbContext())
            {
                var query = context.ProductPriceInZoneListByDate.Where(r => r.ZoneList.Equals(zoneList) && r.ProductId == productId);
                return query.ToList();
            }
        }

        public bool UpdateGiaPhienBanTheoNgay(List<ProductPriceInZoneListByDate> requese)
        {
            using (IDbContext context = new IDbContext())
            {
                try
                {
                    context.ProductPriceInZoneListByDate.UpdateRange(requese);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    return false;
                }
                
            }
        }

        public int UpdatePhienBan(List<Dapper.Data.Models.PhienBans> phienBans, int productId)
        {
            using (IDbContext db = new IDbContext())
            {
                try
                {
                    //Xoa toan bo phien ban cu
                    var old = db.ProductPriceInZoneList.Where(r => r.ProductId == productId);
                    db.RemoveRange(old);
                    //Add new
                    var _new = new List<ProductPriceInZoneList>();
                    foreach (var item in phienBans)
                    {
                        var _newPhienBan = new ProductPriceInZoneList();
                        _newPhienBan.Id = 0;
                        _newPhienBan.ProductId = productId;
                        _newPhienBan.PriceEachNguoiLon = item.priceEachNguoiLon;
                        _newPhienBan.PriceEachTreEm = item.priceEachTreEm;
                        _newPhienBan.NetEachNguoiLon = item.netEachNguoiLon;
                        _newPhienBan.NetEachTreEm = item.netEachTreEm;
                        _newPhienBan.PriceEachNguoiGia = item.priceEachNguoiGia;
                        _newPhienBan.NetEachNguoiGia = item.netEachNguoiGia;
                        _newPhienBan.ZoneList = string.Join(",", item.selectedOptions);
                        _newPhienBan.MinimumNguoiLon = item.minimumNguoiLon;
                        _newPhienBan.MinimumTreEm = item.minimumTreEm;
                        _newPhienBan.MinimumNguoiGia = item.minimumNguoiGia;
                        _newPhienBan.EmailSupplier = item.emailSupplier;
                        _newPhienBan.ConfirmOption = item.confirmOption;
                        _newPhienBan.LastMinuteSetupDay = item.lastMinuteSetupDay;
                        _newPhienBan.LastMinuteSetupTime = item.lastMinuteSetupTime;
                        _new.Add(_newPhienBan);
                    }

                    db.ProductPriceInZoneList.AddRange(_new);
                    db.SaveChanges();

                    //Add tiep cho cac phan ve gia theo ngay thang o day
                    var pricesByDate = new List<ProductPriceInZoneListByDate>();
                    var allDates = GetDatesOfCurrentYear();
                    //var allDatesSelectedYearu = GetDatesOfSelectionYear(2025);
                    //allDates.AddRange(allDatesSelectedYearu);
                    foreach(var item in phienBans)
                    {

                        //Xoa tat ca cac gia theo ngay cua nam do
                        //var oldPricesByDate = db.ProductPriceInZoneListByDate.Where(r => r.ProductId == productId && r.ZoneList.Equals(string.Join("", item.selectedOptions)));
                        //if (oldPricesByDate.Any()) {
                        //    db.ProductPriceInZoneListByDate.RemoveRange(oldPricesByDate);
                        //}
                        var checker = db.ProductPriceInZoneListByDate.Where(r => r.ZoneList.Equals(string.Join(",",item.selectedOptions)) && r.ProductId == productId && r.Date.Year == DateTime.Now.Year).Any();
                        if (!checker)
                        {
                            
                            foreach(var d in allDates)
                            {
                                var pByDate = new ProductPriceInZoneListByDate();
                                pByDate.ZoneList = string.Join(",", item.selectedOptions);
                                pByDate.ProductId = productId;
                                pByDate.Date = d.Date;
                                pByDate.DayOfWeek = d.DayOfWeekAbbreviation;
                                pByDate.PriceEachNguoiLon = item.priceEachNguoiLon;
                                pByDate.PriceEachTreEm = item.priceEachTreEm;
                                pByDate.PriceEachNguoiGia = item.priceEachNguoiGia;
                                pByDate.NetEachNguoiLon = item.netEachNguoiLon;
                                pByDate.NetEachTreEm = item.netEachTreEm;
                                pByDate.NetEachNguoiGia = item.netEachTreEm;
                                pricesByDate.Add(pByDate);

                            }
                        }
                    }
                    db.ProductPriceInZoneListByDate.AddRange(pricesByDate);
                    db.SaveChanges();
                    return productId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                
            }
        }


        public async Task<bool> UpdateProductPriceInZoneListByYear(int year)
        {
            var pricesByDate = new List<ProductPriceInZoneListByDate>();
            var allDates = GetDatesOfSelectionYear(year);

            using (IDbContext context = new IDbContext())
            {
                
                var productZoneLists = await context.ProductPriceInZoneList.ToListAsync();
                foreach(var item in productZoneLists)
                {
                    //xoa tat ca gia cua 2025

                    var oldPrices = context.ProductPriceInZoneListByDate.Where(r => r.ProductId == item.ProductId && r.ZoneList.Equals(item.ZoneList) && r.Date.Year == year);
                    context.ProductPriceInZoneListByDate.RemoveRange(oldPrices);
                    await context.SaveChangesAsync();

                    foreach(var d in allDates)
                    {
                        var pByDate = new ProductPriceInZoneListByDate();
                        pByDate.ZoneList = item.ZoneList;
                        pByDate.ProductId = item.ProductId;
                        pByDate.Date = d.Date;
                        pByDate.DayOfWeek = d.DayOfWeekAbbreviation;
                        pByDate.PriceEachNguoiLon = item.PriceEachNguoiLon;
                        pByDate.PriceEachTreEm = item.PriceEachTreEm;
                        pByDate.PriceEachNguoiGia = item.PriceEachNguoiGia;
                        pByDate.NetEachNguoiLon = item.NetEachNguoiLon;
                        pByDate.NetEachTreEm = item.NetEachTreEm;
                        pByDate.NetEachNguoiGia = item.NetEachNguoiGia;
                        pricesByDate.Add(pByDate);
                    }
                }

                await context.ProductPriceInZoneListByDate.AddRangeAsync(pricesByDate);
                await context.SaveChangesAsync();
                return true;

            }
            return false;
    
        }

        public async Task<bool> UpdateProductPriceInZoneListByProductCodeInYear(int productId, string zoneList, int year)
        {
            var pricesByDate = new List<ProductPriceInZoneListByDate>();
            var allDates = GetDatesOfSelectionYear(year);

            using (IDbContext context = new IDbContext())
            {
                var productZoneList = await context.ProductPriceInZoneList.Where(r => r.ProductId == productId && r.ZoneList.Equals(zoneList)).FirstOrDefaultAsync();

                if (productZoneList != null)
                {
                    var item = productZoneList;
                    var oldPrices = context.ProductPriceInZoneListByDate.Where(r => r.ProductId == item.ProductId && r.ZoneList.Equals(item.ZoneList) && r.Date.Year == year);
                    if (oldPrices.Count() > 0)
                    {
                        context.ProductPriceInZoneListByDate.RemoveRange(oldPrices);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        foreach (var d in allDates)
                        {
                            var pByDate = new ProductPriceInZoneListByDate();
                            pByDate.ZoneList = item.ZoneList;
                            pByDate.ProductId = item.ProductId;
                            pByDate.Date = d.Date;
                            pByDate.DayOfWeek = d.DayOfWeekAbbreviation;
                            pByDate.PriceEachNguoiLon = item.PriceEachNguoiLon;
                            pByDate.PriceEachTreEm = item.PriceEachTreEm;
                            pByDate.PriceEachNguoiGia = item.PriceEachNguoiGia;
                            pByDate.NetEachNguoiLon = item.NetEachNguoiLon;
                            pByDate.NetEachTreEm = item.NetEachTreEm;
                            pByDate.NetEachNguoiGia = item.NetEachNguoiGia;
                            pricesByDate.Add(pByDate);
                        }
                    }
                }


                await context.ProductPriceInZoneListByDate.AddRangeAsync(pricesByDate);
                await context.SaveChangesAsync();
                return true;

            }
            return false;
        }

        public void UpdateCancelPolicies(List<ProductCancelPolicy> cancelPolicies)
        {
            var productId = 0;
            if(cancelPolicies.Count > 0)
            {
                var _first = cancelPolicies.FirstOrDefault();
                if(_first != null)
                {
                    productId = _first.ProductId;
                }
            }
            using (IDbContext context = new IDbContext())
            {
                if(productId > 0)
                {
                    var old = context.ProductCancelPolicy.Where(r => r.ProductId == productId);
                    if (old.Any())
                    {
                        context.ProductCancelPolicy.RemoveRange(old);
                        context.SaveChanges();
                    }
                    foreach(var item in cancelPolicies)
                    {
                        item.Id = 0;
                    }
                    //Them bu vao day
                    context.ProductCancelPolicy.AddRange(cancelPolicies);
                    context.SaveChanges();

                }
                
            }
        }

        private List<MI.Dapper.Data.Models.DateInfo> GetDatesOfCurrentYear()
        {
            var dateList = new List<MI.Dapper.Data.Models.DateInfo>();
            int currentYear = DateTime.Now.Year;

            DateTime startDate = new DateTime(currentYear, 1, 1);
            DateTime endDate = new DateTime(currentYear, 12, 31);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dateInfo = new MI.Dapper.Data.Models.DateInfo
                {
                    Date = date,
                    DayOfWeekAbbreviation = date.ToString("ddd")
                };

                dateList.Add(dateInfo);
            }

            return dateList;
        }
        private List<MI.Dapper.Data.Models.DateInfo> GetDatesOfSelectionYear(int year)
        {
            var dateList = new List<MI.Dapper.Data.Models.DateInfo>();
            int currentYear = year;

            DateTime startDate = new DateTime(currentYear, 1, 1);
            DateTime endDate = new DateTime(currentYear, 12, 31);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var dateInfo = new MI.Dapper.Data.Models.DateInfo
                {
                    Date = date,
                    DayOfWeekAbbreviation = date.ToString("ddd")
                };

                dateList.Add(dateInfo);
            }

            return dateList;
        }

        public bool UpdateTrangThai(Entity.Models.Product entity)
        {
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    _context.Attach(entity);
                    var entry = _context.Entry(entity);
                    entry.Property(p => p.Status).IsModified = true;
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public bool UpdateSort(Entity.Models.Product entity)
        {
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    _context.Attach(entity);
                    var entry = _context.Entry(entity);
                    entry.Property(p => p.SortOrder).IsModified = true;
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public List<Entity.Models.ProductComponent> GetAllProductComponent(Utils.FilterQuery filter, out int total)
        {
            total = 0;
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    IQueryable<Entity.Models.ProductComponent> items = _context.ProductComponent.Where(x=>x.IsDelete != true);

                    if (!String.IsNullOrEmpty(filter.keyword))
                        items = items.Where(x => x.Name.Contains(filter.keyword));

                    total = items.Count();
                    var result = items.OrderByDescending(x => x.CreatedDate).Skip((filter.pageIndex - 1) * filter.pageSize).Take(filter.pageSize).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

            }
            return new List<Entity.Models.ProductComponent>();
        }

        public List<Entity.Models.ProductComponent> GetAllProductComponent()
        {
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    IQueryable<Entity.Models.ProductComponent> result = _context.ProductComponent.Where(x => x.IsDelete != true);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return new List<Entity.Models.ProductComponent>();
        }

        public Entity.Models.ProductComponent GetProductComponentById(int id)
        {
            var result = new Entity.Models.ProductComponent();
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    result = _context.ProductComponent.Find(id);
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }

        public int UpdateProductComponent(Entity.Models.ProductComponent product)
        {
            try
            {
                using (IDbContext db = new IDbContext())
                {
                    var currentItem = new Entity.Models.ProductComponent();
                    currentItem.Id = product.Id;
                    if (currentItem.Id > 0)
                    {
                        currentItem = db.ProductComponent.Find(currentItem.Id);
                    }
                    currentItem.Name = product.Name;
                    currentItem.IsShow = product.IsShow;

                    if(currentItem.Id > 0)
                    {
                        currentItem.UpdatedDate = DateTime.Now;
                    }
                    else
                    {
                        currentItem.CreatedDate = DateTime.Now;
                        db.ProductComponent.Add(currentItem);
                    }
                    db.SaveChanges();
                    return currentItem.Id;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int DeleteProductComponentById(int id)
        {
            var result = 0;
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    var item = _context.ProductComponent.Find(id);
                    item.IsDelete = true;
                    _context.SaveChanges();
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
    }
}
