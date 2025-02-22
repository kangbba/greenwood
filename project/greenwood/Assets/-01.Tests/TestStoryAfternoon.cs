// using System.Collections.Generic;
// using UnityEngine;

// public class TestStoryAfternoon : Story
// {
//     public override List<Element> UpdateElements { get; } = new List<Element>
//     {
//         new CharacterEnter(ECharacterName.Kate, KateEmotionType.YeahRight, KatePoseType.HandsBack, CharacterLocation.Center, 1f),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "흐음… 라이언, 네 생각엔 어떤 빵이 더 인기 있을 것 같아?",
//                 "초콜릿 브라우니? 아니면 바닐라 머핀?",
//             }
//         ),

//         new Dialogue(ECharacterName.Ryan,
//             new List<string>
//             {
//                 "음… 둘 다 맛있겠는데, 오늘은 브라우니가 끌리네.",
//             }
//         ),

//         new EmotionChange(ECharacterName.Kate, KateEmotionType.Exited, KatePoseType.HandsFront),

//         new Dialogue(ECharacterName.Kate, 
//             new List<string>
//             {
//                 "오~ 역시 라이언도 달달한 브라우니 좋아하는구나!",
//                 "좋아! 그럼 특별히 한 조각 서비스~!",
//             }
//         ),

//         new Dialogue(ECharacterName.Mono,
//             new List<string>
//             {
//                 "…이거 또 케이트의 실험적인 브라우니일까?",
//             }
//         ),

//         new CharacterExit(ECharacterName.Kate),
//     };
// }
