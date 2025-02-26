using UniRx;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance { get; private set; }

    private readonly ReactiveProperty<int> _currentActionPointNotifier = new ReactiveProperty<int>();
    public IReadOnlyReactiveProperty<int> CurrentActionPointNotifier => _currentActionPointNotifier;

    [SerializeField] private int _maxActionPoints = 100; // ✅ 최대 행동력 설정

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ResetActionPoints(); // ✅ 시작 시 행동력 초기화
    }

    public void UseActionPoint(int amount)
    {
        if (_currentActionPointNotifier.Value >= amount)
        {
            _currentActionPointNotifier.Value -= amount;
            Debug.Log($"[ActionPointManager] 행동력 사용: -{amount}, 남은 행동력: {_currentActionPointNotifier.Value}");
        }
        else
        {
            Debug.LogWarning("[ActionPointManager] 행동력이 부족합니다!");
        }
    }

    public void RestoreActionPoint(int amount)
    {
        _currentActionPointNotifier.Value = Mathf.Min(_currentActionPointNotifier.Value + amount, _maxActionPoints);
        Debug.Log($"[ActionPointManager] 행동력 회복: +{amount}, 현재 행동력: {_currentActionPointNotifier.Value}");
    }

    public void ResetActionPoints()
    {
        _currentActionPointNotifier.Value = _maxActionPoints;
        Debug.Log($"[ActionPointManager] 행동력 초기화: {_maxActionPoints}");
    }
}
