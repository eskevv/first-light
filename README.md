# FirstLight

Developer branch is a retake on the parsing proccess, with the goal of being more optimized before an official release.  
Some features were removed as they seemed unstable or not useful.

FirstLight is a standalone .NET C# library for importing and fully automating all of your work from the Tiled editor.  
*No ID management is needed to get your map shown on the screen.*
All data is saved inside of a ```TileMap``` class which closeley resembles the original TMX format.

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
> * Hand pick tile, image, and object layers with generic methods

Finding most data throughout the ```TileMap``` instance  is best done with Linq.  
Example of finding all objects under a class for specific layers:
```cs
// first create a list to store what you need
var objectsList = new List<TiledObject>();

// filter layers
IEnumerable<TiledObjectGroup> skyObjects = map.ObjectLayers.Where(x => x.Class == "sky-objects");

// loop through selected layers and look for all objects with class 'planes'
foreach (var item in skyObjects)
{
   if (item.Objects == null) continue; // some layers won't have any objects
   var planes = item.Objects.Where(x => x.Class == "planes");
   objectsList.AddRange(planes);
}

```
It is also pretty easy to render all your tiles out of the box. There are several methods depending if you are making your own tile objects.
The more out of the box method:
```cs
// grab the tile layers you want to render
IEnumerable<TiledTileLayer> vegetation = map.TileLayers.Where(x => x.Class == "vegetation");

// loop though each vegetation layer (usually you will render everything at once)
foreach (var item in vegetation)
{
   // layer data depends on if your using infinite maps
   if (item.LayerData.Gids == null) continue; // i will use gids here as the more simple options 
   for (int x = 0; x < item.LayerData.Gids.Length; x++)
   {
      int gid = item.LayerData.Gids[x];
      LightTile? tile = map.GetTileData(gid, x, item.Width);

      var position = new Position(tile.WorldPositionX, tile.WorldPositionY);
      var srcRect = new Rectangle(tile.SourceX, tile.SourceY, tile.Width, tile.Height);
      var texture = MyTexturebank[tile.ImageName];
      
      // you could also do animation updates by getting the tile.FrameData property

      // very generic draw call
      batch.Draw(texture, position, srcRect);
   }
}
```

And of course you can hook up tile data yourself if you feel the need by calling:
```cs
// access to all the tileset data
var gidTilesetPair = map.GetTileSetPair(tileGid);
// and grab the gids for a layer
var gids = map.TileLayers.First(x => x.Name == "stars").LayerData.Gids;
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
