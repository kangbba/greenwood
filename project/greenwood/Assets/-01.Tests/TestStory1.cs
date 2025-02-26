using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class TestStory1 : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Angry, KatePoseType.HandsFront, CharacterLocation.Center, 1f),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "뭐야, 장난해?",
        }),

        new ItemGain("TestItem"),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Anyway, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "뭐, 어쨌든 상관없어.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Concerned, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "괜찮아? 무슨 일 있는 거 아냐?",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Cry, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "으흑... 이런 거 너무 싫어...",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Disappointed, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "하아... 이럴 줄 알았어.",
        }),
    };
}
