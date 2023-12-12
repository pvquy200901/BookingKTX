using BookingKTX.Models;
using Serilog;
using System.Runtime.InteropServices;

namespace BookingKTX.APIs
{
    public class MyFile
    {
        public MyFile()
        {
        }

        private string createKey(string file)
        {
            string key = file + "|" + DateTime.Now.Ticks.ToString();
            return String.Concat(key.Select(x => ((int)x).ToString("x")));
        }

        public async Task<string> saveFileAsync(string file, byte[] data)
        {
            using (DataContext context = new DataContext())
            {
                string code = createKey(file);
                string path = "./Data";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string code_file = code + ".file";
                string link_file = "";

                try
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        link_file = Path.Combine(path, code_file);
                    }
                    else
                    {
                        link_file = path + "/" + code_file;
                        //Console.WriteLine("Save file on Linux !!!");
                    }

                    await File.WriteAllBytesAsync(link_file, data);
                }
                catch (Exception ex)
                {
                    code = "";
                    Log.Error(ex.ToString());
                }
                if (string.IsNullOrEmpty(code))
                {
                    return code;
                }
                SqlFile m_file = new SqlFile();
                m_file.ID = DateTime.Now.Ticks;
                m_file.key = code;
                m_file.link = link_file;
                m_file.name = file;
                m_file.time = DateTime.Now.ToUniversalTime();
                context.files!.Add(m_file);
                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return code;
                }
                else
                {
                    return "";
                }
            }
        }


        public byte[]? readFile(string code)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlFile> files = context.files!.Where(s => s.key.CompareTo(code) == 0).ToList();
                if (files.Count <= 0)
                {
                    return null;
                }
                byte[] data = File.ReadAllBytes(files[0].link);
                return data;
            }
        }


        public class MyFileResult
        {
            public string name { get; set; }
            public byte[]? data { get; set; }
        }
        //return bytes and fileName
        public MyFileResult readFileWithName(string code)
        {
            using (DataContext context = new DataContext())
            {
                List<SqlFile> files = context.files!.Where(s => s.key.CompareTo(code) == 0).ToList();
                if (files.Count <= 0)
                {
                    return null;
                }
                if (!File.Exists(files[0].link))
                {
                    return null;
                }
                byte[] data = File.ReadAllBytes(files[0].link);
                string name = files[0].name;

                MyFileResult ret = new MyFileResult();
                ret.name = name;
                ret.data = data;

                return ret;
            }
        }

        public async Task<bool> removeFile(string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlFile? file = context.files!.Where(s => s.key.CompareTo(code) == 0).FirstOrDefault();
                if (file == null)
                {
                    return false;
                }

                try
                {
                    string path = file.link;
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                }
                catch
                {
                    return false;
                }

                context.files!.Remove(file);

                int rows = await context.SaveChangesAsync();
                return rows > 0;
            }
        }
    }
}
