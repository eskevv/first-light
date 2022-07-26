using System.Xml.Linq;
using FirstLight.TiledModels;
using FirstLight.Utils;
namespace FirstLight;

public static class MapLoader
{
   static void Main(string[] args) { }

   public static GameMap Load(string filePath)
   {
      XDocument tmxMap = XDocument.Load(filePath);

      XElement? mapElement = tmxMap.Element("map");
      if (mapElement == null) throw new FirstLightException("XDocument could not find root element: map");

      TiledProperty[] mapProperties = ParseRootProperties(mapElement);
      List<TiledLayer> mapLayers = CreateLayers(mapElement);
      List<TiledTileset> mapTilesets = CreateTilesets(mapElement, filePath);
      List<TiledObjectGroup> mapObjectGroups = CreateObjectGroups(mapElement);

      var tiledMap = new TiledMap(mapProperties, mapLayers, mapObjectGroups, mapTilesets);
      return new GameMap(tiledMap);
   }

   // --Attribute Parsers

   private static TiledProperty[] ParseRootProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "version", "string"));
      properties.Add(ConstructTiledProperty(element, "tiledversion", "string"));
      properties.Add(ConstructTiledProperty(element, "orientation", "string"));
      properties.Add(ConstructTiledProperty(element, "renderorder", "string"));
      properties.Add(ConstructTiledProperty(element, "width", "int"));
      properties.Add(ConstructTiledProperty(element, "height", "int"));
      properties.Add(ConstructTiledProperty(element, "tilewidth", "int"));
      properties.Add(ConstructTiledProperty(element, "tileheight", "int"));
      properties.Add(ConstructTiledProperty(element, "infinite", "int"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseLayerProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "name", "string"));
      properties.Add(ConstructTiledProperty(element, "width", "int"));
      properties.Add(ConstructTiledProperty(element, "height", "int"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseObjectProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructObjectProperty(element, "x", "float"));
      properties.Add(ConstructObjectProperty(element, "y", "float"));
      properties.Add(ConstructObjectProperty(element, "width", "float"));
      properties.Add(ConstructObjectProperty(element, "height", "float"));
      properties.Add(ConstructObjectProperty(element, "rotation", "float"));
      properties.Add(ConstructObjectProperty(element, "name", "string"));
      properties.Add(ConstructObjectProperty(element, "class", "string"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseObjectGroupProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "name", "string"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseTmxTilesetProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "firstgid", "int"));
      properties.Add(ConstructTiledProperty(element, "source", "string"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseTsxTilesetProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "name", "string"));
      properties.Add(ConstructTiledProperty(element, "tilewidth", "int"));
      properties.Add(ConstructTiledProperty(element, "tileheight", "int"));
      properties.Add(ConstructTiledProperty(element, "tilecount", "int"));
      properties.Add(ConstructTiledProperty(element, "columns", "int"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseImageProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "source", "string"));
      properties.Add(ConstructTiledProperty(element, "width", "int"));
      properties.Add(ConstructTiledProperty(element, "height", "int"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseFrameProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "tileid", "int"));
      properties.Add(ConstructTiledProperty(element, "duration", "int"));
      return properties.ToArray();
   }

   private static TiledProperty[] ParseAnimationProperties(XElement element)
   {
      var properties = new List<TiledProperty>();
      properties.Add(ConstructTiledProperty(element, "id", "int"));
      return properties.ToArray();
   }

   // --Model Factories

   private static List<TiledLayer> CreateLayers(XElement element)
   {
      var tiledLayers = new List<TiledLayer>();
      IEnumerable<XElement> layers = element.Elements("layer");
      foreach (var item in layers)
      {
         TiledProperty[] properties = ParseLayerProperties(item);
         string? data = item.Element("data")?.Value;
         int[] gids = Helpers.EnsureString(data).ToIntArray();
         tiledLayers.Add(new TiledLayer(properties, gids));
      }
      return tiledLayers;
   }

   private static List<TiledTileset> CreateTilesets(XElement element, string rootPath)
   {
      var tilesets = new List<TiledTileset>();
      IEnumerable<XElement> elements = element.Elements("tileset");
      foreach (var item in elements)
      {
         string? tsxSource = item.Attribute("source")?.Value;
         string fullPath = Helpers.EnsureString(tsxSource).CombineWithPath(rootPath);
         XDocument tsxFile = XDocument.Load(fullPath);
         XElement? tsxElement = tsxFile.Element("tileset");
         if (tsxElement == null) throw new FirstLightException("File does not contain a tileset tag.");

         TiledProperty[] tmxProperties = ParseTmxTilesetProperties(item);
         TiledProperty[] tsxProperties = ParseTsxTilesetProperties(tsxElement);
         TiledImage image = CreateTiledImage(tsxElement);
         List<TiledAnimation> animations = CreateTiledAnimations(tsxElement);

         var properties = Helpers.CombineArrays<TiledProperty>(tmxProperties, tsxProperties);
         tilesets.Add(new TiledTileset(properties, image, animations));
      }
      return tilesets;
   }

   private static List<TiledObjectGroup> CreateObjectGroups(XElement element)
   {
      var tiledObjectGroups = new List<TiledObjectGroup>();
      IEnumerable<XElement> objectGroups = element.Elements("objectgroup");
      foreach (var item in objectGroups)
      {
         TiledProperty[] properties = ParseObjectGroupProperties(item);
         List<TiledObject> objects = CreateObjects(item);
         tiledObjectGroups.Add(new TiledObjectGroup(properties, objects));
      }
      return tiledObjectGroups;
   }

   private static List<TiledObject> CreateObjects(XElement element)
   {
      var tiledObjects = new List<TiledObject>();
      IEnumerable<XElement> objects = element.Elements("object");
      foreach (var item in objects)
      {
         TiledProperty[] properties = ParseObjectProperties(item);
         if (!item.HasElements)
         {
            if (item.Attribute("width") == null && item.Attribute("height") == null)
            {
               tiledObjects.Add(new TiledObject(properties, ShapeType.Point));
               continue;
            }
            tiledObjects.Add(new TiledObject(properties, ShapeType.Rectangle));
            continue;
         }
         XElement? ellipse = item.Element("ellipse");
         XElement? polygon = item.Element("polygon");
         if (polygon != null)
         {
            tiledObjects.Add(new TiledObject(properties, ShapeType.Polygon, GetPolygonPoints(polygon)));
         }
         if (ellipse != null)
         {
            tiledObjects.Add(new TiledObject(properties, ShapeType.Ellipse));
         }
      }
      return tiledObjects;
   }

   private static float[] GetPolygonPoints(XElement polygon)
   {
      var points = new List<float>();

      string? value = polygon.Attribute("points")?.Value;
      if (value == null) throw new FirstLightException("Polygon does not contain the required attribute 'points'.");
      string[]? coordinates = value.Split(' ');

      foreach (var item in coordinates)
      {
         string[] coords = item.Split(',');
         points.Add(float.Parse(coords[0]));
         points.Add(float.Parse(coords[1]));
      }

      return points.ToArray();
   }

   private static TiledImage CreateTiledImage(XElement element)
   {
      XElement? image = element.Element("image");
      if (image == null) throw new FirstLightException("There is no image associated with the tileset.");
      TiledProperty[] properties = ParseImageProperties(image);
      return new TiledImage(properties);
   }

   private static List<TiledAnimation> CreateTiledAnimations(XElement element)
   {
      var tiledAnimations = new List<TiledAnimation>();
      IEnumerable<XElement> animations = element.Elements("tile");
      foreach (var item in animations)
      {
         XElement? animationElement = item.Element("animation");
         if (animationElement == null) continue;

         TiledProperty[] properties = ParseAnimationProperties(item);
         List<TiledFrame> frames = CreateTiledFrames(animationElement);
         tiledAnimations.Add(new TiledAnimation(properties, frames));
      }
      return tiledAnimations;
   }

   private static List<TiledFrame> CreateTiledFrames(XElement element)
   {
      var tiledFrames = new List<TiledFrame>();
      IEnumerable<XElement> frames = element.Elements("frame");
      foreach (var item in frames)
      {
         TiledProperty[] properties = ParseFrameProperties(item);
         tiledFrames.Add(new TiledFrame(properties));
      }
      return tiledFrames;
   }

   // --Helpers

   private static TiledProperty ConstructTiledProperty(XElement element, string name, string type)
   {
      string? value = element.Attribute(name)?.Value;
      if (value == null) throw new FirstLightException($"Attribute {name} was not found");
      return new TiledProperty(name, type, value);
   }

   private static TiledProperty ConstructObjectProperty(XElement element, string name, string type)
   {
      string value = element.Attribute(name)?.Value ?? "-1";
      return new TiledProperty(name, type, value);
   }
}