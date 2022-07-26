using FirstLight.TiledModels;
using FirstLight.Utils;
namespace FirstLight;

public class GameMap
{
   private int Colums => TiledData.Width;
   private int Rows => TiledData.Height;
   private int TileWidth => TiledData.TileWidth;
   private int TileHeight => TiledData.TileHeight;
   private int WorldWidth => TiledData.Width * TiledData.TileWidth;
   private int WorldHeight => TiledData.Height * TiledData.TileHeight;
   private bool IsInfinite => TiledData.Infinite;
   private string RenderOrder => TiledData.RenderOrder;
   private string Orientation => TiledData.Orientation;

   ///<summary>Your specified time step that is used to update the map.</summary>
   public float TimePassed { get; private set; }
   ///<summary>Every renderable tile that is currently enabled on this map by layer order.</summary>
   public List<MapTile> OrderedTiles { get; private set; }
   ///<summary>All data that is associated with the tmx and tsx files.</summary>
   public TiledMap TiledData { get; private set; }

   private readonly List<Layer> _layers;
   private readonly HashSet<Layer> _activeLayers;

   public GameMap(TiledMap mapData)
   {
      TiledData = mapData;
      _layers = LoadLayers();
      _activeLayers = new HashSet<Layer>();
      _layers.ForEach(x => _activeLayers.Add(x));
      OrderedTiles = LoadTiles();
   }

   ///<summary>Updates the map by by the given time.</summary>
   ///<param name="time">The time in seconds that has passed. Note: this value is utomatically converted into to ms for frame animations.</param>
   public void Update(float time)
   {
      TimePassed = time;
      OrderedTiles.ForEach(x => x.Update());
   }

   // --API

   ///<summary>Enable any number of layers by their name. Re-enabling a layer does not have any effect.</summary>
   public void EnableLayers(params string[] layerNames)
   {
      foreach (var item in layerNames)
      {
         Layer subject = _layers.First(x => x.Name == item);
         _activeLayers.Add(subject);
      }
      OrderedTiles = LoadTiles();
   }
   ///<summary>Disable any number of layers by their name. Re-disabling a layer does not have any effect.</summary>
   public void DisableLayers(params string[] layerNames)
   {
      foreach (var item in layerNames)
      {
         Layer subject = _layers.First(x => x.Name == item);
         _activeLayers.Remove(subject);
      }
      OrderedTiles = LoadTiles();
   }
   ///<summary>Returns a layer of type T by the name.</summary>
   public T GetLayer<T>(string name) where T : Layer
   {
      return (T)_layers.First(x => x.Name == name);
   }
   ///<summary>Finds an object of type T in the world(no filtering).</summary>
   public T FindObject<T>(string name) where T : MapObject
   {
      IEnumerable<Layer> layers = _activeLayers.Where(x => x.GetType() == typeof(ObjectLayer));
      List<ObjectLayer> objectLayers = layers.Select(x => (ObjectLayer)x).ToList();
      foreach (var item in objectLayers)
      {
         var t = item.FindByName(name);
         if (t != null) return (T)t;
      }
      throw new FirstLightException("The given object was not found in any object layer.");
   }
   ///<summary>Grab all tiles from this map that appear under only a specific tileset(can be filtered by active layers).</summary>
   public List<MapTile> TilesFromTileset(string imageName, bool layerFilterd = false)
   {
      var tiles = new List<MapTile>();
      foreach (var item in _layers)
      {
         if (item is ObjectLayer) continue;
         var layer = (TileLayer)item;
         for (int x = 0; x < layer.ElementCount; x++)
         {
            if (layer[x].ImageName != imageName) continue;
            if (layerFilterd && !_activeLayers.Contains(_layers[x])) continue;
            tiles.Add(layer[x]);
         }
      }
      return tiles;
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
      foreach (var layer in TiledData.Layers)
      {
         var tiles = new List<MapTile>();
         for (int x = 0; x < layer.Data.Length; x++)
         {
            int gid = layer.Data[x];
            if (gid == 0) continue;
            foreach (var tileset in TiledData.Tilesets)
            {
               tiles.Add(new MapTile(layer.Name, tileset, gid, x, layer.Width, this));
            }
         }
         tileLayers.Add(new TileLayer(layer.Name, tiles));
      }
      return tileLayers;
   }

   private List<ObjectLayer> LoadObjects()
   {
      var objectLayers = new List<ObjectLayer>();
      foreach (var objectGroup in TiledData.ObjectGroups)
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