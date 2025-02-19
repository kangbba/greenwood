using UnityEngine;

public enum EPlaceName
{
    Bakery,
    Harbor,
    Lighthouse
}

public class Place : MonoBehaviour
{
    [Header("Place Settings")]
    [SerializeField] private EPlaceName placeName;

    public EPlaceName PlaceName => placeName;

    public void Show(float duration)
    {
        gameObject.SetAnimActive(true, duration);
    }

    public void Hide(float duration)
    {
        gameObject.SetAnimActive(false, duration);
    }
}
