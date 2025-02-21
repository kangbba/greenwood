using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImaginationEnter : Element
{
    private bool _isOverlay;
    private string _imageID;
    private float _duration;
    private float _scaleMultiplier;

    public ImaginationEnter(bool isOverlay, string imageID, float duration = 1f, float scaleMultiplier = 1.2f)
    {
        _isOverlay = isOverlay;
        _imageID = imageID;
        _duration = duration;
        _scaleMultiplier = scaleMultiplier;
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 배경 패널 페이드 인
        ImaginationManager.Instance.FadeInBackgroundPanel(_isOverlay, _duration);

        // ✅ 이미지 생성 후 반환
        AnimationImage imagination = ImaginationManager.Instance.CreateImageAndShow(_imageID, _isOverlay, _duration);

        if (imagination != null)
        {
            // ✅ 스케일 애니메이션 적용 (기본값: 1 → _scaleMultiplier)
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
