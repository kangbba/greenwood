using System.Collections.Generic;
using UnityEngine;

public class TestStoryMorning : Story
{
    public override List<Element> UpdateElements { get; } = new List<Element>
    {
        new CharacterEnter(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront, CharacterLocation.Left1, 1f),
        new CharacterEnter(ECharacterName.Lisa, KateEmotionType.Happy, KatePoseType.HandsFront, CharacterLocation.Right1, 1f),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "요즘 라이언이랑 자주 다니더라?",
                "그렇게 친했었나? 난 몰랐네?",
            }
        ),

        new Dialogue(ECharacterName.Lisa,
            new List<string>
            {
                "어머, 그냥 우연히 자주 마주친 것뿐인데?",
                "그렇다고 안 친하게 지낼 이유도 없잖아?",
            }
        ),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Angry, KatePoseType.ArmCrossed),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "헤에~ 그렇구나? 근데 진짜 신기하네.",
                "라이언이랑 말도 잘 통하는 것 같던데?",
            }
        ),

        new Dialogue(ECharacterName.Lisa,
            new List<string>
            {
                "그럼~! 라이언이랑 얘기하면 시간 가는 줄도 모르겠더라.",
                "서로 관심사도 비슷하고 말이야.",
            }
        ),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Angry, KatePoseType.ArmCrossed),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "하~ 그래? 관심사까지 비슷하고?",
                "그럼 뭐, 앞으로는 매일 같이 붙어 다니겠네?",
            }
        ),

        new Dialogue(ECharacterName.Lisa,
            new List<string>
            {
                "아, 그럴까? 라이언도 괜찮다면야~",
            }
        ),

        new Dialogue(ECharacterName.Ryan,
            new List<string>
            {
                "응? 뭐가? 난 그냥—",
            }
        ),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Angry, KatePoseType.ArmCrossed),

        new Dialogue(ECharacterName.Kate, 
            new List<string>
            {
                "됐어, 됐어! 나는 빵이나 굽고 있어야겠네.",
                "너희 둘이서 재미있게 놀아~ 난 방해하면 안 되니까!",
            }
        ),

        new CharacterExit(ECharacterName.Kate),

        new CharacterMove(ECharacterName.Lisa, CharacterLocation.Center),
        
        new Dialogue(ECharacterName.Mono,
            new List<string>
            {
                "…케이트, 화난 것 같은데?",
            }
        ),

        new Dialogue(ECharacterName.Lisa,
            new List<string>
            {
                "어머, 그런가? 난 그냥 사실대로 말했을 뿐인데?",
            }
        ),
    };
}
