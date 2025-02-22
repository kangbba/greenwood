using Cysharp.Threading.Tasks;
using UnityEngine;

public class BGMClear : Element
{
    private float _fadeDuration;

    public BGMClear(float fadeDuration = 1f)
    {
        _fadeDuration = fadeDuration;
    }

    public override async UniTask ExecuteAsync()
    {
        SoundManager.Instance.StopBGM(_fadeDuration);
        await UniTask.WaitForSeconds(_fadeDuration);
    }

    public override void ExecuteInstantly()
    {
        SoundManager.Instance.StopBGM();
    }
}
