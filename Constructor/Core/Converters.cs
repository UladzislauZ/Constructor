using System.Drawing;
using System.Globalization;

namespace Core
{
    public static class Converters
    {
        public static Color ColorFromString(string input)
        {
            switch (input) 
            {
                case "серый":return Color.Gray;
                case "желтый": return Color.Yellow;
                case "красный": return Color.Red;
                case "оранжевый": return Color.Orange;
                case "зеленый": return Color.Green;
                case "синий":
                default: 
                    {
                        Console.WriteLine("Цвет не зарегестрирован. По дефолту выставлен синий");
                        return Color.Blue; 
                    }
            }
        }

        public static double ConvertToDouble(string input)
        {
            return double.Parse(input, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}
