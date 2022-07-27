namespace FirstLight.TiledModels;

public class TiledObject
{
   public int Id;
   public float X;
   public float Y;
   public float Width;
   public float Height;
   public float Rotation;
   public TiledShapeType Type;

   public string Name = default!;
   
   public string? Class;
   public FloatCoords[]? Points;
   public TiledProperty[]? CustomProperties;
}

public enum TiledShapeType
{
   Unknown,
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