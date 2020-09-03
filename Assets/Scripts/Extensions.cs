using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//It is common to create a class to contain all of your
//extension methods. This class must be static.
public static class Extensions
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static Vector2 round2d(this Vector2 vector)
    {
        vector.x = (float) Math.Round(vector.x, 1);
        vector.y = (float) Math.Round(vector.y, 1);
        return vector;
    }
    
    public static Vector2 round2d(this Vector3 vector)
    {
        vector.x = (float) Math.Round(vector.x, 1);
        vector.y = (float) Math.Round(vector.y, 1);
        vector.z = 0;
        return vector;
    }
    
    public static T last<T>(this IList<T> list)
    {
        if (list == null)
            throw new ArgumentNullException("list");
        if (list.Count == 0)
            throw new ArgumentException(
                "Cannot get last item because the list is empty");

        int lastIdx = list.Count - 1;
        return list[lastIdx];
    }
    
    public static T get<T>(this IList<T> list, int index)
    {
        if (list == null)
            throw new ArgumentNullException("list");
        if (list.Count == 0)
            throw new ArgumentException(
                "Cannot get last item because the list is empty");

        return list[index];
    }
    
    public static T getAtEnd<T>(this IList<T> list, int index)
    {
        if (list == null)
            throw new ArgumentNullException("list");
        if (list.Count == 0)
            throw new ArgumentException(
                "Cannot get last item because the list is empty");

        return list[list.Count - index];
    }
    
    public static int lastIndex<T>(this IList<T> list)
    {
        if (list == null)
            throw new ArgumentNullException("list");
        if (list.Count == 0)
            throw new ArgumentException(
                "Cannot get last item because the list is empty");

        return list.Count - 1;
    }
    
    
    public static float round(this float value, int decimalCount)
    {
        return (float) Math.Round(value, decimalCount);
    }
}