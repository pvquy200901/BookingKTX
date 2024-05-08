using BookingKTX.Models;
using Microsoft.EntityFrameworkCore;
using static BookingKTX.APIs.MyShop;
using static System.Net.Mime.MediaTypeNames;

namespace BookingKTX.APIs
{
    public class MyProduct
    {
        public MyProduct()
        {
        }

        public async Task<bool> createProductAsync(string token, string code, string name, decimal price, int quantity, List<byte[]> data)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(code))
            {
                return false;
            }

            using (var context = new DataContext())
            {
                string codefile = "";
                List<string> images = new List<string>();

                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (own_user == null || own_user.role!.code.CompareTo("shipper") == 0 || own_user.shop == null)
                {
                    return false;
                }

                SqlProduct? product = context.products!.Include(s => s.shop).Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0 && s.shop != null && s.shop.code.CompareTo(own_user.shop.code) == 0).FirstOrDefault();
                if (product != null)
                {
                    return false;
                }
                else
                {
                    product = new SqlProduct();
                    product.code = code;
                    product.name = name;
                    product.price = price;
                    product.quantity = quantity;
                   // product.shop = own_user.shop;

                    foreach (byte[] item in data)
                    {
                        if (item != null)
                        {
                            codefile = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), item);
                            if (string.IsNullOrEmpty(codefile))
                            {
                                return false;
                            }
                            images.Add(codefile);
                        }
                    }

                    product.images = images;
                    if(own_user.shop != null)
                    {
                        product.shop = own_user.shop;
                    }
                }

                context.products!.Add(product);


                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }

        public async Task<bool> editProductAsync(string token, string code, string name, decimal price, int quantity )
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {


                SqlUser? tmp = context.users!.Where(s => s.token.CompareTo(token) == 0 && s.isdeleted == false).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (tmp == null || tmp.role!.code.CompareTo("shipper") == 0 )
                {
                    return false;
                }

                if (tmp.shop == null )
                {
                    return false;
                }

                SqlProduct? product = context.products!.Include(s => s.shop).Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0 && s.shop != null && s.shop.code.CompareTo(tmp.shop.code) == 0).FirstOrDefault();
                if(product == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    product.name = name;
                }

                product.price = price;
                product.quantity = quantity;

                int rows = await context.SaveChangesAsync();
                return true;
            }
        }


        public async Task<bool> deleteProductAsync(string token, string code)
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

                if (tmp.shop == null)
                {
                    return false;
                }
                SqlProduct? product = context.products!.Include(s => s.shop).Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0 && s.shop != null && s.shop.code.CompareTo(tmp.shop.code) == 0).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }

                product.isdeleted = true;

                int rows = await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> addImageProductAsync(string token, string code, List<byte[]> data)
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

                SqlProduct? product = context.products!.Include(s => s.shop).Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0 && s.shop != null && s.shop.code.CompareTo(own_user.shop.code) == 0).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }
                foreach (byte[] item in data)
                {
                    if (item != null)
                    {
                        codefile = await Program.api_file.saveFileAsync(DateTime.Now.Ticks.ToString(), item);
                        if (string.IsNullOrEmpty(codefile))
                        {
                            return false;
                        }
                        if(product.images == null)
                        {
                            product.images = new List<string>();
                        }
                        product.images.Add(codefile);

                    }
                }


                context.products!.Add(product);


                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }

        public async Task<bool> deleteImageProductAsync(string token, string code, string data)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            using (var context = new DataContext())
            {

                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (own_user == null || own_user.role!.code.CompareTo("shipper") == 0 || own_user.shop == null)
                {
                    return false;
                }

                SqlProduct? product = context.products!.Include(s => s.shop).Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0 && s.shop != null && s.shop.code.CompareTo(own_user.shop.code) == 0).FirstOrDefault();
                if (product == null)
                {
                    return false;
                }

                product.images!.Remove(data);

                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }

        public class ItemProduct
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public decimal price { get; set; }
            public string quantity { get; set; } = "";
            public List<string> images { get; set; } = new List<string>();
        }

        public List<ItemProduct> getListProduct(string shop)
        {
            try
            {
                List<ItemProduct> ret = new List<ItemProduct>();

                using (DataContext context = new DataContext())
                {
                    List<SqlProduct>? products = context.products!.Include(s => s.shop).Where(x => x.shop != null && x.shop.code.CompareTo(shop) == 0 && !x.isdeleted)
                                                .ToList();

                    foreach (SqlProduct product in products)
                    {
                        ItemProduct item = new ItemProduct();

                        item.code = product.code;
                        item.name = product.name;
                        item.price = product.price;
                        item.quantity = product.quantity.ToString();
                        if (product.images != null)
                        {
                            item.images = product.images;
                        }
                        ret.Add(item);
                    }
                }

                return ret;
            }
            catch (Exception ex)
            {
                return new List<ItemProduct>();
            }

        }

        public ItemProduct getDetailProduct(string shop)
        {
            try
            {
                ItemProduct item = new ItemProduct();


                using (DataContext context = new DataContext())
                {
                    SqlProduct? product = context.products!.Where( x => x.code.CompareTo(shop) == 0 && !x.isdeleted)
                                                .FirstOrDefault();

                    if(product != null)
                    {

                        item.code = product.code;
                        item.name = product.name;
                        item.price = product.price;
                        item.quantity = product.quantity.ToString();
                        if (product.images != null)
                        {
                            item.images = product.images;
                        }
                    }
                }

                return item;
            }
            catch (Exception ex)
            {
                return new ItemProduct();
            }

        }
        public async Task<bool> addToCart(string token, string product)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            using (var context = new DataContext())
            {

                try{
                    SqlCustomer? own_user = context.customers!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.cart).ThenInclude(s => s.cartProducts).FirstOrDefault();
                    if (own_user == null || own_user.cart == null)
                    {
                        return false;
                    }

                    SqlProduct? m_product = context.products!.Include(s => s.shop).Where(s => s.isdeleted == false && s.code.CompareTo(product) == 0).FirstOrDefault();
                    if (product == null)
                    {
                        return false;
                    }

                   // own_user.cart.products!.Add(m_product);

                    int rows = await context.SaveChangesAsync();
                    return rows > 0;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

    }
}
