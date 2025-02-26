using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using static BigPlaceNames;
using static SmallPlaceNames;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private ReactiveProperty<BigPlace> _currentBigPlace = new ReactiveProperty<BigPlace>(null);
    private ReactiveProperty<SmallPlace> _currentSmallPlace = new ReactiveProperty<SmallPlace>(null);

    public IReadOnlyReactiveProperty<BigPlace> CurrentBigPlace => _currentBigPlace;
    public IReadOnlyReactiveProperty<SmallPlace> CurrentSmallPlace => _currentSmallPlace;

    public EBigPlaceName? CurrentBigPlaceName => _currentBigPlace.Value?.BigPlaceName;
    public ESmallPlaceName? CurrentSmallPlaceName => _currentSmallPlace.Value?.SmallPlaceName;

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

    /// <summary>
    /// ✅ 플레이어가 BigPlace로 이동 (실제 렌더링은 PlaceManager가 처리)
    /// </summary>
    [Button("Test")]
    public void MoveBigPlace(EBigPlaceName placeName)
    {
        var place = PlaceManager.Instance.GetBigPlace(placeName);
        if (place != null)
        {
            _currentBigPlace.Value = place;
            _currentSmallPlace.Value = null; // ✅ BigPlace 이동 시 SmallPlace 초기화
        }
        else
        {
            Debug.LogWarning($"BigPlace [{placeName}]를 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// ✅ 현재 BigPlace에서 나가기
    /// </summary>
    public void ExitCurrentBigPlace()
    {
        _currentBigPlace.Value = null;
        _currentSmallPlace.Value = null;
    }

    /// <summary>
    /// ✅ 특정 SmallPlace에 들어가기 (실제 렌더링은 PlaceManager가 처리)
    /// </summary>
    [Button("Test")]
    public void EnterSmallPlace(ESmallPlaceName smallPlaceName)
    {
        var smallPlace = PlaceManager.Instance.GetSmallPlace(smallPlaceName);
        if (smallPlace != null)
        {
            _currentSmallPlace.Value = smallPlace;
        }
        else
        {
            Debug.LogWarning($"SmallPlace [{smallPlaceName}]를 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// ✅ 현재 SmallPlace에서 나가기
    /// </summary>
    public void ExitSmallPlace()
    {
        _currentSmallPlace.Value = null;
    }
}
