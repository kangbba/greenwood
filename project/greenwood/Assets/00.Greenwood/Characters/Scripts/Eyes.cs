using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEditor;

public class Eyes : MonoBehaviour
{
    private bool UseAnimation => _closedObj != null && _openedObj != null;

    private Vector2 closedDurationRange = new Vector2(0.1f, 0.2f);

    private Vector2 openedDurationRange = new Vector2(3f, 5f);

    [SerializeField] private GameObject _closedObj;
    [SerializeField] private GameObject _openedObj;

    private CancellationTokenSource _cts;
    private bool _isEyesOpened = true; // 눈 뜬 상태에서 시작

    /// <summary>
    /// 눈 감기/뜨기 상태 전환
    /// </summary>
    private void Toggle()
    {
        if (!UseAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함
        SetOpen(!_isEyesOpened);
    }

    /// <summary>
    /// 눈을 뜨거나 감는 상태 설정
    /// </summary>
    private void SetOpen(bool isOpen)
    {
        if (!UseAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함
        _isEyesOpened = isOpen;

        if (!UseAnimation) return; // 애니메이션을 사용하지 않으면 바로 종료

        if(_closedObj != null)
        {
            _closedObj.SetActive(!isOpen);
        }
        if(_openedObj != null)
        {
            _openedObj.SetActive(isOpen);
        }
    }

       /// <summary>
    /// ✅ **에디터에서 Transform (Local Position, Local Rotation) 고정**
    /// </summary>
    /// 
    #if UNITY_EDITOR
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


   public async UniTaskVoid Play()
    {
        if (!UseAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함


        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            while (!token.IsCancellationRequested)
            {
                if (_isEyesOpened)
                {
                    SetOpen(false); // 눈 감기
                    await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(closedDurationRange.x, closedDurationRange.y)), cancellationToken: token);
                }
                else
                {
                    SetOpen(true); // 눈 뜨기
                    await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(openedDurationRange.x, openedDurationRange.y)), cancellationToken: token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // 예외 처리 (필요 시 추가 가능)
        }
    }

    /// <summary>
    /// 눈 깜빡임 중단 후 기본 상태 유지 (눈 뜬 상태 보장)
    /// </summary>
    public void Stop()
    {
        if (!UseAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        SetOpen(true); // 눈을 뜬 상태로 종료 보장
    }

    /// <summary>
    /// 눈 감기/뜨기 미리보기 버튼 (Sirenix Odin)
    /// </summary>
    [Button("👀 눈 깜빡이기 미리보기", ButtonSizes.Large)]
    public void PreviewBlink()
    {
        Toggle();
    }
}
