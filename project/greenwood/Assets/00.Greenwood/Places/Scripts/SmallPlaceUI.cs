using UnityEngine;
using TMPro;
using UniRx;
using Cysharp.Threading.Tasks;
public class SmallPlaceUI : AnimationImage
{
    [SerializeField] private ButtonGroup _leftButtonGroup;
    [SerializeField] private ButtonGroup _soloTalkButtonGroup;
    [SerializeField] private TextMeshProUGUI _placeNameText;

    public void Init()
    {
        FadeOut(0f);

        _leftButtonGroup.AddButton(CommonActionTypes.Exit, () => PlayerManager.Instance.ExitSmallPlace());

        // ✅ 플레이어의 현재 SmallPlace 상태 감지하여 UI 업데이트
        PlayerManager.Instance.CurrentSmallPlace
            .CombineLatest(ScenarioManager.Instance.IsScenarioPlaying,
                (smallPlace, isScenarioPlaying) => (smallPlace, isScenarioPlaying))
            .Subscribe(tuple =>
            {
                (SmallPlace smallPlace, bool isScenarioPlaying) = tuple;

                if (smallPlace != null)
                {
                //    Debug.Log($"[SmallPlaceUI] 현재 SmallPlace: {smallPlace.SmallPlaceName}");
                    _placeNameText.text = smallPlace.SmallPlaceName.ToString();

                    // ✅ SmallPlaceSchedule에서 버튼 구성

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
}