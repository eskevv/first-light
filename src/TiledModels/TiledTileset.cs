namespace FirstLight.TiledModels;

public class TiledTileset : TiledModel
{
   public readonly int FirstGid;
   public readonly int TileWidth;
   public readonly int TileHeight;
   public readonly int TileCount;
   public readonly int Columns;
   public readonly string Source;
   public readonly string Name;
   public readonly TiledImage Image;
   public readonly List<TiledAnimation> Animations;

   public TiledTileset(TiledProperty[] properties, TiledImage image, List<TiledAnimation> animations)
   {
      FirstGid = FindIntValue("firstgid", properties);
      TileWidth = FindIntValue("tilewidth", properties);
      TileHeight = FindIntValue("tileheight", properties);
      TileCount = FindIntValue("tilecount", properties);
      Columns = FindIntValue("columns", properties);
      Source = FindStringValue("source", properties);
      Name = FindStringValue("name", properties);

      Image = image;
      Animations = animations;
   }
}