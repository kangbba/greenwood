using UnityEngine;
using Cysharp.Threading.Tasks;
using static CharacterEnums;

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

    public ECharacterName CharacterName { get => _characterName; }

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
// ✅ Lisa 전용 생성자 추가
    public CharacterEnter(ECharacterName characterName, LisaEmotionType lisaEmotionType, LisaPoseType lisaPoseType, CharacterLocation location, float duration = 1f)
    {
        _characterName = characterName;
        _initialEmotionID = lisaEmotionType.ToString();
        _initialPoseID = lisaPoseType.ToString();
        _location = location;
        _duration = duration;
    }

    // ✅ 공용(Common) 캐릭터 전용 생성자 추가
    public CharacterEnter(ECharacterName characterName, CommonEmotionType commonEmotionType, CommonPoseType commonPoseType, CharacterLocation location, float duration = 1f)
    {
        _characterName = characterName;
        _initialEmotionID = commonEmotionType.ToString();
        _initialPoseID = commonPoseType.ToString();
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
        CharacterSetting characterSetting = CharacterManager.Instance.GetCharacterSetting(_characterName);
        if (characterSetting == null)
        {
            Debug.LogError($"[CharacterEnter] characterSetting 반환 실패");
            return;
        }
        character.MoveToLocationY(characterSetting.Height, 0f);
        character.MoveToLocationX(_location ,0f);
        character.Init(_initialEmotionID, _initialPoseID, _duration);
        // 페이드 인 애니메이션 (캐릭터가 처음 등장할 때)
        await UniTask.WaitForSeconds(_duration);
    }

}
