using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using static CharacterEnums;

public class DialogueClear : Element
{
    private float _duration;
    public DialogueClear(float duration = 1f)
    {
        _duration = duration;
    }
    public override void ExecuteInstantly()
    {
        DialogueService.HideDialogue(0f);
    }
    public override async UniTask ExecuteAsync()
    {
        DialogueService.HideDialogue(_duration);
        await UniTask.WaitForSeconds(_duration);
    }
}
