using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

static class Tuple
{
    public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
    {
        return new Tuple<T1, T2>(item1, item2);
    }
    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
    {
        return new Tuple<T1, T2, T3>(item1, item2, item3);
    }
}

class Tuple<T1, T2> : IFormattable
{
    public T1 Item1 { get; private set; }
    public T2 Item2 { get; private set; }
    
    public Tuple(T1 item1, T2 item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
    
    #region Optional - If you need to use in dictionaries or check equality
    private static readonly IEqualityComparer<T1> Item1Comparer = EqualityComparer<T1>.Default;
    private static readonly IEqualityComparer<T2> Item2Comparer = EqualityComparer<T2>.Default;
    
    public override int GetHashCode()
    {
        var hc = 0;
        if (!object.ReferenceEquals(Item1, null))
            hc = Item1Comparer.GetHashCode(Item1);
        if (!object.ReferenceEquals(Item2, null))
            hc = (hc << 3) ^ Item2Comparer.GetHashCode(Item2);
        return hc;
    }
    public override bool Equals(object obj)
    {
        var other = obj as Tuple<T1, T2>;
        if (object.ReferenceEquals(other, null))
            return false;
        else
            return Item1Comparer.Equals(Item1, other.Item1) && Item2Comparer.Equals(Item2, other.Item2);
    }
    #endregion
    
    #region Optional - If you need to do string-based formatting
    public override string ToString() { return ToString(null, CultureInfo.CurrentCulture); }
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return string.Format(formatProvider, format ?? "{0},{1}", Item1, Item2);
    }
    #endregion
}

class Tuple<T1, T2, T3> : IFormattable
{
    public T1 Item1 { get; private set; }
    public T2 Item2 { get; private set; }
    public T3 Item3 { get; private set; }
    
    public Tuple(T1 item1, T2 item2, T3 item3)
    {
        Item1 = item1;
        Item2 = item2;
        Item3 = item3;
    }
    
    #region Optional - If you need to use in dictionaries or check equality
    private static readonly IEqualityComparer<T1> Item1Comparer = EqualityComparer<T1>.Default;
    private static readonly IEqualityComparer<T2> Item2Comparer = EqualityComparer<T2>.Default;
    private static readonly IEqualityComparer<T3> Item3Comparer = EqualityComparer<T3>.Default;
    
    public override int GetHashCode()
    {
        var hc = 0;
        if (!object.ReferenceEquals(Item1, null))
            hc = Item1Comparer.GetHashCode(Item1);
        if (!object.ReferenceEquals(Item2, null))
            hc = (hc << 3) ^ Item2Comparer.GetHashCode(Item2);
        if (!object.ReferenceEquals(Item3, null))
            hc = (hc << 6) ^ Item3Comparer.GetHashCode(Item3);
        return hc;
    }
    public override bool Equals(object obj)
    {
        var other = obj as Tuple<T1, T2, T3>;
        if (object.ReferenceEquals(other, null))
            return false;
        else
            return Item1Comparer.Equals(Item1, other.Item1) && Item2Comparer.Equals(Item2, other.Item2) && Item3Comparer.Equals(Item3, other.Item3);
    }
    #endregion
    
    #region Optional - If you need to do string-based formatting
    public override string ToString() { return ToString(null, CultureInfo.CurrentCulture); }
    public string ToString(string format, IFormatProvider formatProvider)
    {
        return string.Format(formatProvider, format ?? "{0},{1},{2}", Item1, Item2, Item3);
    }
    #endregion
}