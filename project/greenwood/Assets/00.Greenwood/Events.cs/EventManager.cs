using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField, FoldoutGroup("이벤트 조건")]
    private EventConditions _eventConditions; // ✅ 이벤트 조건 바인딩

    [SerializeField, FoldoutGroup("이벤트 결과")]
    private EventResults _eventResults; // ✅ 이벤트 결과 바인딩

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (_eventConditions == null || _eventResults == null)
        {
            Debug.LogError("❌ [EventManager] EventConditions 또는 EventResults가 설정되지 않음!");
            return;
        }

        Debug.Log("📡 [EventManager] 이벤트 감지 시작...");
        _eventConditions.Initialize();

        _eventConditions.IsCleared
            .Subscribe(isSatisfied =>
            {
                if (isSatisfied)
                {
                    Debug.Log("🔥 [EventManager] 모든 이벤트 조건 충족! 이벤트 실행 시작...");
                    _eventResults.ExecuteAll();
                }
            })
            .AddTo(this);
    }
}
