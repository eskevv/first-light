namespace FirstLight;

public class TileLayer : Layer
{
   public MapTile this[int index] => _elements[index]; 
   
   private List<MapTile> _elements;
   
   public TileLayer(string name, List<MapTile> elements) : base(name, elements.Count)
   {
      _elements = elements;
   }


}