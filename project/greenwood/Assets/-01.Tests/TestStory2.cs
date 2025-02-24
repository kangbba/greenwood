using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class TestStory2 : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        // ✅ 케이트의 밝은 인사
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront, CharacterLocation.Center, 1f),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "요새 표정이 안좋아보여",
            "무슨 일 있는걸까나?",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "아니야 아무것도...",
        }),
    };
}
