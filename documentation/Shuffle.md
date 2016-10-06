# Shuffle


Shuffles the items within the IEnumerable<T>

## Syntax
```c#
public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
```

## Parameters
|name | description|
|---|---|
|source | The source collection which items are to be shuffled.|

##Remarks

This shuffle extension is threadsafe, the random class is locked.
The algorithm used is [Fisher–Yates](https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm)

##Example
```csharp
var result = new string[]{"A","B","C","D"}.Shuffle().UnSplit(",");
//result contains a shuffled version of "A,B,C,D"
```
