namespace FirstLight.TiledModels;

public class TiledTileLayer
{
   public int Id;
   public string? Name;
   public string? Class;
   public float OffsetX;
   public float OffsetY;
   public float ParallaxX;
   public float ParallaxY;
   public int Width;
   public int Height;
   public TiledLayerData LayerData = default!;
   public TiledProperty[]? CustomProperties;
}

public class TiledLayerData
{
   public int[]? Gids;
   public TiledChunk[]? Chunks;
}