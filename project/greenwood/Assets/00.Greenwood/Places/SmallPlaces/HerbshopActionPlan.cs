using System;
using System.Collections.Generic;
using UnityEngine;
using static SmallPlaceNames;

public class HerbshopActionPlan : SmallPlaceActionPlan, IBuyable, IHealable
{
    public HerbshopActionPlan() : base(ESmallPlaceName.Herbshop) { }

    public Action OnBuy  { get; set; } = () => Debug.Log("약초 가게 기본 구매");
    public Action OnHeal { get; set; } = () => Debug.Log("약초 가게 기본 치료");
}
