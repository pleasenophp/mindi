# MinDI
Lightweight dependency injection framework for .NET and Unity3D with powerfull features and strong phylosophycal principles.

**NOTE** that MinDI is not maintained anymore. You can use instead the best maintained framework as of 2021, [Extenject (Zenject)](https://github.com/svermeulen/Extenject) that has the same philosophy as MinDI, is not opinionated, and implements much more features.

## Overview

MinDI is a context-oriented IoC/DI framework, that was initially started in 2015 as a project to extend the [MinIOC](https://bitbucket.org/Baalrukh/minioc/wiki/Home) framework with some syntax sugar, but then quickly turned into its own project, with quite different features and philosophy. 
MinDI supports Dependency Injection for Unity MonoBehaviour and Scenes, allowing to work with them as if they are normal C# objects and inject them to other classes and other way around. 
The kernel of MinDI is a minimized version of MinIOC library.

## Key features:

* Multi-layer context-oriented IoC container, creating new context layer with prototype reference to parent one, specifying and overriding custom dependencies on any level
* Easy syntax for configuration of bindings based on lambda-expressions
* Easy reflection-based property and method dependency injection
* Complex object-graphs support, circular references supported
* Binding multiple interfaces to one implementation supported 
* Custom factories supports for easy objects instantiation
* Support for generic bindings
* Support fot custom lifetime management
* Context introspection support
* Support for context configuration in class libraries
* Unity3D support, Scenes and MonoBehaviours act as part of the framework
* In Unity3D allowing to use MonoBehaviours and normal object injections fully transparently. Automatically tracking the destruction of dependent mono behaviours if the object, that caused their creation, was destroyed.
* Lightweight framework, can be used in AOT modes
* Flexible and easily extensible, can be integrated with other DI frameworks 

## Tested with

* .NET/Mono 3.5/4.5 standalone applications
* ASP.NET MVC
* Unity 3D 4,5,2017-2019 - OSX, Windows, WebGL, iOS

The framework is successfully tested on several big commercial projects.

## Installation

NuGet package is not available yet. You can build from source.

To use with Unity 5, 2017 and newer, or just in non-Unity project, use unity5 branch: 
```bash
git checkout unity5
```

To use with Unity 4: 
```bash
git checkout unity4
```

**Before building you have to manually copy UnityEngine.dll from your Unity installation into *lib* folder.**

Note: even if you build for non-unity3d usage, the dependency on UnityEngine.dll is still required for now. This will be fixed soon.
As a workaround, you can manually build all projects, except **mindi-unity**, that you don't need if you don't need to use it with Unity 3D.

The solution file is in the **solution** folder. You can use VS or Rider IDE to build.

If you want to build from command line and use some more automation, then use Unix shell or cygwin to run Makefile commands.

First run:
```bash
./configure
```
Then edit **config.mk** file manually to set custom paths and settings.

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

To automatically copy dlls to the $DEPLOY\_DIR from *config.mk*
```bash
# Normal deploy
make install

# Unity deploy
make install-unity
```

### Manual deploy
See Makefile for the path to the dlls that need to be copied into your project.
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

### Unity package
Import **mindi-unity-package.unitypackage** into your project for the Unity 3D integration prefabs and scripts.
The Unity package doesn't include dlls, so you still need to copy them.

## Usage

There will be uploaded usage documentation later, but for now you can consult this big article, that shows a lot of usage cases, and explains the main concepts behind MinDI:

- https://pleasenophp.github.io/posts/meet-the-ioc-container.html

Also you can use the following code-tutorials:

- Start with this demo to learn basic MinDI features in the standalone application: https://github.com/pleasenophp/mindi-demo
- Continue with this demo if you need to learn how to use it in Unity 3D: https://github.com/pleasenophp/mindi-unity-demo

The projects are organized the way, each next commit message is a next step in the tutorial, and the message itself tells what's being added. Start with checking out the very first commit, see the code, build, and then update to the next commit, etc.

## TODO

* Get rid of dependency on UnityEngine.dll if compiling just from master branch for non-unity project
* Make automatical configure script to find UnityEngine.dll and Nunit
* Make usage documentation 
* Make NuGet package
* Make Unity Asset Store package
* Expression or interface way to pass requirements in Construction.
* Support restoring dependencies on deserialization
* Extend fetching contexts by reflection to support automatical multi-domain configurations
* Support generating context type providers for AOT projects and other complex configurations
* See how to easily see context of each providing library (context initializers observaton)
* Context profiler feature
* Use constructor to auto generate a method for injection? Can also auto-generate factories.

