using System;
using System.Collections.Generic;
using UnityEngine;
using static SmallPlaceNames;

public class CafeActionPlan : SmallPlaceActionPlan, ITalkable, IOrderable
{
    public CafeActionPlan() : base(ESmallPlaceName.Cafe) { }

    public Action OnTalk { get; set; } = () => Debug.Log("카페에서 기본 대화하기");
    public Action OnOrder { get; set; } = () => Debug.Log("카페에서 기본 주문하기");
}
