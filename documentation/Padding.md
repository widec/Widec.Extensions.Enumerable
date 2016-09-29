# Padding

Pads an IEnumerable left or right with items.

## Syntax
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

## Parameters
|name | description|
|---|---|
|source | The source collection that is to be padded.| 
|totalWidth | The total number of items returned in the resulting IEnumerable<T>|
|paddingItem | The closure that returns the item used to pad the source IEnumerable<T>, the integer parameter indicates the index in the collection|
