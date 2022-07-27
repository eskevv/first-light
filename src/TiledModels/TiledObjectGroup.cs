using FirstLight.TiledModels;

public class TiledObjectGroup
{
   public int Id;
   public float OffsetX;
   public float OffsetY;
   public float ParallaxX;
   public float ParallaxY;
   
   public string Name = default!;

   public string? Class;
   public TiledObject[]? Objects;
   public TiledProperty[]? CustomProperties;
}