using Cysharp.Threading.Tasks;
using UnityEngine;

public static class DialogueService
{
    private static DialoguePlayer _dialoguePlayer;

    /// <summary>
    /// 다이얼로그 실행
    /// </summary>
    public static async UniTask ShowDialogue(Dialogue dialogue)
    {
        if (_dialoguePlayer == null)
        {
            _dialoguePlayer = Object.Instantiate(UIManager.Instance.DialoguePlayerPrefab, UIManager.Instance.UICanvas.DialoguePlayerLayer);
        }
        
        Character character = CharacterManager.Instance.GetActiveCharacter(dialogue.CharacterName);
        CharacterSetting characterSetting = CharacterManager.Instance.GetCharacterSetting(dialogue.CharacterName);
       
        string displayName = characterSetting.DisplayName;
        Color ownerTextColor = characterSetting.CharacterColor;
        Color ownerBackgroundColor = Color.Lerp(Color.black, ownerTextColor, .1f);

        _dialoguePlayer.Init(displayName, ownerTextColor, ownerBackgroundColor, dialogue.Sentences, dialogue.Speed);
        await _dialoguePlayer.PlayDialogue(
            OnStart : ()=> {
                character?.PlayMouthWithCurrentEmotion(true);
            }, 
            OnPunctuationPause : ()=> {
                character?.PlayMouthWithCurrentEmotion(false);
            }, 
            OnPunctuationResume : ()=> {
                character?.PlayMouthWithCurrentEmotion(true);
            }, 
            OnComplete : ()=>{
                character?.PlayMouthWithCurrentEmotion(false);
            }
        );
    }

    public static void HideDialogue(float duration)
    {
        _dialoguePlayer.FadeOut(duration);
    }

    /// <summary>
    /// 현재 다이얼로그를 즉시 완료
    /// </summary>
    public static void SkipCurrentDialogue()
    {
        if(_dialoguePlayer != null){
            _dialoguePlayer.SkipCurrentDialogue();
        }
    }

    /// <summary>
    /// 다이얼로그 UI를 강제로 닫음
    /// </summary>
    public static void ForceCloseDialogue()
    {
        if (_dialoguePlayer != null)
        {
            Object.Destroy(_dialoguePlayer.gameObject);
            _dialoguePlayer = null;
        }
    }
}
