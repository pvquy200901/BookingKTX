using BookingKTX.Models;

namespace BookingKTX.APIs
{
    public class MyType
    {
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                List<SqlType> types = context.types!.Where(s => s.code.CompareTo("food") == 0 && s.isdeleted == false).ToList();
                if (types.Count <= 0)
                {
                    SqlType type = new SqlType();
                    type.ID = DateTime.Now.Ticks;
                    type.code = "food";
                    type.name = "Đồ ăn";
                    type.isdeleted = false;
                    context.types!.Add(type);
                }

               
                int rows = await context.SaveChangesAsync();
            }
        }
    }
}
