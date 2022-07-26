using FirstLight.TiledModels;
using FirstLight.Utils;
namespace FirstLight;

public class GameMap
{
   public int Colums => _mapData.Width;
   public int Rows => _mapData.Height;
   public int TileWidth => _mapData.TileWidth;
   public int TileHeight => _mapData.TileHeight;
   public int WorldWidth => _mapData.Width * _mapData.TileWidth;
   public int WorldHeight => _mapData.Height * _mapData.TileHeight;
   public bool IsInfinite => _mapData.Infinite;
   public string RenderOrder => _mapData.RenderOrder;
   public string Orientation => _mapData.Orientation;

   public List<MapTile> OrderedTiles { get; private set; }

   private readonly TiledMap _mapData;
   private readonly List<Layer> _layers;
   private readonly HashSet<Layer> _activeLayers;

   public GameMap(TiledMap mapData)
   {
      _mapData = mapData;
      _layers = LoadLayers();
      _activeLayers = new HashSet<Layer>();
      _layers.ForEach(x => _activeLayers.Add(x));
      OrderedTiles = LoadTiles();
   }

   // --API

   public void EnableLayers(params string[] layerNames)
   {
      foreach (var item in layerNames)
      {
         Layer subject = _layers.First(x => x.Name == item);
         _activeLayers.Add(subject);
      }
      OrderedTiles = LoadTiles();
   }

   public void DisableLayers(params string[] layerNames)
   {
      foreach (var item in layerNames)
      {
         Layer subject = _layers.First(x => x.Name == item);
         _activeLayers.Remove(subject);
      }
      OrderedTiles = LoadTiles();
   }

   public T GetLayer<T>(string name) where T : Layer
   {
      return (T)_layers.First(x => x.Name == name);
   }

   // --Private Methods

   private List<MapTile> LoadTiles()
   {
      var tiles = new List<MapTile>();
      foreach (var layer in _activeLayers)
      {
         if (layer is ObjectLayer) continue;
         var tileLayer = (TileLayer)layer;
         for (int x = 0; x < tileLayer.ElementCount; x++)
         {
            tiles.Add(tileLayer[x]);
         }
      }
      return tiles;
   }

   private List<Layer> LoadLayers()
   {
      var layers = new List<Layer>();
      layers.AddRange(LoadTileLayers());
      layers.AddRange(LoadObjects());
      return layers;
   }

   private List<TileLayer> LoadTileLayers()
   {
      var tileLayers = new List<TileLayer>();
      foreach (var layer in _mapData.Layers)
      {
         var tiles = new List<MapTile>();
         for (int x = 0; x < layer.Data.Length; x++)
         {
            int gid = layer.Data[x];
            if (gid == 0) continue;
            foreach (var tileset in _mapData.Tilesets)
            {
               tiles.Add(new MapTile(layer.Name, tileset, gid, x, layer.Width));
            }
         }
         tileLayers.Add(new TileLayer(layer.Name, tiles));
      }
      return tileLayers;
   }

   private List<ObjectLayer> LoadObjects()
   {
      var objectLayers = new List<ObjectLayer>();
      foreach (var objectGroup in _mapData.ObjectGroups)
      {
         var objects = new List<MapObject>();
         foreach (var item in objectGroup.Objects)
         {
            MapObject qualified = QualifyObject(item, objectGroup.Name);
            objects.Add(qualified);
         }
         objectLayers.Add(new ObjectLayer(objectGroup.Name, objects));
      }
      return objectLayers;
   }

   private MapObject QualifyObject(TiledObject item, string layerName)
   {
      return item.Type switch
      {
         ShapeType.Ellipse => new EllipseObject(layerName, item),
         ShapeType.Point => new PointObject(layerName, item),
         ShapeType.Rectangle => new RectangleObject(layerName, item),
         ShapeType.Polygon => new PolygonObject(layerName, item),
         _ => throw new FirstLightException($"Shape type '{item.Type}' is not accepted.")
      };
   }
}