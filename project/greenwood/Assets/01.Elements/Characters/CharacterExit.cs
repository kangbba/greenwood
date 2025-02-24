using UnityEngine;
using Cysharp.Threading.Tasks;
using static CharacterEnums;

public class CharacterExit : Element
{
    private ECharacterName _characterName;
    private float _duration;

    public CharacterExit(ECharacterName characterName, float duration = 2f)
    {
        _characterName = characterName;
        _duration = duration;
    }

    public override void ExecuteInstantly()
    {
        _duration = 0;
        ExecuteAsync().Forget();
    }

    public override async UniTask ExecuteAsync()
    {
        // 활성화된 캐릭터 가져오기
        Character character = CharacterManager.Instance.GetActiveCharacter(_characterName);
        if (character == null)
        {
            Debug.LogWarning($"[CharacterExit] `{_characterName}` 캐릭터가 활성화되어 있지 않습니다.");
            return;
        }

        // 캐릭터 페이드 아웃 후 제거
        CharacterManager.Instance.FadeOutAndDestroyCharacter(_characterName, _duration);
        await UniTask.WaitForSeconds(_duration);
    }
}
