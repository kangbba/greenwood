using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [SerializeField, ListDrawerSettings(ShowFoldout = true)]
    private List<EventPair> _eventPairs; // ✅ 여러 개의 이벤트 저장

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
        Debug.Log("📡 [EventManager] 이벤트 감지 시작...");

        foreach (var eventPair in _eventPairs)
        {
            eventPair.IsSatisfiedAllStream()
                .Subscribe(isSatisfied =>
                {
                    if (isSatisfied)
                    {
                        Debug.Log("🔥 [EventManager] 이벤트 조건 충족! 이벤트 실행...");
                        eventPair.Execute();
                    }
                })
                .AddTo(this);
        }
    }
}
