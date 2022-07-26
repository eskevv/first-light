using FirstLight.TiledModels;
namespace FirstLight;

public class MapObject : MapElement
{
   public readonly string? Class;
   public readonly string? Name;

   public MapObject(string layerName, TiledObject tiledObject) : base(layerName)
   {
      WorldPositionX = tiledObject.X;
      WorldPositionY = tiledObject.Y;
      Class = tiledObject.Class;
      Name = tiledObject.Name;
   }
}
