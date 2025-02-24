using System;
using System.Collections.Generic;
using UnityEngine;
using static SmallPlaceNames;

public class BakeryActionPlan : SmallPlaceActionPlan, ITalkable, IBuyable
{
    public BakeryActionPlan() : base(ESmallPlaceName.Bakery) { }

    public Action OnTalk { get; set; } = () => Debug.Log("빵집 기본 대화하기");
    public Action OnBuy  { get; set; } = () => Debug.Log("빵집 기본 구매하기");
}
