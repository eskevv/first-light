# FirstLight

Developer branch is a retake on the parsic proccess.
-With the goal of being more optimized before an official release. (some features were removed as they seemed unstable or not useful)

FirstLight is a standalone .NET C# library for importing and fully automating all of your work from the Tiled editor.  
*No ID management is needed to get your map shown on the screen.* This also means animations.
All data is saved inside of a MapModel which closeley resembles the original TMX format, but you interact with it through the ```GameMap``` class.

Since FirstLight is a standalone library, it doesn't rely on any external frameworks or game engines in order to use it. It still 
ensures that maps can be quickly displayed on the screen with minimal effort, and the API provides methods to be more felxible with how tiles are rendered.

Note: It is up to you to store the actual textures, (preferably to cache them by tileset name). As a standalone library, FirstLight doesn't do any of the
actual rendering, but provides an interface to all of the necessary tmx data. This versatility means you can use it with popular game engines like Unity3D, Godot, or CryEngine.

---
### Installation
Option 1: With Nuget Package Manager:  
```
dotnet add package firstlight 
```
Option 2: Clone or download the repo into a dir of choice.
Then add to your .csproj file in your own project:
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
