using MI.Dal.IDbContext;
//using MI.Dapper.Data.Models;
using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MI.Entity.Models;

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
                        _newPhienBan.ZoneList = string.Join(",", item.selectedOptions);
                        _new.Add(_newPhienBan);
                    }

                    db.ProductPriceInZoneList.AddRange(_new);
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
