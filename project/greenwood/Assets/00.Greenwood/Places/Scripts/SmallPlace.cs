using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using static SmallPlaceNames;

public class SmallPlace : AnimationImage
{
    [SerializeField] private ESmallPlaceName _smallPlaceName;

    public ESmallPlaceName SmallPlaceName => _smallPlaceName;

    public void Init()
    {
    }
    
}
