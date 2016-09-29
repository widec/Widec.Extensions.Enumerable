[![Build status](https://ci.appveyor.com/api/projects/status/vonlfkd1wxdxekp4/branch/master?svg=true)](https://ci.appveyor.com/project/widec/widec-extensions-enumerable/branch/master)
[![MyGet](https://img.shields.io/myget/widec/v/Widec.Extensions.Enumerable.svg?label=myget%20Widec.Extensions.Enumerable)](https://www.myget.org/feed/widec/package/nuget/Widec.Extensions.Enumerable)
[![NuGet](https://img.shields.io/nuget/v/Widec.Extensions.Enumerable.svg?label=NuGet%20Widec.Extensions.Enumerable)](https://www.nuget.org/packages/Widec.Extensions.Enumerable/)
# Widec.Extensions.Enumerable

## Abstract

This assembly contains a number of extensions on IEnumerable<T>. The package is available on NuGet as [Widec.Extensions.Enumerable](https://www.nuget.org/packages/Widec.Extensions.Enumerable). 
All extensions follow the deffered execution logic of IEnumerable<T>. The following explains the basic usage of the extensions.  

The following extensions are available

|Extension|Description|
|---|---|
|[UnSplit](documentation/UnSplit.md)|The reverse of the Split method on String.|
|[Crudonize](documentation/Crudonize.md)|Compares two IEnumerable instances based on a compare method and invokes create, update and delete actions.|
|[Padding](documentation/Padding.md)|Pads an IEnumerable left or right with items.|
|[Sequence](documentation/Sequence.md)|Sequence the items in an IEnumerable<T>|
|[Buffer](documentation/Buffer.md)|Takes an IEnumerable<T> and breaks it down into an IEnumerable<IEnumerable<T>> of specified size.|
|[Median](documentation/Median.md)|Takes the median of the IEnumerable<T>|
|[ExceptWith](documentation/ExceptWith.md)|Filters an IEnumerable<T> with another IEnumerable<Q>|


