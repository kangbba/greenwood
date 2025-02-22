using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImaginationOverlayClear : Element
{
    private float _duration;

    public ImaginationOverlayClear(float duration = 1f)
    {
        _duration = duration;
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 오버레이 배경 페이드 아웃
        ImaginationManager.Instance.FadeOutBackgroundPanel(true, _duration);

        // ✅ 현재 오버레이 이미지 제거
        ImaginationManager.Instance.DestroyCurrentImage(true, _duration);

        await UniTask.WaitForSeconds(_duration);
    }

    public override void ExecuteInstantly()
    {
        _duration = 0f;
        ExecuteAsync().Forget();
    }
}
