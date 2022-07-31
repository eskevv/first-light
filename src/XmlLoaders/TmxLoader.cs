using FirstLight.Utils;
using System.Xml.Linq;
namespace FirstLight.Loaders;

public class TmxLoader : TiledLoader
{
   // --Loading TiledMap
   public TiledMap LoadTmx(string filePath)
   {
      if (!File.Exists(filePath)) throw new FirstLightException($"{filePath} not found.");

      if (!filePath.EndsWith(".tmx")) throw new FirstLightException("Unsupported file format");

      XDocument tmxDocument = XDocument.Load(filePath);

      return ParseMap(tmxDocument);
   }

   private TiledMap ParseMap(XDocument document)
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
      tiledMap.Version = mapRoot.Attribute("version")?.Value ?? "0";
      tiledMap.TiledVersion = mapRoot.Attribute("tiledversion")?.Value ?? "0";
      tiledMap.Orientation = mapRoot.Attribute("orientation")?.Value ?? "0";
      tiledMap.RenderOrder = mapRoot.Attribute("renderorder")?.Value ?? "0";
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

      return tiledMap;
   }

   // --Tilesets
   private TiledMapTileset[] ParseMapTilesets(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledMapTileset>();
      foreach (var item in nodes)
      {
         var tileset = new TiledMapTileset();
         tileset.FirstGid = int.Parse(item.Attribute("firstgid")?.Value ?? "0");
         tileset.Source = item.Attribute("source")?.Value ?? "0";
         output.Add(tileset);
      }
      return output.ToArray();
   }

   // --Objects
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
         objectGroup.Name = item.Attribute("name")?.Value ?? "0";
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
         shapeObject.Name = item.Attribute("name")?.Value ?? "0";
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

         output.Add(shapeObject);
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

   // --ImageLayers
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
         imageLayer.Name = item.Attribute("name")?.Value ?? "0";
         imageLayer.Class = item.Attribute("class")?.Value;

         if (properties != null)
         {
            imageLayer.CustomProperties = ParseCustomProperties(properties.Elements("property"));
         }

         output.Add(imageLayer);
      }
      return output.ToArray();
   }

   // --TileLayers
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
         layer.Name = item.Attribute("name")?.Value ?? "0";
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
}
