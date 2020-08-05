# Unreal Conan Generator

Unreal generator for Conan C/C++ Package manager https://conan.io

It is a **generator package**, it is not embedded in the main conan.io codebase, but an independent package. Read http://docs.conan.io for more info.

## Using the generator

### Installing

The `unreal` generator is packaged inside `UnrealGen/1.0@frozenstorm-interactive/stable`. So, you need to require it on your 
conanfile. You can then configuring `unreal` or `Unreal` generator.

```txt
[requires]
...
UnrealGen/1.0@frozenstorm-interactive/stable
```

This generator create a file named `Source/ConanBuildInfo/ConanInfo.Build.cs` that will be used by Unreal Build System.

```txt
...
[generators]
unreal
```

You can add the `Source/ConanBuildInfo` folder in your ignorefile.

```txt
# Conan
Source/ConanBuildInfo/*
```

### Setup Conan in Unreal

On your `.Target.cs` and `.Build.cs`, import `Conan` namespace with:

```c#
using Conan;
```

Init Conan in your `TargetRules` or `ModuleRules` constructor with:

```c#
ConanBuildInfo.Setup();
```

### Linking

#### Link one library

```c#
this.LinkConanLibrary("Assimp");
```

#### Link multiple libraries

```c#
this.LinkConanLibraries(new string[] { "Assimp", "Args" });
```

#### Link all libraries

```c#
this.LinkAllConanLibraries();
```

## Troubleshooting

### The type or namespace name 'type/namespace' could not be found (are you missing a using directive or an assembly reference?)

You must run `conan install` in your Unreal Project root (where the `Source` folder is located).
Check if `Source/ConanBuildInfo/ConanInfo.Build.cs` is created.

## License

[MIT](LICENSE)
