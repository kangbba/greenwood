// using Cysharp.Threading.Tasks;
// using UnityEngine;

// public static class PlaceService
// {
//     public static async UniTask MovePlace(string placeID, float duration)
//     {
//         Debug.Log($"[PlaceService] Moving to place: {placeID} with transition duration: {duration}");

//         ExitCurrentPlace(duration).Forget();
//         Place place = PlaceManager.Instance.CreatePlace(placeID);
//         if (place != null)
//         {
//             place.Show(duration);
//         }
//         await UniTask.WaitForSeconds(duration);
//     }

//     public static async UniTask ExitCurrentPlace(float duration)
//     {
//         Debug.Log($"[PlaceService] Exiting current place with transition duration: {duration}");

//         PlaceManager.Instance.DestroyCurrentPlace(duration);
//         await UniTask.WaitForSeconds(duration);
//     }
// }
