using BookingKTX.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingKTX.APIs
{
    public class MyShop
    {
        public MyShop()
        {
        }

        private string createCodeShop()
        {
            using (DataContext context = new DataContext())
            {
                string code = DataContext.randomString(16);
                while (true)
                {
                    SqlShop? shop = context.shops!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).AsNoTracking().FirstOrDefault();
                    if (shop == null)
                    {
                        break;
                    }
                    code = DataContext.randomString(64);
                }
                return code;
            }
        }
        public async Task<bool> createShopAsync(string token,string name, string? type, byte[] image)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(name))
            {
                return false;
            }
            string codefile = "";

            using (var context = new DataContext())
            {
                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (own_user == null || own_user.role!.code.CompareTo("shipper") == 0)
                {
                    return false;
                }

                SqlShop? shop = new SqlShop();

                shop.code = createCodeShop();
                shop.name = name;
                shop.createdTime = DateTime.Now.ToUniversalTime();
                shop.lastestTime = DateTime.Now.ToUniversalTime();
                if(shop.users == null)
                {
                    shop.users = new List<SqlUser>();
                }
                shop.users.Add(own_user);
                codefile = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), image);
                if (string.IsNullOrEmpty(codefile))
                {
                    return false;
                }
                shop.image = codefile;

                SqlType? m_type = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                if(m_type == null)
                {
                    return false;
                }

                shop.type = m_type;

                if(own_user.shop == null)
                {
                    own_user.shop = shop;
                }

                context.shops!.Add(shop);
                

                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }

        public async Task<bool> editShopAsync(string token, string? code, string name, string? type, byte[]? image)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {

                string codefile = "";
                SqlUser? tmp = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (tmp == null || tmp.role!.code.CompareTo("shipper") == 0)
                {
                    return false;
                }

                if(tmp.role.code.CompareTo("admin") == 0)
                {
                    SqlShop? shop = context.shops!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if(shop == null)
                    {
                        return false;
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        shop.name = name;
                    }

                    if (!string.IsNullOrEmpty(type))
                    {
                        SqlType? tmpType = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                        if (tmpType == null)
                        {
                            return false;
                        }

                        shop.type = tmpType;
                    }

                    if (image != null)
                    {
                        codefile = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), image);
                        if (string.IsNullOrEmpty(codefile))
                        {
                            return false;
                        }
                        shop.image = codefile;
                    }


                }
                else
                {
                    if (tmp.shop == null || tmp.shop.code.CompareTo(code) != 0)
                    {
                        return false;
                    }

                    if (!string.IsNullOrEmpty(name))
                    {
                        tmp.shop.name = name;
                    }

                    if (!string.IsNullOrEmpty(type))
                    {
                        SqlType? tmpType = context.types!.Where(s => s.isdeleted == false && s.code.CompareTo(type) == 0).FirstOrDefault();
                        if (tmpType == null)
                        {
                            return false;
                        }

                        tmp.shop.type = tmpType;
                    }

                    if (image != null)
                    {
                        codefile = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), image);
                        if (string.IsNullOrEmpty(codefile))
                        {
                            return false;
                        }
                        tmp.shop.image = codefile;
                    }
                }
                int rows = await context.SaveChangesAsync();
                return true;
            }
        }


        public async Task<bool> deleteShopAsync(string token, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {


                SqlUser? tmp = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (tmp == null || tmp.role!.code.CompareTo("shipper") == 0)
                {
                    return false;
                }

                if (tmp.shop == null || tmp.shop.code.CompareTo(code) != 0)
                {
                    return false;
                }

                tmp.shop.isdeleted = true;

                int rows = await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> addImageShopAsync(string token, string code, byte[] data)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            using (var context = new DataContext())
            {
                string codefile = "";

                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (own_user == null || own_user.role!.code.CompareTo("shipper") == 0 || own_user.shop == null)
                {
                    return false;
                }

                SqlShop? shop = context.shops!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if (shop == null)
                {
                    return false;
                }
                if (data != null)
                {
                    codefile = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), data);
                    if (string.IsNullOrEmpty(codefile))
                    {
                        return false;
                    }
                    shop.image = codefile;


                }
                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }

        public async Task<bool> deleteImageShopAsync(string token, string code, string data)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            using (var context = new DataContext())
            {

                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.role).FirstOrDefault();
                if (own_user == null || own_user.role!.code.CompareTo("shipper") == 0 || own_user.shop == null)
                {
                    return false;
                }

                SqlShop? shop = context.shops!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0 && s.image.CompareTo(data) == 0).FirstOrDefault();
                if (shop == null)
                {
                    return false;
                }

                shop.image = "";
                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }
        public class itemUser
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";

        }

        public class itemProduct
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public List<string>? image { get; set; }

        }

        public class ShopItem
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string image { get; set; } = "";
            public string des { get; set; } = "";
            public List<itemUser>? users { get; set; }
            public List<itemProduct> products { get; set; } = new List<itemProduct>();
        }

        public List<ShopItem> getListShop()
        {
            try
            {
                List<ShopItem> ret = new List<ShopItem>();

                using (DataContext context = new DataContext())
                {
                    List<SqlShop>? shops = context.shops!.Where(x => !x.isdeleted)
                                                .Include(x => x.users)
                                                .Include(x => x.products)
                                                .ToList();

                    foreach (SqlShop shop in shops)
                    {
                        ShopItem item = new ShopItem();

                        item.code = shop.code;
                        item.name = shop.name;
                        item.image = shop.image;
                        item.des = shop.des;

                        item.users = shop.users!.Select(x => new itemUser
                        {
                            code = x.code,
                            name = x.displayName,
                        }).ToList();

                        item.products = shop.products!.Select(x => new itemProduct
                        {
                            code = x.code,
                            name = x.name,
                            image = x.images
                        }).ToList();

                        ret.Add(item);
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                return new List<ShopItem>();
            }

        }

        public ShopItem getInfoShop (string token)
        {
            using (var context = new DataContext())
            {
                ShopItem tmp = new ShopItem();
                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.shop).ThenInclude(s => s.products).FirstOrDefault();
                if (own_user == null ||  own_user.shop == null)
                {
                    return new ShopItem();
                }

                tmp.code = own_user.shop.code;
                tmp.name = own_user.shop.name;
                tmp.image = own_user.shop.image;

                if(own_user.shop.products != null)
                {
                    foreach (SqlProduct item in own_user.shop.products)
                    {
                        if(item.isdeleted == false)
                        {
                            itemProduct product = new itemProduct();

                            product.code = item.code;
                            product.name = item.name;
                            product.image = item.images;

                            tmp.products.Add(product);
                        }
                    }
                }

                return tmp;

            }
        }
    }
}
