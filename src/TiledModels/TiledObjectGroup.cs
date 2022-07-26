namespace FirstLight.TiledModels;

public class TiledObjectGroup : TiledModel
{
   public readonly string Name;
   public readonly List<TiledObject> Objects;

   public TiledObjectGroup(TiledProperty[] properties, List<TiledObject> objects)
   {
      Name = FindStringValue("name", properties);
      Objects = objects;
   }
}