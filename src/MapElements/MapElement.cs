namespace FirstLight;

public abstract class MapElement
{
   public float WorldPositionX { get; init; }
   public float WorldPositionY { get; init; }
   public string ParentLayer { get; init; }

   public MapElement(string parentLayer)
   {
      ParentLayer = parentLayer;
   }
}