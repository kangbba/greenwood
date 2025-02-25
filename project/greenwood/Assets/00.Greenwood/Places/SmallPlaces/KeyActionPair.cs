using System;
using System.Collections.Generic;
using UnityEngine;
using static SmallPlaceNames;

public class KeyActionPair
{
    public string Key { get; }  // 키 (읽기 전용)
    public Action Action { get; } // 액션 (nullable)

    public KeyActionPair(string key, Action action = null)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        }

        Key = key;
        Action = action; // ✅ Action은 null 가능
    }
}
