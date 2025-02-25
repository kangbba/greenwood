using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class KateEmotionChangePractice : Scenario
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
