
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class CharacterMove : Element
{
    private ECharacterName _characterName; // ✅ 캐릭터 Enum으로 변경
    private CharacterLocation _targetLocation; // ✅ Enum으로 변경 (목표 위치)
    private float _duration;
    private Ease _easeType;

    public CharacterMove(ECharacterName characterName, CharacterLocation targetLocation, float duration = 1f, Ease easeType = Ease.OutQuad)
    {
        this._characterName = characterName;
        this._targetLocation = targetLocation;
        this._duration = duration;
        this._easeType = easeType;
    }

    public override void ExecuteInstantly()
    {
        _duration = 0;
        ExecuteAsync().Forget(); // 즉시 실행
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 캐릭터 가져오기 (ECharacterName 기반)
        Character character = CharacterManager.Instance.GetActiveCharacter(_characterName);

        if (character == null)
        {
            Debug.LogWarning($"[CharacterMove] No active character found with Name: {_characterName} to move.");
            return;
        }

        // ✅ 캐릭터 이동 (CharacterLocation Enum 사용)
        character.MoveToLocationX(_targetLocation, _duration, _easeType);

        // ✅ 이동 완료 후 대기 (WaitForSeconds 사용)
        await UniTask.WaitForSeconds(_duration);
    }
}
