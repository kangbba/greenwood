using UnityEngine;
using TMPro;
using UniRx;
using System.Collections.Generic;
using System;
public class SmallPlaceUI : AnimationImage
{
    [SerializeField] private ButtonGroup leftButtonGroup;
    [SerializeField] private TextMeshProUGUI placeNameText;

    public void Init()
    {
        FadeOut(0f);

        PlaceManager.Instance.CurrentSmallPlaceNotifier
            .Subscribe(smallPlace =>
            {
                if (smallPlace != null)
                {
                    Debug.Log("[SmallPlaceUI] SmallPlace detected. Showing UI.");
                    FadeIn(0.3f);

                    // ✅ Exit 버튼을 마지막에 배치한 후 UI 업데이트
                    leftButtonGroup.SetButtonGroup(SortActionsWithExitLast(smallPlace.GetActions()));
                    placeNameText.text = smallPlace.SmallPlaceName.ToString();
                }
                else
                {
                    Debug.Log("[SmallPlaceUI] SmallPlace exited. Hiding UI.");
                    FadeOut(0.3f);
                    placeNameText.text = "";
                }
            })
            .AddTo(this);
    }

    /// <summary>
    /// ✅ Exit 버튼을 항상 마지막에 배치하는 메서드
    /// </summary>
    private Dictionary<string, Action> SortActionsWithExitLast(Dictionary<string, Action> actions)
    {
        var orderedActions = new Dictionary<string, Action>();

        // ✅ "Exit"이 아닌 버튼 먼저 추가
        foreach (var action in actions)
        {
            if (action.Key != "Exit")
            {
                orderedActions[action.Key] = action.Value;
            }
        }

        // ✅ "Exit" 버튼 마지막으로 추가
        if (actions.ContainsKey("Exit"))
        {
            orderedActions["Exit"] = actions["Exit"];
        }

        return orderedActions;
    }
}
