using System;
using System.Collections.Generic;
using UnityEngine;
using static SmallPlaceNames;
public class SmallPlaceBase : AnimationImage
{
  
    [SerializeField] private ESmallPlaceName _smallPlaceName;
    private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

    public ESmallPlaceName SmallPlaceName => _smallPlaceName;

    public void Init()
    {
        // 예: FadeOut(0f);
    }

    public void SetActions(Dictionary<string, Action> actions)
    {
        // 항상 Exit 버튼 추가
        _actions = new Dictionary<string, Action>(actions)
        {
            ["Exit"] = () => PlaceManager.Instance.ExitSmallPlace(0.3f)
        };
    }

    public Dictionary<string, Action> GetActions()
    {
        return _actions;
    }
}
