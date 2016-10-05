# ToUpper

Takes an IEnumerable<T> or IEnumerable<string> and executes a ToUpper on every item.

## Syntax
```c#
public static IEnumerable<string> ToUpper(
  this IEnumerable<string> source);
  
public static IEnumerable<string> ToUpper<T>(
  this IEnumerable<T> source);
  
public static IEnumerable<string> ToUpper<T>(
  this IEnumerable<T> source, 
  Func<T,string> convert);
```

## Parameters
|name | description|
|---|---|
|source | The source collection which will be used.|
|convert | A function to convert an item of T to string.|

##Remarks

The overload with no convert and of type IEnumerable<T> executes the ToString() method on the items 
before executing the ToUpper() method.

##Example
```csharp
var result = new string[]{"A","b","c"}.TuUpper(2).Unsplit(",");
//result contains "A,B,C"
```
