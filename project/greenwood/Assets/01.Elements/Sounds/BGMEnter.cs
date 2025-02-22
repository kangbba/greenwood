using Cysharp.Threading.Tasks;
using UnityEngine;

public class BGMEnter : Element
{
    private string _soundID;
    private float _volume;
    private float _fadeDuration;

    public BGMEnter(string soundID, float volume = 1f, float fadeDuration = 1f)
    {
        _soundID = soundID;
        _volume = volume;
        _fadeDuration = fadeDuration;
    }

    public override async UniTask ExecuteAsync()
    {
        SoundManager.Instance.PlaySound(_soundID, isBgm: true, _volume, loop: true, _fadeDuration);
        await UniTask.Yield(); // ✅ 즉시 실행 후 다음 실행 대기
    }

    public override void ExecuteInstantly()
    {
        SoundManager.Instance.PlaySound(_soundID, isBgm: true, _volume, loop: true, _fadeDuration);
    }
}
