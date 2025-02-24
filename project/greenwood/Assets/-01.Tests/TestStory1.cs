using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class TestStory1 : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        // ✅ 케이트의 밝은 인사
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront, CharacterLocation.Center, 1f),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "좋아, 오늘도 완벽한 아침이야!",
            "라이언, 너도 그렇게 생각하지 않아?",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "음… 아직 완벽하진 않은데.",
        }),
    };
}
