using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class TestStory4 : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        // ✅ 케이트의 밝은 인사
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Serious, KatePoseType.HandsFront, CharacterLocation.Center, 1f),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "나 고민있어",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "왜?",
        }),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "조금만 더 가까이오면 말해줄수도",
        }),
    };
}
