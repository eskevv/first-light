namespace FirstLight;

public class TiledTileLayer
{
   public int Id;
   public float OffsetX;
   public float OffsetY;
   public float ParallaxX;
   public float ParallaxY;
   public int Width;
   public int Height;

   public string Name = default!;
   public TiledLayerData LayerData = default!;
   
   public string? Class;
   public TiledProperty[]? CustomProperties;
}
