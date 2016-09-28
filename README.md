[![Build status](https://ci.appveyor.com/api/projects/status/vonlfkd1wxdxekp4/branch/master?svg=true)](https://ci.appveyor.com/project/widec/widec-extensions-enumerable/branch/master)
[![MyGet](https://img.shields.io/myget/widec/v/Widec.Extensions.Enumerable.svg?label=myget%20Widec.Extensions.Enumerable)](https://www.myget.org/feed/widec/package/nuget/Widec.Extensions.Enumerable)
[![NuGet](https://img.shields.io/nuget/v/Widec.Extensions.Enumerable.svg?label=NuGet%20Widec.Extensions.Enumerable)](https://www.nuget.org/packages/Widec.Extensions.Enumerable/)
# Widec.Extensions.Enumerable

## Abstract

This assembly contains a number of extensions on IEnumerable<T>. The package is available on NuGet as [Widec.Extensions.Enumerable](https://www.nuget.org/packages/Widec.Extensions.Enumerable). 
All extensions follow the deffered execution logic of IEnumerable<T>. The following explains the basic usage of the extensions.  

## UnSplit


The counterpart of the Split method on String. Concatenates the items in the IEnumerable<T> into a string seperated with a specified seperator

### Syntax
```c#
public static string UnSplit(
    this IEnumerable<string> source, 
    string seperator)
```

### Parameters
|name | description|
|---|---|
|source | The source collection which items are to be concatenated.|
|seperator | The string that will be used as seperator between items.|

###Example
```csharp
var result = new string[]{"A","B","C","D"}.UnSplit(",");
//result contains "A,B,C,D"
```


## Crudonize


Compares two IEnumerable instances based on a compare method and invokes create, update and delete actions.

### Syntax
```c#
 public static void Crudonize<TMaster, TSlave>(
            this IEnumerable<TMaster> masterList,
            IEnumerable<TSlave> slaveList,
            Func<TMaster, TSlave, bool> compare,
            Action<TMaster> create,
            Action<TMaster, TSlave> update,
            Action<TSlave> delete);
```
### Remarks
This is your one stop shop for comparing IEnumerable<T> instances. These can be the same types or different types. Basically it compares the **masterList** with the **slaveList** by using the **compare** function. 
For every item that is in the masterList and not in the slaveList the **create** action is called, for every item that is both in the masterList and in the slaveList the **update** action is called and finally 
for every item that is in the slaveList and not in the masterList the **delete** action is called.

### Parameters
|name | description|
|---|---|
|masterList | The IEnumerable<TMaster> used as master in the compare |
|slaveList | The IEnumerable<TSlave> used as slave in the compare |
|compare | The compare method used to compare items of type TMaster with items of type TSlave |
|create | The method called when an item exists in the master list but not in the slave list.|
|update | The method called when an item exists in the master list and in the slave list.| 
|delete | The method calle when an item exists in the slave list but not in the masterlist. |


## Padding

Pads an IEnumerable left or right with items.

### Syntax
```c#
public static IEnumerable<TSource> PadRight<TSource>(
    this IEnumerable<TSource> source, 
    int totalWidth, 
    Func<int, TSource> paddingItem)
```

```c#
public static IEnumerable<TSource> PadLeft<TSource>(
    this IEnumerable<TSource> source, 
    int totalWidth, 
    Func<int, TSource> paddingItem)
```

### Parameters
|name | description|
|---|---|
|source | The source collection that is to be padded.| 
|totalWidth | The total number of items returned in the resulting IEnumerable<T>|
|paddingItem | The closure that returns the item used to pad the source IEnumerable<T>, the integer parameter indicates the index in the collection|


## Sequence


Sequence the items in an IEnumerable<T> with a sequencenumber starting from a startIndex. Returns an IEnumerable<ISequencedItem<T>>

### Syntax
```c#
public static IEnumerable<ISequencedItem<T>> Sequence<T>(
    this IEnumerable<T> source, 
    int startIndex)
```

###Remarks

The returned IEnumerable contains items of the ISequencedItem<T> interface.

```c#
public interface ISequencedItem<T>
{
    T Item { get; }
    int Sequence { get; }
}
```

### Parameters
|name | description|
|---|---|
|source | The source collection which items are to be concatenated.|
|startIndex | The index to start sequencing.|

###Example

```c#
var result = new string[]{"A","B","C","D"}.
    Sequence(10).
    Select(si => string.Format("{0}-{1}", si.Sequence, si.Item)).
    UnSplit(",");

// result contains "10-A,11-B,12-C,13-D"
```

