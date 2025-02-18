using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas Instance { get; private set; }

    [Header("🔹 UI 레이어")]
    [SerializeField] private Transform _characterLayer; 
    [SerializeField] private Transform _placeLayer; 

    public Transform CharacterLayer => _characterLayer;

    public Transform PlaceLayer { get => _placeLayer; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
