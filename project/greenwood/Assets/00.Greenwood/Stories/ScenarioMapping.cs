using System;
using Sirenix.OdinInspector;
using UnityEngine;
using static SmallPlaceNames;
using static BigPlaceNames;

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

}
