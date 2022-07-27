using FirstLight.TiledModels;
using FirstLight.Loaders;
using FirstLight.Utils;
namespace FirstLight;

public static class MapLoader
{
   public static TileMap Load(string filePath)
   {
      var tiledMap = TmxLoader.LoadTmx(filePath);
      var tilesets = LoadTilesets(tiledMap.Tilesets, filePath);

      return new TileMap(tiledMap, tilesets);
   }

   private static Dictionary<int, TiledTileset> LoadTilesets(TiledMapTileset[]? mapTilesets, string filePath)
   {
      var output = new Dictionary<int, TiledTileset>();

      if (mapTilesets == null) return output;

      foreach (var item in mapTilesets)
      {
         int firstGid = item.FirstGid;
         string source = item.Source ?? "0";
         string fullPath = source.CombineWithPath(filePath);;
         output[firstGid] = TsxLoader.LoadTsx(fullPath);
      }
      return output;
   }
}
