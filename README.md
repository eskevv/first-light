# FirstLight

FirstLight is a standalone .NET C# library for importing and fully automating all of your work from the Tiled editor.  
*No ID management is needed to get your map shown on the screen.* This also means animations.
All data is saved inside of a MapModel which closeley resembles the original TMX format, but you interact with it through the ```GameMap``` class.

Since FirstLight is a standalone library, it doesn't rely on any external frameworks or game engines in order to use it. It still 
ensures that maps can be quickly displayed on the screen with minimal effort, and the API provides methods to be more felxible with how tiles are rendered.

Note: It is up to you to store the actual textures, (preferably to cache them by tileset name). As a standalone library, FirstLight doesn't do any of the
actual rendering, but provides an interface to all of the necessary tmx data. This versatility means you can use it with popular game engines like Unity3D, Godot, or CryEngine.

**Tile Structure** 
> Drawing tiles is as simple as pulling data from a collection of a built in type which includes values like:  
> * Image name
> * Tileset SourceCoords
> * World position
> * Animation data

**Working with Layers**  
> Layers provide indexing into tiles and can be toggled on and off
> * Enable and disable layers by their name
> * Hand pick tile, image, and object layers with generic methods
> * Get tiles that appear under only a specific tileset (with filtering options)
> * Get all filtered and correctly ordered tile from a single list  

Finding an object:
```cs
TileLayer ground = map.GetLayer<TileLayer>("ground");
PolygonObject branch = ground.FindByName("Branch");

// or more efficiently if the object has a unique name:
PolygonObject branch = map.FindObject("Branch");
```

It's also possible to find any object in the given map using Linq, but FirstLight has the task of doing this so you won't
have to cast it back to the type you are looking for. It offers similar methods for finding any object or layer by name, class or type.

**Animated Tiles**
> FirstLight does all the tile animations for you but can also provide the frame data if you wish to do all the updating.  
> In the update loop you can call the map instance's ``Update`` that takes in how many seconds have passed(delta):  
```cs
// this will update your game tiles and layer scrolling if it has any parallax effects
map.Update(time);

// then in the draw call (varies by rendering methods):
Draw(texture, tilePosition, tileSource);
```

---
### Installation
**Option 1**: With Nuget Package Manager:  
```
dotnet add package firstlight 
```
**Option 2**: (Source code will usually be a bit more up to date)  

Clone or download the repo into a dir of choice and add to your .csproj file in your own project:
```xml
<ItemGroup>
  <ProjectReference Include = "<path to FirstLight>\FirstLight.csproj" />
</ItemGroup>
```
---
### Using FirstLight to Render all Tiles
Initialize and configure your ```GameMap```:
```cs
// load the map
GameMap map = MapLoader.Load(filePath);

// disbale / enable layers (all are enabled by default)
map.DisableLayers("background", "water", "trees");
map.EnableLayers("water");
```
The ```Update``` loop:
```cs
// animations and parallax scrolling updates
_map.Update(dt);

// draw all tiles with filtering applied
foreach (var item in map.OrderedTiles)
{
  // pull data
  var texture = GetTexture(item.ImageName); // or where ever you you are storing your textures
  var position = new PositionStruct(item.WorldPositionX, item.WorldPositionY); // example struct
  var tileSrc = new RectStruct(item.SourceX, item.SourceY, item.Width, item.Height); // example struct

  // draw the tile (very generic method here)
  batch.Draw(texture, position, tileSrc);
}
```
---
### Building And Version Support
You need .NET 6 to start using First light as it was built with a more modern syntax that may not be suppored by earlier versions of C#.

Tiled should work with most if not all versions of Tiled up to it's latest stable release v1.9.
You can also read what package supports your version of Tiled. But it's recommended that you use the lastest stable one.

---
### Contribution
Feel free to open up an issue or request any improvements to the project. If you have any plan to modify the code please open up an issue first with the details on what you would like to change. You may also currently do a pull request directly from the main branch as it is also still the development branch.

---
### License
MIT
