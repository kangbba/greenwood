using UnityEngine;

public class PopupCanvas : MonoBehaviour
{
    public static PopupCanvas Instance { get; private set; }

    [Header("🔹 UI 레이어")]
    [SerializeField] private Transform _characterLayer; 

    public Transform CharacterLayer => _characterLayer;

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
