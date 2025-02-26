// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using static BigPlaceNames;
// using static CharacterEnums;
// using static SmallPlaceNames;
// public static class CafeActionTypes
// {
//     public const string Order = "Order";
// }

// public static class BakeryActionTypes
// {
//     public const string Buy = "Buy";
// }

// public static class HerbshopActionTypes
// {
//     public const string Buy = "Buy";
//     public const string Heal = "Heal";
// }


// public static class CommonActionTypes
// {
//     public const string SoloTalk = "SoloTalk";
//     public const string Exit = "Exit";
// }

// public enum SmallPlaceActionType
// {
//     Exit,
//     Order,
//     Buy,
//     Sell,
// }

// public class SmallPlaceMenuAction
// {
//     public SmallPlaceActionType ActionType { get; private set; }
//     public Action ExecuteAction { get; private set; }

//     public SmallPlaceMenuAction(SmallPlaceActionType actionType, Action executeAction)
//     {
//         ActionType = actionType;
//         ExecuteAction = executeAction;
//     }

//     public void Execute()
//     {
//         ExecuteAction?.Invoke();
//     }
// }
