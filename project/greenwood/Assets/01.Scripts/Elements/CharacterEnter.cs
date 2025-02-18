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
        SetCharacterPosition(character, _location);
        character.Init(_initialEmotionID, _initialPoseID, _duration);
        // 페이드 인 애니메이션 (캐릭터가 처음 등장할 때)
        await UniTask.WaitForSeconds(_duration);
    }

    /// <summary>
    /// 캐릭터 위치 설정
    /// </summary>
    private void SetCharacterPosition(Character character, CharacterLocation location)
    {
        float screenWidth = UIManager.Instance.GameCanvas.GetComponent<RectTransform>().rect.width;
        float targetX = GetPositionX(location, screenWidth);

        RectTransform characterTransform = character.GetComponent<RectTransform>();
        characterTransform.anchoredPosition = new Vector2(targetX, characterTransform.anchoredPosition.y);
    }

    /// <summary>
    /// `CharacterLocation` Enum에 따라 X 좌표 계산
    /// </summary>
    private float GetPositionX(CharacterLocation location, float screenWidth)
    {
        return location switch
        {
            CharacterLocation.Left2 => screenWidth * -0.4f,
            CharacterLocation.Left1 => screenWidth * -0.2f,
            CharacterLocation.Center => 0f,
            CharacterLocation.Right1 => screenWidth * 0.2f,
            CharacterLocation.Right2 => screenWidth * 0.4f,
            _ => 0f
        };
    }
}
