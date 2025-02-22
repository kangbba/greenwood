using Cysharp.Threading.Tasks;
using UnityEngine;

public class SFXClear : Element
{
    private float _fadeDuration;

    public SFXClear(float fadeDuration = 1f)
    {
        _fadeDuration = fadeDuration;
    }

    public override async UniTask ExecuteAsync()
    {
        SoundManager.Instance.StopSFX(_fadeDuration);
        await UniTask.WaitForSeconds(_fadeDuration);
    }

    public override void ExecuteInstantly()
    {
        SoundManager.Instance.StopSFX();
    }
}
