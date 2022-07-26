using FirstLight.TiledModels;
namespace FirstLight;

public class RectangleObject : MapObject
{
   public readonly float Width;
   public readonly float Height;

   public RectangleObject(string layerName, TiledObject tiledObject) : base(layerName, tiledObject)
   {
      Width = tiledObject.Width;
      Height = tiledObject.Height;
   }
}
