namespace FirstLight.TiledModels;

public class TiledGroup
{
   public int Id;
   public float OffsetX;
   public float OffsetY;
   public float ParallaxX;
   public float ParallaxY;
   
   public string Name = default!;

   public string? Class;
   public TiledProperty[]? CustomProperties;
}