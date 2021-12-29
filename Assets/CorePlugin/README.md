# CoreManager

Reasons to use this package:

- It allows avoiding reference serialization via Inspector.
- It removes the need for GOD objects.
- Provides high script flexibility and low cohesion.

## Technical details

Package created on version 2020.3.6f1 but compatible with the versions starting from 2020.x.x

Any OS supported. Unity version 2020 and above. (Package requires C# 8.0)

## Features

1. <b>Core Manager</b> - used for scene, subscription, and reference initialization.
2. <b>Cross Events</b> - replacement for the traditional event serialization and subscription.
3. <b>Reference Distributor</b> - reference container for data distribution in one scene.
4. <b>Cross Scene Data Handler</b> - data container(not references) which allows data distribution between scenes.
   Supports both classes and structures.
5. <b>Custom Validation Attributes</b> - allows validating serialized data. This plugin contains both predefined
   validation attributes and mechanisms to implement new validation attributes. Supports both class and field
   attributes.
6. <b>Custom Editor</b> - used for attribute validation and displaying errors in Inspector.
7. <b>Custom Logger</b> - use this logger if you want to show logs in debug build/editor, but not in release build.
8. <b>Custom Play Mode</b> entering - prevents Play Mode start if the current scene contains validation failure.
9. <b>Custom Build</b> start - prevents application build if scenes included in the build or prefabs with validation
   attributes contain validation failures.
10. <b>SaveSystem</b> - system to save/load JSON files.
11. <b>UIStateTools & UIManager</b> - base UI system for page-based UI.
12. <b>Extensions</b> - for Editor and base classes.

## Improvements

1. <b>RequireInterfaceAttribute</b>
2. <b>Delegate</b> combination for subscribing (checkout samples to learn how to use)

## Breaking changes

### v1.1.2

1. Event interfaces accept arrays instead of <b>IEnumerable</b>.

### v1.1.3

1. Event interfaces accept <b>params</b> arrays.

### v2.0.1

1. Striped part of functions is now available on GitHub repo.
    1. <b>[Scene Loader]</b> - allows to asynchronously load scene through an intermediate scene and allows to serialize
       SceneAssets through Inspector (use SceneLoaderAsset).
    2. <b>[Runtime console]</b> - console with Unity logs for debug and/or release builds. Allows display console logs
       like in Unity Editor. Strips from release build if other not predetermined.
    3. <b>[Editor Symbol Definer]</b> - allows defining Scripting Define Symbols in the project thought attribute or
       button in Inspector.

### v2.0.2

1. Fixed build issue with Validation Attributes
2. Removed redundant prefabs and scripts
3. Added:
    1. SelectImplementation Attribute
    2. SelectType Attribute

[Scene Loader]: https://github.com/uurha/AdvancedSceneManagement

[Runtime console]: https://github.com/uurha/UnityConsole

[Editor Symbol Definer]: https://github.com/uurha/EditorSymbolDefiner