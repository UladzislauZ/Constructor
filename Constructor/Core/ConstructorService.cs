using System.Drawing;

namespace Core
{
    public class ConstructorService : IConstructorService
    {
        public ConstructorService()
        {
        }

        public bool TryBuildMainRectangle(
            Rectangle[] rectangles, 
            Rectangle declaredRectangle, 
            string[] colorWhiteList, 
            bool onTransformColorWhiteListToBlackList, 
            out Rectangle mainRectangle)
        {
            bool isSave = false;
            double xMin = 0;
            double yMin = 0;
            double xMax = 0;
            double yMax = 0;
            foreach (var rectangle in rectangles)
            {
                if (CheckBlockColor(rectangle.Color, colorWhiteList, onTransformColorWhiteListToBlackList)) { break; }

                if (IsContainsValue(rectangle.BotLeft.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) && 
                    IsContainsValue(rectangle.BotLeft.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y) ||
                    IsContainsValue(rectangle.TopLeft.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) &&
                    IsContainsValue(rectangle.TopLeft.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y) ||
                    IsContainsValue(rectangle.TopRight.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) &&
                    IsContainsValue(rectangle.TopRight.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y) ||
                    IsContainsValue(rectangle.BotRight.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) &&
                    IsContainsValue(rectangle.BotRight.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y))
                {
                    if (!isSave)
                    {
                        xMin = rectangle.BotLeft.X;
                        yMin = rectangle.BotLeft.Y;
                        xMax = rectangle.TopRight.X;
                        yMax = rectangle.TopRight.Y;
                        isSave = true;
                    }
                    else
                    {
                        if (xMin > rectangle.BotLeft.X)
                        {    
                            xMin = rectangle.BotLeft.X;
                        }    
                             
                        if (yMin > rectangle.BotLeft.Y)
                        {    
                            yMin = rectangle.BotLeft.Y;
                        }    
                             
                        if (xMax < rectangle.TopRight.X)
                        {    
                            xMax = rectangle.TopRight.X;
                        }    
                             
                        if (yMax < rectangle.TopRight.Y)
                        {    
                            yMax = rectangle.TopRight.Y;
                        }    
                    }
                }                
            }

            mainRectangle = new(declaredRectangle.Color, new(xMin,yMin), new(xMax,yMax));
            return isSave;
        }

        public bool TryBuildMainRectangleWithoutOverflow(
            Rectangle[] rectangles, 
            Rectangle declaredRectangle, 
            string[] colorWhiteList, 
            bool onTransformColorWhiteListToBlackList, 
            out Rectangle mainRectangle)
        {
            bool isSaveBotLeft = false;
            bool isSaveTopLeft = false;
            bool isSaveTopRight = false;
            bool isSaveBotRight = false;
            double xMin = 0;
            double yMin = 0;
            double xMax = 0;
            double yMax = 0;
            foreach (var rectangle in rectangles)
            {
                if (CheckBlockColor(rectangle.Color, colorWhiteList, onTransformColorWhiteListToBlackList)) { break; }

                if (IsContainsValue(rectangle.BotLeft.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) &&
                    IsContainsValue(rectangle.BotLeft.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y))
                {
                    if (!isSaveBotLeft)
                    {
                        xMin = rectangle.BotLeft.X;
                        yMin = rectangle.BotLeft.Y;
                        isSaveBotLeft = true;
                    }
                    else
                    {
                        if (xMin > rectangle.BotLeft.X)
                        {
                            xMin = rectangle.BotLeft.X;
                        }

                        if (yMin > rectangle.BotLeft.Y)
                        {
                            yMin = rectangle.BotLeft.Y;
                        }
                    }
                }
                if (IsContainsValue(rectangle.TopLeft.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) &&
                    IsContainsValue(rectangle.TopLeft.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y))
                {
                    if (!isSaveTopLeft)
                    {
                        xMin = rectangle.TopLeft.X;
                        yMax = rectangle.TopLeft.Y;
                        isSaveTopLeft = true;
                    }
                    else
                    {
                        if (xMin > rectangle.BotLeft.X)
                        {
                            xMin = rectangle.BotLeft.X;
                        }

                        if (yMax < rectangle.TopRight.Y)
                        {
                            yMax = rectangle.TopRight.Y;
                        }
                    }
                }
                if (IsContainsValue(rectangle.TopRight.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) &&
                    IsContainsValue(rectangle.TopRight.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y))
                {
                    if (!isSaveTopRight)
                    {
                        xMax = rectangle.TopRight.X;
                        yMax = rectangle.TopRight.Y;
                        isSaveTopRight = true;
                    }
                    else
                    {
                        if (xMax < rectangle.TopRight.X)
                        {
                            xMax = rectangle.TopRight.X;
                        }

                        if (yMax < rectangle.TopRight.Y)
                        {
                            yMax = rectangle.TopRight.Y;
                        }
                    }
                }
                if (IsContainsValue(rectangle.BotRight.X, declaredRectangle.BotLeft.X, declaredRectangle.TopRight.X) &&
                    IsContainsValue(rectangle.BotRight.Y, declaredRectangle.BotLeft.Y, declaredRectangle.TopRight.Y))
                {
                    if (!isSaveBotRight)
                    {
                        xMax = rectangle.BotRight.X;
                        yMin = rectangle.BotRight.Y;
                        isSaveBotRight = true;
                    }
                    else
                    {
                        if (xMax < rectangle.TopRight.X)
                        {
                            xMax = rectangle.TopRight.X;
                        }

                        if (yMin > rectangle.BotLeft.Y)
                        {
                            yMin = rectangle.BotLeft.Y;
                        }
                    }
                }
            }

            mainRectangle = new(declaredRectangle.Color, new(xMin, yMin), new(xMax, yMax));
            return isSaveBotLeft && isSaveTopLeft && isSaveTopRight && isSaveBotRight;
        }           

        private bool IsContainsValue(double value, double minValue, double maxValue)
        {
            return value >= minValue && value <= maxValue;
        }

        private bool ValidateMainRectangle(Rectangle rectangle, Rectangle mainRectangle)
        {
            if(rectangle.BotLeft.X != mainRectangle.BotLeft.X || 
               rectangle.BotLeft.Y != mainRectangle.BotLeft.Y ||
               rectangle.TopRight.X != mainRectangle.TopRight.X ||
               rectangle.TopRight.Y != mainRectangle.TopRight.Y)
                return true;
            else
                return false;
        }

        private bool CheckBlockColor(Color color, string[] colorWhiteList, bool onTransformColorWhiteListToBlackList)
        {
            if (colorWhiteList.Length != 0 && onTransformColorWhiteListToBlackList)
            {
                return colorWhiteList.Any(x => Converters.ColorFromString(x) == color) ? true : false;
            }
            else if (colorWhiteList.Length != 0)
            {
                return colorWhiteList.Any(x => Converters.ColorFromString(x) == color) ? false : true;
            }

            return false;
        }
    }
}
