// using System.Collections.Generic;
// using Game.Enums;
// using UnityEngine;

// public class TestStory : Story
// {
//     public override List<Element> UpdateElements { get; } = new List<Element>
//     {
//         new CharacterEnter(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront, CharacterLocation.Center, 1f),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "아라~? 라이언! 오늘도 와줬구나♪",
//                 "이렇게 매일 오는 거면… 설마~ 나한테 반한 거 아니야? 후훗!",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "…아침부터 기분이 좋네.",
//                 "이 사람, 손님한테도 이렇게 장난치는 걸까, 아니면 나한테만 이러는 걸까?",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "하하, 설마. 그냥 여기 빵이 맛있어서 오는 거지.",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Exited, KatePoseType.HandsBack),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "역시 내 빵의 매력을 알아보는구나! 라이언은 보는 눈이 있어~♪",
//                 "그런데 오늘은 말이야, ‘특별한 메뉴’가 준비되어 있다구!",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "특별한 메뉴…?",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.YeahRight, KatePoseType.ArmCrossed),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "후훗, 바로바로~ 케이트 특제 ‘{비밀 레시피 빵}’!",
//                 "이거야말로 진정한 {한정판} 메뉴! 자, 특별히 너한테 먼저 맛보게 해줄게!",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "…‘비밀 레시피’라는 말이 제일 무서운 단어인데.",
//                 "지난번에도 ‘{특별 레시피}’라고 만든 빵이 지나치게 단맛 폭탄이었던 걸 잊었나?",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "...혹시 이번엔 무슨 재료가 들어갔는지 물어봐도 돼?",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Exited, KatePoseType.HandsBack),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "당연하지~! 사과, 꿀, 바닐라 크림, 그리고!",
//                 "비장의 재료, ‘{깜짝 재료}’를 살짝♪",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "…깜짝 재료?",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "이제 확신했다. ‘살짝’이 아닐 것이다.",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "자자~ 너무 긴장하지 마! 이게 의외로 조합이 좋다니까?",
//                 "자, 시식 타임~!",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "...케이트.",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "이거 먹으면… 내 위장은 무사할까?",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Shy, KatePoseType.HandsBack),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "에에~? 그렇게 의심하면 곤란한데?",
//                 "이거, 실은 한정 메뉴라구! 너만을 위한 특별한 시식 기회!",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "그 ‘한정’이라는 단어가 가장 무서운 거야.",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "하지만, 저렇게 기대하는 눈빛을 보니 거절하기도 애매한데…",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Exited, KatePoseType.HandsBack),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "좋아! 그럼 내가 먼저 한 입 먹어볼게!",
//                 "봐봐~ 한 입 먹으면~...",
//             }
//         ),

//         // 🔥 케이트가 한 입 먹고 예상치 못한 반응 🔥
//         new CharacterEnter(ECharacterName.Kate, KateEmotionType.Angry, KatePoseType.ArmCrossed, CharacterLocation.Center, 1f),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "…어, 어라?",
//                 "이거… 생각보다 엄청 고소한데…? 아니, 너무 고소한데?!",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "…뭐가 문제야?",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "이 반응, 설마…",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Shy, KatePoseType.HandsBack),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "흐아아…! 고소해! 너무 고소해!!",
//                 "앗… 설마, 견과류를 세 배로 넣었나?!",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "그러니까, 난 안 먹어도 되겠지?",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "오늘도 무사히(?) 넘겼다.",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Happy, KatePoseType.HandsFront),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "큭… 인정! 이번 건 실패! 하지만 다음 번엔 더 기막힌 걸로 준비할 테니까 기대해!",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "무섭게 말하지 마.",
//             }
//         ),

//         new CharacterExit(ECharacterName.Kate),
        
//         new CharacterExit(ECharacterName.Ryan),
//     };
// }
