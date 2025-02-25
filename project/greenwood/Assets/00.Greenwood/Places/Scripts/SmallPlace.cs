using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using static SmallPlaceNames;

public class SmallPlace : AnimationImage
{
    [SerializeField] private ESmallPlaceName _smallPlaceName;

    private List<Character> _activeCharacters = new List<Character>();

    private List<KeyScenariosPair> _keyScenarioPairs = new List<KeyScenariosPair>();

    public ESmallPlaceName SmallPlaceName => _smallPlaceName;

    public List<KeyScenariosPair> KeyScenariosPairs { get => _keyScenarioPairs; }

    public void Init()
    {
        // 예: FadeOut(0f);
    }
    
    public void SetKeyScenariosPairs( List<KeyScenariosPair> ksps)
    {
        if (ksps.IsNullOrEmpty())
        {
            Debug.LogError("[SmallPlace] ❌ ksps nullorempty");
            return;
        }
        
        _keyScenarioPairs = ksps;
    }

    /// <summary>
    /// ✅ 특정 키(Key)에 해당하는 시나리오 리스트 반환
    /// </summary>
    public List<Scenario> GetKeyScenarios(string key)
    {
        var pair = _keyScenarioPairs.Find(p => p.Key == key);
        return pair?.Scenarios ?? new List<Scenario>(); // 없으면 빈 리스트 반환
    }

}
