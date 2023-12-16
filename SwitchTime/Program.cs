using System.Runtime.InteropServices;
namespace SwitchTime
{
    class MainClass
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME time);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetSystemTime(ref SYSTEMTIME time);

        public static void SetData(ref SYSTEMTIME time)
        {
            if (!SetSystemTime(ref time))
            {
                Console.WriteLine("Не удалось установить системное время. Код ошибки: " + Marshal.GetLastWin32Error());
            }
            else
            {
                Console.WriteLine("Системное время успешно установлено.");
            }
        }

        public static void InitData(string dateStr, ref SYSTEMTIME time)
        {
            dateStr = dateStr.Substring(5, 20);

            List<string> strs = (dateStr.Split(' ')).ToList();

            time.wDay = Convert.ToInt16(strs[0]);

            foreach (var item in MonthNumber.dictionary)
            {
                if (item.Key == strs[1])
                {
                    time.wMonth = item.Value;
                    break;
                }
            }

            time.wYear = Convert.ToInt16(strs[2]);

            strs = strs[3].Split(':').ToList();

            time.wHour = Convert.ToInt16(strs[0]);

            time.wMinute = Convert.ToInt16(strs[1]);

            time.wSecond = Convert.ToInt16(strs[2]);
        }

        public static async Task Main(string[] args)
        {
            SYSTEMTIME time = new SYSTEMTIME();

            string dateStr = await HttpRequestData.GetDataByLink("https://www.time100.ru/Moscow");

            InitData(dateStr, ref time);

            SetData(ref time);
        }
    }
}