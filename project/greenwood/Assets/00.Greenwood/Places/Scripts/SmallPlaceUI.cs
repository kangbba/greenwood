using UnityEngine;
using TMPro;
using UniRx;
using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using static CharacterEnums;

public class SmallPlaceUI : AnimationImage
{
    [SerializeField] private ButtonGroup _leftButtonGroup;
    [SerializeField] private ButtonGroup _soloTalkButtonGroup;
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
        ConsistLeftButtonGroup(ksps);
        ConsistMiddleButtonGroup(ksps);
    }
    private void ConsistLeftButtonGroup(List<KeyScenariosPair> ksps)
    {
        _leftButtonGroup.ClearButtons();

        foreach (var ksp in ksps)
        {
            if (ksp.Key == CommonActionTypes.SoloTalk)
            {
                // ✅ SoloTalk는 중앙 버튼 그룹에서 처리하므로 스킵
                continue;
            }

            // ✅ 일반 대화 버튼 추가
            _leftButtonGroup.AddButton(ksp.Key, 
                () => ScenarioManager.Instance.ExecuteOneScenarioFromList(ksp.Scenarios).Forget());
        }

        // ✅ 나가기 버튼 추가 (항상 존재)
        _leftButtonGroup.AddButton(CommonActionTypes.Exit, () => PlaceManager.Instance.ExitSmallPlace(.5f));
    }

    public void ConsistMiddleButtonGroup(List<KeyScenariosPair> ksps)
    {
        _soloTalkButtonGroup.ClearButtons();

        Dictionary<ECharacterName, List<Scenario>> characterScenarioMap = new();

        foreach (var ksp in ksps)
        {
            if (ksp.Key != CommonActionTypes.SoloTalk)
            {
                continue;
            }

            foreach (var scenario in ksp.Scenarios)
            {
                CharacterEnter firstCharacterEnter = scenario.GetFirstCharacterEnter();
                if (firstCharacterEnter == null) continue;

                ECharacterName characterName = firstCharacterEnter.CharacterName;

                if (!characterScenarioMap.ContainsKey(characterName))
                {
                    characterScenarioMap[characterName] = new List<Scenario>();
                }
                characterScenarioMap[characterName].Add(scenario);
            }
        }

        // ✅ 모든 ECharacterName을 포함한 ButtonEntries 리스트 구성
        _soloTalkButtonGroup.ButtonEntries = new List<ButtonEntry>();
        
        foreach (ECharacterName characterName in Enum.GetValues(typeof(ECharacterName)))
        {
            string displayName = CharacterManager.Instance.GetCharacterSetting(characterName)?.DisplayName ?? characterName.ToString();
            _soloTalkButtonGroup.ButtonEntries.Add(new ButtonEntry(characterName.ToString(), displayName));
        }

        // ✅ 캐릭터별 버튼 생성 (시나리오 있는 경우만)
        foreach (var pair in characterScenarioMap)
        {
            ECharacterName characterName = pair.Key;
            List<Scenario> mergedScenarios = pair.Value;

            string displayName = CharacterManager.Instance.GetCharacterSetting(characterName)?.DisplayName ?? characterName.ToString();

            Button button = _soloTalkButtonGroup.AddButton(displayName, 
                () => ScenarioManager.Instance.ExecuteOneScenarioFromList(mergedScenarios).Forget());

            // ✅ 버튼에 CharacterCardUI 설정
            CharacterCardUI cardUI = button.GetComponent<CharacterCardUI>();
            if (cardUI != null)
            {
                cardUI.SetCharacter(CharacterManager.Instance.GetCharacterPrefab(characterName), displayName);
            }
        }
    }







}
