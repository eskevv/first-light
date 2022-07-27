using System.Xml.Linq;
using FirstLight.TiledModels;
using FirstLight.Utils;
namespace FirstLight;

public class TiledLoader
{
   public TiledMap Map;
   public Dictionary<int, TiledTileset> Tilesets;

   public TiledLoader()
   {
      Tilesets = new Dictionary<int, TiledTileset>();
      Map = new TiledMap();
   }

   public void Load(string filePath)
   {
      if (!File.Exists(filePath)) throw new FirstLightException($"{filePath} not found.");

      if (!filePath.EndsWith(".tmx")) throw new FirstLightException("Unsupported file format");

      XDocument tmxDocument = XDocument.Load(filePath);
      
      ParseMap(tmxDocument);
      ParseTilesets(Map.Tilesets);
   }

   private void ParseTilesets(TiledMapTileset[]? mapTilesets)
   {
      if (mapTilesets == null) return;

      foreach (var item in mapTilesets)
      {
         int firstGid = item.FirstGid;
         string? source = item.Source ?? "0";
         Tilesets[firstGid] = ParseTileset(source);
      }
   }

   private TiledTileset ParseTileset(string filePath)
   {
      if (!File.Exists(filePath)) throw new FirstLightException($"{filePath} not found.");

      if (!filePath.EndsWith(".tsx")) throw new FirstLightException("Unsupported file format");

      XDocument tsxDocument = XDocument.Load(filePath);
      XElement? tilesetRoot = tsxDocument.Element("tileset");

      if (tilesetRoot == null) throw new FirstLightException("This tsx file is not parseable.");

      XElement? properties  = tilesetRoot.Element("properties");
      IEnumerable<XElement> tiles  = tilesetRoot.Elements("tile");
      var tileset  = new TiledTileset();
      tileset.Name = tilesetRoot.Attribute("name")?.Value;
      tileset.Class = tilesetRoot.Attribute("class")?.Value;
      tileset.Version = tilesetRoot.Attribute("version")?.Value;
      tileset.TiledVersion = tilesetRoot.Attribute("tiledversion")?.Value;
      tileset.TileWidth = int.Parse(tilesetRoot.Attribute("tilewidth")?.Value ?? "0");
      tileset.TileHeight = int.Parse(tilesetRoot.Attribute("tileheight")?.Value ?? "0");
      tileset.TileCount = int.Parse(tilesetRoot.Attribute("tilecount")?.Value ?? "0");
      tileset.Columns = int.Parse(tilesetRoot.Attribute("columns")?.Value ?? "0");
      tileset.Image = ParseTiledimage(tilesetRoot);
      tileset.Animations = ParseTiledAnimations(tiles);

      if (properties != null)
      {
         tileset.CustomProperties = ParseCustomProperties(properties.Elements("property"));
      }

      return tileset;
   }  

   private TiledAnimation[]? ParseTiledAnimations(IEnumerable<XElement> nodes)
   {
      if (nodes.Count() == 0) return null;
      var output = new List<TiledAnimation>();

      foreach (var item in nodes)
      {
         XElement? animationElement = item.Element("animation");
         if (animationElement == null) throw new FirstLightException("Tileset animation does not specify animation tag.");
         TiledAnimation animation = new TiledAnimation();
         animation.Frames = ParseFrames(animationElement.Elements("frame"));
         animation.Id = int.Parse(item.Attribute("id")?.Value ?? "0");
         output.Add(animation);
      }
      return output.ToArray();
   }

   private TiledFrame[] ParseFrames(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledFrame>();
      foreach (var item in nodes)
      {
         var frame = new TiledFrame();
         frame.TiledId = int.Parse(item.Attribute("tileid")?.Value ?? "0");
         frame.Duration = int.Parse(item.Attribute("duration")?.Value ?? "0");
         output.Add(frame);
      }

      return output.ToArray();
   }

   private TiledImage ParseTiledimage(XElement element)
   {
      var output = new TiledImage();
      output.Source = element.Attribute("source")?.Value;
      output.Width = int.Parse(element.Attribute("width")?.Value ?? "0");
      output.Height = int.Parse(element.Attribute("height")?.Value ?? "0");
      return output;
   }

   #region TmxFile

   private void ParseMap(XDocument document)
   {
      XElement? mapRoot = document.Element("map");
      if (mapRoot == null) throw new FirstLightException("This tmx file is not parseable.");

      XElement? properties = mapRoot.Element("properties");
      IEnumerable<XElement> tilesets = mapRoot.Elements("tileset");
      IEnumerable<XElement> tileLayers = mapRoot.Elements("layer");
      IEnumerable<XElement> imagelayers = mapRoot.Elements("imagelayer");
      IEnumerable<XElement> objectgroups = mapRoot.Elements("objectgroup");

      var tiledMap = new TiledMap();
      tiledMap.Width = int.Parse(mapRoot.Attribute("width")?.Value ?? "0");
      tiledMap.Height = int.Parse(mapRoot.Attribute("height")?.Value ?? "0");
      tiledMap.TileWidth = int.Parse(mapRoot.Attribute("tilewidth")?.Value ?? "0");
      tiledMap.TileHeight = int.Parse(mapRoot.Attribute("tileheight")?.Value ?? "0");
      tiledMap.Version = mapRoot.Attribute("version")?.Value;
      tiledMap.TiledVersion = mapRoot.Attribute("tiledversion")?.Value;
      tiledMap.Orientation = mapRoot.Attribute("orientation")?.Value;
      tiledMap.RenderOrder = mapRoot.Attribute("renderorder")?.Value;
      tiledMap.Infinite = (mapRoot.Attribute("infinite")?.Value == "1");
      tiledMap.Class = mapRoot.Attribute("class")?.Value;
      tiledMap.ParallaxOriginX = int.Parse(mapRoot.Attribute("parallaxoriginx")?.Value ?? "0");
      tiledMap.ParallaxOriginY = int.Parse(mapRoot.Attribute("parallaxoriginy")?.Value ?? "0");
      tiledMap.Tilesets = ParseMapTilesets(tilesets);
      tiledMap.TileLayers = ParseTileLayers(tileLayers);
      tiledMap.ImageLayers = ParseImageLayers(imagelayers);
      tiledMap.ObjectGroups = ParseObjectGroups(objectgroups);

      if (properties != null)
      {
         tiledMap.CustomProperties = ParseCustomProperties(properties.Elements("property"));
      }

      Map = tiledMap;
   }

   // Tilesets

   private TiledMapTileset[] ParseMapTilesets(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledMapTileset>();
      foreach (var item in nodes)
      {
         var tileset = new TiledMapTileset();
         tileset.FirstGid = int.Parse(item.Attribute("firstgid")?.Value ?? "0");
         tileset.Source = item.Attribute("source")?.Value;
         output.Add(tileset);
      }
      return output.ToArray();
   }

   // Objects

   private TiledObjectGroup[] ParseObjectGroups(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledObjectGroup>();
      foreach (var item in nodes)
      {
         XElement? properties = item.Element("properties");
         IEnumerable<XElement> objectElements = item.Elements("object");
         var objectGroup = new TiledObjectGroup();
         objectGroup.Id = int.Parse(item.Attribute("id")?.Value ?? "0");
         objectGroup.OffsetX = float.Parse(item.Attribute("offsetx")?.Value ?? "0");
         objectGroup.OffsetY = float.Parse(item.Attribute("offsety")?.Value ?? "0");
         objectGroup.ParallaxX = float.Parse(item.Attribute("parallaxx")?.Value ?? "0");
         objectGroup.ParallaxY = float.Parse(item.Attribute("parallaxy")?.Value ?? "0");
         objectGroup.Name = item.Attribute("name")?.Value;
         objectGroup.Class = item.Attribute("class")?.Value;
         objectGroup.Objects = ParseObjects(objectElements);

         if (properties != null)
         {
            objectGroup.CustomProperties = ParseCustomProperties(properties.Elements("property"));
         }
         output.Add(objectGroup);
      }
      return output.ToArray();
   }

   private TiledObject[]? ParseObjects(IEnumerable<XElement> nodes)
   {
      if (nodes.Count() == 0) return null;

      var output = new List<TiledObject>();
      foreach (var item in nodes)
      {
         XElement? properties = item.Element("properties");
         XElement? ellipse = item.Element("ellipse");
         XElement? polygon = item.Element("polygon");
         var shapeObject = new TiledObject();
         shapeObject.Id = int.Parse(item.Attribute("id")?.Value ?? "0");
         shapeObject.X = float.Parse(item.Attribute("x")?.Value ?? "0");
         shapeObject.Y = float.Parse(item.Attribute("y")?.Value ?? "0");
         shapeObject.Width = float.Parse(item.Attribute("width")?.Value ?? "0");
         shapeObject.Height = float.Parse(item.Attribute("height")?.Value ?? "0");
         shapeObject.Rotation = float.Parse(item.Attribute("rotation")?.Value ?? "0");
         shapeObject.Name = item.Attribute("name")?.Value;
         shapeObject.Class = item.Attribute("class")?.Value;

         if (ellipse != null)
         {
            shapeObject.Type = TiledShapeType.Ellipse;
         }
         if (polygon != null)
         {
            shapeObject.Type = TiledShapeType.Polygon;
            shapeObject.Points = ParsePolygonPoints(polygon);
         }
         if (properties != null)
         {
            shapeObject.CustomProperties = ParseCustomProperties(properties.Elements("property"));
         }
      }
      return output.ToArray();
   }

   private FloatCoords[] ParsePolygonPoints(XElement node)
   {
      var output = new List<FloatCoords>();
      string value = node.Attribute("points")?.Value ?? "0";
      string[] points = value.Split(' ');
      foreach (var item in points)
      {
         float pointX = float.Parse(item.Split(',')[0]);
         float pointY = float.Parse(item.Split(',')[1]);
         var point = new FloatCoords(pointX, pointY);
         output.Add(point);
      }
      return output.ToArray();
   }

   // ImageLayers

   private TiledImageLayer[] ParseImageLayers(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledImageLayer>();
      foreach (var item in nodes)
      {
         var imageLayer = new TiledImageLayer();
         XElement? properties = item.Element("properties");
         imageLayer.Id = int.Parse(item.Attribute("id")?.Value ?? "0");
         imageLayer.OffsetX = float.Parse(item.Attribute("offsetx")?.Value ?? "0");
         imageLayer.OffsetY = float.Parse(item.Attribute("offsety")?.Value ?? "0");
         imageLayer.ParallaxX = float.Parse(item.Attribute("parallaxx")?.Value ?? "0");
         imageLayer.ParallaxY = float.Parse(item.Attribute("parallaxy")?.Value ?? "0");
         imageLayer.Name = item.Attribute("name")?.Value;
         imageLayer.Class = item.Attribute("class")?.Value;

         if (properties != null)
         {
            imageLayer.CustomProperties = ParseCustomProperties(properties.Elements("property"));
         }

         output.Add(imageLayer);
      }
      return output.ToArray();
   }

   // TileLayers

   public TiledTileLayer[] ParseTileLayers(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledTileLayer>();
      foreach (var item in nodes)
      {
         XElement? properties = item.Element("properties");
         XElement? data = item.Element("data");
         var layer = new TiledTileLayer();
         layer.Id = int.Parse(item.Attribute("id")?.Value ?? "0");
         layer.Width = int.Parse(item.Attribute("width")?.Value ?? "0");
         layer.Height = int.Parse(item.Attribute("height")?.Value ?? "0");
         layer.OffsetX = float.Parse(item.Attribute("offsetx")?.Value ?? "0");
         layer.OffsetY = float.Parse(item.Attribute("offsety")?.Value ?? "0");
         layer.ParallaxX = float.Parse(item.Attribute("parallaxx")?.Value ?? "0");
         layer.ParallaxY = float.Parse(item.Attribute("parallaxy")?.Value ?? "0");
         layer.Name = item.Attribute("name")?.Value;
         layer.Class = item.Attribute("class")?.Value;

         if (data != null)
         {
            layer.LayerData = ParseLayerData(data);
         }
         if (properties != null)
         {
            layer.CustomProperties = ParseCustomProperties(properties.Elements("property"));
         }
         output.Add(layer);
      }
      return output.ToArray();
   }

   private TiledLayerData ParseLayerData(XElement node)
   {
      string? gids = node.Value;
      IEnumerable<XElement> nodes = node.Elements("chunk");
      var output = new TiledLayerData();
      output.Gids = gids.ToIntArray();
      output.Chunks = ParseChunkData(nodes);
      return output;
   }

   private TiledChunk[]? ParseChunkData(IEnumerable<XElement> nodes)
   {
      if (nodes.Count() == 0) return null;

      var output = new List<TiledChunk>();
      foreach (var item in nodes)
      {
         var chunk = new TiledChunk();
         chunk.X = int.Parse(item.Attribute("x")?.Value ?? "0");
         chunk.Y = int.Parse(item.Attribute("y")?.Value ?? "0");
         chunk.Width = int.Parse(item.Attribute("width")?.Value ?? "0");
         chunk.Height = int.Parse(item.Attribute("height")?.Value ?? "0");
         chunk.Data = item.Value.ToIntArray();
         output.Add(chunk);
      }
      return output.ToArray();
   }

   // CustomProperties

   private TiledProperty[] ParseCustomProperties(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledProperty>();
      foreach (var item in nodes)
      {
         var property = new TiledProperty();
         property.Name = item.Attribute("name")?.Value;
         property.Type = item.Attribute("type")?.Value;
         property.Value = item.Attribute("value")?.Value;
         output.Add(property);
      }
      return output.ToArray();
   }

   #endregion
}
