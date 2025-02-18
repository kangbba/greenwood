using System.Collections.Generic;
using UnityEngine;

public class TestStory : Story
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Concerned, KatePoseType.HandsBack, CharacterLocation.Center, 1f),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "또 왔어? 바쁜 예비 신랑님이 왜 이 시간에 여기 앉아 있는 거야?",
                "설마, 결혼 준비가 귀찮다고 여기로 온 건 아니지?",
            }
        ),
        
        new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.ArmCrossed),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "...아니, 뭐. 네가 여기 오는 게 나쁘다는 건 아니야. 다만 신부님이 알까봐...",
            }
        ),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Concerned, KatePoseType.ArmCrossed),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "근데 말이야... 네가 나한테 오면 올수록, 나도 헷갈린단 말이지?",
                "이게 그냥 오래된 습관인지, 아니면..."
            }
        ),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "뭐, 됐어! 고민은 네가 하고, 나는 주문이나 받을래.",
                "근데 말이야... 오늘은 커피 말고, 다른 걸로 해볼래? 같은 거만 반복하면 재미없잖아?"
            }
        ),
    };
}
