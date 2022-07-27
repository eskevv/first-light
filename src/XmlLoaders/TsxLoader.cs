using FirstLight.TiledModels;
using FirstLight.Utils;
using System.Xml.Linq;
namespace FirstLight.Loaders;

public class TsxLoader : TiledLoader
{
   // --Loading Tileset
   public TiledTileset LoadTsx(string filePath)
   {
      if (!File.Exists(filePath)) throw new FirstLightException($"{filePath} not found.");

      if (!filePath.EndsWith(".tsx")) throw new FirstLightException("Unsupported file format");

      XDocument tsxDocument = XDocument.Load(filePath);

      return ParseTileset(tsxDocument);
   }

   private TiledTileset ParseTileset(XDocument document)
   {
      XElement? tilesetRoot = document.Element("tileset");

      if (tilesetRoot == null) throw new FirstLightException("This tsx file is not parseable.");

      XElement? properties = tilesetRoot.Element("properties");
      IEnumerable<XElement> tiles = tilesetRoot.Elements("tile");
      var tileset = new TiledTileset();
      tileset.Name = tilesetRoot.Attribute("name")?.Value ?? "0";
      tileset.Class = tilesetRoot.Attribute("class")?.Value ?? "0";
      tileset.Version = tilesetRoot.Attribute("version")?.Value ?? "0";
      tileset.TiledVersion = tilesetRoot.Attribute("tiledversion")?.Value ?? "0";
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

   // --Animations

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

   // --TiledImage

   private TiledImage ParseTiledimage(XElement element)
   {
      var output = new TiledImage();
      output.Source = element.Attribute("source")?.Value ?? "0";
      output.Width = int.Parse(element.Attribute("width")?.Value ?? "0");
      output.Height = int.Parse(element.Attribute("height")?.Value ?? "0");
      return output;
   }
}