using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("🏠 Initializing Game...");
        
        // 최초 PlaceGroup 설정 (Town)
        BigPlaceManager.Instance.CreateBigPlace(EBigPlaceName.Town);
    }
}
