using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AllScenarioElementsClear : Element
{
    private float _duration;

    public AllScenarioElementsClear(float duration)
    {
        _duration = duration;
    }

    public override async UniTask ExecuteAsync()
    {
        await new ParallelElement(
            new List<Element>(){
                new SFXClear(_duration),
                new BGMClear(_duration),
                new ImaginationOverlayClear(_duration),
                new ImaginationUnderlayClear(_duration),
                new DialogueClear(_duration),
                new AllCharactersClear(_duration),
            }
        ).ExecuteAsync();
    }

    public override void ExecuteInstantly()
    {
        _duration = 0;
        Execute();
    }
}
