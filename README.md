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

To use with Unity 5 or 2017: git checkout unity5 
To use with Unity 4: git checkout unity4

**Copy UnityEngine.dll from your Unity installation into lib folder.**

The solution file is in the **solution** folder. You can use VS or Rider IDE to build.
You can use Unix shell or cygwin to run Makefile commands:

cd solution 

To restore packages: make restore
To build: make build
To run tests: make test

See solution/deploy.sh file for the dlls that need to be copied into your project.
**Don't use mindi-unity.dll if you use MinDI outside of Unity 3D.**

Create / symlink the solution/output-dir and use **make deploy** to automatically update the dlls. 

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

