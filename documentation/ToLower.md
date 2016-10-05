# ToLower

Takes an IEnumerable<T> or IEnumerable<string> and executes a ToLower on every item.

## Syntax
```c#
public static IEnumerable<string> ToLower(
  this IEnumerable<string> source);
  
public static IEnumerable<string> ToLower<T>(
  this IEnumerable<T> source);
  
public static IEnumerable<string> ToLower<T>(
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
before executing the ToLower() method.

##Example
```csharp
var result = new string[]{"a","B","C"}.ToLower(2).Unsplit(",");
//result contains "a,b,c"
```
