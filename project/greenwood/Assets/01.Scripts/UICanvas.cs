using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public static UICanvas Instance { get; private set; }
    [Header("🔹 UI 레이어")]
    [SerializeField] private Transform _mapLayer; 

    public Transform MapLayer { get => _mapLayer; }

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
