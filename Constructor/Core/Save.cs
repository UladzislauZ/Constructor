using System.Text;

namespace Core
{
    public static class Save
    {
        public static void SaveResult(string data)
        {
            using (FileStream fstream = new FileStream($"{DateTime.Now.Ticks}.txt", FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(data);
                fstream.Write(buffer);
                Console.WriteLine("Текст записан в файл");
            }
        }
    }
}