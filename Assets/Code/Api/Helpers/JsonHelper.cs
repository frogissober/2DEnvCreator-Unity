using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static List<T> ParseJsonArray<T>(string jsonArray)
    {
        if (string.IsNullOrEmpty(jsonArray)) return new List<T>();
        string wrappedJson = $"{{\"list\":{jsonArray}}}";
        var container = JsonUtility.FromJson<JsonList<T>>(wrappedJson);
        return container?.list ?? new List<T>();
    }
}

[Serializable]
public class JsonList<T>
{
    [SerializeField]
    public List<T> list;
}