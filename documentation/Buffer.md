# Buffer

Takes an IEnumerable<T> and breaks it down into an IEnumerable<IEnumerable<T>> of specified size.

## Syntax
```c#
public static IEnumerable<IEnumerable<T>> Buffer<T>(
    this IEnumerable<T> source, 
    int size)
```

## Parameters
|name | description|
|---|---|
|source | The source collection which will be buffered.|
|size | the size of individual buffers.|

##Example
```csharp
var result = new string[]{"A","B","C","D","E","F","G"}.Buffer(2).Select(b => b.Unsplit(":")).Unsplit(",");
//result contains "A:B,C:D,E:F,G"
```
