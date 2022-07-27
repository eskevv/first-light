namespace FirstLight.TiledModels;

public class TiledObjectGroup
{
   public int Id;
   public string? Name;
   public string? Class;
   public float OffsetX;
   public float OffsetY;
   public float ParallaxX;
   public float ParallaxY;
   public TiledObject[]? Objects;
   public TiledProperty[]? CustomProperties;
}