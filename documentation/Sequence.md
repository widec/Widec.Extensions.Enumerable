# Sequence


Sequence the items in an IEnumerable<T> with a sequencenumber starting from a startIndex. Returns an IEnumerable<ISequencedItem<T>>

## Syntax
```c#
public static IEnumerable<ISequencedItem<T>> Sequence<T>(
    this IEnumerable<T> source, 
    int startIndex)
```

##Remarks

The returned IEnumerable contains items of the ISequencedItem<T> interface.

```c#
public interface ISequencedItem<T>
{
    T Item { get; }
    int Sequence { get; }
}
```

## Parameters
|name | description|
|---|---|
|source | The source collection which items are to be concatenated.|
|startIndex | The index to start sequencing.|

##Example

```c#
var result = new string[]{"A","B","C","D"}.
    Sequence(10).
    Select(si => string.Format("{0}-{1}", si.Sequence, si.Item)).
    UnSplit(",");

// result contains "10-A,11-B,12-C,13-D"
```
