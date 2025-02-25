using System.Collections.Generic;

public static class ScenarioData
{
    // ✅ 카페 시나리오 리스트
    public static readonly List<Scenario> CafeTalk = new List<Scenario>
    {
    };

    public static readonly List<Scenario> CafeOrder = new List<Scenario>
    {
    };

    // ✅ 빵집 시나리오 리스트
    public static readonly List<Scenario> BakeryTalk = new List<Scenario>
    {
        new KateEmotionChangePractice(),
    };

    public static readonly List<Scenario> BakeryBuy = new List<Scenario>
    {
    };

    // ✅ 약초 가게 시나리오 리스트
    public static readonly List<Scenario> HerbshopBuy = new List<Scenario>
    {
    };

    public static readonly List<Scenario> HerbshopHeal = new List<Scenario>
    {
    };
}