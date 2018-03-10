# MinDI
MinDI dependency injection framework

## Overview

MinDI is a context-oriented IoC/DI framework, that was initially started as a project to extend the [MinIOC](https://bitbucket.org/Baalrukh/minioc/wiki/Home) framework with some syntax sugar, but then quickly turned into its own project, with quite different features and ideology. 

## Key features:

* Multi-layer context-oriented IoC container, overriding bindings on any level
* Easy syntax for configuration of bindings based on lambda-expressions
* Easy reflection-based property and method dependency-injection
* IoC container is not accessible by user-level objects
* Custom factories supports for easy objects instantiation
* Support for generic bindings
* Support fot custom lifetime management
* Context introspection support
* Support for context configuration in class libraries
* Unity3D support, Scenes and MonoBehaviours act as part of the framework
* Lightweight framework, can be used in AOT modes
* Easily extensible, integrates with other frameworks 

## Tested with

* .NET 4.5 standalone applications
* ASP.NET MVC
* Unity 3D 4,5,2017 - OSX, Windows, WebGL, iOS

The framework is successfully tested on several big commercial projects.

## Installation

NuGet package is not available yet. You can build from source.

To use with Unity 5 or 2017: 
```bash
git checkout unity5
```

To use with Unity 4: 
```bash
git checkout unity4
```

*Before building you have to manually copy UnityEngine.dll from your Unity installation into **lib** folder.*

The solution file is in the **solution** folder. You can use VS or Rider IDE to build.
You can use Unix shell or cygwin to run Makefile commands:

First:
```bash
cd solution 
```

To restore packages:
```bash
make restore
```

To build with xbuild: 
```bash
make build
```

To run tests: 
```bash
make test
```

See solution/deploy.sh file for the path to the dlls that need to be copied into your project.
To use MinDI in non-Unity 3D project you need the following dlls:
* mindi.dll
* mindi-interop.dll
* minioc.dll

**Don't use mindi-unity.dll if you use MinDI outside of Unity 3D.**

Create / symlink the solution/output-dir and use *make deploy* to automatically update the dlls. 

To use inside of Unity3D, put the following dlls into Plugins/\* folder:
* mindi.dll
* mindi-interop.dll
* minioc.dll
* mindi-unity.dll

Then, import **mindi-unity-package.unitypackage** into your project for the Unity 3D integration prefabs and scripts.

## Usage

There will be uploaded usage documentation later

## TODO

* Make usage documentation 
* Make NuGet package
* Expression or interface way to pass requirements in Construction.
* Support restoring dependencies on deserialization
* See how to easily see context of each providing library (context initializers observaton)
* Context profiler feature
* Use constructor to auto generate a method for injection? Can also auto-generate factories.

