using Sirenix.OdinInspector;
using UnityEngine;

public class Pose : AnimationImage
{
    [SerializeField] private string _poseID; // ✅ Inspector에서 직접 설정 가능
    public string PoseID => _poseID;

    /// <summary>
    /// ✅ 포즈 초기화 (필요한 경우 추가)
    /// </summary>
    public void Init()
    {
        // 포즈 초기화 로직 (추가 가능)
    }
}
