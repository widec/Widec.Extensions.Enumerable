# ExceptWith

Filters an IEnumerable<T> with another IEnumerable<Q>

## Syntax
```c#
 public static IEnumerable<TSource> ExceptWith<TSource, TOther>(
            this IEnumerable<TSource> source,
            IEnumerable<TOther> other,
            Func<TSource, TOther> convert)
```

## Parameters
|name | description|
|---|---|
|source | The collection to be filtered.|
|other | The collection that contains the filter items.|
|convert | A conversion method that converts items of type TSource to items of type TOther|

##Example
```csharp
var result = new int[]{1,3,5,8}.ExceptWith(new int[]{5,1}, i => i).Unsplit(",");
//result will be "3,8"
```
