using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class TestStory4 : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        
        new EmotionChange(ECharacterName.Kate, KateEmotionType.Embrassed, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "에에?! 무슨 소리야!",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "헤헤, 좋은 일이 생길 것 같아.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Raged, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "으으으... 나 진짜 화났어!",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Sad, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "조금... 외롭네.",
        }),
    };
}
