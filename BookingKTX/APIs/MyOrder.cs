using BookingKTX.Models;
using Microsoft.EntityFrameworkCore;
using static BookingKTX.APIs.MyCustomer;

namespace BookingKTX.APIs
{
    public class MyOrder
    {
        public MyOrder()
        {
        }

        public string generatorcode()
        {
            using (DataContext context = new DataContext())
            {
                string code = DataContext.randomString(16);
                while (true)
                {
                    SqlOrder? tmp = context.orders!.Where(s => s.code.CompareTo(code) == 0).FirstOrDefault();
                    if (tmp == null)
                    {
                        return code;
                    }
                }
            }
        }
        public async Task<bool> createOrderAsync(List<string> idCartProduct, string token, double total, string note, string phone, string address)
        {
            using (var context = new DataContext())
            {

                SqlOrder? order = new SqlOrder();
                order.ID = DateTime.Now.Ticks;
                order.code = generatorcode();

                /*SqlShop? shop = context.shops!.Where(s => s.isdeleted == false && s.code.CompareTo(codeShop) == 0).FirstOrDefault();
                if(shop == null)
                {
                    return false;
                }
                order.shop = shop;*/

                SqlCustomer? customer = context.customers!.Where(s => s.isdeleted == false && (s.token.CompareTo(token) == 0)).FirstOrDefault();
                if (customer == null)
                {
                    return false;
                }
                customer.phone = phone;
                customer.address = address;

                order.customer = customer;
                if (idCartProduct.Count <= 0) 
                {
                    return false;
                }
                foreach (string item in idCartProduct)
                {
                    SqlCartProduct? cartProduct = context.cartProducts!.Where(s => s.isdeleted == false && s.ID == long.Parse(item)).Include(s => s.product).ThenInclude(s => s.shop).FirstOrDefault();
                    if(cartProduct == null)
                    {
                        return false;
                    }
                    if (cartProduct.product != null)
                    {
                        if (cartProduct.product.quantity <= 0)
                        {
                            return false;
                        }
                        order.shop = cartProduct.product.shop;
                        cartProduct.product.totalBuy++;
                        cartProduct.product.quantity = cartProduct.product.quantity - 1;

                    }
                    cartProduct.isFinish = true;

                    if(order.cartProducts == null)
                    {
                        order.cartProducts = new List<SqlCartProduct>();
                    }

                    order.cartProducts.Add(cartProduct);
                    
                }
                order.note = note;
                order.total = total;

                order.createdTime = DateTime.Now.ToUniversalTime();
                order.lastestTime = DateTime.Now.ToUniversalTime();

                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code.CompareTo("new") == 0).FirstOrDefault();
                if(state == null)
                {
                    return false;
                }

                order.state = state;
                SqlLogOrder? tmpLog = new SqlLogOrder();
                tmpLog.ID = DateTime.Now.Ticks;
                tmpLog.order = order;
                tmpLog.state = state;
                tmpLog.time = order.createdTime;

                if(order.logs == null)
                {
                    order.logs = new List<SqlLogOrder>();
                }
                order.logs.Add(tmpLog);

                context.logOrders!.Add(tmpLog);
                context.orders!.Add(order);
                int rows = await context.SaveChangesAsync();
                if(rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> cancelOrderAsync( string token, string code)
        {
            using (var context = new DataContext())
            {
                SqlCustomer? customer = context.customers!.Where(s => s.isdeleted == false && (s.token.CompareTo(token) == 0)).Include(s => s.orders).FirstOrDefault();
                if (customer == null)
                {
                    return false;
                }
                if(customer.orders != null)
                {
                    SqlOrder? tmp = customer.orders.Where(s => s.isFinish == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                    if(tmp == null)
                    {
                        return false;
                    }

                    tmp.isDelete = true;
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

        public class ItemLogOrder
        {
            public long ID { get; set; }
            public string state { get; set; } = "";
            public string user { get; set; } = "";
            public string phone { get; set; } = "";
            public string time { get; set; } = "";
            public string note { get; set; } = "";
        }
        public class ItemOrder
        {
            public string code { get; set; } = "";
            public string shop { get; set; } = "";
            public string state { get; set; } = "";
            public string note { get; set; } = "";
            public string total { get; set; } = "";
            public string time { get; set; } = "";
            public string customer { get; set; } = "";
            public string phone { get; set; } = "";
            public string address { get; set; } = "";
            public List<ItemCartProduct> cartProducts { get; set; } = new List<ItemCartProduct>();
            public List<ItemLogOrder> logs { get; set; } = new List<ItemLogOrder>();
        }

        public List<ItemOrder> getListOrder(string token)
        {
            try
            {
                List<ItemOrder> ret = new List<ItemOrder>();

                using (DataContext context = new DataContext())
                {
                    SqlCustomer? customer = context.customers!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0)
                        .Include(s => s.orders!).ThenInclude(s => s.state)
                        .Include(s => s.orders!).ThenInclude(s => s.cartProducts!).ThenInclude(s => s.product)
                        .Include(s => s.orders!).ThenInclude(s => s.logs!).ThenInclude(s => s.state)
                        .Include(s => s.orders!).ThenInclude(s => s.logs!).ThenInclude(s => s.user)
                        .FirstOrDefault();
                    if(customer == null)
                    {
                        return new List<ItemOrder>();
                    }

                    if(customer.orders != null)
                    {
                        customer.orders = customer.orders.OrderByDescending(s => s.createdTime).ToList();
                        foreach (SqlOrder item in customer.orders)
                        {
                           if(item.isDelete == false && item.isFinish == false)
                            {
                                ItemOrder tmp = new ItemOrder();

                                tmp.code = item.code;
                                if (item.shop != null)
                                {
                                    tmp.shop = item.shop.name;
                                }
                                if (item.state != null)
                                {
                                    tmp.state = item.state.name;
                                }
                                tmp.note = item.note;
                                tmp.time = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                                tmp.total = item.total.ToString();

                                if (item.cartProducts != null)
                                {
                                    foreach (SqlCartProduct cartProduct in item.cartProducts)
                                    {
                                        ItemCartProduct itemCartProduct = new ItemCartProduct();

                                        itemCartProduct.ID = cartProduct.ID.ToString();
                                        itemCartProduct.quantity = cartProduct.quantity;

                                        if (cartProduct.product != null)
                                        {
                                            itemCartProduct.product.code = cartProduct.product.code;
                                            itemCartProduct.product.name = cartProduct.product.name;
                                            itemCartProduct.product.images = cartProduct.product.images;
                                            itemCartProduct.product.price = cartProduct.product.price;
                                        }

                                        tmp.cartProducts.Add(itemCartProduct);
                                    }
                                }

                                if (item.logs != null)
                                {
                                    foreach (SqlLogOrder logOrder in item.logs)
                                    {
                                        ItemLogOrder itemLog = new ItemLogOrder();

                                        itemLog.ID = logOrder.ID;
                                        itemLog.state = logOrder.state!.name;
                                        itemLog.note = logOrder.note;
                                        if(logOrder.user != null)
                                        {
                                            itemLog.user = logOrder.user.displayName;
                                            itemLog.phone = logOrder.user.phoneNumber;
                                        }
                                        itemLog.time = logOrder.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");


                                        tmp.logs.Add(itemLog);
                                    }
                                }
                                ret.Add(tmp);
                            }
                        }
                    }
                    
                    
                }

                return ret;
            }
            catch (Exception ex)
            {
                return new List<ItemOrder>();
            }

        }

        public List<ItemOrder> getListOrderForUser(string token)
        {
            try
            {
                List<ItemOrder> ret = new List<ItemOrder>();

                using (DataContext context = new DataContext())
                {
                    SqlUser? user = context.users!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0)
                        .Include(s => s.role).Include(s => s.shop)
                        .FirstOrDefault();
                    if (user == null)
                    {
                        return new List<ItemOrder>();
                    }

                    if (user.shop == null) {
                        return new List<ItemOrder>();                    
                    }
                    List<SqlOrder>? orders = context.orders!.Include(s => s.shop).Where(s => s.isDelete == false && s.isFinish == false && s.shop != null && s.shop.code.CompareTo(user.shop.code) == 0)
                                                            .Include(s => s.cartProducts!).ThenInclude(s => s.product)
                                                            .Include(s => s.logs!).ThenInclude(s => s.state)
                                                            .Include(s => s.customer).OrderByDescending(s => s.createdTime)
                                                            .ToList();

                    if (orders != null)
                    {
                        foreach (SqlOrder item in orders)
                        {
                            if (item.isDelete == false && item.isFinish == false)
                            {
                                ItemOrder tmp = new ItemOrder();

                                tmp.code = item.code;
                                if (item.shop != null)
                                {
                                    tmp.shop = item.shop.name;
                                }
                                if (item.state != null)
                                {
                                    tmp.state = item.state.name;
                                }
                                tmp.note = item.note;
                                if(item.customer != null)
                                {
                                    tmp.customer = item.customer.name;
                                    tmp.phone = item.customer.phone;
                                    tmp.address = item.customer.address;
                                }
                                tmp.time = item.createdTime.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");
                                tmp.total = item.total.ToString();

                                if (item.cartProducts != null)
                                {
                                    foreach (SqlCartProduct cartProduct in item.cartProducts)
                                    {
                                        ItemCartProduct itemCartProduct = new ItemCartProduct();

                                        itemCartProduct.ID = cartProduct.ID.ToString();
                                        itemCartProduct.quantity = cartProduct.quantity;

                                        if (cartProduct.product != null)
                                        {
                                            itemCartProduct.product.code = cartProduct.product.code;
                                            itemCartProduct.product.name = cartProduct.product.name;
                                            itemCartProduct.product.images = cartProduct.product.images;
                                            itemCartProduct.product.price = cartProduct.product.price;
                                        }

                                        tmp.cartProducts.Add(itemCartProduct);
                                    }
                                }

                                if (item.logs != null)
                                {
                                    foreach (SqlLogOrder logOrder in item.logs)
                                    {
                                        ItemLogOrder itemLog = new ItemLogOrder();

                                        itemLog.ID = logOrder.ID;
                                        itemLog.state = logOrder.state!.name;
                                        itemLog.note = logOrder.note;
                                        if (logOrder.user != null)
                                        {
                                            itemLog.user = logOrder.user.displayName;
                                            itemLog.phone = logOrder.user.phoneNumber;
                                        }
                                        itemLog.time = logOrder.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");


                                        tmp.logs.Add(itemLog);
                                    }
                                }
                                ret.Add(tmp);
                            }
                        }
                    }


                }

                return ret;
            }
            catch (Exception ex)
            {
                return new List<ItemOrder>();
            }

        }
        public List<ItemOrder> getListFinishOrder(string token)
        {
            try
            {
                List<ItemOrder> ret = new List<ItemOrder>();

                using (DataContext context = new DataContext())
                {
                    SqlCustomer? customer = context.customers!.Where(s => s.isdeleted == false && s.token.CompareTo(token) == 0)
                                           .Include(s => s.orders!).ThenInclude(s => s.state)
                                           .Include(s => s.orders!).ThenInclude(s => s.cartProducts!).ThenInclude(s => s.product)
                                           .Include(s => s.orders!).ThenInclude(s => s.logs!).ThenInclude(s => s.state)
                                           .Include(s => s.orders!).ThenInclude(s => s.logs!).ThenInclude(s => s.user)
                                           .FirstOrDefault(); 
                    if (customer == null)
                    {
                        return new List<ItemOrder>();
                    }

                    if (customer.orders != null)
                    {
                        foreach (SqlOrder item in customer.orders)
                        {
                            if (item.isDelete == false && item.isFinish == true)
                            {
                                ItemOrder tmp = new ItemOrder();

                                tmp.code = item.code;
                                if (item.shop != null)
                                {
                                    tmp.shop = item.shop.name;
                                }
                                if (item.state != null)
                                {
                                    tmp.state = item.state.name;
                                }
                                tmp.note = item.note;
                                tmp.total = item.total.ToString();

                                if (item.cartProducts != null)
                                {
                                    foreach (SqlCartProduct cartProduct in item.cartProducts)
                                    {
                                        ItemCartProduct itemCartProduct = new ItemCartProduct();

                                        itemCartProduct.ID = cartProduct.ID.ToString();
                                        itemCartProduct.quantity = cartProduct.quantity;

                                        if (cartProduct.product != null)
                                        {
                                            itemCartProduct.product.code = cartProduct.product.code;
                                            itemCartProduct.product.name = cartProduct.product.name;
                                            itemCartProduct.product.images = cartProduct.product.images;
                                            itemCartProduct.product.price = cartProduct.product.price;
                                        }

                                        tmp.cartProducts.Add(itemCartProduct);
                                    }
                                }

                                if (item.logs != null)
                                {
                                    foreach (SqlLogOrder logOrder in item.logs)
                                    {
                                        ItemLogOrder itemLog = new ItemLogOrder();

                                        itemLog.ID = logOrder.ID;
                                        itemLog.state = logOrder.state!.name;
                                        itemLog.note = logOrder.note;
                                        if (logOrder.user != null)
                                        {
                                            itemLog.user = logOrder.user.displayName;
                                            itemLog.phone = logOrder.user.phoneNumber;
                                        }
                                        itemLog.time = logOrder.time.ToLocalTime().ToString("dd-MM-yyyy HH:mm:ss");


                                        tmp.logs.Add(itemLog);
                                    }
                                }
                                ret.Add(tmp);
                            }
                        }
                    }


                }

                return ret;
            }
            catch (Exception ex)
            {
                return new List<ItemOrder>();
            }

        }


        public async Task<bool> confirmOrderAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {


                SqlOrder? tmp = context.orders!.Where(s => s.code.CompareTo(code) == 0 && s.isDelete == false && s.isFinish == false).Include(s => s.logs).Include(s => s.state).FirstOrDefault();
                if (tmp == null)
                {
                    return false;
                }
                if(tmp.state!.code.CompareTo("new") != 0)
                {
                    return false;
                }

                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code.CompareTo("confirm") == 0).FirstOrDefault();
                if(state == null)
                {
                    return false;
                }

                tmp.state = state;

                SqlLogOrder? tmpLog = new SqlLogOrder();
                tmpLog.ID = DateTime.Now.Ticks;
                tmpLog.order = tmp;
                tmpLog.state = state;
                tmpLog.time = DateTime.Now.ToUniversalTime();

                if (tmp.logs == null)
                {
                    tmp.logs = new List<SqlLogOrder>();
                }
                tmp.logs.Add(tmpLog);

                context.logOrders!.Add(tmpLog);

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

        public async Task<bool> setStateOrderAsync(string? user, string code, string codeState)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {


                SqlOrder? tmp = context.orders!.Where(s => s.code.CompareTo(code) == 0 && s.isDelete == false && s.isFinish == false).Include(s => s.logs).Include(s => s.state).FirstOrDefault();
                if (tmp == null)
                {
                    return false;
                }
                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code.CompareTo(codeState) == 0).FirstOrDefault();
                if (state == null)
                {
                    return false;
                }

                if (tmp.state!.code.CompareTo("new") == 0)
                {
                    return false;
                }

                tmp.state = state;

                

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    SqlLogOrder? tmpLog = new SqlLogOrder();
                    tmpLog.ID = DateTime.Now.Ticks;
                    tmpLog.order = tmp;
                    tmpLog.state = state;
                    SqlUser? mUser = context.users!.Where(s => s.code.CompareTo(user) == 0 && s.isdeleted == false).FirstOrDefault();
                    if (mUser != null)
                    {
                        tmpLog.user = mUser;
                    }
                    tmpLog.time = DateTime.Now.ToUniversalTime();

                    if (tmp.logs == null)
                    {
                        tmp.logs = new List<SqlLogOrder>();
                    }
                    tmp.logs.Add(tmpLog);

                    context.logOrders!.Add(tmpLog);

                    await context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> finishOrderAsync(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }
            using (DataContext context = new DataContext())
            {


                SqlOrder? tmp = context.orders!.Where(s => s.code.CompareTo(code) == 0 && s.isDelete == false && s.isFinish == false).Include(s => s.logs).FirstOrDefault();
                if (tmp == null)
                {
                    return false;
                }

                SqlState? state = context.states!.Where(s => s.isdeleted == false && s.code.CompareTo("done") == 0).FirstOrDefault();
                if (state == null)
                {
                    return false;
                }

                tmp.state = state;
                tmp.isFinish = true;

                SqlLogOrder? tmpLog = new SqlLogOrder();
                tmpLog.ID = DateTime.Now.Ticks;
                tmpLog.order = tmp;
                tmpLog.state = state;
                tmpLog.time = DateTime.Now.ToUniversalTime();

                if (tmp.logs == null)
                {
                    tmp.logs = new List<SqlLogOrder>();
                }
                tmp.logs.Add(tmpLog);

                context.logOrders!.Add(tmpLog);

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
