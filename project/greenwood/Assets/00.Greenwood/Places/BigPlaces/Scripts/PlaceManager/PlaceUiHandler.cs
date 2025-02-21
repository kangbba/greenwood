using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class PlaceUiHandler : MonoBehaviour
{

    [SerializeField] private BottomUiPanel _placeBottomUI;
    [SerializeField] private Button _enterSmallPlaceBtnPrefab;
    [SerializeField] private Map _mapPrefab;

    private List<Button> _enterPlaceBtns = new List<Button>();
    private Map _currentMapInstance;

    public void Init(){
    }
    
    public void FadeInBottomPanel(float duration){
        _placeBottomUI.FadeIn(duration);
    }
    public void FadeOutBottomPanel(float duration){
        _placeBottomUI.FadeOut(duration);
    }

    public void SlideInBottomPanel(float duration){
        _placeBottomUI.SlideIn(duration);
    }
    public void SlideOutBottomPanel(float duration){
        _placeBottomUI.SlideOut(Vector2.down * 200f, duration);
    }


    public void SetBottomPanelButtons(Dictionary<BottomUiButtonType, System.Action> buttonActions)
    {
        _placeBottomUI.SetButtons(buttonActions);
    }
    public void ClearBottomPanelButtons(){
        _placeBottomUI.ClearButtons();
    }
    /// <summary>
    /// BigPlace에 머무를 때 SmallPlace 입장 버튼 생성
    /// </summary>
    public void CreateEnterPlaceBtns(BigPlace bigPlace)
    {

        foreach (var door in bigPlace.SmallPlaceDoors)
        {

            Button newButton = Instantiate(_enterSmallPlaceBtnPrefab, door.transform);
            newButton.transform.localPosition = Vector2.zero;
            newButton.gameObject.SetAnimToFrom(true, false, 1f);
            newButton.onClick.AddListener(() =>
            {
                PlaceManager.Instance.EnterSmallPlace(door.SmallPlaceName).Forget();
            });
            _enterPlaceBtns.Add(newButton);
        }
    }

    /// <summary>
    /// SmallPlace에서 나올 때 입장 버튼 제거
    /// </summary>
    public void DestroyEnterPlaceBtns(float duration)
    {

        foreach (var button in _enterPlaceBtns)
        {
            button.gameObject.SetAnimDestroy(duration);
        }
        _enterPlaceBtns.Clear();
    }

    /// <summary>
    /// 맵을 열고 새로운 BigPlace 선택
    /// </summary>
    public async UniTask<EBigPlaceName?> CreateMapAndShow(bool useRestrict, List<EBigPlaceName> availablePlaces = null)
    {
        _currentMapInstance = Instantiate(_mapPrefab, UIManager.Instance.UICanvas.MapLayer);

        _currentMapInstance.InitMap(useRestrict, availablePlaces);

        // ✅ `ShowMap()`이 단순히 장소만 반환하도록 유지
        EBigPlaceName? selectedBigPlaceName = await _currentMapInstance.ShowMap();
        return selectedBigPlaceName;
    }


        // //BOTTOM
        // var btnDictionary = new Dictionary<BottomUiButtonType, System.Action>
        // {
        //     { BottomUiButtonType.GoingOut, () =>  GoingOut().Forget()},
        //     { BottomUiButtonType.Rest, () => TimeManager.Instance.ToTheNextDay() },
        // };
    public async UniTask BigPlaceUI(BigPlace bigPlace, bool b, float duration){
        if(b){
            var btnDictionary = new Dictionary<BottomUiButtonType, Action>
            {
                { BottomUiButtonType.GoingOut, () =>  PlaceManager.Instance.GoingOut().Forget()},
                { BottomUiButtonType.Rest, () => TimeManager.Instance.ToTheNextDay() },
            };
            CreateEnterPlaceBtns(bigPlace);
            SetBottomPanelButtons(btnDictionary);
            FadeInBottomPanel(duration);
     //       SlideInBottomPanel(duration);
            await UniTask.WaitForSeconds(duration);
        }
        else{
            DestroyEnterPlaceBtns(duration);
            FadeOutBottomPanel(duration);
        //    SlideOutBottomPanel(duration);
            await UniTask.WaitForSeconds(duration);
            ClearBottomPanelButtons();
        }
    }
    public async UniTask SmallPlaceUI(SmallPlace smallPlace, bool b, float duration){
        //MIDDLE
        if(b){
            var btnDictionary = new Dictionary<BottomUiButtonType, Action>
            {
                { BottomUiButtonType.ExitSmallPlace, () =>  PlaceManager.Instance.ExitSmallPlace().Forget()},
            };
            SetBottomPanelButtons(btnDictionary);
            FadeInBottomPanel(duration);
        //    SlideInBottomPanel(duration);
            await UniTask.WaitForSeconds(duration);
        }
        else{
            FadeOutBottomPanel(duration);
        //    SlideOutBottomPanel(duration);
            await UniTask.WaitForSeconds(duration);
            ClearBottomPanelButtons();
        }
    }
}
