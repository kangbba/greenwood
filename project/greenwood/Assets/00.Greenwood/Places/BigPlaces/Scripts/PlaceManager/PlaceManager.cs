
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.UIElements.Experimental;

public enum EBigPlaceName
{
    Town,
    CafeSeabreezeFront
}
public enum ESmallPlaceName{
    Bakery,
    Herbshop,
    CafeSeabreeze
}

public enum EPlaceState
{
    None,       // 아무 장소에도 없음 (초기 상태)
    InBigPlace, // 현재 BigPlace에 머무르는 상태
    InSmallPlace // 현재 SmallPlace에 머무르는 상태
}

public class PlaceManager : MonoBehaviour
{   

    const float UiDuration = .5f;
    public static PlaceManager Instance { get; private set; }

    [Header("Place Handlers")]
    [SerializeField] private PlaceUiHandler _placeUiHandler;
    [SerializeField] private BigPlaceHandler _bigPlaceHandler;
    [SerializeField] private SmallPlaceHandler _smallPlaceHandler;

    //플레이스 상태

    private ReactiveProperty<EPlaceState> _placeStateNotifier = new ReactiveProperty<EPlaceState>(EPlaceState.None);

    // ✅ BigPlace 및 SmallPlace 상태를 직접 핸들러에서 가져오도록 변경
    public IReadOnlyReactiveProperty<BigPlace> CurrentBigPlaceNotifier => _bigPlaceHandler.CurrentBigPlaceNotifier;
    public IReadOnlyReactiveProperty<SmallPlace> CurrentSmallPlaceNotifier => _smallPlaceHandler.CurrentSmallPlaceNotifier;


    public ReactiveProperty<EPlaceState> PlaceStateNotifier { get => _placeStateNotifier; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _placeUiHandler.Init();
        _bigPlaceHandler.Init();
        _smallPlaceHandler.Init();

        // ✅ 장소 상태 변경에 따라 UI 업데이트
        _placeStateNotifier
            .Subscribe(state =>
            {
                Debug.Log($"_placeStateNotifier {state}");
                OnChangedPlaceState(state);
            })
            .AddTo(this);
    }

    private void OnChangedPlaceState(EPlaceState state)
    {
            
        switch (state)
        {
            case EPlaceState.None:
                break;

            case EPlaceState.InBigPlace:
                break;

            case EPlaceState.InSmallPlace:
                break;
        }
    }


    public async UniTask GoingOut(){
        
        await _placeUiHandler.BigPlaceUI(CurrentBigPlaceNotifier.Value, false, .5f);
        EBigPlaceName? selectedBigPlace = await _placeUiHandler.CreateMapAndShow(false);

        if(!selectedBigPlace.HasValue){
            Debug.Log("선택된 big place 없음");
            await _placeUiHandler.BigPlaceUI(CurrentBigPlaceNotifier.Value, true, .5f);
            return;
        }
        else{
            MoveBigPlace(selectedBigPlace.Value).Forget();
        }
    }

    public async UniTask MoveBigPlace(EBigPlaceName newPlaceName)
    {   
        if(CurrentBigPlaceNotifier.Value?.BigPlaceName == newPlaceName){
            Debug.LogWarning("같은 빅 플레이스로의 이동 시도");
            return;
        }
        _placeStateNotifier.Value = EPlaceState.InBigPlace;
        await ExitBigPlace();
        BigPlace bigPlace = _bigPlaceHandler.CreateBigPlace(newPlaceName);
        bigPlace.FadeIn(.5f);
        await UniTask.WaitForSeconds(.5f);
        await StoryManager.Instance.TriggerStoryIfExist();
        await _placeUiHandler.BigPlaceUI(CurrentBigPlaceNotifier.Value, true, .5f);
    }
    public async UniTask ExitBigPlace()
    {   
        _placeUiHandler.BigPlaceUI(CurrentBigPlaceNotifier.Value, false, .5f).Forget();
        _bigPlaceHandler.ExitCurrentBigPlace(.5f);
        await UniTask.WaitForSeconds(.5f);
    }

    public async UniTask EnterSmallPlace(ESmallPlaceName smallPlaceName)
    {
        if(CurrentSmallPlaceNotifier.Value?.SmallPlaceName == smallPlaceName){
            Debug.LogWarning("같은 스몰 플레이스로의 이동 시도");
            return;
        }
        _placeStateNotifier.Value = EPlaceState.InSmallPlace;
        await _placeUiHandler.BigPlaceUI(CurrentBigPlaceNotifier.Value, false, .5f);
        SmallPlace smallPlace = _smallPlaceHandler.CreateSmallPlace(smallPlaceName);
        smallPlace.FadeIn(.5f);
        await UniTask.WaitForSeconds(.5f);

        await StoryManager.Instance.TriggerStoryIfExist();
        await _placeUiHandler.SmallPlaceUI(CurrentSmallPlaceNotifier.Value, true, .5f);
    }

    public async UniTask ExitSmallPlace()
    {
        _placeStateNotifier.Value = EPlaceState.InBigPlace;
        _placeUiHandler.SmallPlaceUI(CurrentSmallPlaceNotifier.Value, false, .5f).Forget();
        _smallPlaceHandler.ExitSmallPlace(.5f);
        await UniTask.WaitForSeconds(.5f);
        await _placeUiHandler.BigPlaceUI(CurrentBigPlaceNotifier.Value, true, .5f);
    }
}
