using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public static FlagManager Instance { get; private set; }

    private readonly Dictionary<string, BoolReactiveProperty> _flags = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Button]
    public void SetFlag(string flagName, bool value)
    {
        if (!_flags.ContainsKey(flagName))
        {
            _flags[flagName] = new BoolReactiveProperty(value);
        }
        else
        {
            _flags[flagName].Value = value;
        }
    }

    public BoolReactiveProperty GetFlagProperty(string flagName)
    {
        if (!_flags.ContainsKey(flagName))
        {
            _flags[flagName] = new BoolReactiveProperty(false);
        }
        return _flags[flagName];
    }
}
