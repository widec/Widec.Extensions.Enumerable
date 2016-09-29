# Median


Takes the median of the IEnumerable<T>

## Syntax
```c#
public static int Median<T>(
    this IEnumerable<T> source, 
    Func<T, int> selector)
```

## Parameters
|name | description|
|---|---|
|source | The source collection to retrieve the median from.|
|selector | The select method that extracts the integer value from and instance of T.|

##Example
```csharp
var result = new int[]{1,3,5,8}.Median(i => i);
//result will be 4   => ((3+5)/2)
```
