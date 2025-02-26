// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using static BigPlaceNames;
// using static CharacterEnums;
// using static SmallPlaceNames;
// public class SmallPlaceSchedule
// {
//     public ESmallPlaceName PlaceName { get; private set; }
//     public List<SmallPlaceMenuAction> SmallPlaceMenuActions { get; private set; } // ✅ 여러 개의 행동 가능
//     public List<CharacterSoloTalk> CharacterSoloTalks { get; private set; } // ✅ 여러 캐릭터와 대화 가능

//     public SmallPlaceSchedule(ESmallPlaceName placeName, List<SmallPlaceMenuAction> menuActions, List<CharacterSoloTalk> characterSoloTalks)
//     {
//         PlaceName = placeName;
//         SmallPlaceMenuActions = menuActions ?? new List<SmallPlaceMenuAction>();
//         CharacterSoloTalks = characterSoloTalks ?? new List<CharacterSoloTalk>();
//     }
// }
