namespace FirstLight.TiledModels;

public class TiledTileset
{
   public string? Version;
   public string? TiledVersion;
   public string? Name;
   public string? Class;
   public int TileWidth;
   public int TileHeight;
   public int TileCount;
   public int Columns;
   public TiledImage? Image;
   public TiledAnimation[]? Animations;
   public TiledProperty[]? CustomProperties;
}

public class TiledImage
{
   public string? Source;
   public int Width;
   public int Height;
}

public class TiledAnimation
{
   public int Id;
   public TiledFrame[]? Frames;
}

public class TiledFrame
{
   public int TiledId;
   public int Duration;
}