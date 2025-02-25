using System.Collections.Generic;

public static class ScenarioData
{
    // ✅ 카페 시나리오 리스트
    public static readonly List<Scenario> CafeTalk = new List<Scenario>
    {
        new TestStory1(), 
        new TestStory2(),
        new TestStory3(),
        new TestStory4()
    };

    public static readonly List<Scenario> CafeOrder = new List<Scenario>
    {
        new TestStory2(),
        new TestStory3()
    };

    // ✅ 빵집 시나리오 리스트
    public static readonly List<Scenario> BakeryTalk = new List<Scenario>
    {
        new TestStory1(),
        new TestStory3()
    };

    public static readonly List<Scenario> BakeryBuy = new List<Scenario>
    {
        new TestStory2(),
        new TestStory4(),
        new TestStory1()
    };

    // ✅ 약초 가게 시나리오 리스트
    public static readonly List<Scenario> HerbshopBuy = new List<Scenario>
    {
        new TestStory3(),
        new TestStory4()
    };

    public static readonly List<Scenario> HerbshopHeal = new List<Scenario>
    {
        new TestStory1(),
        new TestStory2(),
        new TestStory3()
    };
}