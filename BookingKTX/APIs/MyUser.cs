using BookingKTX.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookingKTX.APIs
{
    public class MyUser
    {
        public MyUser()
        {
        }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.code.CompareTo("admin") == 0 && s.isdeleted == false).FirstOrDefault();
                if (user == null)
                {
                    SqlUser item = new SqlUser();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "admin";
                    item.username = "admin";
                    item.password = "123456";
                    item.role = context.roles!.Where(s => s.isdeleted == false && s.code.CompareTo("admin") == 0).FirstOrDefault();
                    item.token = "1234567890";
                    item.displayName = "admin";
                    //item.phoneNumber = "123456";
                    item.isdeleted = false;
                    context.users!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }

        }

        public class InfoUserSystem
        {
            public string user { get; set; } = "";
            public string token { get; set; } = "";
            public string role { get; set; } = "";
        }

        public InfoUserSystem login(string username, string password)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.username.CompareTo(username) == 0 && s.password.CompareTo(password) == 0)
                    .Include(s => s.role)
                    .AsNoTracking().FirstOrDefault();
                if (user == null)
                {
                    return new InfoUserSystem();
                }

                InfoUserSystem info = new InfoUserSystem();
                info.user = user.code;
                info.token = user.token;
                info.role = user.role!.code;

                return info;
            }
        }

        public long checkUser(string token)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (user == null)
                {
                    return -1;
                }

                return user.ID;
            }
        }
        public string createToken()
        {
            using (DataContext context = new DataContext())
            {
                string token = DataContext.randomString(64);
                while (true)
                {
                    SqlUser? user = context.users!.Where(s => s.token.CompareTo(token) == 0).AsNoTracking().FirstOrDefault();
                    if (user == null)
                    {
                        break;
                    }
                    token = DataContext.randomString(64);
                }
                return token;
            }
        }

        public long checkRoles(string token, string[] roles)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (user == null)
                {
                    return -1;
                }
                if (user.role?.code != null && roles.Contains(user.role.code))
                {
                    return user.ID;
                }

                return -1;
            }
        }


        public long checkSystem(string token)
        {
            using (DataContext context = new DataContext())
            {
                SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (user == null)
                {
                    return -1;
                }
                if (user.role!.code.CompareTo("shipper") == 0)
                {
                    return -1;
                }
                return user.ID;
            }
        }
        public async Task<bool> createUserAsync(string token, string code, string username, string password, string displayName, string phoneNumber, string role)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }

            using (var context = new DataContext())
            {
                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).Include(s => s.shop).FirstOrDefault();
                if (own_user == null || own_user.role!.code.CompareTo("shipper") == 0)
                {
                    return false;
                }

                if (own_user.role!.code.CompareTo("manager") == 0 && role.CompareTo("admin") == 0)
                {
                    return false;
                }

                SqlUser? tmp = context.users!.Where(s => s.isdeleted == false && (s.code.CompareTo(code) == 0 || s.username.CompareTo(username) == 0)).Include(s => s.shop).FirstOrDefault();
                if (tmp != null)
                {
                    return false;
                }

                SqlRole? m_role = context.roles!.Where(s => s.isdeleted == false && s.code.CompareTo(role) == 0).FirstOrDefault();
                if (m_role == null)
                {
                    return false;
                }

                SqlUser new_user = new SqlUser();
                new_user.ID = DateTime.Now.Ticks;
                new_user.code = code;
                new_user.username = username;
                new_user.password = password;
                new_user.role = m_role;
                new_user.isdeleted = false;
                new_user.displayName = displayName;
                new_user.phoneNumber = phoneNumber;
                new_user.token = createToken();
                if(own_user.role.code.CompareTo("manager") == 0 && own_user.shop != null)
                {
                    new_user.shop = own_user.shop;
                }

                context.users!.Add(new_user);

                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }

        public async Task<bool> editUserAsync(string token, string code, string password, string displayName, string numberPhone)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (own_user == null)
                {
                    return false;
                }

                SqlUser? tmp = context.users!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).Include(s => s.role).FirstOrDefault();
                if (tmp == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(password) && !string.IsNullOrWhiteSpace(password))
                {
                    tmp.token = createToken();
                    tmp.password = password;
                }


                if (!string.IsNullOrWhiteSpace(displayName))
                {
                    tmp.displayName = displayName;
                }

                if (!string.IsNullOrWhiteSpace(numberPhone))
                {
                    tmp.phoneNumber = numberPhone;
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

        public async Task<bool> updateLatLongUserAsync(string token, string latitude, string longitude)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {
                SqlUser? own_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (own_user == null)
                {
                    return false;
                }
                own_user.latitude = latitude;
                own_user.longitude = longitude;

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

                user.token = createToken();
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

        public async Task<bool> deleteUserAsync(string token, string code)
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

                SqlUser? tmp = context.users!.Where(s => s.code.CompareTo(code) == 0 && s.isdeleted == false).FirstOrDefault();
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
                m_user.token = createToken();
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

        public class ItemUser
        {
            public string user { get; set; } = "";
            public string username { get; set; } = "";
            public string displayName { get; set; } = "";
            public string numberPhone { get; set; } = "";
            public string des { get; set; } = "";
            public string roleName { get; set; } = "";
        }

        public List<ItemUser> listUser(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new List<ItemUser>();
                }
                using (DataContext context = new DataContext())
                {
                   
                    List<SqlUser> users = context.users!.Where(s => s.isdeleted == false)
                            .Include(s => s.role)
                            .ToList();



                    List<ItemUser> items = new List<ItemUser>();
                    foreach (SqlUser user in users)
                    {
                        ItemUser item = new ItemUser();
                        item.user = user.code;
                        item.username = user.username;
                        item.displayName = user.displayName;
                        item.numberPhone = user.phoneNumber;
                        item.roleName = user.role!.name;

                        items.Add(item);
                    }
                    return items;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"func : listUserByArea -> failed -> Ex: {ex.Message}");
                return new List<ItemUser>();
            }

        }

        public List<ItemUser> listUserForShop(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new List<ItemUser>();
                }
                using (DataContext context = new DataContext())
                {
                    List<ItemUser> items = new List<ItemUser>();

                    SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) ==0).Include(s => s.shop).ThenInclude(s => s.users!).ThenInclude(s => s.role)
                            .FirstOrDefault();

                    if (m_user == null)
                    {
                        return new List<ItemUser>();
                    }
                    if (m_user.shop != null)
                    {
                        if (m_user.shop.users != null)
                        {
                            foreach (SqlUser user in m_user.shop.users)
                            {
                                ItemUser item = new ItemUser();
                                item.user = user.code;
                                item.username = user.username;
                                item.displayName = user.displayName;
                                item.numberPhone = user.phoneNumber;
                                item.roleName = user.role!.name;

                                items.Add(item);
                            }
                        }
                    }

                    return items;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"func : listUserByArea -> failed -> Ex: {ex.Message}");
                return new List<ItemUser>();
            }

        }
        public List<ItemUser> listUserShipper(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new List<ItemUser>();
                }
                using (DataContext context = new DataContext())
                {
                    List<ItemUser> items = new List<ItemUser>();

                    SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.shop).ThenInclude(s => s.users!).ThenInclude(s => s.role)
                            .FirstOrDefault();

                    if(m_user == null)
                    {
                        return new List<ItemUser>();
                    }
                    if(m_user.shop != null)
                    {
                        if(m_user.shop.users != null)
                        {
                            foreach (SqlUser user in m_user.shop.users)
                            {
                                if(user.role!.code.CompareTo("shipper") == 0)
                                {
                                    ItemUser item = new ItemUser();
                                    item.user = user.code;
                                    item.username = user.username;
                                    item.displayName = user.displayName;
                                    item.numberPhone = user.phoneNumber;
                                    item.roleName = user.role!.name;

                                    items.Add(item);
                                }
                            }
                        }
                    }

                    return items;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"func : listUserByArea -> failed -> Ex: {ex.Message}");
                return new List<ItemUser>();
            }

        }

        public async Task<bool> chooseShipper(string token, string shipper, string order)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(shipper) || string.IsNullOrEmpty(order))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {
                SqlUser? m_user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0).Include(s => s.role).FirstOrDefault();
                if (m_user == null)
                {
                    return false;
                }

                if (m_user.role!.code.CompareTo("shipper") == 0)
                {
                    return false;
                }

                SqlUser? m_shipper = context.users!.Where(s => s.isdeleted == false && s.code.CompareTo(shipper) == 0).FirstOrDefault();
                if(m_shipper == null)
                {
                    return false;
                }

                await Program.api_order.setStateOrderAsync(shipper, order, "shipping");

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
    }
}
