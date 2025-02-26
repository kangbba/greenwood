using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static SmallPlaceNames;

public class PlaceEventScheduler : MonoBehaviour
{
    public static PlaceEventScheduler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }


}
