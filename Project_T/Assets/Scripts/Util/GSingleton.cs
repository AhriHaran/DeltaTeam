using System;
using System.Collections;
using UnityEngine;

public class GSingleton<T> where T : class, new()
{
    public static T instance
    {
        get;
        private set;
    }

    static GSingleton()
    {
        if(instance == null)
        {
            instance = new T();
        }
    }

    ~GSingleton() { instance = null; }
}
