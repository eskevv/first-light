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

   /// <summary>Gets the Key, Value pair in { FirstGid, Tileset } format.</summary>
   /// <param name="gid">The tile gid you are searching with.</param>
   /// <returns>If the tileset doesn't exist this returns a KeyValue pair of { -1, null }.</returns>
   public KeyValuePair<int, TiledTileset?> GetTilesetPair(int gid)
   {
      var firstGids = _tilesets.Keys;
      for (int x = 0; x < firstGids.Count; x++)
      {
         int key = firstGids.ElementAt(x);
         TiledTileset value = _tilesets[firstGids.ElementAt(x)];

         if (x == firstGids.Count - 1)
         {
            return new KeyValuePair<int, TiledTileset?>(key, value);
         }

         int gid1 = firstGids.ElementAt(x + 0);
         int gid2 = firstGids.ElementAt(x + 1);

         if (gid >= gid1 && gid < gid2)
         {
            return new KeyValuePair<int, TiledTileset?>(key, value);
         }
      }

      return new KeyValuePair<int, TiledTileset?>(-1, null);
   }

   /// <summary>Gets a LightTile with all the basic tile information.</summary>
   /// <param name="tileGid">The gid for the tile as found inside of the layer data.</param>
   /// <param name="mapIteration">The linear tile location when looping through every tile in the layer.</param>
   /// <param name="layerColumns">The total columns found in the layer.</param>
   public LightTile? GetTileData(int tileGid, int mapIteration, int layerColumns)
   {
      var pair = GetTilesetPair(tileGid);
      TiledTileset? tileset = pair.Value;
      int firstGid = pair.Key;

      if (tileset == null) return null;

      int tileId = tileGid - firstGid;
      return new LightTile(tileId, tileset, mapIteration, layerColumns);
   }
}
