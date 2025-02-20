using System.Collections.Generic;
using UnityEngine;

public class TestStoryMorning : Story
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront, CharacterLocation.Center, 1f),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "좋은 아침이야, 라이언!",
                "오늘도 일찍 왔네? 혹시 모닝 크루아상 한 조각 어때?",
            }
        ),

        new Dialogue(ECharacterName.Ryan,
            new List<string>
            {
                "크루아상? 좋아. 갓 구운 거지?",
            }
        ),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Exited, KatePoseType.HandsBack),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "당연하지~! 아침엔 역시 바삭한 크루아상이 최고니까!",
                "특히, 따뜻한 커피랑 같이 먹으면… 캬!",
            }
        ),

        new Dialogue(ECharacterName.Mono,
            new List<string>
            {
                "케이트는 정말 아침부터 에너지가 넘치네.",
            }
        ),

        new CharacterExit(ECharacterName.Kate),
    };
}
