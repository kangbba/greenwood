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
            _dialoguePlayer = Object.Instantiate(UIManager.Instance.DialoguePlayerPrefab, UIManager.Instance.PopupCanvas.transform);
        }

        // 🔥 Init을 통해 초기화 후 실행
        _dialoguePlayer.Init(dialogue.CharacterName, dialogue.Sentences, dialogue.Speed);
        await _dialoguePlayer.PlayDialogue();
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
