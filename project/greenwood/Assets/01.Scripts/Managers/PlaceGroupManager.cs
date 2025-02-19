using System.Collections.Generic;
using UnityEngine;

public enum EPlaceGroupName
{
    Town,
    Sea
}

public class PlaceGroupManager : MonoBehaviour
{
    public static PlaceGroupManager Instance { get; private set; }

    [Header("PlaceGroup Prefabs")]
    [SerializeField] private List<PlaceGroup> _placeGroupPrefabs;

    private Dictionary<EPlaceGroupName, PlaceGroup> _placeGroupInstances = new Dictionary<EPlaceGroupName, PlaceGroup>();
    private PlaceGroup _currentPlaceGroup;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void CreatePlaceGroup(EPlaceGroupName groupName)
    {
        if (_placeGroupInstances.ContainsKey(groupName))
        {
            Debug.LogWarning($"[PlaceGroupManager] PlaceGroup '{groupName}' already exists.");
            return;
        }

        PlaceGroup prefab = _placeGroupPrefabs.Find(pg => pg.PlaceGroupName == groupName);
        if (prefab == null)
        {
            Debug.LogError($"[PlaceGroupManager] ERROR - PlaceGroup '{groupName}' not found in prefabs!");
            return;
        }

        PlaceGroup instance = Instantiate(prefab, UIManager.Instance.GameCanvas.PlaceGroupLayer);
        _placeGroupInstances[groupName] = instance;
        instance.gameObject.SetAnimTrueFromFalse(1f);

        Debug.Log($"[PlaceGroupManager] Created PlaceGroup: {groupName}");
    }
}
