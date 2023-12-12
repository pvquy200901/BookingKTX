using BookingKTX.Models;

namespace BookingKTX.APIs
{
    public class MyRole
    {
        public MyRole()
        {
        }

        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlRole> roles = context.roles!.Where(s => s.code.CompareTo("admin") == 0 && s.isdeleted == false).ToList();
                if (roles.Count <= 0)
                {
                    SqlRole role = new SqlRole();
                    role.ID = DateTime.Now.Ticks;
                    role.code = "admin";
                    role.name = "admin";
                    role.des = "admin";
                    role.isdeleted = false;
                    role.note = "admin";
                    context.roles!.Add(role);
                }

                roles = context.roles!.Where(s => s.code.CompareTo("manager") == 0 && s.isdeleted == false).ToList();
                if (roles.Count <= 0)
                {
                    SqlRole role = new SqlRole();
                    role.ID = DateTime.Now.Ticks;
                    role.code = "manager";
                    role.name = "manager";
                    role.des = "manager";
                    role.isdeleted = false;
                    role.note = "manager";
                    context.roles!.Add(role);
                }

                roles = context.roles!.Where(s => s.code.CompareTo("shipper") == 0 && s.isdeleted == false).ToList();
                if (roles.Count <= 0)
                {
                    SqlRole role = new SqlRole();
                    role.ID = DateTime.Now.Ticks;
                    role.code = "shipper";
                    role.name = "shipper";
                    role.des = "shipper";
                    role.isdeleted = false;
                    role.note = "shipper";
                    context.roles!.Add(role);
                }
                int rows = await context.SaveChangesAsync();
            }
        }
    }
}
