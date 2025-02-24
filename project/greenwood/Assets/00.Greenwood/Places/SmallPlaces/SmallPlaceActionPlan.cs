using System;
using System.Collections.Generic;
using static SmallPlaceNames;

public interface ITalkable
{
    Action OnTalk { get; set; }
}

public interface IOrderable
{
    Action OnOrder { get; set; }
}

public interface IBuyable
{
    Action OnBuy { get; set; }
}

public interface ISellable
{
    Action OnSell { get; set; }
}

public interface IHealable
{
    Action OnHeal { get; set; }
}

public interface IRepairable
{
    Action OnRepair { get; set; }
}
public abstract class SmallPlaceActionPlan
{
    public ESmallPlaceName SmallPlaceName { get; }

    protected SmallPlaceActionPlan(ESmallPlaceName smallPlaceName)
    {
        SmallPlaceName = smallPlaceName;
    }

    /// <summary>
    /// ✅ SmallPlaceBase 에 액션을 적용
    /// </summary>
    public virtual void ApplyActions(SmallPlaceBase smallPlace)
    {
        var actions = GenerateInterfaceActions();
        smallPlace.SetActions(actions);
    }

    /// <summary>
    /// ✅ 각 인터페이스를 감지해 Dictionary를 자동 생성
    /// </summary>
    protected Dictionary<string, Action> GenerateInterfaceActions()
    {
        var dict = new Dictionary<string, Action>();

        // ✅ ITalkable
        if (this is ITalkable talkable)
        {
            dict["Talk"] = talkable.OnTalk;
        }

        // ✅ IOrderable
        if (this is IOrderable orderable)
        {
            dict["Order"] = orderable.OnOrder;
        }

        // ✅ IBuyable
        if (this is IBuyable buyable)
        {
            dict["Buy"] = buyable.OnBuy;
        }

        // ✅ ISellable
        if (this is ISellable sellable)
        {
            dict["Sell"] = sellable.OnSell;
        }

        // ✅ IHealable
        if (this is IHealable healable)
        {
            dict["Heal"] = healable.OnHeal;
        }

        // ✅ IRepairable
        if (this is IRepairable repairable)
        {
            dict["Repair"] = repairable.OnRepair;
        }

        return dict;
    }
}
