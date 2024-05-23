using BookingKTX.Models;

namespace BookingKTX.APIs
{
    public class MyState
    {
        public MyState()
        {
        }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlState? state = context.states!.Where(s => s.code.CompareTo("new") == 0 && s.isdeleted == false).FirstOrDefault();
                if (state == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "new";
                    item.name = "Mới";
                    //item.phoneNumber = "123456";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                state = context.states!.Where(s => s.code.CompareTo("confirm") == 0 && s.isdeleted == false).FirstOrDefault();
                if (state == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "confirm";
                    item.name = "Đã nhận";
                    //item.phoneNumber = "123456";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                state = context.states!.Where(s => s.code.CompareTo("working") == 0 && s.isdeleted == false).FirstOrDefault();
                if (state == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "working";
                    item.name = "Đang làm";
                    //item.phoneNumber = "123456";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                state = context.states!.Where(s => s.code.CompareTo("shipping") == 0 && s.isdeleted == false).FirstOrDefault();
                if (state == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "shipping";
                    item.name = "Đang giao";
                    //item.phoneNumber = "123456";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                state = context.states!.Where(s => s.code.CompareTo("done") == 0 && s.isdeleted == false).FirstOrDefault();
                if (state == null)
                {
                    SqlState item = new SqlState();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "done";
                    item.name = "Đã hoàn thành";
                    //item.phoneNumber = "123456";
                    item.isdeleted = false;
                    context.states!.Add(item);
                }

                int rows = await context.SaveChangesAsync();
            }

        }
    }
}
