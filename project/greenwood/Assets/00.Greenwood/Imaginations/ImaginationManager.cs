using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class ImaginationManager : MonoBehaviour
{
    public static ImaginationManager Instance { get; private set; }
    public AnimationImage CurImaginationOverlay { get => _curImaginationOverlay; }
    public AnimationImage CurImaginationUnderlay { get => _curImaginationUnderlay; }

    private Dictionary<string, Sprite> _imaginations = new Dictionary<string, Sprite>();

    private AnimationImage _curImaginationOverlay;
    private AnimationImage _curImaginationUnderlay;

    [SerializeField] private AnimationImage _backgroundOverlayImg;
    [SerializeField] private AnimationImage _backgroundUnderlayImg;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadImaginations();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Resources/Imaginations 폴더에서 모든 이미지를 로드
    /// </summary>
    private void LoadImaginations()
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>("Imaginations");
        foreach (var sprite in loadedSprites)
        {
            if (!_imaginations.ContainsKey(sprite.name))
            {
                _imaginations.Add(sprite.name, sprite);
            }
        }
    }
    public void FadeInBackgroundPanel(bool isOverlay, float duration)
    {
        AnimationImage backgroundAnim = isOverlay ? _backgroundOverlayImg : _backgroundUnderlayImg;
        backgroundAnim?.FadeFrom(1f, 0f, duration);
    }

    public void FadeOutBackgroundPanel(bool isOverlay, float duration)
    {
        AnimationImage backgroundAnim = isOverlay ? _backgroundOverlayImg : _backgroundUnderlayImg;
        backgroundAnim?.Fade(0f, duration);
    }

   public AnimationImage CreateImageAndShow(string imageID, bool isOverlay, float duration)
    {
        if (!_imaginations.TryGetValue(imageID, out Sprite sprite))
        {
            Debug.LogError($"[ImaginationManager] Image ID '{imageID}' not found in Resources/Imaginations!");
            return null;
        }
        AnimationImage curImagination = GetCurrentImage(isOverlay);
        if(curImagination != null){
            // 기존 이미지 제거 (중복 방지)
            curImagination?.FadeAndDestroy(duration);
        }

        Transform parentLayer = isOverlay ? UIManager.Instance.GameCanvas.ImaginationOverlayLayer : UIManager.Instance.GameCanvas.ImaginationUnderlayLayer;


        // 새 이미지 생성
        Image imgObj = new GameObject($"Imagination_{imageID}").AddComponent<Image>();
        imgObj.sprite = sprite;
        imgObj.transform.SetParent(parentLayer, false);
        imgObj.ApplyStretchWithAspectRatio();
        AnimationImage animationImage = imgObj.gameObject.AddComponent<AnimationImage>();

        SetCurrentImage(isOverlay, animationImage);

        // ✅ 새 이미지 페이드 인 적용
        animationImage.FadeFrom(1f, 0f, duration);

        return animationImage;
    }

    public void SetCurrentImage(bool isOverlay, AnimationImage animationImage){
        
        if (isOverlay)
            _curImaginationOverlay = animationImage;
        else
            _curImaginationUnderlay = animationImage;

    }

    public AnimationImage GetCurrentImage(bool isOverlay)
    {
        return isOverlay ? _curImaginationOverlay : _curImaginationUnderlay;
    }

    public void DestroyCurrentImage(bool isOverlay, float duration)
    {
        AnimationImage curImage = GetCurrentImage(isOverlay);

        if (curImage == null)
        {
            Debug.LogWarning($"[ImaginationManager] No active {(isOverlay ? "Overlay" : "Underlay")} image to destroy.");
            return;
        }

        if (isOverlay)
            _curImaginationOverlay = null;
        else
            _curImaginationUnderlay = null;

        curImage.FadeAndDestroy(duration);
    }



}
