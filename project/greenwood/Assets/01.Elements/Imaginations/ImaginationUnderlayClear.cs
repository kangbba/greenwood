using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImaginationUnderlayClear : Element
{
    private float _duration;

    public ImaginationUnderlayClear(float duration = 1f)
    {
        _duration = duration;
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 언더레이 배경 페이드 아웃
        ImaginationManager.Instance.FadeOutBackgroundPanel(false, _duration);

        // ✅ 현재 언더레이 이미지 제거
        ImaginationManager.Instance.DestroyCurrentImage(false, _duration);

        await UniTask.WaitForSeconds(_duration);
    }

    public override void ExecuteInstantly()
    {
        _duration = 0f;
        ExecuteAsync().Forget();
    }
}
