using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public static UICanvas Instance { get; private set; }
    [Header("🔹 UI 레이어")]
    [SerializeField] private Transform _mapLayer; 
    [SerializeField] private Transform _placeUiLayer; 
    [SerializeField] private Transform _dialoguePlayerLayer; 

    public Transform MapLayer { get => _mapLayer; }
    public Transform DialoguePlayerLayer { get => _dialoguePlayerLayer; }
    public Transform PlaceUiLayer { get => _placeUiLayer; }

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
