namespace Core
{
    public class Rectangle
    {
        public System.Drawing.Color Color { get; set; }
        public Point BotLeft { get; set; }
        public Point TopLeft { get; set; }
        public Point TopRight { get; set; }
        public Point BotRight { get; set; }

        public Rectangle(string color, Point botLeft, Point topRight)
        {
            Color = Converters.ColorFromString(color);
            BotLeft = botLeft;
            BotRight = new Point(topRight.X, botLeft.Y);
            TopLeft = new Point(botLeft.X, topRight.Y);
            TopRight = topRight;
        }
        public Rectangle(System.Drawing.Color color, Point botLeft, Point topRight)
        {
            Color = color;
            BotLeft = botLeft;
            BotRight = new Point(topRight.X, botLeft.Y);
            TopLeft = new Point(botLeft.X, topRight.Y);
            TopRight = topRight;
        }
    }
}
