using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class TailList<T> : List<T>
{
    public int Capacity = 1;
    public TailList(int capacity)
    {
        Capacity = capacity;
    }
    public void Add (T item)
    {
        base.Add(item);
        if(Count > Capacity)
            RemoveAt(0);
    }
    public void Insert (int index, T item)
    {
        base.Insert(index,item);
        if(Count > Capacity)
            RemoveAt(0);
    }
}