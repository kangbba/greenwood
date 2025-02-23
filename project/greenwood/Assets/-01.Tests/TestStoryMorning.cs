using System.Collections.Generic;
using UnityEngine;

public class TestStoryMorning : Scenario
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

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Surprised, KatePoseType.HandsBack),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "에?! 뭐가 부족한데?",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "크루아상. 아직 못 먹어봤잖아.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Rage, KatePoseType.ArmCrossed),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "흥! 그렇게 중요한 거였어? 빵 없으면 하루가 망하는 거야?",
        }),

        new MonoDialogue(new List<string>
        {
            "케이트는 뺨을 부풀리며 귀엽게 투덜거렸다.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.YeahRight, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "알겠어, 알겠어. 특별히 갓 구운 크루아상을 준비했지!",
        }),

        // ✅ 상상 연출: 갓 구운 크루아상의 향
        new ImaginationOverlayEnter("FreshBread", 1.5f),
        new MonoDialogue(new List<string>
        {
            "따뜻한 버터 향이 퍼지며 공기를 가득 채웠다.",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "이 향기… 이건 반칙이잖아.",
        }),

        new ImaginationOverlayClear(1f),

        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "그치? 그러니까 한 입만 먹어봐!",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "좋아, 이번엔 기대해볼까?",
        }),

        new MonoDialogue(new List<string>
        {
            "라이언은 케이트가 내민 크루아상을 받아 들었다.",
            "겉은 바삭하고 속은 부드러운 완벽한 식감이었다.",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "…이제 완벽한 아침이야.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Shy, KatePoseType.HandsBack),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "으, 으응? 그, 그래?",
            "그렇게까지 맛있진 않을 텐데…?",
        }),

        new MonoDialogue(new List<string>
        {
            "케이트는 괜히 머리를 긁적이며 시선을 피했다.",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "뭐야, 방금 반응. 설마 부끄러운 거야?",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Rage, KatePoseType.ArmCrossed),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "누, 누가 부끄럽다는 거야! 그런 거 아니거든?!",
        }),

        new MonoDialogue(new List<string>
        {
            "케이트는 얼굴을 붉히며 허둥지둥 손을 흔들었다.",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "알았어, 알았어. 그러니까 크루아상 하나만 더 줘.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Serious, KatePoseType.HandsFront),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "…너 지금 내 빵을 노리는 거야?",
        }),

        new Dialogue(ECharacterName.Ryan, new List<string>
        {
            "아니, 내 하루를 완벽하게 만들려고.",
        }),

        new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsBack),
        new Dialogue(ECharacterName.Kate, new List<string>
        {
            "흥~ 어쩔 수 없네. 특별히 한 개 더!",
        }),

        new MonoDialogue(new List<string>
        {
            "창밖으로 아침 햇살이 퍼지고 있었다.",
            "케이트와 함께하는 이 순간이, 예상보다 훨씬 따뜻하게 느껴졌다.",
        }),
    };
}
