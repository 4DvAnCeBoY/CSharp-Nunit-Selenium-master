
# Lambda-NUnit-Sample

Demonstrates how to use NUnit to run parallel tests on Sauce Labs platfrom using nmake as the build system. 

Uses [NuGet](http://docs.nuget.org/) as package manager.

###Dependencies:

* MS Visual Studio 2013 or later.
* [NUnit3.0](https://www.nunit.org/)
* [NuGet](https://dist.nuget.org/index.html) Plugin for Visual Studio
* [NuGet](https://dist.nuget.org/index.html) CLI executable installed in your path.


### Setup:

* Install NuGet packages for the project: <br>
```cd Packages```<br>
```nuget.exe install ..\ParallelSelenium\packages.config```<br>

* Clean and rebuild project:<br>
```nmake clean build```

### Set Credentials <br>


### Run Tests in parallel
```nmake test``` <br>
**or**<br>
```nmake all```<br>
