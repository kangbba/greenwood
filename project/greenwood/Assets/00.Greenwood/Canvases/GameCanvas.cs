using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public static GameCanvas Instance { get; private set; }

    [Header("🔹 UI 레이어")]
    [SerializeField] private Transform _bigPlaceLayer; 
    [SerializeField] private Transform _smallPlaceLayer; 
    [SerializeField] private Transform _imaginationUnderlayLayer; 
    [SerializeField] private Transform _characterLayer; 
    [SerializeField] private Transform _imaginationOverlayLayer; 


    public Transform BigPlaceLayer { get => _bigPlaceLayer; }
    public Transform SmallPlaceLayer { get => _smallPlaceLayer; }
    public Transform CharacterLayer => _characterLayer;

    public Transform ImaginationUnderlayLayer { get => _imaginationUnderlayLayer; }
    public Transform ImaginationOverlayLayer { get => _imaginationOverlayLayer; }

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
