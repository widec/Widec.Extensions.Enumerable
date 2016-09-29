#Crudonize
Compares two IEnumerable instances based on a compare method and invokes create, update and delete actions.

## Syntax
```c#
 public static void Crudonize<TMaster, TSlave>(
            this IEnumerable<TMaster> masterList,
            IEnumerable<TSlave> slaveList,
            Func<TMaster, TSlave, bool> compare,
            Action<TMaster> create,
            Action<TMaster, TSlave> update,
            Action<TSlave> delete);
```
## Remarks
This is your one stop shop for comparing IEnumerable<T> instances. These can be the same types or different types. Basically it compares the **masterList** with the **slaveList** by using the **compare** function. 
For every item that is in the masterList and not in the slaveList the **create** action is called, for every item that is both in the masterList and in the slaveList the **update** action is called and finally 
for every item that is in the slaveList and not in the masterList the **delete** action is called.

## Parameters
|name | description|
|---|---|
|masterList | The IEnumerable<TMaster> used as master in the compare |
|slaveList | The IEnumerable<TSlave> used as slave in the compare |
|compare | The compare method used to compare items of type TMaster with items of type TSlave |
|create | The method called when an item exists in the master list but not in the slave list.|
|update | The method called when an item exists in the master list and in the slave list.| 
|delete | The method calle when an item exists in the slave list but not in the masterlist. |
