using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class SAnimPlan
{
    [Serializable]
    public class SituationPlan
    {
        public SituationType situationType;
        [ToggleLeft] public bool _useAsAwakeStatus;
        [HideIf("_useAsAwakeStatus")] public SAnimData animation;

        public SituationPlan(SituationType type, SAnimData anim, bool useAsAwakeStatus = false)
        {
            situationType = type;
            animation = anim;
            _useAsAwakeStatus = useAsAwakeStatus;
        }
    }
    
    [OnValueChanged("OnValueChangeObjToAnimate")]
    [SerializeField] private SAnimObject _sAnimObject;

    [SerializeField] private List<SituationPlan> _situationPlans = new List<SituationPlan>();

    public SAnimObject SAnimObject { get => _sAnimObject; }

    public SAnimPlan(SAnimObject sAnimObject)
    {
        _sAnimObject = sAnimObject;
        OnValueChangeObjToAnimate();
    }
    

#if UNITY_EDITOR
    private void OnValueChangeObjToAnimate()
    {
        Debug.Log("OnValueChangeObjToAnimate");
        if (_sAnimObject == null) return;

        if(_situationPlans.Count == 0){
            _situationPlans = new List<SituationPlan>(){
                new SituationPlan(SituationType.Enter, new SAnimData(), false),
                new SituationPlan(SituationType.Exit, new SAnimData(), true)
            };
        }
    }
#endif

    public SituationPlan GetSituationPlan(SituationType situationType){
        return  _situationPlans.Find(a => a.situationType == situationType);
    }
    
    public void RegisterRestoreAnimData(){
        foreach (var interaction in _situationPlans)
        {
            if(_sAnimObject == null){
                Debug.LogWarning("_sAnimObject 가 null인 개체 존재합니다");
                continue;
            }
            if (interaction._useAsAwakeStatus)
            {
                interaction.animation = new SAnimData(_sAnimObject.ObjectColor, _sAnimObject.ObjectScale, _sAnimObject.ObjectFillAmount, .25f, Ease.OutQuad);
            }
        }
    }

}
