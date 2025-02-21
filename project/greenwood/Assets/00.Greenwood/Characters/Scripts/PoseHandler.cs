using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

public class PoseHandler : MonoBehaviour
{
    [FoldoutGroup("🎭 Pose List")]
    [SerializeField, ListDrawerSettings(ShowFoldout = true)]
    private List<Pose> _poses = new();

    [EnumPaging, OnValueChanged(nameof(OnValueChangedCurrentPose))]
    [SerializeField]
    private KatePoseType _previewPoseType; // 유지

    private string _currentPoseID; // 실제 포즈 ID는 string으로 저장

    private void Awake()
    {
        FetchPoses();
    }

    /// <summary>
    /// 인스펙터에서 `_currentPoseType` 값이 바뀔 때마다 호출
    /// </summary>
    private void OnValueChangedCurrentPose()
    {
        FetchPoses();
        SetPose(_previewPoseType.ToString(), 0f);
        
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        };
#endif
    }

    public void FetchPoses()
    {
        _poses = new List<Pose>(GetComponentsInChildren<Pose>(true));
    }

    public void SetPose(string poseID, float duration)
    {
        if(poseID == null){
            Debug.LogWarning("poseID null");
            return;
        }

        Pose newPose = GetPose(poseID);
        if(newPose == null){

            Debug.LogWarning($"[EmotionHandler] 감정 `{poseID}`이(가) 존재하지 않습니다.");
            return;
        }

        if(_currentPoseID == poseID){
            Debug.LogWarning("poseID already same");
            return;
        }
        _currentPoseID = poseID;

        foreach (var p in _poses)
        {
            bool isTarget = p == newPose;
            p.gameObject.SetActive(isTarget);
            if(isTarget){
                p.FadeIn(duration);
            }
            else{
                p.FadeOut(duration);
            }
        }

        Debug.Log($"[PoseHandler] 포즈 변경: `{_currentPoseID}`");
    }

    public Pose GetPose(string poseID)
    {
        return _poses.Find(p => p.PoseType.ToString() == poseID);
    }
}
