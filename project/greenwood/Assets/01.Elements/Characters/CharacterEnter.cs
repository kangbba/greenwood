using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

public enum CharacterLocation
{
    Left2, Left1, Center, Right1, Right2
}

public class CharacterEnter : Element
{
    private ECharacterName _characterName;
    private string _initialEmotionID;
    private string _initialPoseID;
    private CharacterLocation _location;
    private float _duration;
    public CharacterEnter(ECharacterName characterName, string emotionID, string poseID, CharacterLocation location, float duration = 1f)
    {
        _characterName = characterName;
        _initialEmotionID = emotionID;
        _initialPoseID = poseID;
        _location = location;
        _duration = duration;
    }

    // KateEmotionType & KatePoseType
    public CharacterEnter(ECharacterName characterName, KateEmotionType kateEmotionType, KatePoseType katePoseType, CharacterLocation location, float duration = 1f)
    {
        _characterName = characterName;
        _initialEmotionID = kateEmotionType.ToString();
        _initialPoseID = katePoseType.ToString();
        _location = location;
        _duration = duration;
    }


    public override void ExecuteInstantly()
    {
        _duration = 0;
        ExecuteAsync().Forget();
    }

    public override async UniTask ExecuteAsync()
    {
        // 캐릭터 생성 (이미 존재하면 기존 캐릭터 반환)
        Character character = CharacterManager.Instance.CreateCharacter(_characterName);
        if (character == null)
        {
            Debug.LogError($"[CharacterEnter] 캐릭터 `{_characterName}` 생성 실패");
            return;
        }
        character.MoveToLocationX(_location ,0f);
        character.Init(_initialEmotionID, _initialPoseID, _duration);
        // 페이드 인 애니메이션 (캐릭터가 처음 등장할 때)
        await UniTask.WaitForSeconds(_duration);
    }

}
