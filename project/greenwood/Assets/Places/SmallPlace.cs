using UnityEngine;

public class SmallPlace : MonoBehaviour
{
    [Header("SmallPlace Settings")]
    [SerializeField] private ESmallPlaceName smallPlaceName;

    public ESmallPlaceName SmallPlaceName => smallPlaceName;

    public void Show(float duration)
    {
        gameObject.SetAnimActive(true, duration);
    }

    public void Hide(float duration)
    {
        gameObject.SetAnimActive(false, duration);
    }
}
