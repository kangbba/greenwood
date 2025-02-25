using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class TestStory3 : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        // ✅ 케이트의 밝은 인사
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Shy, KatePoseType.HandsFront, CharacterLocation.Center, 1f),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "...",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "무슨일이야?",
        }),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "아무것도 아니야",
        }),
    };
}
