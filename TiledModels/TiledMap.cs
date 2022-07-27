namespace FirstLight.TiledModels;

public class TiledMap
{
   public string? Version;
   public string? TiledVersion;
   public string? Class;
   public int Width;
   public int Height;
   public int TileWidth;
   public int TileHeight;
   public bool Infinite;
   public float ParallaxOriginX;
   public float ParallaxOriginY;
   public string? RenderOrder;
   public string? Orientation;
   public TiledTileLayer[]? TileLayers;
   public TiledImageLayer[]? ImageLayers;
   public TiledObjectGroup[]? ObjectGroups;
   public TiledMapTileset[]? Tilesets;
   public TiledProperty[]? CustomProperties;
}

public class TiledMapTileset
{
   public int FirstGid;
   public string? Source;
}
