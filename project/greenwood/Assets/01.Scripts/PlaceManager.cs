using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    public static PlaceManager Instance { get; private set; }

    [Header("Place Sprites")]
    private Dictionary<string, Sprite> _placeSprites = new Dictionary<string, Sprite>();

    private Place _currentPlace;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        Debug.Log("[PlaceManager] Awake - Instance initialized");
        LoadPlaceSprites();
    }

    private void LoadPlaceSprites()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("PlaceSprites");
        _placeSprites = sprites.ToDictionary(sprite => sprite.name, sprite => sprite);
        Debug.Log($"[PlaceManager] Loaded {_placeSprites.Count} place sprites");
    }

    public Place CreatePlace(string placeID)
    {
        if (!_placeSprites.TryGetValue(placeID, out Sprite sprite))
        {
            Debug.LogError($"[PlaceManager] ERROR - Place ID '{placeID}' not found in sprites!");
            return null;
        }

        Debug.Log($"[PlaceManager] Creating place: {placeID}");

        GameObject placeObject = new GameObject($"Place_{placeID}");
        Place place = placeObject.AddComponent<Place>();
        place.Init(sprite);

        place.transform.SetParent(UIManager.Instance.GameCanvas.PlaceLayer, worldPositionStays: false);

        _currentPlace = place;
        return place;
    }

    public void DestroyCurrentPlace(float duration)
    {
        if (_currentPlace == null)
        {
            Debug.Log("[PlaceManager] No current place to destroy.");
            return;
        }

        Debug.Log($"[PlaceManager] Destroying current place: {_currentPlace.name} over {duration}s");
        _currentPlace.Hide(duration);
        Destroy(_currentPlace.gameObject, duration);
        _currentPlace = null;
    }
}
