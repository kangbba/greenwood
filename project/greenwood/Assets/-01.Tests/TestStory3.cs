using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class TestStory3 : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Shy, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "으응... 그, 그런 말 하지 마...",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Smile, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "후후, 그런 말 들으니 기분 좋네.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Surprised, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "에? 진짜?!",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.YeahRight, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "푸웃, 그런 거 믿는 거야?",
        }),
    };
}
