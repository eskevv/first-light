using FirstLight.TiledModels;
namespace FirstLight;

public class TiledImageLayer
{
   public int Id;
   public string? Name;
   public string? Class;
   public float OffsetX;
   public float OffsetY;
   public float ParallaxX;
   public float ParallaxY;
   public bool RepeatX;
   public bool RepeatY;
   public TiledProperty[]? CustomProperties;
}