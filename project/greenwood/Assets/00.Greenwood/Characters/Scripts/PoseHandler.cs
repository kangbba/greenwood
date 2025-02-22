using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

public class PoseHandler : MonoBehaviour
{
    [FoldoutGroup("🎭 Pose List")]
    [SerializeField, ListDrawerSettings(ShowFoldout = true)]
    private List<Pose> _poses = new();

    private int _currentPoseIndex = 0;
    private string _currentPoseID; // 실제 포즈 ID는 string으로 저장

    private void Awake()
    {
        FetchPoses();
    }

 #if UNITY_EDITOR
    [Button("➡ Next Pose")]
    private void CyclePose()
    {
        FetchPoses();

        if (_poses.Count == 0)
        {
            Debug.LogWarning("[PoseHandler] No poses found.");
            return;
        }

        // ✅ 순회 로직 (리스트 끝에 도달하면 처음으로 돌아감)
        _currentPoseIndex = (_currentPoseIndex + 1) % _poses.Count;
        SetPose(_poses[_currentPoseIndex].PoseID, 0f, false);

        // ✅ SceneView & 인스펙터 갱신
        EditorApplication.delayCall += () =>
        {
            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        };
    }
    /// <summary>
    /// ✅ **에디터에서 Transform (Local Position, Local Rotation) 고정**
    /// </summary>
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = Vector3.zero; // ✅ 항상 (0,0,0) 유지
            transform.localRotation = Quaternion.identity; // ✅ 항상 회전 없음
            transform.localScale = Vector3.one;

            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        }
    }
    #endif

    public void FetchPoses()
    {
        _poses = new List<Pose>(GetComponentsInChildren<Pose>(true));
    }

    public void SetPose(string poseID, float duration, bool isRuntime = true)
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
            if(isTarget){
                p.gameObject.SetActive(true);
                p.FadeFrom(target : 1f, 0f, duration);
            }
            else{
                p.FadeOut(duration);
                p.gameObject.SetActive(false, duration);
            }
        }

        if(isRuntime){
            newPose.Init();
        }

        Debug.Log($"[PoseHandler] 포즈 변경: `{_currentPoseID}`");
    }

    public Pose GetPose(string poseID)
    {
        return _poses.Find(p => p.PoseID == poseID);
    }
}
