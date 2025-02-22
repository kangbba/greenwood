using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class ScreenShake : Element
{
    private float _strength;
    private float _duration;
    private float _scaleMultiplier = 1.1f; // ✅ 흔들릴 때 확대 비율
    private float _scaleDuration = 0.1f;   // ✅ 확대 & 복구 애니메이션 지속 시간

    public ScreenShake(float strength = 30f, float duration = 1f)
    {
        // ✅ 최소 지속 시간을 보장 (_scaleDuration * 2)
        _duration = Mathf.Max(duration, _scaleDuration * 2);
        _strength = strength;
    }

    public override async UniTask ExecuteAsync()
    {
        UIManager uiManager = UIManager.Instance;
        if (uiManager == null)
        {
            Debug.LogWarning("[ScreenShake] UIManager instance not found.");
            return;
        }

        // ✅ 흔들어야 할 레이어들을 리스트에 담기
        List<Transform> shakeLayers = new List<Transform>
        {
            uiManager.GameCanvas.BigPlaceLayer,
            uiManager.GameCanvas.SmallPlaceLayer,
            uiManager.GameCanvas.ImaginationUnderlayLayer,
            uiManager.GameCanvas.ImaginationOverlayLayer
        };

        // ✅ 실제 존재하는 레이어만 추려서 흔들기
        shakeLayers.RemoveAll(layer => layer == null);
        if (shakeLayers.Count == 0)
        {
            Debug.LogWarning("[ScreenShake] No active layers to shake.");
            return;
        }

        // ✅ 원래 스케일 저장
        Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();

        // ✅ 줌인 (대기 없이 즉시 실행)
        foreach (var layer in shakeLayers)
        {
            originalScales[layer] = layer.localScale; // 기존 스케일 저장
            layer.DOScale(originalScales[layer] * _scaleMultiplier, _scaleDuration).SetEase(Ease.OutQuad);
        }

        // ✅ 모든 레이어 흔들기 (줌인과 동시에 실행)
        foreach (var layer in shakeLayers)
        {
            layer.DOShakePosition(_duration, _strength, 10, 90f, false, true);
        }

        await UniTask.WaitForSeconds(_duration - _scaleDuration); // ✅ 전체 지속 시간 대기

        // ✅ 줌아웃 (대기 없이 즉시 실행)
        foreach (var layer in shakeLayers)
        {
            layer.DOScale(originalScales[layer], _scaleDuration).SetEase(Ease.OutQuad);
        }

        await UniTask.WaitForSeconds(_scaleDuration); // ✅ 전체 지속 시간 대기
    }

    public override void ExecuteInstantly()
    {
    }
}
