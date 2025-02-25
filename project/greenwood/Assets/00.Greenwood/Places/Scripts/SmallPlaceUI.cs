using UnityEngine;
using TMPro;
using UniRx;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;

public class SmallPlaceUI : AnimationImage
{
    [SerializeField] private ButtonGroup _leftButtonGroup;
    [SerializeField] private TextMeshProUGUI _placeNameText;

    // // ✅ 현재 SmallPlace의 시나리오 정보를 저장 (Editor 빌드에서 OnGUI로 표시)
    // private List<KeyScenariosPair> _currentScenarioPairs = new List<KeyScenariosPair>();

    public void Init()
    {
        FadeOut(0f);

        // ✅ smallPlace와 isScenarioPlaying을 구독
        PlaceManager.Instance.CurrentSmallPlaceNotifier
            .CombineLatest(ScenarioManager.Instance.IsScenarioPlayingNotifier,
                (smallPlace, isScenarioPlaying) => (smallPlace, isScenarioPlaying))
            .Subscribe(tuple =>
            {
                var (smallPlace, isScenarioPlaying) = tuple;

                // ✅ smallPlace가 null이 아닐 때만
                if (smallPlace != null)
                {
                    Debug.Log("smallPlace keyscnariopairs count : " + smallPlace.KeyScenariosPairs.Count);
                    _placeNameText.text = smallPlace.SmallPlaceName.ToString();
                    // ✅ 버튼 설정
                    ConsistKspButtons(smallPlace.KeyScenariosPairs);
                    // ✅ 시나리오 진행 중이면 UI 감추기, 아니면 UI 표시
                    if (isScenarioPlaying)
                    {
                        FadeOut(0.3f);
                    }
                    else
                    {
                        FadeIn(0.3f);
                    }
                }
                else
                {
                    // ✅ smallPlace == null 이면 UI 감추기
                    FadeOut(0.3f);
                    _placeNameText.text = "";
                }
            })
            .AddTo(this);
    }

    public void ConsistKspButtons(List<KeyScenariosPair> ksps)
    {
        _leftButtonGroup.ClearButtons();
        foreach(KeyScenariosPair ksp in ksps)
        {   
            _leftButtonGroup.AddButton(ksp.Key, () => ScenarioManager.Instance.ExecuteOneScenarioFromList(ksp.Scenarios).Forget());
        }
        _leftButtonGroup.AddButton(CommonActionTypes.Exit, () => PlaceManager.Instance.ExitSmallPlace(.5f));
    }
}
