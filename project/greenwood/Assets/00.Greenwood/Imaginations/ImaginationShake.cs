using Cysharp.Threading.Tasks;
using UnityEngine;

public class ImaginationShake : Element
{
    private bool _isOverlay;
    private float _strength;
    private float _duration;

    public ImaginationShake(bool isOverlay, float strength = 10f, float duration = 0.5f)
    {
        _isOverlay = isOverlay;
        _strength = strength;
        _duration = duration;
    }

    public override async UniTask ExecuteAsync()
    {
        AnimationImage curImage = ImaginationManager.Instance.GetCurrentImage(_isOverlay);
        if (curImage == null)
        {
            Debug.LogWarning($"[ImaginationShake] No active {( _isOverlay ? "Overlay" : "Underlay")} image to shake.");
            return;
        }

        curImage.Shake(_strength, _duration);
        await UniTask.WaitForSeconds(_duration);
    }

    public override void ExecuteInstantly()
    {
        AnimationImage curImage = ImaginationManager.Instance.GetCurrentImage(_isOverlay);
        curImage?.Shake(_strength, 0f);
    }
}
