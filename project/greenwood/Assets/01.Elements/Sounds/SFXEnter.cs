using Cysharp.Threading.Tasks;
using UnityEngine;

public class SFXEnter : Element
{
    private string _soundID;
    private float _volume;
    private bool _loop;

    public SFXEnter(string soundID, float volume = 1f, bool loop = false)
    {
        _soundID = soundID;
        _volume = volume;
        _loop = loop;
    }

    public override async UniTask ExecuteAsync()
    {
        SoundManager.Instance.PlaySound(_soundID, isBgm: false, _volume, _loop);
        await UniTask.Yield(); // ✅ 즉시 실행 후 다음 실행 대기
    }

    public override void ExecuteInstantly()
    {
        SoundManager.Instance.PlaySound(_soundID, isBgm: false, _volume, _loop);
    }
}
