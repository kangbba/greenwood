using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public enum SituationType { Enter, Exit, Down, Up }

public class SAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    [Space]
    [SerializeField, OnValueChanged("OnAnimatedObjectsChanged")]
    private List<SAnimPlan> animPlans = new List<SAnimPlan>();

    private void Reset()
    {
        if (animPlans == null || animPlans.Count == 0)
        {
            animPlans = new List<SAnimPlan> { new SAnimPlan(null) };
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => PlayAnimation(SituationType.Enter);
    public void OnPointerExit(PointerEventData eventData) => PlayAnimation(SituationType.Exit);
    public void OnPointerDown(PointerEventData eventData) => PlayAnimation(SituationType.Down);
    public void OnPointerUp(PointerEventData eventData) => PlayAnimation(SituationType.Up);

    private void OnAnimatedObjectsChanged(){
        Debug.Log("OnAnimatedObjectsChanged");
    }
    private void Awake()
    {
        foreach (var animPlan in animPlans)
        {
            animPlan.RegisterRestoreAnimData();
        }
    }
    private void PlayAnimation(SituationType situationType)
    {
        Debug.Log($"실행 {situationType}");
        foreach (var animPlan in animPlans)
        {
            var situationPlan = animPlan.GetSituationPlan(situationType);
            if (situationPlan != null)
            {
                animPlan.SAnimObject.PlayAnimation(situationPlan.animation);
            }
        }
    }
}
