using TMPro;
using UniRx;
using UnityEngine;

public class ActionPointDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionPointText; // ✅ UI 텍스트 (TMP)

    private void Start()
    {
        // ✅ 현재 행동력을 구독하여 UI에 업데이트
        ActionManager.Instance.CurrentActionPointNotifier
            .Subscribe(actionPoints =>
            {
                _actionPointText.text = $"{actionPoints} / 100";
            })
            .AddTo(this);
    }
}
