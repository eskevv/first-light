namespace FirstLight.TiledModels;

public class TiledTileset
{
   public int TileWidth;
   public int TileHeight;
   public int TileCount;
   public int Columns;

   public string Name = default!;
   public string Version = default!;
   public string TiledVersion = default!;
   public TiledImage Image = default!;
   
   public string? Class;
   public TiledAnimation[]? Animations;
   public TiledProperty[]? CustomProperties;
}