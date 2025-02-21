using UnityEngine;
using UnityEngine.UI;

public static class ImageUtils
{
    /// <summary>
    /// ✅ Image에서 AnimationImage 가져오거나 추가 (Image가 존재하지 않을 경우 예외 처리)
    /// </summary>
    public static AnimationImage GetAnim(this Image objImg)
    {
        if (objImg == null)
        {
            Debug.LogError("[ImageUtils] GetAnim failed: Image component is null!");
            return null;
        }

        return GetAnim(objImg.gameObject);
    }

    /// <summary>
    /// ✅ GameObject에서 AnimationImage 가져오거나 추가 (Image 여부 확인 후 동작)
    /// </summary>
    public static AnimationImage GetAnim(this GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("[ImageUtils] GetAnim failed: GameObject is null!");
            return null;
        }

        var image = obj.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError($"[ImageUtils] GetAnim failed: GameObject '{obj.name}' does not have an Image component!");
            return null;
        }

        var animationImage = obj.GetComponent<AnimationImage>();
        if (animationImage == null)
        {
            animationImage = obj.AddComponent<AnimationImage>();
        }
        return animationImage;
    }
 /// <summary>
    /// ✅ **가로는 부모 크기로 고정 & 세로 비율 유지**
    /// </summary>
    public static void ApplyStretchWithAspectRatio(this Image image)
    {
        if (image == null || image.sprite == null)
        {
            Debug.LogError("[ImageUtils] Image 또는 Sprite가 존재하지 않습니다!");
            return;
        }

        RectTransform rectTransform = image.rectTransform;
        RectTransform parentRect = image.transform.parent as RectTransform;

        if (rectTransform == null || parentRect == null)
        {
            Debug.LogError("[ImageUtils] RectTransform 또는 부모 RectTransform을 찾을 수 없습니다!");
            return;
        }

        // ✅ 부모의 가로 크기를 가져와서 가로를 맞춤
        float parentWidth = parentRect.rect.width;

        // ✅ 원본 이미지의 가로:세로 비율 계산
        float aspectRatio = image.sprite.rect.width / image.sprite.rect.height;

        // ✅ 세로 길이를 비율 유지하면서 자동 조정
        float newHeight = parentWidth / aspectRatio;

        // ✅ 가로는 부모 크기로 고정, 세로는 자동 조정
        rectTransform.sizeDelta = new Vector2(0, newHeight);

        // ✅ Stretch 적용 (좌우만)
        rectTransform.anchorMin = new Vector2(0, 0.5f);
        rectTransform.anchorMax = new Vector2(1, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0, 0); // ✅ 가로 패딩 0 유지

        Debug.Log($"[ImageUtils] '{image.gameObject.name}' - 가로 Stretch 적용, 세로 비율 유지! (New Height: {newHeight})");
    }/// <summary>
    /// ✅ **전체 화면 Stretch (가로 & 세로)**
    /// </summary>
    public static void ApplyFullStretch(this Image image)
    {
        if (image == null)
        {
            Debug.LogError("[ImageUtils] Image가 존재하지 않습니다!");
            return;
        }

        RectTransform rectTransform = image.rectTransform;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = Vector2.zero;

        Debug.Log($"[ImageUtils] '{image.gameObject.name}' - 전체 Stretch 적용 완료!");
    }

}
