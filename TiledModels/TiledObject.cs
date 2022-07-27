namespace FirstLight.TiledModels;

public class TiledObject
{
   public int Id;
   public string? Name;
   public string? Class;
   public float X;
   public float Y;
   public float Width;
   public float Height;
   public float Rotation;
   public FloatCoords[]? Points;
   public TiledProperty[]? CustomProperties;
   public TiledShapeType Type;
}

public enum TiledShapeType
{
   Point,
   Rectangle,
   Ellipse,
   Polygon
}

public struct FloatCoords
{
   float X;
   float Y;

   public FloatCoords(float x, float y)
   {
      X = x;
      Y = y;
   }
}