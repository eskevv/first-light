using FirstLight.TiledModels;
using FirstLight.Utils;
namespace FirstLight;

public class PolygonObject : MapObject
{
   public Coordinates[] Points;

   public PolygonObject(string layerName, TiledObject tiledObject) : base(layerName, tiledObject)
   {
      if (tiledObject.Points == null) throw new FirstLightException("The TiledPolygon was not constructed properly.");
      Points = tiledObject.Points.ToCoordinates();
   }
}
