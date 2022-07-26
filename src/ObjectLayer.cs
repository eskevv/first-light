namespace FirstLight;

public class ObjectLayer : Layer
{
   private List<MapObject> _elements;

   public ObjectLayer(string name, List<MapObject> elements) : base(name, elements.Count)
   {
      _elements = elements;
   }

   /// <summary>Every object of type T belonging to this layer.</summary>
   public List<T> AllOfType<T>() where T : MapObject
   {
      IEnumerable<MapObject> ofType = _elements.Where(x => x.GetType() == typeof(T));
      return ofType.Select(x => (T)x).ToList();
   }

   /// <summary>The first object in this layer with arguement name.</summary>
   public MapObject FindByName(string name)
   {
      return _elements.First(x => x.Name == name);
   }
}