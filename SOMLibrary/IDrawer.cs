namespace SomLibrary
{    
    public interface IDrawer
    {
        void Clear();

        void DrawLine(Point pt1, Point pt2, float penWidth, int penColour);        
        void DrawCircle(Point pt, float radius, float penWidth, int penColour);
        void FillCircle(Point pt, float radius, int paintColour);
        void DrawRectangle(Point bottomLeft, float width, float height, float penWidth, int penColour);
        void FillRectangle(Point bottomLeft, float width, float height, int paintColour);
        void FillPolygon(Point[] pts, int paintColour);
        void DrawText(Point pt, string text, float size, int penColour);
    }
}
