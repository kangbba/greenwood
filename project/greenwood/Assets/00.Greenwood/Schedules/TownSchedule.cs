// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using static BigPlaceNames;
// using static CharacterEnums;
// using static SmallPlaceNames;

// public class TownSchedule : BigPlaceSchedule
// {
//     public TownSchedule() : base(EBigPlaceName.Town)
//     {
//         SmallPlaceSchedules = new List<SmallPlaceSchedule>
//         {

//             new SmallPlaceSchedule(
//                 placeName : ESmallPlaceName.Bakery,
//                 menuActions : new List<SmallPlaceMenuAction>
//                 {
//                     new SmallPlaceMenuAction(SmallPlaceActionType.Buy, () => Debug.Log("🍞 베이커리에서 빵 구매")),
//                     new SmallPlaceMenuAction(SmallPlaceActionType.Order, () => Debug.Log("🥖 빵 주문"))
//                 },
//                 characterSoloTalks : new List<CharacterSoloTalk>
//                 {
//                     new CharacterSoloTalk(ECharacterName.Kate, new KateEmotionChangePractice()),
//                     new CharacterSoloTalk(ECharacterName.Lisa, new KateEmotionChangePractice())
//                 }
//             ),
//             new SmallPlaceSchedule(
//                 placeName : ESmallPlaceName.Cafe,
//                 menuActions : new List<SmallPlaceMenuAction>
//                 {
//                     new SmallPlaceMenuAction(SmallPlaceActionType.Order, () => Debug.Log("☕ 카페에서 주문")),
//                     new SmallPlaceMenuAction(SmallPlaceActionType.Buy, () => Debug.Log("🍞 베이커리에서 빵 구매")),
//                 },
//                 characterSoloTalks : new List<CharacterSoloTalk>
//                 {
//                     new CharacterSoloTalk(ECharacterName.Joseph, new KateEmotionChangePractice()),
//                     new CharacterSoloTalk(ECharacterName.Lisa, new KateEmotionChangePractice())
//                 }
//             ),
//         };
//     }
// }
