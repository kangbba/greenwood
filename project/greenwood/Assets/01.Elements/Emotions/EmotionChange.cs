using Cysharp.Threading.Tasks;
using UnityEngine;
using static CharacterEnums;

public class EmotionChange : Element
{
    private ECharacterName _characterName;
    private string _emotionID;
    private string _poseID;
    private float _duration;

   // 생성자: 문자열 기반 (직접 지정)
    public EmotionChange(ECharacterName characterName, string emotionID, string poseID, float duration = 1f)
    {
        _characterName = characterName;
        _emotionID = emotionID;
        _poseID = poseID;
        _duration = duration;
    }

    // 생성자: KateEmotionType & KatePoseType 사용
    public EmotionChange(ECharacterName characterName, KateEmotionType emotionType, KatePoseType poseType, float duration = 1f)
    {
        _characterName = characterName;
        _emotionID = emotionType.ToString();
        _poseID = poseType.ToString();
        _duration = duration;
    }

    // 생성자: KateEmotionType & KatePoseType 사용
    public EmotionChange(ECharacterName characterName, LisaEmotionType emotionType, LisaPoseType poseType, float duration = 1f)
    {
        _characterName = characterName;
        _emotionID = emotionType.ToString();
        _poseID = poseType.ToString();
        _duration = duration;
    }

    public override void ExecuteInstantly()
    {
        _duration = 0;
        ExecuteAsync().Forget();
    }

    public override async UniTask ExecuteAsync()
    {
        // 캐릭터 가져오기
        Character activeCharacter = CharacterManager.Instance.GetActiveCharacter(_characterName);
        if (activeCharacter == null)
        {
            Debug.LogWarning($"[EmotionChange] No active character found with ID {_characterName}.");
            return;
        }
        activeCharacter.SetEmotion(_emotionID, _duration);
        activeCharacter.SetPose(_poseID, _duration);
        await UniTask.WaitForSeconds(_duration);
    }
}
