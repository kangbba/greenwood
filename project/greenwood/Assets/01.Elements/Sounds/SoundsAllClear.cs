using Cysharp.Threading.Tasks;
using UnityEngine;

public class SoundsAllClear : Element
{
    private float _fadeDuration;

    public SoundsAllClear(float fadeDuration = 1f)
    {
        _fadeDuration = fadeDuration;
    }

    public override async UniTask ExecuteAsync()
    {
        SoundManager.Instance.StopBGM(_fadeDuration);
        SoundManager.Instance.StopSFX(_fadeDuration);
        await UniTask.WaitForSeconds(_fadeDuration);
    }

    public override void ExecuteInstantly()
    {
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.StopSFX();
    }
}
