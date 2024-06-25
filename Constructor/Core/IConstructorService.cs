namespace Core
{
    public interface IConstructorService
    {
        bool TryBuildMainRectangle(
            Rectangle[] rectangles, 
            Rectangle declaredRectangle, 
            string[] colorWhiteList, 
            bool onTransformColorWhiteListToBlackList, 
            out Rectangle mainRectangle);
        bool TryBuildMainRectangleWithoutOverflow(
            Rectangle[] rectangles, 
            Rectangle declaredRectangle, 
            string[] colorWhiteList, 
            bool onTransformColorWhiteListToBlackList, 
            out Rectangle mainRectangle);
    }
}
