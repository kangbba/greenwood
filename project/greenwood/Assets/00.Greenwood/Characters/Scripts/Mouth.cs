using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEditor;

public class Mouth : MonoBehaviour
{
    [SerializeField] private bool _useAnimation = true; // 애니메이션 사용 여부

    private Vector2 closedDurationRange = new Vector2(0.2f, 0.4f);

    private Vector2 openedDurationRange = new Vector2(0.2f, 0.4f);

    [ShowIf("_useAnimation")]
    [SerializeField] private GameObject _closedObj;

    [ShowIf("_useAnimation")]
    [SerializeField] private GameObject _openedObj;

    private CancellationTokenSource _cts;
    private bool _isMouthOpened = false; // 입 닫힌 상태에서 시작


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
    /// <summary>
    /// 입 열기/닫기 상태 전환
    /// </summary>
    private void Toggle()
    {
        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함
        SetOpen(!_isMouthOpened);
    }

    /// <summary>
    /// 입을 열거나 닫는 상태 설정
    /// </summary>
    private void SetOpen(bool isOpen)
    {
        _isMouthOpened = isOpen;

        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 바로 종료

        if(_closedObj != null){
           _closedObj.SetActive(!isOpen);
        }
        if(_openedObj != null){
           _openedObj.SetActive(isOpen);
        }
    }

   /// <summary>
    /// 일정 주기로 입을 닫았다 열었다 반복
    /// </summary>
    public async UniTaskVoid Play()
    {
        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함

        SetOpen(false);

        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            while (!token.IsCancellationRequested)
            {
                if (_isMouthOpened)
                {
                    SetOpen(false); // 입 닫기
                    await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(closedDurationRange.x, closedDurationRange.y)), cancellationToken: token);
                }
                else
                {
                    SetOpen(true); // 입 열기
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
    /// 입 동작 중단 후 기본 상태 유지 (입 닫힌 상태 보장)
    /// </summary>
    public void Stop()
    {
        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함

        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        SetOpen(false); // 입을 닫은 상태로 종료 보장
    }

    /// <summary>
    /// 입 열기/닫기 미리보기 버튼 (Sirenix Odin)
    /// </summary>
    [Button("👄 입 움직이기 미리보기", ButtonSizes.Large)]
    public void PreviewMouth()
    {
        Toggle();
    }
}
