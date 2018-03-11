# MinDI
Lightweight dependency injection framework for .NET

## Overview

MinDI is a context-oriented IoC/DI framework, that was initially started in 2015 as a project to extend the [MinIOC](https://bitbucket.org/Baalrukh/minioc/wiki/Home) framework with some syntax sugar, but then quickly turned into its own project, with quite different features and ideology. 

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
* In Unity3D allowing to use MonoBehaviours and normal object injections fully transparently. Automatically tracking the destruction of dependent mono behaviours if the object, that caused their creation, was destroyed.
* Lightweight framework, can be used in AOT modes
* Easily extensible, integrates with other frameworks 

## Tested with

* .NET/Mono 3.5/4.5 standalone applications
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

**Before building you have to manually copy UnityEngine.dll from your Unity installation into *lib* folder.**

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

**Don't use mindi-unity.dll if you use MinDI in non - Unity 3D application.**

You can also create/symlink the *solution/output-dir* and use *make deploy* to automatically update the dlls. 

To use inside of Unity3D or in a class-library, that is designed for Unity3D, add the following dlls into project (in Unity put into Plugins/\* folder)
* mindi.dll
* mindi-interop.dll
* minioc.dll
* mindi-unity.dll

Then, import **mindi-unity-package.unitypackage** into your project for the Unity 3D integration prefabs and scripts.

## Usage

There will be uploaded usage documentation later, but for now you can consult this big article, that shows a lot of usage cases, and explains the main concepts behind MinDI:

- https://pleasenophp.github.io/posts/meet-the-ioc-container.html

Also you can use the following code-tutorials:

- Start with this demo to learn basic MinDI features in the standalone application: https://github.com/pleasenophp/mindi-demo
- Continue with this demo if you need to learn how to use it in Unity 3D: https://github.com/pleasenophp/mindi-unity-demo

The projects are organized the way, each next commit message is a next step in the tutorial, and the message itself tells what's being added. Start with checking out the very first commit, see the code, build, and then update to the next commit, etc.

## TODO

* Make usage documentation 
* Make NuGet package
* Expression or interface way to pass requirements in Construction.
* Support restoring dependencies on deserialization
* See how to easily see context of each providing library (context initializers observaton)
* Context profiler feature
* Use constructor to auto generate a method for injection? Can also auto-generate factories.

