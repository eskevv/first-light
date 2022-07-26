namespace FirstLight.TiledModels;

public class TiledMap : TiledModel
{
   public readonly int Width;
   public readonly int Height;
   public readonly int TileWidth;
   public readonly int TileHeight;
   public readonly bool Infinite;
   public readonly string RenderOrder;
   public readonly string Orientation;
   public readonly List<TiledLayer> Layers;
   public readonly List<TiledObjectGroup> ObjectGroups;
   public readonly List<TiledTileset> Tilesets;

   public TiledMap(TiledProperty[] properties, List<TiledLayer> tileLayers, List<TiledObjectGroup> objectGroups, List<TiledTileset> tilesets)
   {
      Width = FindIntValue("width", properties);
      Height = FindIntValue("height", properties);
      TileWidth = FindIntValue("tilewidth", properties);
      TileHeight = FindIntValue("tileheight", properties);
      Infinite = FindIntValue("infinite", properties) == 1;
      RenderOrder = FindStringValue("renderorder", properties);
      Orientation = FindStringValue("orientation", properties);

      Layers = tileLayers;
      ObjectGroups = objectGroups;
      Tilesets = tilesets;
   }
}