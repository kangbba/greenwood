using System.Collections.Generic;
using System.Linq;
using static SmallPlaceNames;

public class Schedule
{
    private List<SmallPlaceActionPlan> _plans = new List<SmallPlaceActionPlan>();

    /// <summary>
    /// ✅ Schedule 생성자 (필요하면 인자로 리스트 받기도 가능)
    /// </summary>
    public Schedule() { }

    /// <summary>
    /// ✅ 액션 플랜을 추가
    /// </summary>
    public Schedule AddPlan(SmallPlaceActionPlan plan)
    {
        _plans.Add(plan);
        return this; // 체이닝을 위해 this 반환
    }

    /// <summary>
    /// ✅ 특정 SmallPlaceName에 해당하는 액션 플랜을 찾음
    /// </summary>
    public SmallPlaceActionPlan FindPlan(ESmallPlaceName placeName)
    {
        return _plans.FirstOrDefault(plan => plan.SmallPlaceName == placeName);
    }
}
