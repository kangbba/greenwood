using UnityEngine;

public class PopupCanvas : MonoBehaviour
{
    public static PopupCanvas Instance { get; private set; }
    public Transform ItemGainLayer { get => _itemGainLayer; }

    [SerializeField] private Transform _itemGainLayer;

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
