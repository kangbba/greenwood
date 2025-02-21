using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImaginationExit : Element
{
    private bool _isOverlay;
    private float _duration;

    public ImaginationExit(bool isOverlay, float duration = 1f)
    {
        _isOverlay = isOverlay;
        _duration = duration;
    }

    public override async UniTask ExecuteAsync()
    {
        ImaginationManager.Instance.FadeOutBackgroundPanel(_isOverlay, _duration);
        ImaginationManager.Instance.DestroyCurrentImage(_isOverlay, _duration);
        await UniTask.WaitForSeconds(_duration);
    }

    public override void ExecuteInstantly()
    {
        _duration = 0f;
        ExecuteAsync().Forget();
    }
}
