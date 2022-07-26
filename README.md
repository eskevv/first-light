# FirstLight

FirstLight is a standalone .NET C# library for importing and fully automating all of your work from the Tiled editor.  
*No ID management is needed to get your map shown on the screen.* This also includes animation data.  
All data is saved inside of a MapModel which closeley resembles the original TMX format.

Since FirstLight is a standalone library, it doesn't not rely on any external frameworks or game engines in order to start using it. It still 
ensures that maps can be quickly displayed on the screen with minimal effort, and offers helper methods to be more felxible with it.  

**Tile Structure** 
> Drawing tiles is as simple as pulling data from a collection of a built in type which includes values like:  
> * Image name
> * Tileset SourceCoords
> * World position
> * Animation data

**Working with Layers**  
> Layers provide indexing into tiles and can be toggled on and off
> * Enable and disable layers by their name
> * Hand pick tile or object layers with generic methods
> * Get tiles only from a specific tileset (filtering options)
> * Get all filtered tiles in correct layer order from one list

Finding an object from a layer
```cs
TileLayer ground = map.GetLayer<TileLayer>("ground");
PolygonObject branch = ground.FindByName("Branch");

// or more efficiently if the object has a unique name:
PolygonObject branch = map.FindObject("Branch);
```

### Using FirstLight -example
```cs
// load the map 
TileMap map = MapLoader.Load(filePath);

// disbale / enable layers
map.DisableLayers("background", "water", "trees");
map.EnableLayers("water");

// draw filtered tiles
foreach (var item in map.OrderedTiles)
{
  // pull data
  var texture = GetTexture(item.ImageName);
  var position = new PositionStruct(item.WorldPositionX, item.WorldPositionY);
  var tileSrc = new RectStruct(item.SourceCoords.X, item.SourceCoords.Y, item.Width, item.Height);
  
  // draw the tile
  Draw(texture, position, tileSrc);
}

```

---
### Todo:
* Parallax factors
* Opacity
* Tile flipping
* Offsets
* Alternative data encodings (currently only works with csv layers)
