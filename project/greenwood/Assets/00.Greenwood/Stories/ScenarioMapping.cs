using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ScenarioMapping
{
    [Title("스토리 설정")]
    [SerializeField] private string _scenarioName; // ✅ 캡슐화 적용

    [Title("조건 활성화 여부")]
    [SerializeField, ToggleLeft] private bool _useBigPlace = true;
    [SerializeField, ToggleLeft] private bool _useSmallPlace = false;
    [SerializeField, ToggleLeft] private bool _useTargetDay = true;
    [SerializeField, ToggleLeft] private bool _useTargetTimePhase = false;

    [Title("조건 값")]
    [SerializeField, ShowIf("_useBigPlace")] private EBigPlaceName _bigPlaceName;
    [SerializeField, ShowIf("_useSmallPlace")] private ESmallPlaceName _smallPlaceName;
    [SerializeField, ShowIf("_useTargetDay"), MinValue(1)] private int _targetDay;
    [SerializeField, ShowIf("_useTargetTimePhase")] private TimePhase _targetTimePhase;

    // ✅ 프로퍼티 추가 (읽기 전용)
    public string ScenarioName => _scenarioName;
    public bool UseBigPlace => _useBigPlace;
    public bool UseSmallPlace => _useSmallPlace;
    public bool UseTargetDay => _useTargetDay;
    public bool UseTargetTimePhase => _useTargetTimePhase;
    public EBigPlaceName BigPlaceName => _bigPlaceName;
    public ESmallPlaceName SmallPlaceName => _smallPlaceName;
    public int TargetDay => _targetDay;
    public TimePhase TargetTimePhase => _targetTimePhase;

    /// <summary>
    /// 현재 BigPlace, SmallPlace, 날짜, 시간에 대해 이 스토리가 실행 가능한지 확인
    /// </summary>
    public bool IsMatching(BigPlace bigPlace, SmallPlace smallPlace, int currentDay, TimePhase currentTimePhase)
    {
        Debug.Log($"[ScenarioMapping] Checking Scenario: {ScenarioName}");
        if (_useBigPlace)
        {
            Debug.Log($"- Target BigPlace: {_bigPlaceName} (Current: {(bigPlace != null ? bigPlace.BigPlaceName.ToString() : "None")})");
            if (bigPlace == null || bigPlace.BigPlaceName != _bigPlaceName)
            {
                Debug.Log("[ScenarioMapping] ❌ BigPlace mismatch!");
                return false;
            }
        }

        if (_useSmallPlace)
        {
            Debug.Log($"- Target SmallPlace: {_smallPlaceName} (Current: {(smallPlace != null ? smallPlace.SmallPlaceName.ToString() : "None")})");
            if (smallPlace == null || smallPlace.SmallPlaceName != _smallPlaceName)
            {
                Debug.Log("[ScenarioMapping] ❌ SmallPlace mismatch!");
                return false;
            }
        }

        if (_useTargetDay)
        {
            Debug.Log($"- Target Day: {_targetDay} (Current: {currentDay})");
            if (_targetDay != currentDay)
            {
                Debug.Log("[ScenarioMapping] ❌ Day mismatch!");
                return false;
            }
        }

        if (_useTargetTimePhase)
        {
            Debug.Log($"- Target TimePhase: {_targetTimePhase} (Current: {currentTimePhase})");
            if (_targetTimePhase != currentTimePhase)
            {
                Debug.Log("[ScenarioMapping] ❌ TimePhase mismatch!");
                return false;
            }
        }

        Debug.Log($"✅ Scenario '{ScenarioName}' matches all conditions!");
        return true;
    }
}
