using System;
using UnityEngine;
using static Character;

public class Pose : MonoBehaviour
{
    [SerializeField] private KatePoseType _poseType; // Inspector에서 직접 설정
    public KatePoseType PoseType => _poseType;

    public void Init()
    {
        // 포즈 초기화 로직 (필요 시 추가)
    }
}
