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
                    Debug.Log($"[SmallPlaceUI] 현재 SmallPlace: {smallPlace.SmallPlaceName}");
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


// /// <summary>
// /// ✅ SmallPlaceSchedule을 기반으로 버튼 구성
// /// </summary>
// private void ConsistSmallPlaceButtons(SmallPlaceSchedule schedule)
// {
//     if (schedule == null)
//     {
//         Debug.LogWarning("[SmallPlaceUI] SmallPlaceSchedule is null, 버튼 설정을 스킵합니다.");
//         return;
//     }

//     ConsistLeftButtonGroup(schedule);
//     ConsistMiddleButtonGroup(schedule);
// }

// /// <summary>
// /// ✅ SmallPlace의 일반 메뉴 버튼 구성 (좌측 버튼 그룹)
// /// </summary>
// private void ConsistLeftButtonGroup(SmallPlaceSchedule schedule)
// {
//     _leftButtonGroup.ClearButtons();

//     foreach (var menuAction in schedule.SmallPlaceMenuActions)
//     {
//         _leftButtonGroup.AddButton(menuAction.ActionType.ToString(), () => menuAction.Execute());
//     }

//     // ✅ 나가기 버튼 추가 (항상 존재)
//     _leftButtonGroup.AddButton(CommonActionTypes.Exit, () => PlayerManager.Instance.ExitSmallPlace());
// }

// /// <summary>
// /// ✅ 캐릭터별 솔로 토크 버튼 구성 (중앙 버튼 그룹)
// /// </summary>
// private void ConsistMiddleButtonGroup(SmallPlaceSchedule schedule)
// {
//     _soloTalkButtonGroup.ClearButtons();

//     Dictionary<ECharacterName, Scenario> characterScenarioMap = new();

//     // ✅ 캐릭터별 솔로 토크 데이터를 매핑
//     foreach (var soloTalk in schedule.CharacterSoloTalks)
//     {
//         characterScenarioMap[soloTalk.CharacterName] = soloTalk.Scenario;
//     }

//     // ✅ 모든 ECharacterName을 포함한 ButtonEntries 리스트 구성
//     _soloTalkButtonGroup.ButtonEntries = new List<ButtonEntry>();

//     foreach (ECharacterName characterName in Enum.GetValues(typeof(ECharacterName)))
//     {
//         string displayName = CharacterManager.Instance.GetCharacterSetting(characterName)?.DisplayName ?? characterName.ToString();
//         _soloTalkButtonGroup.ButtonEntries.Add(new ButtonEntry(characterName.ToString(), displayName));
//     }

//     // ✅ 캐릭터별 버튼 생성 (시나리오가 있는 경우만)
//     foreach (var pair in characterScenarioMap)
//     {
//         ECharacterName characterName = pair.Key;
//         Scenario scenario = pair.Value;

//         string displayName = CharacterManager.Instance.GetCharacterSetting(characterName)?.DisplayName ?? characterName.ToString();

//         Button button = _soloTalkButtonGroup.AddButton(displayName, 
//             () => ScenarioManager.Instance.ExecuteOneScenario(scenario).Forget());

//         // ✅ 버튼에 CharacterCardUI 설정
//         CharacterCardUI cardUI = button.GetComponent<CharacterCardUI>();
//         if (cardUI != null)
//         {
//             cardUI.SetCharacter(CharacterManager.Instance.GetCharacterPrefab(characterName), displayName);
//         }
//     }
// }
