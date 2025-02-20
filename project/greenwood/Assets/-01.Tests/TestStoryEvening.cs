using System.Collections.Generic;
using UnityEngine;

public class TestStoryEvening : Story
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Shy, KatePoseType.HandsBack, CharacterLocation.Center, 1f),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "후아아… 하루가 벌써 이렇게 지나가다니!",
                "라이언, 오늘도 고생 많았어. 잠깐 쉬었다 가는 건 어때?",
            }
        ),

        new Dialogue(ECharacterName.Ryan,
            new List<string>
            {
                "괜찮아? 너야말로 하루 종일 서 있었잖아.",
            }
        ),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Shy, KatePoseType.HandsFront),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "하하, 뭐… 가끔 이렇게 조용한 시간이 좋기도 하지.",
                "이럴 땐 따뜻한 허브티 한 잔이 최고라구!",
            }
        ),

        new Dialogue(ECharacterName.Mono,
            new List<string>
            {
                "이 분위기… 묘하게 차분하네.",
            }
        ),

        new CharacterExit(ECharacterName.Kate),
    };
}
