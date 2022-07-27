using FirstLight.TiledModels;
using FirstLight.Loaders;
namespace FirstLight;

public static class MapLoader
{
   public static TileMap Load(string filePath)
   {
      var tiledMap = TmxLoader.LoadTmx(filePath);
      var tilesets = LoadTilesets(tiledMap.Tilesets);

      return new TileMap(tiledMap, tilesets);
   }

   private static Dictionary<int, TiledTileset> LoadTilesets(TiledMapTileset[]? mapTilesets)
   {
      var output = new Dictionary<int, TiledTileset>();

      if (mapTilesets == null) return output;

      foreach (var item in mapTilesets)
      {
         int firstGid = item.FirstGid;
         string? source = item.Source ?? "0";
         output[firstGid] = TsxLoader.LoadTsx(source);
      }
      return output;
   }
}
