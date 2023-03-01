using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// GOT THIS FROM: https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html
/// </summary>
public class CopyComponentValues : MonoBehaviour
{
    public static T GetCopyOf<T>(Component thisComponent, T other) where T : Component
    {
        Type type = thisComponent.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(thisComponent, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(thisComponent, finfo.GetValue(other));
        }
        return thisComponent as T;
    }
}
