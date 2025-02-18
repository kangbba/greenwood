using UnityEngine;
using UniRx;

public class DataController : MonoBehaviour
{
    public static DataController Instance { get; private set; }

    // ✅ ReactiveProperty를 사용하여 행동력 관리 (즉시 반응 가능)
    public ReactiveProperty<int> ActionPoint { get; private set; } = new ReactiveProperty<int>(0);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// ✅ 행동력 설정 (자동으로 반응)
    /// </summary>
    public void SetActionPoint(int value)
    {
        ActionPoint.Value = value; // ✅ 변경하면 자동으로 구독자(UI)에 반영됨
    }
}
