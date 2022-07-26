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

**Animated Tiles**
> FirstLight does all the tile animations by itself but can also provide the frame data if you wish to do all the updating.  
> After loading the map you can specify a fixed timestep for your animations:  
```cs
map.SetTimeStep(60);
```
> However this is inconsistent if you know your frame rate will vary and therefore only optional.  
> Instead in your game ```update``` method:
```cs
// takes in a float that indicates how much time has passed
map.Update(time);

// then in the draw call (varies by rendering methods):
Draw(texture, tile.Position, tile.Source);
```

---
### Installation
1) With Nuget Package Manager:  
-this will automatically add the latest package to your project
```
dotnet add package firstlight 
```
2) Clone or download the repo into a dir of choice.
Then add to your .csproj file in your own project:
```xml
<ItemGroup>
  <ProjectReference Include = "<path to FirstLight\FirstLight.csproj>" />
</ItemGroup>
```

---
### Using FirstLight to Render all Tiles
```cs
// load the map 
TileMap map = MapLoader.Load(filePath);

// disbale / enable layers (all are enabled by default)
map.DisableLayers("background", "water", "trees");
map.EnableLayers("water");

// draw all tiles with filtering applied
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
### Features Not Implemented Yet
* Parallax factors
* Opacity
* Tile flipping

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
