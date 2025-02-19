using System.Collections.Generic;
using UnityEngine;

public enum EBigPlaceName
{
    Town,
    Sea
}
public enum ESmallPlaceName{
    Bakery,
    Sea
}

public class BigPlaceManager : MonoBehaviour
{
    public static BigPlaceManager Instance { get; private set; }

    [Header("BigPlace Prefabs")]
    [SerializeField] private List<BigPlace> _bigPlacePrefabs;

    private Dictionary<EBigPlaceName, BigPlace> _bigPlaceInstances = new Dictionary<EBigPlaceName, BigPlace>();
    private BigPlace _currentBigPlace;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void CreateBigPlace(EBigPlaceName placeName)
    {
        if (_bigPlaceInstances.ContainsKey(placeName))
        {
            Debug.LogWarning($"[BigPlaceManager] BigPlace '{placeName}' already exists.");
            return;
        }

        BigPlace prefab = _bigPlacePrefabs.Find(bp => bp.BigPlaceName == placeName);
        if (prefab == null)
        {
            Debug.LogError($"[BigPlaceManager] ERROR - BigPlace '{placeName}' not found in prefabs!");
            return;
        }

        BigPlace instance = Instantiate(prefab, UIManager.Instance.GameCanvas.BigPlaceLayer);
        _bigPlaceInstances[placeName] = instance;
        instance.gameObject.SetAnimTrueFromFalse(1f);

        Debug.Log($"[BigPlaceManager] Created BigPlace: {placeName}");
    }
}
