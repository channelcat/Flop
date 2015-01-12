using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class TailList<T> : List<T>
{
    new public int Capacity = 1;
    public TailList(int capacity)
    {
        Capacity = capacity;
    }
    new public void Add (T item)
    {
        base.Add(item);
        if(Count > Capacity)
            RemoveAt(0);
    }
    new public void Insert (int index, T item)
    {
        base.Insert(index,item);
        if(Count > Capacity)
            RemoveAt(0);
    }
}