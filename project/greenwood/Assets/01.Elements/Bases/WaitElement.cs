using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 일정 시간 동안 대기하는 Element.
/// </summary>
public class WaitElement : Element
{
    private readonly float _duration; // 대기 시간 (초)

    public WaitElement(float duration)
    {
        _duration = Mathf.Max(0, duration); // ✅ 음수 방지
    }

    public override void ExecuteInstantly()
    {
        // ✅ 즉시 실행 시 대기 시간을 무시하고 바로 다음 실행
    }

    public override async UniTask ExecuteAsync()
    {
        await UniTask.WaitForSeconds(_duration);
    }
}
