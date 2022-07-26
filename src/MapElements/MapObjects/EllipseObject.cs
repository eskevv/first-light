using FirstLight.TiledModels;
namespace FirstLight;

public class EllipseObject : MapObject
{
   public readonly float Width;
   public readonly float Height;

   public EllipseObject(string layerName, TiledObject tiledObject) : base(layerName, tiledObject)
   {
      Width = tiledObject.Width;
      Height = tiledObject.Height;
   }
}
