using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BigPlace : MonoBehaviour
{
    [Header("BigPlace Settings")]
    [SerializeField] private Image _background;
    [SerializeField] private EBigPlaceName _bigPlaceName;
    private List<SmallPlaceLocation> _smallPlaceLocations = new List<SmallPlaceLocation>();

    private Dictionary<ESmallPlaceName, SmallPlace> _smallPlacePrefabs = new Dictionary<ESmallPlaceName, SmallPlace>();

    public EBigPlaceName BigPlaceName => _bigPlaceName;

    public List<SmallPlaceLocation> SmallPlaceLocations { get => _smallPlaceLocations; }

    private void Awake()
    {
        _smallPlaceLocations = GetComponentsInChildren<SmallPlaceLocation>().ToList();
        LoadSmallPlacePrefabs();
    }

    private void LoadSmallPlacePrefabs()
    {
        foreach (var smallPlaceLocation in _smallPlaceLocations)
        {
            if (!_smallPlacePrefabs.ContainsKey(smallPlaceLocation.SmallPlaceName))
            {
                _smallPlacePrefabs[smallPlaceLocation.SmallPlaceName] = smallPlaceLocation.SmallPlacePrefab;
                _smallPlacePrefabs[smallPlaceLocation.SmallPlaceName].Init(_bigPlaceName);
            }
        }
    }
    /// <summary>
    /// 특정 SmallPlace 프리팹을 가져옴
    /// </summary>
    public SmallPlace GetSmallPlacePrefab(ESmallPlaceName smallPlaceName)
    {
        return _smallPlacePrefabs.TryGetValue(smallPlaceName, out SmallPlace smallPlace) ? smallPlace : null;
    }

    /// <summary>
    /// SmallPlace를 생성하여 반환
    /// </summary>
    public SmallPlace CreateSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlace smallPlacePrefab = GetSmallPlacePrefab(smallPlaceName);
        if (smallPlacePrefab == null)
        {
            Debug.LogError($"[BigPlace] ERROR - SmallPlace '{smallPlaceName}' not found in BigPlace '{_bigPlaceName}'!");
            return null;
        }

        return Instantiate(smallPlacePrefab, UIManager.Instance.GameCanvas.SmallPlaceLayer);
    }

    public async UniTask Show()
    {
        gameObject.SetAnim(true, 1f);
        await UniTask.WaitForSeconds(1f);
    }

    public async UniTask HideAndDestroy()
    {
        gameObject.SetAnimDestroy(1f);
        await UniTask.WaitForSeconds(1f);
    }
}
