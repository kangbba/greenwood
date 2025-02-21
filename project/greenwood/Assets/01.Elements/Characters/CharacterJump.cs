using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class CharacterJump : Element
{
    private ECharacterName _characterName; // ✅ 캐릭터 Enum 사용
    private float _duration;

    public CharacterJump(ECharacterName characterName, float duration = .5f)
    {
        this._characterName = characterName;
        _duration = duration;
    }

    public override void ExecuteInstantly()
    {
        _duration = 0;
        ExecuteAsync().Forget(); // 즉시 실행
    }

    public override async UniTask ExecuteAsync()
    {
        // ✅ 캐릭터 가져오기
        Character character = CharacterManager.Instance.GetActiveCharacter(_characterName);

        if (character == null)
        {
            Debug.LogWarning($"[CharacterJump] Failed to find active character: {_characterName}");
            return;
        }

        // ✅ 점프 애니메이션 실행
        character.Jump(100, _duration);

        // ✅ 애니메이션 완료까지 대기
        await UniTask.WaitForSeconds(_duration);
    }
}
