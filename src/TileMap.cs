using FirstLight.TiledModels;
namespace FirstLight;

public class TileMap
{
   public readonly int Rows;
   public readonly int Columns;
   public readonly bool IsInfinite;
   public readonly string TiledVersion;
   public readonly string Orientation;
   public readonly string RenderOrder;
   public readonly float[] ParallaxOrigin;
   public readonly int[] TileDimensions;
   public readonly TiledProperty[]? CustomProperties;
   public readonly TiledTileLayer[]? TileLayers;
   public readonly TiledObjectGroup[]? ObjectLayers;
   public readonly TiledImageLayer[]? ImageLayers;
   private readonly Dictionary<int, TiledTileset> _tilesets;

   public TileMap(TiledMap map, Dictionary<int, TiledTileset> tilesets)
   {
      Rows = map.Height;
      Columns = map.Width;
      IsInfinite = map.Infinite;
      TiledVersion = map.TiledVersion ?? "0";
      Orientation = map.Orientation ?? "0";
      RenderOrder = map.RenderOrder ?? "0";
      ParallaxOrigin = new[] { map.ParallaxOriginX, map.ParallaxOriginY };
      TileDimensions = new[] { map.TileWidth, map.TileHeight };
      CustomProperties = map.CustomProperties;
      TileLayers = map.TileLayers;
      ObjectLayers = map.ObjectGroups;
      ImageLayers = map.ImageLayers;
      _tilesets = tilesets;
   }

   public KeyValuePair<int, TiledTileset>? GetTilesetPair(int gid)
   {
      var firstGids = _tilesets.Keys;
      for (int x = 0; x < firstGids.Count; x++)
      {
         int key = firstGids.ElementAt(x);
         TiledTileset value = _tilesets[firstGids.ElementAt(x)];

         if (x == firstGids.Count - 1)
         {
            return new KeyValuePair<int, TiledTileset>(key, value);
         }

         int gid1 = firstGids.ElementAt(x + 0);
         int gid2 = firstGids.ElementAt(x + 1);

         if (gid >= gid1 && gid < gid2)
         {
            return new KeyValuePair<int, TiledTileset>(key, value);
         }
      }

      return null;
   }
}