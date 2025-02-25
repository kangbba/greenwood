using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class AllCharactersClear : Element
{
    private float _duration;

    public AllCharactersClear(float duration = .3f)
    {
        this._duration = duration;
    }

    public override void ExecuteInstantly()
    {
        _duration = 0;
        ExecuteAsync().Forget(); // 즉시 실행
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 현재 활성화된 캐릭터 리스트 복사 (ToList)
        var activeCharactersCopy = CharacterManager.Instance.ActiveCharacters.ToList();

        // ✅ 각 캐릭터에 대해 CharacterExit 실행
        foreach (var character in activeCharactersCopy)
        {
            new CharacterExit(character.CharacterName, _duration).Execute();
        }

        // ✅ 모든 캐릭터가 사라지는 시간 동안 대기
        await UniTask.WaitForSeconds(_duration);
    }
}
