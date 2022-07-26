namespace FirstLight.TiledModels;

public enum ShapeType
{
   Point,
   Rectangle,
   Ellipse,
   Polygon
}

public class TiledObject : TiledModel
{
   public readonly float X;
   public readonly float Y;
   public readonly float Width;
   public readonly float Height;
   public readonly float Rotation;
   public readonly ShapeType Type;

   public readonly string? Name;
   public readonly string? Class;
   public readonly float[]? Points;

   public TiledObject(TiledProperty[] properties, ShapeType type)
   {
      X = FindFloatValue("x", properties);
      Y = FindFloatValue("y", properties);
      Width = FindFloatValue("width", properties);
      Height = FindFloatValue("height", properties);
      Rotation = FindFloatValue("rotation", properties);

      string parsedName = FindStringValue("name", properties);
      Name = parsedName == "-1" ? null : parsedName;
      string parsedClass = FindStringValue("class", properties);
      Class = parsedClass == "-1" ? null : parsedClass;

      Type = type;
   }

   public TiledObject(TiledProperty[] properties, ShapeType type, float[] points) : this(properties, type)
   {
      Points = points;
   }
}