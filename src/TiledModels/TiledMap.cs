namespace FirstLight;

public class TiledMap
{
   public int Width;
   public int Height;
   public int TileWidth;
   public int TileHeight;
   public bool Infinite;
   public float ParallaxOriginX;
   public float ParallaxOriginY;

   public string Version = default!;
   public string TiledVersion = default!;
   public string RenderOrder = default!;
   public string Orientation = default!;
   
   public string? Class;
   public TiledTileLayer[]? TileLayers;
   public TiledImageLayer[]? ImageLayers;
   public TiledObjectGroup[]? ObjectGroups;
   public TiledMapTileset[]? Tilesets;
   public TiledProperty[]? CustomProperties;
}