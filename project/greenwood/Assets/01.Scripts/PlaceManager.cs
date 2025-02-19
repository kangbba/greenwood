// using System.Collections.Generic;
// using UnityEngine;
// using Cysharp.Threading.Tasks;

// public class PlaceManager : MonoBehaviour
// {
//     public static PlaceManager Instance { get; private set; }

//     [Header("Place Prefabs")]
//     [SerializeField] private List<Place> placePrefabs;

//     private Dictionary<string, Place> _placeInstances = new Dictionary<string, Place>();
//     private Place _currentPlace;

//     private void Awake()
//     {
//         if (Instance == null) Instance = this;
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Debug.Log("[PlaceManager] Awake - Instance initialized");
//         LoadPlacePrefabs();
//     }

//     private void LoadPlacePrefabs()
//     {
//         _placeInstances.Clear();
//         foreach (Place place in placePrefabs)
//         {
//             if (!_placeInstances.ContainsKey(place.PlaceID))
//             {
//                 _placeInstances[place.PlaceID] = place;
//                 Debug.Log($"[PlaceManager] Loaded place prefab: {place.PlaceID}");
//             }
//             else
//             {
//                 Debug.LogError($"[PlaceManager] Duplicate PlaceID detected: {place.PlaceID}");
//             }
//         }
//     }

//     public Place CreatePlace(string placeID)
//     {
//         if (!_placeInstances.TryGetValue(placeID, out Place placePrefab))
//         {
//             Debug.LogError($"[PlaceManager] ERROR - Place ID '{placeID}' not found in prefabs!");
//             return null;
//         }

//         Debug.Log($"[PlaceManager] Creating place: {placeID}");

//         Place placeInstance = Instantiate(placePrefab, UIManager.Instance.GameCanvas.PlaceLayer);
//         _currentPlace = placeInstance;
//         placeInstance.Init();
//         return placeInstance;
//     }

//     public void DestroyCurrentPlace(float duration)
//     {
//         if (_currentPlace == null)
//         {
//             Debug.Log("[PlaceManager] No current place to destroy.");
//             return;
//         }

//         Debug.Log($"[PlaceManager] Destroying current place: {_currentPlace.name} over {duration}s");
//         _currentPlace.Hide(duration);
//         Destroy(_currentPlace.gameObject, duration);
//         _currentPlace = null;
//     }
// }
