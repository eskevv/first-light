namespace FirstLight.TiledModels;

public class TiledProperty
{
   public readonly string Name;
   public readonly string Type;
   public readonly string Value;

   public TiledProperty(string name, string type, string value)
   {
      Name = name;
      Type = type;
      Value = value;
   }
}