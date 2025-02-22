using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImaginationOverlayEnter : Element
{
    private string _imageID;
    private float _duration;
    private float _scaleMultiplier;

    public ImaginationOverlayEnter(string imageID, float duration = 1f, float scaleMultiplier = 1.1f)
    {
        _imageID = imageID;
        _duration = duration;
        _scaleMultiplier = scaleMultiplier;
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 오버레이 배경 페이드 인
        ImaginationManager.Instance.FadeInBackgroundPanel(true, _duration);

        // ✅ 이미지 생성 후 반환
        AnimationImage imagination = ImaginationManager.Instance.CreateImageAndShow(_imageID, true, _duration);

        if (imagination != null)
        {
            // ✅ 스케일 애니메이션 적용
            imagination.Scale(_scaleMultiplier, _duration);
        }

        await UniTask.WaitForSeconds(_duration);
    }

    public override void ExecuteInstantly()
    {
        _duration = 0f;
        ExecuteAsync().Forget();
    }
}
