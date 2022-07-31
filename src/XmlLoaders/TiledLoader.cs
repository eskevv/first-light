using System.Xml.Linq;
namespace FirstLight.Loaders;

public abstract class TiledLoader
{
   protected TiledProperty[] ParseCustomProperties(IEnumerable<XElement> nodes)
   {
      var output = new List<TiledProperty>();
      foreach (var item in nodes)
      {
         var property = new TiledProperty();
         property.Name = item.Attribute("name")?.Value ?? "0";
         property.Type = item.Attribute("type")?.Value ?? "0";
         property.Value = item.Attribute("value")?.Value ?? "0";
         output.Add(property);
      }
      return output.ToArray();
   }
}