# UnSplit


The counterpart of the Split method on String. Concatenates the items in the IEnumerable<T> into a string seperated with a specified seperator

## Syntax
```c#
public static string UnSplit(
    this IEnumerable<string> source, 
    string seperator)
```

## Parameters
|name | description|
|---|---|
|source | The source collection which items are to be concatenated.|
|seperator | The string that will be used as seperator between items.|

##Example
```csharp
var result = new string[]{"A","B","C","D"}.UnSplit(",");
//result contains "A,B,C,D"
```
