using FirstLight.Utils;
namespace FirstLight.TiledModels;

public abstract class TiledModel
{
   protected int FindIntValue(string name, TiledProperty[] properties)
   {
      foreach (var item in properties)
      {
         if (item.Name != name) continue;
         if (item.Type != "int") throw new FirstLightException($"Propery {name} is not an integer type.");
         return int.Parse(item.Value);
      }
      throw new FirstLightException($"Propery {name} not found.");
   }

   protected float FindFloatValue(string name, TiledProperty[] properties)
   {
      foreach (var item in properties)
      {
         if (item.Name != name) continue;
         if (item.Type != "float") throw new FirstLightException($"Propery {name} is not a float type.");
         return float.Parse(item.Value);
      }
      throw new FirstLightException($"Propery {name} not found.");
   }

   protected string FindStringValue(string name, TiledProperty[] properties)
   {
      foreach (var item in properties)
      {
         if (item.Name != name) continue;
         if (item.Type != "string") throw new FirstLightException($"Propery {name} is not a string type.");
         return item.Value;
      }
      throw new FirstLightException($"Propery {name} not found.");
   }
}
