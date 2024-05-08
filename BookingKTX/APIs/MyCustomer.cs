using BookingKTX.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookingKTX.APIs
{
    public class MyCustomer
    {
        public MyCustomer()
        {
        }

        public class InfoUserSystem
        {
            public string user { get; set; } = "";
            public string token { get; set; } = "";
            public string image { get; set; } = "";
        }

        public InfoUserSystem login(string username, string password)
        {
            using (DataContext context = new DataContext())
            {
                SqlCustomer? user = context.customers!.Where(s => s.isdeleted == false && s.username.CompareTo(username) == 0 && s.password.CompareTo(password) == 0)
                    .AsNoTracking().FirstOrDefault();
                if (user == null)
                {
                    return new InfoUserSystem();
                }

                InfoUserSystem info = new InfoUserSystem();
                info.user = user.code;
                info.token = user.token;
                info.image = user.avarta;

                return info;
            }
        }

        private string createCodeCustomer()
        {
            using (DataContext context = new DataContext())
            {
                string code = DataContext.randomString(16);
                while (true)
                {
                    SqlCustomer? shop = context.customers!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).AsNoTracking().FirstOrDefault();
                    if (shop == null)
                    {
                        break;
                    }
                    code = DataContext.randomString(16);
                }
                return code;
            }
        }

        public async Task<bool> createUserAsync(  string username, string password, string displayName, string phoneNumber, string address)
        {
            if ( string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            using (var context = new DataContext())
            {
               

                SqlCustomer? tmp = context.customers!.Where(s => s.isdeleted == false && (s.username.CompareTo(username) == 0) || s.phone.CompareTo(phoneNumber) == 0).FirstOrDefault();
                if (tmp != null)
                {
                    return false;
                }



                SqlCustomer new_user = new SqlCustomer();
                new_user.ID = DateTime.Now.Ticks;
                new_user.code = createCodeCustomer();
                new_user.username = username;
                new_user.password = password;
                new_user.isdeleted = false;
                new_user.name = displayName;
                new_user.address = address;
                new_user.phone = phoneNumber;
                new_user.token = Program.api_user.createToken();

                SqlCart tmpCart = new SqlCart();
                tmpCart.ID = DateTime.Now.Ticks;


                new_user.cart = tmpCart;

                context.customers!.Add(new_user);

                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }

        public async Task<bool> editUserAsync(string code, string password, string displayName, string numberPhone, string address)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
               

                SqlCustomer? tmp = context.customers!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (tmp == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
                {
                    tmp.token = Program.api_user.createToken();
                    tmp.password = password;
                }


                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    tmp.name = displayName;
                }

                if (!string.IsNullOrWhiteSpace(numberPhone))
                {
                    tmp.phone = numberPhone;
                }
                if (!string.IsNullOrWhiteSpace(address))
                {
                    tmp.address = address;
                }

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> editPasswordAsync(string token, string curPass, string newPass)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0 && s.password.CompareTo(curPass) == 0).Include(s => s.role).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(newPass) || string.IsNullOrWhiteSpace(newPass))
                {
                    return false;
                }

                user.token = Program.api_user.createToken();
                user.password = newPass;

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> deleteCustomerAsync(string token, string code)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).AsNoTracking().FirstOrDefault();
                if (own_user == null)
                {
                    return false;
                }
                if(own_user.role!.code.CompareTo("admin") != 0)
                {
                    return false;
                }

                SqlCustomer? tmp = context.customers!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
                if (tmp == null)
                {
                    return false;
                }

                tmp.isdeleted = true;

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> changePassword(string token, string oldPass, string newPass)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0 && s.password.CompareTo(oldPass) == 0).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                m_user.password = newPass;
                m_user.token = Program.api_user.createToken();
                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class ItemCustomer
        {
            public string user { get; set; } = "";
            public string username { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
            public string avatar { get; set; } = "";
            public string address { get; set; } = "";
        }

        public int countCarProduct(string token)
        {
            try
            {
                using (DataContext context = new DataContext())
                {

                    SqlCustomer? user = context.customers!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.cart).ThenInclude(s => s.cartProducts)
                            .FirstOrDefault();

                    if (user != null)
                    {
                        if (user.cart != null && user.cart.cartProducts != null)
                        {
                            return user.cart.cartProducts.Count();
                        }
                        else
                        {
                            return 0;
                        }

                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }

        public ItemCustomer detailCustomer(string code)
        {
            try
            {
                if (string.IsNullOrEmpty(code))
                {
                    return new ItemCustomer();
                }
                using (DataContext context = new DataContext())
                {

                    SqlCustomer? user = context.customers!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0)
                            .FirstOrDefault();

                    if (user != null)
                    {
                        ItemCustomer item = new ItemCustomer();
                        item.user = user.code;
                        item.username = user.username;
                        item.displayName = user.name;
                        item.numberPhone = user.phone;
                        item.avatar = user.avarta;
                        item.address = user.address;
                        return item;

                    }
                    else
                    {
                        return new ItemCustomer();
                    }


                }
            }
            catch (Exception ex)
            {
                Log.Error($"func : listUserByArea -> failed -> Ex: {ex.Message}");
                return new ItemCustomer();
            }

        }

        public List<ItemCustomer> listCustomer(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new List<ItemCustomer>();
                }
                using (DataContext context = new DataContext())
                {

                    List<SqlCustomer> users = context.customers!.Where(s => s.isdeleted == false)
                            .ToList();



                    List<ItemCustomer> items = new List<ItemCustomer>();
                    foreach (SqlCustomer user in users)
                    {
                        ItemCustomer item = new ItemCustomer();
                        item.user = user.code;
                        item.username = user.username;
                        item.displayName = user.name;
                        item.numberPhone = user.phone;
                        item.avatar = user.avarta;
                        item.address = user.address;

                        items.Add(item);
                    }
                    return items;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"func : listUserByArea -> failed -> Ex: {ex.Message}");
                return new List<ItemCustomer>();
            }

        }


    }
}
