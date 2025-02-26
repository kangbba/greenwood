using System.Collections.Generic;
using UnityEngine;
using static CharacterEnums;

public class FirstBakeryVisit : Scenario
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Smile, KatePoseType.HandsFront, CharacterLocation.Center, 1f),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "오! 새로운 얼굴이네? 반가워! 여긴 내 빵집이야!",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "오늘 빵이 엄청 맛있게 구워졌어! 하나 먹어볼래?",
        }),

        new ItemGain("ButterCroissant"),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "내가 구운 버터 크루아상이야! 처음 보는 손님한테는 서비스지~",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Surprised, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "아참, 난 케이트야! 혹시 여행 온 거야?",
        }),

        new Choice("케이트의 질문에 어떻게 대답할까?", new List<ChoiceOption>
        {
            new ChoiceOption("응, 여행 중이야", new List<Element>
            {
                new EmotionChange(ECharacterName.Kate, KateEmotionType.YeahRight, KatePoseType.HandsFront),
                new Dialogue(ECharacterName.Kate, new List<string>
                {
                    "여행이라니 멋지다! 여기 오래 머물 거야?",
                })
            }),
            new ChoiceOption("아니, 그냥 머물고 있어", new List<Element>
            {
                new EmotionChange(ECharacterName.Kate, KateEmotionType.Smile, KatePoseType.HandsFront),
                new Dialogue(ECharacterName.Kate, new List<string>
                {
                    "오, 그럼 앞으로 자주 보겠네! 잘 부탁해!",
                })
            }),
        }),

        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "아무튼, 여기 빵 정말 맛있으니까 자주 와! 내가 맛있는 거 많이 만들어 줄게~",
        })
    };
}
