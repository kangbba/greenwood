using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImaginationUnderlayEnter : Element
{
    private string _imageID;
    private float _duration;
    private float _scaleMultiplier;

    public ImaginationUnderlayEnter(string imageID, float duration = 1f, float scaleMultiplier = 1.2f)
    {
        _imageID = imageID;
        _duration = duration;
        _scaleMultiplier = scaleMultiplier;
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 언더레이 배경 페이드 인
        ImaginationManager.Instance.FadeInBackgroundPanel(false, _duration);

        // ✅ 이미지 생성 후 반환
        AnimationImage imagination = ImaginationManager.Instance.CreateImageAndShow(_imageID, false, _duration);

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
