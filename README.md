# Unreal Conan Generator

Unreal generator for Conan C/C++ Package manager https://conan.io

It is a **generator package**, it is not embedded in the main conan.io codebase, but an independent package. Read http://docs.conan.io for more info.

## Using the generator

### Installing

The `unreal` generator is packaged inside `UnrealGen/2.0@frozenstorm-interactive/stable`. So, you need to require it on your 
conanfile. You can then configuring `unreal` or `Unreal` generator.

```txt
[requires]
...
UnrealGen/2.0@frozenstorm-interactive/stable
```

This generator create plugins with externals modules.

```txt
...
[generators]
unreal
```

You can add the `Source/ConanVendor` folder in your ignorefile.

```txt
# Conan
Source/ConanVendor/*
```

### Linking

```c#
PublicDependencyModuleNames.AddRange(new string[] { 
    "Core", 
    
    ...

    "Rapidjson", 
    "Rttr",
});
```

## Limitations

- Conanfile must be located next to the uproject file.
- Only works on Windows.
- It doesn't support integration into plugins.

## Troubleshooting

### The type or namespace name 'type/namespace' could not be found (are you missing a using directive or an assembly reference?)

You must run `conan install` in your Unreal Project root (where the `Source` folder is located).
Check if `Source/ConanBuildInfo/ConanInfo.Build.cs` is created.

## License

[MIT](LICENSE)
